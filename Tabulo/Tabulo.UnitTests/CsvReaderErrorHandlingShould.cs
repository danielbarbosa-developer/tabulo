namespace Tabulo.UnitTests;

public class CsvReaderErrorHandlingShould
{
    private string InvalidCsv =>
        @"id,name,price
1,Notebook,3500.50
INVALID_LINE
3,Keyboard,250.00";

    [Fact]
    public void SkipInvalidLinesWhenSkipInvalidTrue()
    {
        using var reader = new StringReader(InvalidCsv);
        var csv = new CsvReader<ProductDto>(reader);

        var list = csv.ReadAll(skipInvalid: true).ToList();

        Assert.Equal(2, list.Count);
        Assert.Equal("Notebook", list[0].Name);
        Assert.Equal("Keyboard", list[1].Name);
    }

    [Fact]
    public void ThrowExceptionWhenSkipInvalidFalse()
    {
        using var reader = new StringReader(InvalidCsv);
        var csv = new CsvReader<ProductDto>(reader);

        var ex = Assert.Throws<FormatException>(() =>
        {
            csv.ReadAll(skipInvalid: false).ToList();
        });

        Assert.Contains("INVALID_LINE", ex.Message);
    }
    
    [Fact]
    public void CallOnErrorForInvalidLinesWheCallbackDefined()
    {
        using var reader = new StringReader(InvalidCsv);
        var csv = new CsvReader<ProductDto>(reader);

        var errors = new List<string>();

        var list = csv.ReadAll(
            skipInvalid: true,
            onError: line => errors.Add(line.ToString())
        ).ToList();

        Assert.Equal(2, list.Count);
        Assert.Equal("Notebook", list[0].Name);
        Assert.Equal("Keyboard", list[1].Name);

        Assert.Single(errors);
        Assert.Equal("INVALID_LINE", errors[0]);
    }
}