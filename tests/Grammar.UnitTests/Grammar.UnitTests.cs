using Grammar;
using Xunit;

namespace Grammar.UnitTests;

public sealed class GrammarValidatorTests
{
    private readonly DeaExpressionGrammarValidator _v = new();

    public static TheoryData<string> Valid() => new()
    {
        // Числовые литералы
        "2025",
        "3.14",
        "0",
        "123.456",

        // Унарные
        "-5",
        "-3.14",
        "+5",
        "+42",
        "!1",
        "!0",

        // Степень
        "2 ^ 3",
        "2 ^ 3 ^ 2",
        "(-2) ^ 3",
        "4 ^ 0.5",

        // Мультипликативные
        "2 * 3",
        "6 / 2",
        "7 // 2",
        "7 % 3",

        // Деление на ноль — синтаксически валидно
        "5 / 0",
        "5 // 0",
        "5 % 0",

        // Аддитивные
        "1 + 2",
        "5 - 3",

        // Сравнения
        "3 < 5",
        "5 < 5",
        "6 < 5",
        "3 > 5",
        "5 > 5",
        "6 > 5",
        "3 <= 5",
        "5 <= 5",
        "6 <= 5",
        "3 >= 5",
        "5 >= 5",
        "6 >= 5",

        // Равенство/неравенство (числа)
        "5 == 5",
        "5 == 3",
        "5 != 3",
        "5 != 5",

        // Логические
        "1 && 1",
        "1 && 0",
        "0 && 1",
        "0 && 0",
        "1 || 1",
        "1 || 0",
        "0 || 1",
        "0 || 0",

        // Приоритет/ассоциативность
        "10 - 3 - 2",
        "12 / 3 / 2",
        "-3 + 2",
        "2 ^ 3 * 2",
        "2 * 3 ^ 2",
        "2 + 3 * 4",
        "3 + 2 * 3",
        "1 + 2 > 2",
        "1 < 2 == 5 > 4",

        // Скобки
        "(1 + 2) * 3",
        "2 * (3 + 4)",
        "((1 + 2) * 3) + 4",

        // Функции
        "abs(-5)",
        "abs(5)",
        "abs(-3.14)",
        "min(7, 3, 5)",
        "min(1)",
        "min(5, 2)",
        "max(2, 8, 4)",
        "max(1)",
        "max(5, 2)",

        // Комбинированные
        "abs(-5) + 3",
        "min(10, 20) * 2",
        "max(1, 2, 3) ^ 2",
        "(1 + 2) * abs(-3)",
    };

    public static TheoryData<string> Invalid() => new()
    {
        // Строки: в спецификации аналитика нет строковых литералов в выражениях
        "\"hello\"",
        "\"\"",
        "\"hello\" + \"world\"",
        "\"test\" + \"\"",
        "\"hello\" == \"hello\"",
        "\"hello\" == \"world\"",
        "\"hello\" != \"world\"",
        "1 + \"hello\"",
        "\"world\" - 5",
        "\"test\" * 2",
        "\"abc\" / 3",

        // Ошибочные вызовы функций (пустые аргументы / дырка)
        "abs()",
        "min()",
        "max()",
        "min(1, )",
    };

    [Theory]
    [MemberData(nameof(Valid))]
    public void Valid_inputs_pass(string code)
    {
        var res = _v.ValidateText(code);
        Assert.True(res.IsValid, string.Join("\n", res.Errors.Select(e => $"{e.Line}:{e.Column} {e.Message}")));
    }

    [Theory]
    [MemberData(nameof(Invalid))]
    public void Invalid_inputs_fail(string code)
    {
        var res = _v.ValidateText(code);
        Assert.False(res.IsValid);
        Assert.NotEmpty(res.Errors);
    }
}