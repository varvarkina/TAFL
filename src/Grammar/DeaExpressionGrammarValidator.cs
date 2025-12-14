using Antlr4.Runtime;
using Grammar.Generated;

namespace Grammar;

public sealed record GrammarError(int Line, int Column, string Message);

public sealed record GrammarValidationResult(bool IsValid, IReadOnlyList<GrammarError> Errors);

public sealed class DeaExpressionGrammarValidator
{
    public GrammarValidationResult ValidateText(string text)
    {
        var input = CharStreams.fromString(text); // стандартный способ для ANTLR runtime [web:61]

        var lexer = new DeaExprLexer(input);
        var lexErrors = new List<GrammarError>();
        lexer.RemoveErrorListeners();
        lexer.AddErrorListener(new CollectingLexerErrorListener(lexErrors));

        var tokens = new CommonTokenStream(lexer);

        var parser = new DeaExprParser(tokens);
        var parseErrors = new List<GrammarError>();
        parser.RemoveErrorListeners();
        parser.AddErrorListener(new CollectingParserErrorListener(parseErrors));

        parser.unit(); // expression EOF

        var all = lexErrors.Concat(parseErrors).ToArray();
        return new GrammarValidationResult(all.Length == 0, all);
    }

    public GrammarValidationResult ValidateFile(string path)
    {
        var text = File.ReadAllText(path);
        return ValidateText(text);
    }

    private sealed class CollectingLexerErrorListener : IAntlrErrorListener<int>
    {
        private readonly List<GrammarError> _errors;
        public CollectingLexerErrorListener(List<GrammarError> errors) => _errors = errors;

        public void SyntaxError(
            TextWriter output,
            IRecognizer recognizer,
            int offendingSymbol,
            int line,
            int charPositionInLine,
            string msg,
            RecognitionException e)
        {
            _errors.Add(new GrammarError(line, charPositionInLine, msg));
        }
    }

    private sealed class CollectingParserErrorListener : BaseErrorListener
    {
        private readonly List<GrammarError> _errors;
        public CollectingParserErrorListener(List<GrammarError> errors) => _errors = errors;

        public override void SyntaxError(
            TextWriter output,
            IRecognizer recognizer,
            IToken offendingSymbol,
            int line,
            int charPositionInLine,
            string msg,
            RecognitionException e)
        {
            _errors.Add(new GrammarError(line, charPositionInLine, msg));
        }
    }
}
