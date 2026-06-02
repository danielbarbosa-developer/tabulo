using AwesomeAssertions;

namespace Tabulo.UnitTests;

public class CsvReaderNullableSupportTest
{
    [Fact]
    public void ReadAll_ShouldParse_AllNullableValues_WhenFilled()
    {
        const string csv =
            """
            Id,Price,Active,CreatedAt
            1,10.5,true,2024-01-01
            2,20.0,false,2025-05-10
            """;

        using var reader = new StringReader(csv);

        var csvReader = new CsvReader<NullableProductDto>(reader);

        var list = csvReader.ReadAll().ToList();

        list.Should().HaveCount(2);

        list[0].Id.Should().Be(1);
        list[0].Price.Should().Be(10.5m);
        list[0].Active.Should().BeTrue();
        list[0].CreatedAt.Should().Be(new DateTime(2024, 1, 1));

        list[1].Id.Should().Be(2);
        list[1].Price.Should().Be(20.0m);
        list[1].Active.Should().BeFalse();
        list[1].CreatedAt.Should().Be(new DateTime(2025, 5, 10));
    }

    [Fact]
    public void ReadAll_ShouldParse_AllNullValues_WhenEmpty()
    {
        const string csv =
            """
            Id,Price,Active,CreatedAt
            ,,,,
            """;

        using var reader = new StringReader(csv);

        var csvReader = new CsvReader<NullableProductDto>(reader);

        var list = csvReader.ReadAll().ToList();

        list.Should().HaveCount(1);

        list[0].Id.Should().BeNull();
        list[0].Price.Should().BeNull();
        list[0].Active.Should().BeNull();
        list[0].CreatedAt.Should().BeNull();
    }

    [Fact]
    public void ReadAll_ShouldParse_MixedNullableValues()
    {
        const string csv =
            """
            Id,Price,Active,CreatedAt
            1,,true,
            ,10.5,,2024-01-01
            """;

        using var reader = new StringReader(csv);

        var csvReader = new CsvReader<NullableProductDto>(reader);

        var list = csvReader.ReadAll().ToList();

        list.Should().HaveCount(2);

        list[0].Id.Should().Be(1);
        list[0].Price.Should().BeNull();
        list[0].Active.Should().BeTrue();
        list[0].CreatedAt.Should().BeNull();

        list[1].Id.Should().BeNull();
        list[1].Price.Should().Be(10.5m);
        list[1].Active.Should().BeNull();
        list[1].CreatedAt.Should().Be(new DateTime(2024, 1, 1));
    }
}

[CsvRecord]
public partial class NullableProductDto
{
    public int? Id { get; set; }

    public decimal? Price { get; set; }

    public bool? Active { get; set; }

    public DateTime? CreatedAt { get; set; }
}