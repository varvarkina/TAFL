using Lexer;

namespace Lexer.UnitTests;

public class LexerTest
{
    [Theory]
    [MemberData(nameof(GetTokenizeData))]
    public void Can_tokenize_DEA(string source, List<Token> expected)
    {
        List<Token> actual = Tokenize(source);
        Assert.Equal(expected, actual);
    }

    public static TheoryData<string, List<Token>> GetTokenizeData()
    {
        return new TheoryData<string, List<Token>>
        {
            // 1. Ключевые слова
            {
                "var const func proc return if else while for to downto break continue true false input print",
                [
                    new Token(TokenType.Var), new Token(TokenType.Const), new Token(TokenType.Func),
                    new Token(TokenType.Proc), new Token(TokenType.Return), new Token(TokenType.If),
                    new Token(TokenType.Else), new Token(TokenType.While), new Token(TokenType.For),
                    new Token(TokenType.To), new Token(TokenType.Downto), new Token(TokenType.Break),
                    new Token(TokenType.Continue), new Token(TokenType.True), new Token(TokenType.False),
                    new Token(TokenType.Input), new Token(TokenType.Print),
                    new Token(TokenType.EndOfFile)
                ]
            },
            // Регистронезависимость
            {
                "VAR Var var",
                [
                    new Token(TokenType.Var), new Token(TokenType.Var), new Token(TokenType.Var),
                    new Token(TokenType.EndOfFile)
                ]
            },
            // 2. Идентификаторы
            {
                "x myVar var1",
                [
                    new Token(TokenType.Identifier, new TokenValue("x")),
                    new Token(TokenType.Identifier, new TokenValue("myVar")),
                    new Token(TokenType.Identifier, new TokenValue("var1")),
                    new Token(TokenType.EndOfFile)
                ]
            },
            // 3. Числа
            {
                "0 123 1.23 0.5",
                [
                    new Token(TokenType.IntegerLiteral, new TokenValue(0m)),
                    new Token(TokenType.IntegerLiteral, new TokenValue(123m)),
                    new Token(TokenType.FloatLiteral, new TokenValue(1.23m)),
                    new Token(TokenType.FloatLiteral, new TokenValue(0.5m)),
                    new Token(TokenType.EndOfFile)
                ]
            },
            // 4. Строки
            {
                "\"hello\" \"\" \"line\\nnew\"",
                [
                    new Token(TokenType.StringLiteral, new TokenValue("hello")),
                    new Token(TokenType.StringLiteral, new TokenValue("")),
                    new Token(TokenType.StringLiteral, new TokenValue("line\nnew")),
                    new Token(TokenType.EndOfFile)
                ]
            },
            // 5. Операторы
            {
                "= == != < <= > >= + - * / // % && || !",
                [
                    new Token(TokenType.Assign), new Token(TokenType.Equal), new Token(TokenType.NotEqual),
                    new Token(TokenType.Less), new Token(TokenType.LessOrEqual), new Token(TokenType.Greater),
                    new Token(TokenType.GreaterOrEqual), new Token(TokenType.Plus), new Token(TokenType.Minus),
                    new Token(TokenType.Multiply), new Token(TokenType.Divide), new Token(TokenType.IntegerDivide),
                    new Token(TokenType.Modulo), new Token(TokenType.And), new Token(TokenType.Or), new Token(TokenType.Not),
                    new Token(TokenType.EndOfFile)
                ]
            },
            // 6. Разделители
            {
                "; , : ( ) { }",
                [
                    new Token(TokenType.Semicolon), new Token(TokenType.Comma), new Token(TokenType.Colon),
                    new Token(TokenType.OpenParenthesis), new Token(TokenType.CloseParenthesis),
                    new Token(TokenType.OpenBrace), new Token(TokenType.CloseBrace),
                    new Token(TokenType.EndOfFile)
                ]
            },
            // 7. Комментарии
            {
                "var # comment\n x",
                [
                    new Token(TokenType.Var),
                    new Token(TokenType.Identifier, new TokenValue("x")),
                    new Token(TokenType.EndOfFile)
                ]
            },
            {
                "var /* comment */ x",
                [
                    new Token(TokenType.Var),
                    new Token(TokenType.Identifier, new TokenValue("x")),
                    new Token(TokenType.EndOfFile)
                ]
            }
        };
    }

    private List<Token> Tokenize(string source)
    {
        List<Token> results = [];
        Lexer lexer = new(source);

        for (Token t = lexer.ParseToken(); t.Type != TokenType.EndOfFile; t = lexer.ParseToken())
        {
            results.Add(t);
        }
        results.Add(new Token(TokenType.EndOfFile));

        return results;
    }
}