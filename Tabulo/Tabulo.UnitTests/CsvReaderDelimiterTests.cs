using AwesomeAssertions;

namespace Tabulo.UnitTests;

public class CsvReaderDelimiterTests
{
    private string SampleCsv => 
        @"id;name;price
1;Notebook;3500.50
2;Mouse;150.00
3;Keyboard;250.00";
    
    [Fact]
    public void ReadAll_ShouldStreamAllLines()
    {
        using var reader = new StringReader(SampleCsv);
        var csv = new CsvReader<ProductWithCustomDelimiterDto>(reader);

        var list = csv.ReadAll().ToList();

        AssertSampleCsv(list);
    }
    
    private static void AssertSampleCsv(List<ProductWithCustomDelimiterDto> list)
    {
        list.Should().HaveCount(3);
        list.Select(x => x.Name).Should().ContainInOrder("Notebook", "Mouse", "Keyboard");
    }
}