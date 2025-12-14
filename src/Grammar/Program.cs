using System;
using Grammar;

var validator = new DeaExpressionGrammarValidator();

Console.WriteLine("DEA Expression Grammar Validator");
Console.WriteLine("Type 'exit' to quit\n");

while (true)
{
    Console.Write("> ");
    string? input = Console.ReadLine();

    if (input == null || input.Equals("exit", StringComparison.OrdinalIgnoreCase))
    {
        break;
    }

    if (string.IsNullOrWhiteSpace(input))
    {
        continue;
    }

    var result = validator.ValidateText(input);

    if (result.IsValid)
    {
        Console.WriteLine("✓ Valid ✓\n");
    }
    else
    {
        Console.WriteLine("✗ Invalid ✗");
        foreach (var error in result.Errors)
        {
            Console.WriteLine($"  {error.Line}:{error.Column} {error.Message}");
        }
        Console.WriteLine();
    }
}

Console.WriteLine("Goodbye!");
