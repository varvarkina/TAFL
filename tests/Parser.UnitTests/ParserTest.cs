using Parser;
using Xunit;

namespace Parser.UnitTests;

public class ParserTests
{
	[Theory]
	[MemberData(nameof(GetExpressionTestData))]
	public void Can_parse_expressions(string code, object expected)
	{
		Row res = Parser.EvaluateExpression(code);

		if (expected == null)
			Assert.Null(res[0]);
		else
			Assert.Equal(expected, res[0]);
	}

	[Theory]
	[MemberData(nameof(GetDifferentTypesExpressionTestData))]
	public void Cant_different_types_parse_expressions(string code)
	{
		Assert.Throws<Exception>(() => Parser.EvaluateExpression(code));
	}

	public static TheoryData<string> GetDifferentTypesExpressionTestData()
	{
		return new TheoryData<string>
		{
			"\"test\" * 2",
			"\"abc\" / 3",
			"\"world\" - 5",
			"5 - \"hello\""
		};
	}

	public static TheoryData<string, object> GetExpressionTestData()
	{
		return new TheoryData<string, object>
		{
			// Числовые литералы
			{ "2025", 2025m },
			{ "3.14", 3.14m },
			{ "0", 0m },
			{ "123.456", 123.456m },

			// Строковые литералы
			{ "\"hello\"", "hello" },
			{ "\"\"", "" },

			// Унарные операторы
			{ "-5", -5m },
			{ "+5", 5m },
			{ "-3.14", -3.14m },
			{ "+42", 42m },

			// Логическое НЕ
			{ "!1", false },
			{ "!0", true },

			// Арифметические операторы
			{ "1 + 2", 3m },
			{ "5 - 3", 2m },
			{ "2 * 3", 6m },
			{ "6 / 2", 3m },
			{ "7 // 2", 3m },
			{ "7 % 3", 1m },
			{ "2 ^ 3", 8m },
			{ "2 ^ 3 ^ 2", 512m },
			{ "(-2) ^ 3", -8m },
			{ "4 ^ 0.5", 2m }, 

			// Конкатенация строк
			{ "\"hello\" + \"world\"", "helloworld" },
			{ "\"test\" + \"\"", "test" },

			// Операторы сравнения
			{ "3 < 5", true },
			{ "5 < 5", false },
			{ "6 < 5", false },
			{ "3 > 5", false },
			{ "5 > 5", false },
			{ "6 > 5", true },
			{ "3 <= 5", true },
			{ "5 <= 5", true },
			{ "6 <= 5", false },
			{ "3 >= 5", false },
			{ "5 >= 5", true },
			{ "6 >= 5", true },

			// Операторы равенства/неравенства
			{ "5 == 5", true },
			{ "5 == 3", false },
			{ "5 != 3", true },
			{ "5 != 5", false },
			{ "\"hello\" == \"hello\"", true },
			{ "\"hello\" == \"world\"", false },
			{ "\"hello\" != \"world\"", true },

			// Логические операторы
			{ "1 && 1", true },
			{ "1 && 0", false },
			{ "0 && 1", false },
			{ "0 && 0", false },
			{ "1 || 1", true },
			{ "1 || 0", true },
			{ "0 || 1", true },
			{ "0 || 0", false },

			// Приоритет операторов
			{ "2 + 3 * 4", 14m },
			{ "10 - 3 - 2", 5m },
			{ "12 / 3 / 2", 2m },
			{ "-3 + 2", -1m },
			{ "3 + 2 * 3", 9m },
			{ "1 + 2 > 2", true },
			{ "1 < 2 == 5 > 4", true },
			{ "2 ^ 3 * 2", 16m },
			{ "2 * 3 ^ 2", 18m },

			// Скобки
			{ "(1 + 2) * 3", 9m },
			{ "2 * (3 + 4)", 14m },
			{ "((1 + 2) * 3) + 4", 13m },

			// Встроенные функции
			{ "abs(-5)", 5m },
			{ "abs(5)", 5m },
			{ "abs(-3.14)", 3.14m },
			{ "min(7, 3, 5)", 3m },
			{ "min(1)", 1m },
			{ "min(5, 2)", 2m },
			{ "max(2, 8, 4)", 8m },
			{ "max(1)", 1m },
			{ "max(5, 2)", 5m },

			// Комбинированные выражения
			{ "abs(-5) + 3", 8m },
			{ "min(10, 20) * 2", 20m },
			{ "max(1, 2, 3) ^ 2", 9m },
			{ "(1 + 2) * abs(-3)", 9m },
		};
	}
}