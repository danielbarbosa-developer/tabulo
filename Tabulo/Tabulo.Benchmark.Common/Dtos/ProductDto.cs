namespace Tabulo.Benchmark
{
    [CsvRecord]
    public partial class ProductDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public bool InStock { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}