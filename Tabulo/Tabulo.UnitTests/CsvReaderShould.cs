namespace Tabulo.UnitTests;

public class CsvReaderShould
{
    private string SampleCsv => 
        @"id,name,price
1,Notebook,3500.50
2,Mouse,150.00
3,Keyboard,250.00";
    
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