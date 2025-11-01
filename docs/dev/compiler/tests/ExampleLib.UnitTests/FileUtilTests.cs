using ExampleLib.UnitTests.Helpers;

using Xunit;

namespace ExampleLib.UnitTests;

public class FileUtilTests
{
    [Fact]
    public void CanSortTextFile()
    {
        const string unsorted = """
                                Играют волны — ветер свищет,
                                И мачта гнется и скрыпит…
                                Увы! он счастия не ищет
                                И не от счастия бежит!
                                """;
        const string sorted = """
                              И мачта гнется и скрыпит…
                              И не от счастия бежит!
                              Играют волны — ветер свищет,
                              Увы! он счастия не ищет
                              """;

        using TempFile file = TempFile.Create(unsorted);
        FileUtil.SortFileLines(file.Path);

        string actual = File.ReadAllText(file.Path);
        Assert.Equal(sorted.Replace("\r\n", "\n"), actual);
    }

    [Fact]
    public void CanSortOneLineFile()
    {
        using TempFile file = TempFile.Create("Играют волны — ветер свищет,");
        FileUtil.SortFileLines(file.Path);

        string actual = File.ReadAllText(file.Path);
        Assert.Equal("Играют волны — ветер свищет,", actual);
    }

    [Fact]
    public void CanSortEmptyFile()
    {
        using TempFile file = TempFile.Create("");

        FileUtil.SortFileLines(file.Path);

        string actual = File.ReadAllText(file.Path);
        Assert.Equal("", actual);
    }

    [Fact]
    public void CanAddLineNumbersToMultiLineFile()
    {
        const string original = """
                              Играют волны — ветер свищет,
                              И мачта гнется и скрыпит…
                              Увы! он счастия не ищет
                              И не от счастия бежит!
                              """;
        const string expected = """
                               1. Играют волны — ветер свищет,
                               2. И мачта гнется и скрыпит…
                               3. Увы! он счастия не ищет
                               4. И не от счастия бежит!
                               """;

        using TempFile file = TempFile.Create(original);
        FileUtil.AddLineNumbers(file.Path);

        string actual = File.ReadAllText(file.Path);
        Assert.Equal(expected.Replace("\r\n", "\n"), actual);
    }

    [Fact]
    public void CanAddLineNumbersToSingleLineFile()
    {
        const string original = "Играют волны — ветер свищет,";
        const string expected = "1. Играют волны — ветер свищет,";

        using TempFile file = TempFile.Create(original);
        FileUtil.AddLineNumbers(file.Path);

        string actual = File.ReadAllText(file.Path);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CanAddLineNumbersToEmptyFile()
    {
        using TempFile file = TempFile.Create("");

        FileUtil.AddLineNumbers(file.Path);

        string actual = File.ReadAllText(file.Path);
        Assert.Equal("", actual);
    }
}