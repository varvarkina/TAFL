using Execution;
using Parser;

namespace Parser.UnitTests;

public class ParseTopLevelStatementsTest
{
    [Fact]
    public void Can_parse_input_output_program()
    {
        FakeEnvironment env = new(42.0);
        string code = """
            var x;
            input(x);
            print(x);
            """;

        Parser parser = new(code, env);
        parser.ParseProgram();

        Assert.Single(env.Results);
        Assert.Equal(42.0, env.Results[0]);
    }

    [Fact]
    public void Can_parse_var_and_const_usage()
    {
        FakeEnvironment env = new();
        string code = """
            const PI = 3.14;
            var r = 2;
            print(PI * r * r);
            """;

        Parser parser = new(code, env);
        parser.ParseProgram();

        Assert.Single(env.Results);
        Assert.Equal(12.56, env.Results[0], 2);
    }

    [Fact]
    public void Can_parse_builtins_in_print()
    {
        FakeEnvironment env = new();
        string code = """
            print(min(10, 3, 15));
            print(max(1, 5));
            print(abs(-5));
            """;

        Parser parser = new(code, env);
        parser.ParseProgram();

        Assert.Equal(3, env.Results.Count);
        Assert.Equal(3.0, env.Results[0]);
        Assert.Equal(5.0, env.Results[1]);
        Assert.Equal(5.0, env.Results[2]);
    }

    [Fact]
    public void Print_multiple_arguments_emits_all_results()
    {
        FakeEnvironment env = new();
        string code = """
            print(1, 2 + 3);
            """;

        Parser parser = new(code, env);
        parser.ParseProgram();

        Assert.Equal(new[] { 1.0, 5.0 }, env.Results);
    }

    [Fact]
    public void Divide_by_zero_throws()
    {
        FakeEnvironment env = new();
        string code = """
            var x = 1;
            print(x / 0);
            """;

        Parser parser = new(code, env);
        Assert.Throws<DivideByZeroException>(() => parser.ParseProgram());
    }

    [Fact]
    public void Missing_closing_brace_reports_error()
    {
        FakeEnvironment env = new();
        string code = """
            {
                var x = 1;
                print(x);
            """;

        Parser parser = new(code, env);
        Assert.Throws<UnexpectedLexemeException>(() => parser.ParseProgram());
    }

    [Fact]
    public void Print_without_arguments_produces_no_results()
    {
        FakeEnvironment env = new();
        string code = """
            print();
            ;
            """;

        Parser parser = new(code, env);
        parser.ParseProgram();

        Assert.Empty(env.Results);
    }

    [Fact]
    public void Using_undeclared_variable_throws()
    {
        FakeEnvironment env = new();
        string code = """
            print(x);
            """;

        Parser parser = new(code, env);
        Assert.Throws<Exception>(() => parser.ParseProgram());
    }
}