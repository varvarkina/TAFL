using System;
using System.Collections.Generic;
using Xunit;
using Interpreter;
using Parser;
using Execution;

namespace Interpreter.Specs;

public class InterpreterTests
{
    [Fact]
    public void Test_CircleSquare()
    {
        string code = @"
            const PI = 3.14159265359;
            var r;
            input(r);
            var area = PI * r ^ 2;
            print(area);
        ";

        // Input radius = 10
        var env = new FakeEnvironment(10.0);
        var interpreter = new Interpreter(env);
        interpreter.Execute(code);

        Assert.Single(env.Results);
        Assert.Equal(314.159265359, env.Results[0], 5);
    }

    [Fact]
    public void Test_FahrenheitToCelsius()
    {
        string code = @"
            var f;
            input(f);
            var c = (f - 32) * 5 / 9;
            print(c);
        ";

        var env = new FakeEnvironment(100.0);
        var interpreter = new Interpreter(env);
        interpreter.Execute(code);

        Assert.Single(env.Results);
        Assert.Equal(37.77777777, env.Results[0], 5);
    }

    [Fact]
    public void Test_SumNumbers()
    {
        string code = @"
            var a;
            input(a);
            var b;
            input(b);
            var sum = a + b;
            print(sum);
        ";

        var env = new FakeEnvironment(10.0, 20.0);
        var interpreter = new Interpreter(env);
        interpreter.Execute(code);

        Assert.Single(env.Results);
        Assert.Equal(30, env.Results[0]);
    }
}
