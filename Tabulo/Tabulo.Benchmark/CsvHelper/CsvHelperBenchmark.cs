

using BenchmarkDotNet.Attributes;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace Tabulo.Benchmark;

[MemoryDiagnoser]
public class CsvHelperBenchmark
{
    private string csvFile;

    [GlobalSetup]
    public void Setup()
    {
        var generator = new FakeCsvGenerator();
        csvFile = generator.GenerateTempFile();
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        var generator = new FakeCsvGenerator();
        generator.DeleteTempFile(csvFile);
    }

    [Benchmark]
    public void ParseWithCsvHelper()
    {
        using var reader = new StreamReader(csvFile);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        csv.Context.RegisterClassMap<ProductDtoMap>();

        var records = csv.GetRecords<ProductDto>().ToList();
    }
}

public sealed class ProductDtoMap : ClassMap<ProductDto>
{
    public ProductDtoMap()
    {
        Map(m => m.Id).Name("ProductId");
        Map(m => m.Name).Name("ProductName");
        Map(m => m.Price).Name("ProductPrice");
        Map(m => m.InStock).Name("InStockFlag");
        Map(m => m.CreatedAt).Name("CreatedAt");
    }
}