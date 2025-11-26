using System.Text;

namespace Lexer;

public class LexicalStats
{
    public static string CollectFromFile(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"File not found: {path}");
        }

        string content = File.ReadAllText(path);
        var lexer = new Lexer(content);
        
        int keywords = 0;
        int identifiers = 0;
        int numberLiterals = 0;
        int stringLiterals = 0;
        int operators = 0;
        int otherLexemes = 0;

        Token token = lexer.ParseToken();
        while (token.Type != TokenType.EndOfFile)
        {
            switch (token.Type)
            {
                case TokenType.Var:
                case TokenType.Const:
                case TokenType.Func:
                case TokenType.Proc:
                case TokenType.Return:
                case TokenType.If:
                case TokenType.Else:
                case TokenType.While:
                case TokenType.For:
                case TokenType.To:
                case TokenType.Downto:
                case TokenType.Break:
                case TokenType.Continue:
                case TokenType.True:
                case TokenType.False:
                case TokenType.Input:
                case TokenType.Print:
                    keywords++;
                    break;
                
                case TokenType.Identifier:
                    identifiers++;
                    break;
                
                case TokenType.IntegerLiteral:
                case TokenType.FloatLiteral:
                    numberLiterals++;
                    break;
                
                case TokenType.StringLiteral:
                    stringLiterals++;
                    break;

                case TokenType.Assign:
                case TokenType.NotEqual:
                case TokenType.Equal:
                case TokenType.LessOrEqual:
                case TokenType.GreaterOrEqual:
                case TokenType.Less:
                case TokenType.Greater:
                case TokenType.Multiply:
                case TokenType.Divide:
                case TokenType.IntegerDivide:
                case TokenType.Modulo:
                case TokenType.Minus:
                case TokenType.Plus:
                case TokenType.And:
                case TokenType.Or:
                case TokenType.Not:
                    operators++;
                    break;
                
                case TokenType.Error:
                    otherLexemes++; 
                    break;

                default:
                    otherLexemes++;
                    break;
            }
            token = lexer.ParseToken();
        }

        var sb = new StringBuilder();
        sb.AppendLine($"keywords: {keywords}");
        sb.AppendLine($"identifier: {identifiers}");
        sb.AppendLine($"number literals: {numberLiterals}");
        sb.AppendLine($"string literals: {stringLiterals}");
        sb.AppendLine($"operators: {operators}");
        sb.Append($"other lexemes: {otherLexemes}");
        
        return sb.ToString();
    }
}
