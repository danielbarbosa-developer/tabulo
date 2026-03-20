namespace Tabulo.UnitTests;

public class CsvReaderShould
{
    private string SampleCsv => 
        @"id,name,price
1,Notebook,3500.50
2,Mouse,150.00
3,Keyboard,250.00";

    [Fact]
    public void ParseLine_ShouldReturnCorrectObject()
    {
        var parser = new ProductDto();
        parser.SetColumnMap(new Dictionary<string, int> { ["id"] = 0, ["name"] = 1, ["price"] = 2 });

        var result = parser.ParseLine("4,Fone,300.75".AsSpan());

        Assert.Equal(4, result.Id);
        Assert.Equal("Fone", result.Name);
        Assert.Equal(300.75m, result.Price);
    }

    [Fact]
    public void ReadAll_ShouldStreamAllLines()
    {
        using var reader = new StringReader(SampleCsv);
        var csv = new CsvReader<ProductDto>(reader);

        var list = csv.ReadAll().ToList();

        Assert.Equal(3, list.Count);
        Assert.Equal("Notebook", list[0].Name);
        Assert.Equal("Mouse", list[1].Name);
        Assert.Equal("Keyboard", list[2].Name);
    }

}