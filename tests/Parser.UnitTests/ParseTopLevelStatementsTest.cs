using Execution;
using Parser;

namespace Parser.UnitTests;

public class ParseTopLevelStatementsTest
{
    // Объявление переменной: `var x;`
    [Fact]
    public void VariableDeclaration_WithoutInitialization()
    {
        FakeEnvironment env = new();
        string code = """
            var x;
            """;

        Parser parser = new(code, env);
        parser.ParseProgram();

        Assert.True(true);
    }

    // Объявление с числовым литералом: `var x = 10;`
    [Fact]
    public void VariableDeclaration_WithLiteral_Only()
    {
        FakeEnvironment env = new();
        string code = """
            var x = 10;
            """;

        Parser parser = new(code, env);
        parser.ParseProgram();

        Assert.True(true);
    }

    // Объявление с числовым литералом: `var x = 10; print(x);`
    [Fact]
    public void VariableDeclaration_WithLiteral()
    {
        FakeEnvironment env = new();
        string code = """
            var x = 10;
            print(x);
            """;

        Parser parser = new(code, env);
        parser.ParseProgram();

        Assert.Single(env.Results);
        Assert.Equal(10.0, env.Results[0]);
    }

    // Объявление с выражением: `var x = 2 + 3;`
    [Fact]
    public void VariableDeclaration_WithExpression()
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

    // Объявление с использованием переменной: `var a = 10; var b = 20; var sum = a + b;`
    [Fact]
    public void VariableDeclaration_WithVariable()
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

    // Объявление константы: `const PI = 3.14;`
    [Fact]
    public void ConstantDeclaration()
    {
        FakeEnvironment env = new();
        string code = """
            const PI = 3.14;
            print(PI);
            """;

        Parser parser = new(code, env);
        parser.ParseProgram();

        Assert.Single(env.Results);
        Assert.Equal(3.14, env.Results[0]);
    }

    // Использование константы в выражении: `const PI = 3.14; var r = 2; print(PI * r * r);`
    [Fact]
    public void ConstantUsage_InExpression()
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

    // Присваивание числового литерала: `var x; x = 20; print(x);`
    [Fact]
    public void Assignment_WithLiteral()
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

    // Присваивание выражения: `var x; var y = 5; x = y * 2; print(x);`
    [Fact]
    public void Assignment_WithExpression()
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

    // Присваивание с использованием встроенных функций: `var x; x = abs(-10); print(x);`
    [Fact]
    public void Assignment_WithBuiltinFunction()
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

    // Ввод в объявленную переменную: `var x; input(x);`
    [Fact]
    public void Input_ToVariable()
    {
        FakeEnvironment env = new(42.0);
        string code = """
            var x;
            input(x);
            """;

        Parser parser = new(code, env);
        parser.ParseProgram();

        Assert.True(true);
    }

    // Ввод с последующим использованием: `var x; input(x); print(x);`
    [Fact]
    public void Input_ThenPrint()
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

    // Вывод числа: `print(10);`
    [Fact]
    public void Print_Number()
    {
        FakeEnvironment env = new();
        string code = """
            print(10);
            """;

        Parser parser = new(code, env);
        parser.ParseProgram();

        Assert.Single(env.Results);
        Assert.Equal(10.0, env.Results[0]);
    }

    // Вывод переменной: `var x = 5; print(x);`
    [Fact]
    public void Print_Variable()
    {
        FakeEnvironment env = new();
        string code = """
            var x = 5;
            print(x);
            """;

        Parser parser = new(code, env);
        parser.ParseProgram();

        Assert.Single(env.Results);
        Assert.Equal(5.0, env.Results[0]);
    }

    // Вывод выражения: `var x = 10; print(x + 5);`
    [Fact]
    public void Print_Expression()
    {
        FakeEnvironment env = new();
        string code = """
            var x = 10;
            print(x + 5);
            """;

        Parser parser = new(code, env);
        parser.ParseProgram();

        Assert.Single(env.Results);
        Assert.Equal(15.0, env.Results[0]);
    }

