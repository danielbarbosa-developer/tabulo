// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using Tabulo.Benchmark;

Console.WriteLine("Starting benchmarks...");

var summaryTabulo = BenchmarkRunner.Run<TabuloCsvReaderBenchmark>();
Console.WriteLine("Finished Tabulo benchmarks.");
Console.WriteLine();

var summaryCsvHelper = BenchmarkRunner.Run<CsvHelperBenchmark>();
Console.WriteLine("Finished CsvHelper benchmarks.");
Console.WriteLine();

Console.WriteLine("All benchmarks completed.");