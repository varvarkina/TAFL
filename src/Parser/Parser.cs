using Lexer;

namespace Parser;

/// <summary>
/// Выполняет синтаксический разбор выражений языка DEA.
/// Грамматика описана в файле `docs/specification/expressions-grammar.md`.
/// </summary>
public class Parser
{
	private readonly TokenStream _tokens;

	private Parser(string code)
	{
		_tokens = new TokenStream(code);
	}

	/// <summary>
	/// Выполняет разбор выражения и возвращает результат.
	/// </summary>
	public static Row EvaluateExpression(string code)
	{
		Parser p = new(code);
		object result = p.ParseExpression();
		return new Row(result);
	}

	/// <summary>
	/// Разбирает выражение.
	/// Правила: expression = logical_or ;
	/// </summary>
	private object ParseExpression()
	{
		return ParseLogicalOr();
	}

	/// <summary>
	/// Разбирает логическое ИЛИ выражение.
	/// Правила: logical_or = logical_and, { "||", logical_and } ;
	/// </summary>
	private object ParseLogicalOr()
	{
		object left = ParseLogicalAnd();

		while (_tokens.Peek().Type == TokenType.Or)
		{
			_tokens.Advance();
			object right = ParseLogicalAnd();

			bool leftBool = Convert.ToBoolean(left);
			bool rightBool = Convert.ToBoolean(right);
			left = leftBool || rightBool;
		}

		return left;
	}

	/// <summary>
	/// Разбирает логическое И выражение.
	/// Правила: logical_and = equality, { "&&", equality } ;
	/// </summary>
	private object ParseLogicalAnd()
	{
		object left = ParseEquality();

		while (_tokens.Peek().Type == TokenType.And)
		{
			_tokens.Advance();
			object right = ParseEquality();

			bool leftBool = Convert.ToBoolean(left);
			bool rightBool = Convert.ToBoolean(right);
			left = leftBool && rightBool;
		}

		return left;
	}

	/// <summary>
	/// Разбирает выражение равенства/неравенства.
	/// Правила: equality = comparison, { ( "==" | "!=" ), comparison } ;
	/// </summary>
	private object ParseEquality()
	{
		object left = ParseComparison();

		while (_tokens.Peek().Type == TokenType.Equal || _tokens.Peek().Type == TokenType.NotEqual)
		{
			Token operatorToken = _tokens.Peek();
			_tokens.Advance();

			object right = ParseComparison();
			left = EvaluateEquality(left, right, operatorToken.Type);
		}

		return left;
	}

	/// <summary>
	/// Разбирает выражение сравнения.
	/// Правила: comparison = additive, { ( "<" | "<=" | ">" | ">=" ), additive } ;
	/// </summary>
	private object ParseComparison()
	{
		object left = ParseAdditive();

		while (IsComparisonOperator(_tokens.Peek().Type))
		{
			Token operatorToken = _tokens.Peek();
			_tokens.Advance();

			object right = ParseAdditive();
			left = EvaluateComparison(left, right, operatorToken.Type);
		}

		return left;
	}

	/// <summary>
	/// Разбирает аддитивное выражение.
	/// Правила: additive = multiplicative, { ( "+" | "-" ), multiplicative } ;
	/// </summary>
	private object ParseAdditive()
	{
		object left = ParseMultiplicative();

		while (_tokens.Peek().Type == TokenType.Plus || _tokens.Peek().Type == TokenType.Minus)
		{
			Token operatorToken = _tokens.Peek();
			_tokens.Advance();

			object right = ParseMultiplicative();
			left = EvaluateAdditiveOperator(left, right, operatorToken.Type);
		}

		return left;
	}

	/// <summary>
	/// Разбирает мультипликативное выражение.
	/// Правила: multiplicative = power, { ( "*" | "/" | "//" | "%" ), power } ;
	/// </summary>
	private object ParseMultiplicative()
	{
		object left = ParsePower();

		while (IsMultiplicativeOperator(_tokens.Peek().Type))
		{
			Token operatorToken = _tokens.Peek();
			_tokens.Advance();

			object right = ParsePower();
			left = EvaluateMultiplicativeOperator(left, right, operatorToken.Type);
		}

		return left;
	}

	/// <summary>
	/// Разбирает выражение возведения в степень.
	/// Правила: power = unary, [ "^", power ] ;
	/// Оператор ^ правоассоциативный
	/// </summary>
	private object ParsePower()
	{
		object left = ParseUnary();

		if (_tokens.Peek().Type == TokenType.Power)
		{
			_tokens.Advance();
			object right = ParsePower(); // Рекурсия для правой ассоциативности
			
			decimal leftNum = Convert.ToDecimal(left);
			decimal rightNum = Convert.ToDecimal(right);
			left = (decimal)Math.Pow((double)leftNum, (double)rightNum);
		}

		return left;
	}

	/// <summary>
	/// Разбирает унарное выражение.
	/// Правила: unary = "+" , unary | "-" , unary | "!" , unary | primary ;
	/// </summary>
	private object ParseUnary()
	{
		if (_tokens.Peek().Type == TokenType.Plus)
		{
			_tokens.Advance();
			object operand = ParseUnary();
			// Унарный + не меняет значение
			return operand;
		}

		if (_tokens.Peek().Type == TokenType.Minus)
		{
			_tokens.Advance();
			object operand = ParseUnary();
			return -Convert.ToDecimal(operand);
		}

		if (_tokens.Peek().Type == TokenType.Not)
		{
			_tokens.Advance();
			object operand = ParseUnary();
			return !Convert.ToBoolean(operand);
		}

