using Xunit;

namespace ExampleLib.UnitTests;

public class TextUtilTest
{
    [Fact]
    public void Can_extract_russian_words()
    {
        const string text = """
                            Играют волны — ветер свищет,
                            И мачта гнётся и скрыпит…
                            Увы! он счастия не ищет
                            И не от счастия бежит!
                            """;
        List<string> expected =
        [
            "Играют",
            "волны",
            "ветер",
            "свищет",
            "И",
            "мачта",
            "гнётся",
            "и",
            "скрыпит",
            "Увы",
            "он",
            "счастия",
            "не",
            "ищет",
            "И",
            "не",
            "от",
            "счастия",
            "бежит",
        ];

        List<string> actual = TextUtil.ExtractWords(text);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Can_extract_words_with_hyphens()
    {
        const string text = "Что-нибудь да как-нибудь, и +/- что- то ещё";
        List<string> expected =
        [
            "Что-нибудь",
            "да",
            "как-нибудь",
            "и",
            "что",
            "то",
            "ещё",
        ];

        List<string> actual = TextUtil.ExtractWords(text);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Can_extract_words_with_apostrophes()
    {
        const string text = "Children's toys and three cats' toys";
        List<string> expected =
        [
            "Children's",
            "toys",
            "and",
            "three",
            "cats'",
            "toys",
        ];

        List<string> actual = TextUtil.ExtractWords(text);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Can_extract_words_with_grave_accent()
    {
        const string text = "Children`s toys and three cats` toys, all of''them are green";
        List<string> expected =
        [
            "Children`s",
            "toys",
            "and",
            "three",
            "cats`",
            "toys",
            "all",
            "of'",
            "them",
            "are",
            "green",
        ];

        List<string> actual = TextUtil.ExtractWords(text);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Can_parse_6_digit_hex_color()
    {
        RgbColor color = TextUtil.ParseCssRbgColor("#00a400");
        Assert.Equal(0, color.Red);
        Assert.Equal(164, color.Green);
        Assert.Equal(0, color.Blue);
    }

    [Fact]
    public void Can_parse_6_digit_hex_color_uppercase()
    {
        RgbColor color = TextUtil.ParseCssRbgColor("#FF00AA");
        Assert.Equal(255, color.Red);
        Assert.Equal(0, color.Green);
        Assert.Equal(170, color.Blue);
    }

    [Fact]
    public void Can_parse_3_digit_hex_color()
    {
        RgbColor color = TextUtil.ParseCssRbgColor("#fff");
        Assert.Equal(255, color.Red);
        Assert.Equal(255, color.Green);
        Assert.Equal(255, color.Blue);
    }

    [Fact]
    public void Can_parse_3_digit_hex_color_mixed_case()
    {
        RgbColor color = TextUtil.ParseCssRbgColor("#aBc");
        Assert.Equal(170, color.Red);  // aa
        Assert.Equal(187, color.Green); // bb
        Assert.Equal(204, color.Blue);  // cc
    }

    [Fact]
    public void Can_parse_black_color()
    {
        RgbColor color = TextUtil.ParseCssRbgColor("#000000");
        Assert.Equal(0, color.Red);
        Assert.Equal(0, color.Green);
        Assert.Equal(0, color.Blue);
    }

    [Fact]
    public void Can_parse_white_color()
    {
        RgbColor color = TextUtil.ParseCssRbgColor("#FFFFFF");
        Assert.Equal(255, color.Red);
        Assert.Equal(255, color.Green);
        Assert.Equal(255, color.Blue);
    }

    [Fact]
    public void Throws_exception_for_empty_string()
    {
        Assert.Throws<ArgumentException>(() => TextUtil.ParseCssRbgColor(""));
    }

    [Fact]
    public void Throws_exception_for_null_string()
    {
        Assert.Throws<ArgumentException>(() => TextUtil.ParseCssRbgColor(null!));
    }

    [Fact]
    public void Throws_exception_for_string_without_hash()
    {
        Assert.Throws<ArgumentException>(() => TextUtil.ParseCssRbgColor("FF00AA"));
    }

    [Fact]
    public void Throws_exception_for_wrong_length()
    {
        Assert.Throws<ArgumentException>(() => TextUtil.ParseCssRbgColor("#FF00"));
        Assert.Throws<ArgumentException>(() => TextUtil.ParseCssRbgColor("#FF00AAAA"));
    }

    [Fact]
    public void Throws_exception_for_invalid_hex_characters()
    {
        Assert.Throws<ArgumentException>(() => TextUtil.ParseCssRbgColor("#GG0000"));
        Assert.Throws<ArgumentException>(() => TextUtil.ParseCssRbgColor("#FF00ZZ"));
        Assert.Throws<ArgumentException>(() => TextUtil.ParseCssRbgColor("#123G"));
    }
}