namespace Tabulo.UnitTests;

[CsvRecord]
public partial class ProductDto
{
    [CsvColumn("id")]
    public int Id { get; set; }

    [CsvColumn("name")]
    public string Name { get; set; }

    [CsvColumn("price")]
    public decimal Price { get; set; }

}