		return ParsePrimary();
	}

	/// <summary>
	/// Разбирает первичное выражение.
	/// Правила: primary = number | identifier | function_call | "(", expression, ")" ;
	/// </summary>
	private object ParsePrimary()
	{
		Token token = _tokens.Peek();

		switch (token.Type)
		{
			case TokenType.IntegerLiteral:
			case TokenType.FloatLiteral:
				_tokens.Advance();
				return token.Value!.ToDecimal();

			case TokenType.StringLiteral:
				_tokens.Advance();
				return token.Value!.ToString() ?? "";

			case TokenType.Identifier:
				return ParseFunctionCallOrIdentifier();

			case TokenType.OpenParenthesis:
				_tokens.Advance();
				object result = ParseExpression();
				Match(TokenType.CloseParenthesis);
				return result;

			default:
				throw new UnexpectedLexemeException("primary expression", token);
		}
	}

	/// <summary>
	/// Разбирает вызов функции или идентификатор.
	/// Правила: function_call = function_name, "(", [ argument_list ], ")" ;
	/// </summary>
	private object ParseFunctionCallOrIdentifier()
	{
		Token identifierToken = _tokens.Peek();
		string functionName = identifierToken.Value!.ToString();
		_tokens.Advance();
		
		if (_tokens.Peek().Type == TokenType.OpenParenthesis)
		{
			_tokens.Advance();

			List<decimal> arguments = new List<decimal>();
			
			if (_tokens.Peek().Type != TokenType.CloseParenthesis)
			{
				arguments.Add(Convert.ToDecimal(ParseExpression()));
				
				while (_tokens.Peek().Type == TokenType.Comma)
				{
					_tokens.Advance();
					arguments.Add(Convert.ToDecimal(ParseExpression()));
				}
			}

			Match(TokenType.CloseParenthesis);
			return BuiltinFunctions.Invoke(functionName, arguments);
		}

		throw new UnexpectedLexemeException("function call", identifierToken);
	}

	private bool IsComparisonOperator(TokenType type)
	{
		return type == TokenType.Less || type == TokenType.Greater ||
			   type == TokenType.LessOrEqual || type == TokenType.GreaterOrEqual;
	}

	private bool IsMultiplicativeOperator(TokenType type)
	{
		return type == TokenType.Multiply || type == TokenType.Divide ||
			   type == TokenType.IntegerDivide || type == TokenType.Modulo;
	}

	private object EvaluateMultiplicativeOperator(object left, object right, TokenType operatorType)
	{
		if (left?.GetType() != right?.GetType())
		{
			throw new Exception($"Cannot compare different types: {left?.GetType().Name} and {right?.GetType().Name}");
		}
		decimal leftNum = Convert.ToDecimal(left);
		decimal rightNum = Convert.ToDecimal(right);

		return operatorType switch
		{
			TokenType.Multiply => leftNum * rightNum,
			TokenType.Divide => rightNum != 0 ? leftNum / rightNum : throw new DivideByZeroException(),
			TokenType.IntegerDivide => rightNum != 0 ? Math.Floor(leftNum / rightNum) : throw new DivideByZeroException(),
			TokenType.Modulo => rightNum != 0 ? leftNum % rightNum : throw new DivideByZeroException(),
			_ => throw new Exception($"Unsupported multiplicative operator: {operatorType}")
		};
	}

	private object EvaluateAdditiveOperator(object left, object right, TokenType operatorType)
	{
		// Конкатенация строк работает только для оператора +
		if (operatorType == TokenType.Plus && (left is string || right is string))
		{
			return (left?.ToString() ?? "") + (right?.ToString() ?? "");
		}

		if (left?.GetType() != right?.GetType())
		{
			throw new Exception($"Cannot compare different types: {left?.GetType().Name} and {right?.GetType().Name}");
		}

		decimal leftNum = Convert.ToDecimal(left);
		decimal rightNum = Convert.ToDecimal(right);

		return operatorType switch
		{
			TokenType.Plus => leftNum + rightNum,
			TokenType.Minus => leftNum - rightNum,
			_ => throw new Exception($"Unsupported additive operator: {operatorType}")
		};
	}

	private object EvaluateEquality(object left, object right, TokenType operatorType)
	{
		if (left?.GetType() != right?.GetType())
		{
			throw new Exception($"Cannot compare different types: {left?.GetType().Name} and {right?.GetType().Name}");
		}

		return operatorType switch
		{
			TokenType.Equal => left!.Equals(right),
			TokenType.NotEqual => !left!.Equals(right),
			_ => throw new Exception($"Unsupported equality operator: {operatorType}")
		};
	}

	private object EvaluateComparison(object left, object right, TokenType operatorType)
	{
		if (left?.GetType() != right?.GetType())
		{
			throw new Exception($"Cannot compare different types: {left?.GetType().Name} and {right?.GetType().Name}");
		}

		return operatorType switch
		{
			TokenType.Less => Convert.ToDecimal(left) < Convert.ToDecimal(right),
			TokenType.Greater => Convert.ToDecimal(left) > Convert.ToDecimal(right),
			TokenType.LessOrEqual => Convert.ToDecimal(left) <= Convert.ToDecimal(right),
			TokenType.GreaterOrEqual => Convert.ToDecimal(left) >= Convert.ToDecimal(right),
			_ => throw new Exception($"Unsupported comparison operator: {operatorType}")
		};
	}

	private void Match(TokenType expected)
	{
		Token t = _tokens.Peek();
		if (t.Type != expected)
		{
			throw new UnexpectedLexemeException(expected, t);
		}
		_tokens.Advance();
	}
}