namespace Tabulo.UnitTests;

[CsvRecord(Delimiter = ';')]
public partial class ProductWithCustomDelimiterDto
{
    [CsvColumn("id")]
    public int Id { get; set; }

    [CsvColumn("name")]
    public string Name { get; set; }

    [CsvColumn("price")]
    public decimal Price { get; set; }
}