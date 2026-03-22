using BenchmarkDotNet.Attributes;
using System.IO;
using System.Linq;

namespace Tabulo.Benchmark;

[MemoryDiagnoser]
public class TabuloCsvReaderBenchmark
{
    private string tempCsv;
    private StreamReader textReader;
    private CsvReader<ProductDto> csvReader;

    [GlobalSetup]
    public void Setup()
    {
        var generator = new FakeCsvGenerator();
        tempCsv = generator.GenerateTempFile();

        textReader = new StreamReader(tempCsv); 
        csvReader = new CsvReader<ProductDto>(textReader);
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        textReader?.Dispose();

        var generator = new FakeCsvGenerator();
        generator.DeleteTempFile(tempCsv);
    }

    [Benchmark]
    public void ParseWithCustomParser()
    {
        textReader.BaseStream.Seek(0, SeekOrigin.Begin);
        textReader.DiscardBufferedData();

        csvReader = new CsvReader<ProductDto>(textReader);
        var records = csvReader.ReadAll().ToList();
    }
}