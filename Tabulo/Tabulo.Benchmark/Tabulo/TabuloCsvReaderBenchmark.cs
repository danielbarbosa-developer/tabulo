using BenchmarkDotNet.Attributes;

namespace Tabulo.Benchmark;

[MemoryDiagnoser]
public class TabuloCsvReaderBenchmark
{
    [Params(50_000, 100_000)] 
    public int RowCount { get; set; }

    private string tempCsv;
    private CsvReader<ProductDto> csvReader;

    [GlobalSetup]
    public void Setup()
    {
        var generator = new FakeCsvGenerator();
        tempCsv = generator.GenerateTempFile(RowCount);
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        var generator = new FakeCsvGenerator();
        generator.DeleteTempFile(tempCsv);
    }

    [Benchmark]
    public void ParseSync()
    {
        using var stream = new FileStream(tempCsv, FileMode.Open, FileAccess.Read, FileShare.Read);
        csvReader = new CsvReader<ProductDto>(stream);

        var records = csvReader.ReadAsStream().ToList();
    }

    [Benchmark]
    public async Task ParseAsync()
    {
        using var stream = new FileStream(tempCsv, FileMode.Open, FileAccess.Read, FileShare.Read);
        csvReader = new CsvReader<ProductDto>(stream);

        var processed = new List<ProductDto>();
        await foreach (var record in csvReader.ReadAsStreamAsync())
            processed.Add(record);
    }

    [Benchmark]
    public void ParseSync_LinqFilter()
    {
        using var stream = new FileStream(tempCsv, FileMode.Open, FileAccess.Read, FileShare.Read);
        csvReader = new CsvReader<ProductDto>(stream);

        var records = csvReader.ReadAsStream()
            .Where(r => r.Price > 100)
            .Select(r => new { r.Id, r.Name })
            .ToList();
    }

    [Benchmark]
    public async Task ParseAsync_Filter()
    {
        using var stream = new FileStream(tempCsv, FileMode.Open, FileAccess.Read, FileShare.Read);
        csvReader = new CsvReader<ProductDto>(stream);

        var processed = new List<(int Id, string Name)>();
        await foreach (var record in csvReader.ReadAsStreamAsync())
        {
            if (record.Price > 100)
                processed.Add((record.Id, record.Name));
        }
    }
}