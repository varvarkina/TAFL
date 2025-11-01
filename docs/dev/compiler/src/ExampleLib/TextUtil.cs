using System.Text;

namespace ExampleLib;

public struct RgbColor
{
    public RgbColor(byte red, byte green, byte blue)
    {
        Red = red;
        Green = green;
        Blue = blue;
    }

    public byte Red { get; set; }

    public byte Green { get; set; }

    public byte Blue { get; set; }
}

public static class TextUtil
{
    // Символы Unicode, которые мы принимаем как дефис.
    private static readonly Rune[] Hyphens = [new Rune('‐'), new Rune('-')];

    // Символы Unicode, которые мы принимаем как апостроф.
    private static readonly Rune[] Apostrophes = [new Rune('\''), new Rune('`')];

    // Состояния распознавателя слов.
    private enum WordState
    {
        NoWord,
        Letter,
        Hyphen,
        Apostrophe,
    }

    /// <summary>
    ///  Распознаёт слова в тексте. Поддерживает Unicode, в том числе английский и русский языки.
    ///  Слово состоит из букв, может содержать дефис в середине и апостроф в середине либо в конце.
    /// </summary>
    /// <remarks>
    ///  Функция использует автомат-распознаватель с четырьмя состояниями:
    ///   1. NoWord — автомат находится вне слова;
    ///   2. Letter — автомат находится в буквенной части слова;
    ///   3. Hyphen — автомат обработал дефис;
    ///   4. Apostrophe — автомат обработал апостроф.
    ///
    ///  Переходы между состояниями:
    ///   - NoWord → Letter — при получении буквы;
    ///   - Letter → Hyphen — при получении дефиса;
    ///   - Letter → Apostrophe — при получении апострофа;
    ///   - Letter → NoWord — при получении любого символа, кроме буквы, дефиса или апострофа;
    ///   - Hyphen → Letter — при получении буквы;
    ///   - Hyphen → NoWord — при получении любого символа, кроме буквы;
    ///   - Apostrophe → Letter — при получении буквы;
    ///   - Apostrophe → NoWord — при получении любого символа, кроме буквы.
    ///
    ///  Разница между состояниями Hyphen и Apostrophe в том, что дефис не может стоять в конце слова.
    /// </remarks>
    public static List<string> ExtractWords(string text)
    {
        WordState state = WordState.NoWord;

        List<string> results = [];
        StringBuilder currentWord = new();
        foreach (Rune ch in text.EnumerateRunes())
        {
            switch (state)
            {
                case WordState.NoWord:
                    if (Rune.IsLetter(ch))
                    {
                        PushCurrentWord();
                        currentWord.Append(ch);
                        state = WordState.Letter;
                    }

                    break;

                case WordState.Letter:
                    if (Rune.IsLetter(ch))
                    {
                        currentWord.Append(ch);
                    }
                    else if (Hyphens.Contains(ch))
                    {
                        currentWord.Append(ch);
                        state = WordState.Hyphen;
                    }
                    else if (Apostrophes.Contains(ch))
                    {
                        currentWord.Append(ch);
                        state = WordState.Apostrophe;
                    }
                    else
                    {
                        state = WordState.NoWord;
                    }

                    break;

                case WordState.Hyphen:
                    if (Rune.IsLetter(ch))
                    {
                        currentWord.Append(ch);
                        state = WordState.Letter;
                    }
                    else
                    {
                        // Убираем дефис, которого не должно быть в конце слова.
                        currentWord.Remove(currentWord.Length - 1, 1);
                        state = WordState.NoWord;
                    }

                    break;

                case WordState.Apostrophe:
                    if (Rune.IsLetter(ch))
                    {
                        currentWord.Append(ch);
                        state = WordState.Letter;
                    }
                    else
                    {
                        state = WordState.NoWord;
                    }

                    break;
            }
        }

        PushCurrentWord();

        return results;

        void PushCurrentWord()
        {
            if (currentWord.Length > 0)
            {
                results.Add(currentWord.ToString());
                currentWord.Clear();
            }
        }
    }

    public static RgbColor ParseCssRbgColor(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            throw new ArgumentException("Строка не может быть пустой.", nameof(text));
        }

        if (text[0] != '#')
        {
            throw new ArgumentException("Цвет должен начинаться с символа #.", nameof(text));
        }

        if (text.Length != 4 && text.Length != 7)
        {
            throw new ArgumentException("Неверная длина HEX-цвета. Ожидается #RGB или #RRGGBB.", nameof(text));
        }

        string hexPart = text.Substring(1);

        foreach (char c in hexPart)
        {
            if (!IsHexDigit(c))
            {
                throw new ArgumentException($"Недопустимый символ '{c}' в HEX-цвете.", nameof(text));
            }
        }

        if (text.Length == 4)
        {
            byte red = (byte)(ParseHexDigit(hexPart[0]) * 16 + ParseHexDigit(hexPart[0]));
            byte green = (byte)(ParseHexDigit(hexPart[1]) * 16 + ParseHexDigit(hexPart[1]));
            byte blue = (byte)(ParseHexDigit(hexPart[2]) * 16 + ParseHexDigit(hexPart[2]));

            return new RgbColor(red, green, blue);
        }
        else // Формат #RRGGBB
        {
            byte red = (byte)(ParseHexDigit(hexPart[0]) * 16 + ParseHexDigit(hexPart[1]));
            byte green = (byte)(ParseHexDigit(hexPart[2]) * 16 + ParseHexDigit(hexPart[3]));
            byte blue = (byte)(ParseHexDigit(hexPart[4]) * 16 + ParseHexDigit(hexPart[5]));

            return new RgbColor(red, green, blue);
        }
    }

    private static bool IsHexDigit(char c)
    {
        return (c >= '0' && c <= '9') || (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f');
    }

    private static byte ParseHexDigit(char c)
    {
        if (c >= '0' && c <= '9')
        {
            return (byte)(c - '0');
        }
        else if (c >= 'A' && c <= 'F')
        {
            return (byte)(c - 'A' + 10);
        }
        else if (c >= 'a' && c <= 'f')
        {
            return (byte)(c - 'a' + 10);
        }
        else
        {
            throw new ArgumentException($"Недопустимый HEX-символ: {c}");
        }
    }
}