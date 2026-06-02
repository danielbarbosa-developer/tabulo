using AwesomeAssertions;

namespace Tabulo.UnitTests;

public class CsvReaderQuoteSupportTests
{
    [Fact]
    public void ReadAll_ShouldSupportQuotedFields()
    {
        const string csvContent =
            """
            Id,Name,Description
            1,"John Doe","Senior Developer"
            2,"Jane Smith","Tech Lead"
            """;

        using var reader = new StringReader(csvContent);

        var csv = new CsvReader<QuotedProductDto>(reader);

        var list = csv.ReadAll().ToList();

        list.Should().HaveCount(2);

        list[0].Id.Should().Be(1);
        list[0].Name.Should().Be("John Doe");
        list[0].Description.Should().Be("Senior Developer");

        list[1].Id.Should().Be(2);
        list[1].Name.Should().Be("Jane Smith");
        list[1].Description.Should().Be("Tech Lead");
    }

    [Fact]
    public void ReadAll_ShouldSupportDelimiterInsideQuotes()
    {
        const string csvContent =
            """
            Id,Name,Description
            1,"John","hello,world"
            2,"Jane","a,b,c,d"
            """;

        using var reader = new StringReader(csvContent);

        var csv = new CsvReader<QuotedProductDto>(reader);

        var list = csv.ReadAll().ToList();

        list.Should().HaveCount(2);

        list[0].Description.Should().Be("hello,world");
        list[1].Description.Should().Be("a,b,c,d");
    }

    [Fact]
    public void ReadAll_ShouldSupportQuotedNumericValues()
    {
        const string csvContent =
            """
            Id,Name,Price
            "1","Notebook","3500.50"
            "2","Mouse","150.00"
            """;

        using var reader = new StringReader(csvContent);

        var csv = new CsvReader<QuotedPriceDto>(reader);

        var list = csv.ReadAll().ToList();

        list.Should().HaveCount(2);

        list[0].Id.Should().Be(1);
        list[0].Price.Should().Be(3500.50m);

        list[1].Id.Should().Be(2);
        list[1].Price.Should().Be(150.00m);
    }

    [Fact]
    public void ReadAll_ShouldSupportQuotedDateTimeValues()
    {
        const string csvContent =
            """
            Id,CreatedAt
            1,"2024-01-01"
            2,"2025-05-10"
            """;

        using var reader = new StringReader(csvContent);

        var csv = new CsvReader<QuotedDateDto>(reader);

        var list = csv.ReadAll().ToList();

        list.Should().HaveCount(2);

        list[0].CreatedAt.Should().Be(new DateTime(2024, 1, 1));
        list[1].CreatedAt.Should().Be(new DateTime(2025, 5, 10));
    }

    [Fact]
    public void ReadAll_ShouldNotBreakOnEscapedQuotes()
    {
        const string csvContent =
            @"Id,Description
1,""hello """"world""""""";

        using var reader = new StringReader(csvContent);

        var csv = new CsvReader<QuotedDescriptionDto>(reader);

        var list = csv.ReadAll().ToList();

        list.Should().HaveCount(1);
        list[0].Description.Should().Contain("world");
    }
}

[CsvRecord]
public partial class QuotedProductDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}

[CsvRecord]
public partial class QuotedPriceDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }
}

[CsvRecord]
public partial class QuotedDateDto
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; }
}

[CsvRecord]
public partial class QuotedDescriptionDto
{
    public int Id { get; set; }

    public string Description { get; set; } = string.Empty;
}