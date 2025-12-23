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
    public void Integer_divide_by_zero_throws()
    {
        FakeEnvironment env = new();
        string code = """
            var x = 5;
            print(x // 0);
            """;

        Parser parser = new(code, env);
        Assert.Throws<DivideByZeroException>(() => parser.ParseProgram());
    }

    [Fact]
    public void Modulo_by_zero_throws()
    {
        FakeEnvironment env = new();
        string code = """
            var x = 5;
            print(x % 0);
            """;

        Parser parser = new(code, env);
        Assert.Throws<DivideByZeroException>(() => parser.ParseProgram());
    }

    [Fact]
    public void Can_parse_assignment_statement()
    {
        FakeEnvironment env = new();
        string code = """
            var x;
            x = 20;
            print(x);
            """;

        Parser parser = new(code, env);
        parser.ParseProgram();

        Assert.Single(env.Results);
        Assert.Equal(20.0, env.Results[0]);
    }

    [Fact]
    public void Can_parse_assignment_with_expression()
    {
        FakeEnvironment env = new();
        string code = """
            var x;
            var y = 5;
            x = y * 2;
            print(x);
            """;

        Parser parser = new(code, env);
        parser.ParseProgram();

        Assert.Single(env.Results);
        Assert.Equal(10.0, env.Results[0]);
    }

    [Fact]
    public void Can_parse_assignment_with_builtin_function()
    {
        FakeEnvironment env = new();
        string code = """
            var x;
            x = abs(-10);
            print(x);
            """;

        Parser parser = new(code, env);
        parser.ParseProgram();

        Assert.Single(env.Results);
        Assert.Equal(10.0, env.Results[0]);
    }

    [Fact]
    public void Can_parse_variable_declaration_with_expression()
    {
        FakeEnvironment env = new();
        string code = """
            var x = 2 + 3;
            print(x);
            """;

        Parser parser = new(code, env);
        parser.ParseProgram();

        Assert.Single(env.Results);
        Assert.Equal(5.0, env.Results[0]);
    }

    [Fact]
    public void Print_without_arguments_produces_no_results()
    {
        FakeEnvironment env = new();
        string code = """
            print();
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

    [Fact]
    public void Can_parse_multiple_variables_and_operations()
    {
        FakeEnvironment env = new();
        string code = """
            var a = 10;
            var b = 20;
            var sum = a + b;
            print(sum);
            """;

        Parser parser = new(code, env);
        parser.ParseProgram();

        Assert.Single(env.Results);
        Assert.Equal(30.0, env.Results[0]);
    }

    [Fact]
    public void Can_parse_arithmetic_operations_in_statements()
    {
        FakeEnvironment env = new();
        string code = """
            var x = 15;
            var y = 3;
            var a = x * y;
            var b = x / y;
            print(a);
            print(b);
            """;

        Parser parser = new(code, env);
        parser.ParseProgram();

        Assert.Equal(2, env.Results.Count);
        Assert.Equal(45.0, env.Results[0]);
        Assert.Equal(5.0, env.Results[1]);
    }

    [Fact]
    public void Can_parse_expressions_with_parentheses()
    {
        FakeEnvironment env = new();
        string code = """
            var result = (10 + 5) * 2;
            print(result);
            """;

        Parser parser = new(code, env);
        parser.ParseProgram();

        Assert.Single(env.Results);
        Assert.Equal(30.0, env.Results[0]);
    }

    [Fact]
    public void Can_parse_power_operation_in_statement()
    {
        FakeEnvironment env = new();
        string code = """
            var base = 2;
            var poow = 8;
            var power = base ^ poow;
            print(power);
            """;

        Parser parser = new(code, env);
        parser.ParseProgram();

        Assert.Single(env.Results);
        Assert.Equal(256.0, env.Results[0]);
    }

    [Fact]
    public void Can_parse_comparison_operations()
    {
        FakeEnvironment env = new();
        string code = """
            var a = 10;
            var b = 5;
            print(a > b);
            print(a < b);
            print(a == b);
            """;

        Parser parser = new(code, env);
        parser.ParseProgram();

        Assert.Equal(3, env.Results.Count);
        Assert.Equal(1.0, env.Results[0]); // true
        Assert.Equal(0.0, env.Results[1]); // false
        Assert.Equal(0.0, env.Results[2]); // false
    }

    [Fact]
    public void Can_parse_logical_operations()
    {
        FakeEnvironment env = new();
        string code = """
            var x = 1;
            var y = 0;
            print(x && y);
            print(x || y);
            """;

        Parser parser = new(code, env);
        parser.ParseProgram();

        Assert.Equal(2, env.Results.Count);
        Assert.Equal(0.0, env.Results[0]); // false
        Assert.Equal(1.0, env.Results[1]); // true
    }

    [Fact]
    public void Can_parse_integer_division_and_modulo()
    {
        FakeEnvironment env = new();
        string code = """
            var a = 17;
            var b = 5;
            var c = a // b;
            var d = a % b;
            print(c);
            print(d);
            """;

        Parser parser = new(code, env);
        parser.ParseProgram();

        Assert.Equal(2, env.Results.Count);
        Assert.Equal(3.0, env.Results[0]);
        Assert.Equal(2.0, env.Results[1]);
    }

}