    // Вывод нескольких аргументов: `print(1, 2 + 3);`
    [Fact]
    public void Print_MultipleArguments()
    {
        FakeEnvironment env = new();
        string code = """
            print(1, 2 + 3);
            """;

        Parser parser = new(code, env);
        parser.ParseProgram();

        Assert.Equal(new[] { 1.0, 5.0 }, env.Results);
    }

    // Вывод без аргументов: `print();`
    [Fact]
    public void Print_NoArguments()
    {
        FakeEnvironment env = new();
        string code = """
            print();
            """;

        Parser parser = new(code, env);
        parser.ParseProgram();

        Assert.Empty(env.Results);
    }

    // Вывод с использованием встроенных функций: `print(min(10, 3, 15)); print(max(1, 5)); print(abs(-5));`
    [Fact]
    public void Print_WithBuiltinFunctions()
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

    // Несколько переменных и операций: `var a = 10; var b = 20; var sum = a + b; print(sum);`
    [Fact]
    public void MultipleVariables_AndOperations()
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

    // Комплексная программа: `const PI = 3.14; var r = 2; print(PI * r * r);`
    [Fact]
    public void ComplexProgram_WithConstant()
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

    // Программа с вводом-выводом: `var x; input(x); print(x);`
    [Fact]
    public void Program_WithInputOutput()
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

    // Арифметические операции в инструкциях: `var x = 15; var y = 3; var a = x * y; var b = x / y; print(a); print(b);`
    [Fact]
    public void ArithmeticOperations_InStatements()
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

    // Выражения со скобками: `var result = (10 + 5) * 2; print(result);`
    [Fact]
    public void Expressions_WithParentheses()
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

    // Возведение в степень в инструкции: `var base = 2; var poow = 8; var power = base ^ poow; print(power);`
    [Fact]
    public void PowerOperation_InStatement()
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

    // Операции сравнения: `var a = 10; var b = 5; print(a > b); print(a < b); print(a == b);`
    [Fact]
    public void ComparisonOperations()
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
        Assert.Equal(1.0, env.Results[0]);
        Assert.Equal(0.0, env.Results[1]);
        Assert.Equal(0.0, env.Results[2]);
    }

    // Логические операции: `var x = 1; var y = 0; print(x && y); print(x || y);`
    [Fact]
    public void LogicalOperations()
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

    // Целочисленное деление и остаток: `var a = 17; var b = 5; var c = a // b; var d = a % b; print(c); print(d);`
    [Fact]
    public void IntegerDivision_AndModulo()
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

    // Обработка ошибок

    // Деление на ноль: `var x = 1; print(x / 0);` => DivideByZeroException
    [Fact]
    public void Division_ByZero_Throws()
    {
        FakeEnvironment env = new();
        string code = """
            var x = 1;
            print(x / 0);
            """;

        Parser parser = new(code, env);
        Assert.Throws<DivideByZeroException>(() => parser.ParseProgram());
    }

    // Целочисленное деление на ноль: `var x = 5; print(x // 0);` => DivideByZeroException
    [Fact]
    public void IntegerDivision_ByZero_Throws()
    {
        FakeEnvironment env = new();
        string code = """
            var x = 5;
            print(x // 0);
            """;

        Parser parser = new(code, env);
        Assert.Throws<DivideByZeroException>(() => parser.ParseProgram());
    }

    // Остаток от деления на ноль: `var x = 5; print(x % 0);` => DivideByZeroException
    [Fact]
    public void Modulo_ByZero_Throws()
    {
        FakeEnvironment env = new();
        string code = """
            var x = 5;
            print(x % 0);
            """;

        Parser parser = new(code, env);
        Assert.Throws<DivideByZeroException>(() => parser.ParseProgram());
    }

    // Использование необъявленной переменной (ошибка): `print(x);` => Exception
    [Fact]
    public void Using_UndeclaredVariable_Throws()
    {
        FakeEnvironment env = new();
        string code = """
            print(x);
            """;

        Parser parser = new(code, env);
        Assert.Throws<Exception>(() => parser.ParseProgram());
    }
}
