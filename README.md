# Tabulo

**High Performance CSV parser for .NET**

Tabulo is an ultra-fast CSV parser for .NET, designed for **processing large volumes of data** with minimal memory allocation and without using reflection. It's perfect for high-performance scenarios, bulk CSV processing, ETL tasks, and microservices that require extremely fast CSV parsing.

> ⚠️ **Status:** Beta. The project is under active development. Some features are still incomplete, and improvements are planned.

---

## Features

- Automatically generated parser from `[CsvRecord]` and `[CsvColumn]` attributes on DTOs  
- Supports types: `int`, `long`, `decimal`, `double`, `bool`, `DateTime`, `string`  
- Zero reflection when parsing lines  
- Direct processing from `ReadOnlySpan<char>` for maximum performance  
- Handles large CSVs without unnecessary memory overhead  

---

## Benchmark

Benchmark using **5,000 CSV lines** comparing Tabulo with **CsvHelper**:

| Method                | Mean       | Allocated Memory |
|---------------------- |-----------:|----------------:|
| ParseWithTabulo | 576.7 µs   | 1.34 MB         |
| ParseWithCsvHelper    | 3.767 ms   | 2.17 MB         |

> Tabulo is **~6x faster than CsvHelper** in this benchmark scenario, with lower memory usage.

---

## Usage

1. Install the library via NuGet (coming soon):

```bash
dotnet add package Tabulo
