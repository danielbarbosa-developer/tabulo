<p align="center">
  <img src="./assets/tabulo-icon.png" width="200" />
</p>

<h1 align="center">Tabulo</h1>

<p align="center">
  Fast, friendly, ands efficient CSV parsing for .NET
</p>

<p align="center">
  <strong>Process large datasets without the pain.</strong>
</p>

---

## ✨ What is Tabulo?

Tabulo is a **high-performance CSV parser for .NET** built for developers who care about speed, memory, and clean APIs.

It uses **source generators** to create optimized parsers at compile time — meaning:
- no reflection
- minimal allocations
- maximum performance

Whether you're building ETL pipelines, microservices, or processing massive CSV files…  
**Tabulo is designed to stay fast and out of your way.**

---

> ⚠️ **Status: Beta**
>
> Tabulo is under active development. Things may change, improve, or break (hopefully not 😄).

---

## ⚡ Why Tabulo?

- 🚀 **Blazing fast** — built for performance-critical workloads  
- 🧠 **Zero reflection** — everything generated at compile time  
- 📦 **Low memory usage** — stream data without loading everything  
- 🧩 **Simple model mapping** using attributes  
- 🔍 **Span-based parsing** for maximum efficiency  

---

## 📊 Benchmark

Parsing CSV files with **Tabulo** compared to **CsvHelper**.

### Small CSV (5,000 rows)

| Method                | Mean       | Allocated Memory |
|---------------------- |-----------:|----------------:|
| ParseWithTabulo       | 576.7 µs   | 1.34 MB         |
| ParseWithCsvHelper    | 3.767 ms   | 2.17 MB         |

👉 **~6x faster than CsvHelper**, with less memory usage.

---

### Large CSV (50,000 – 100,000 rows)

#### Tabulo – High-performance parsing

| Method                  | Rows     | Mean       | Gen0   | Gen1   | Gen2   | Allocated Memory |
|-------------------------|---------:|-----------:|-------:|-------:|-------:|----------------:|
| ParseSync               | 50,000   | 4.214 ms   | 1031   | 250    | 39     | 12.53 MB        |
| ParseAsync              | 50,000   | 5.332 ms   | 1320   | 265    | 39     | 15.97 MB        |
| ParseSync + Linq Filter | 50,000   | 4.205 ms   | 1031   | 250    | 39     | 12.53 MB        |
| ParseAsync + Filter     | 50,000   | 5.339 ms   | 1320   | 265    | 39     | 15.97 MB        |
| ParseSync               | 100,000  | 8.136 ms   | 2062   | 250    | 46     | 24.90 MB        |
| ParseAsync              | 100,000  | 10.616 ms  | 2640   | 250    | 46     | 31.79 MB        |
| ParseSync + Linq Filter | 100,000  | 8.057 ms   | 2062   | 250    | 46     | 24.90 MB        |
| ParseAsync + Filter     | 100,000  | 10.385 ms  | 2640   | 250    | 46     | 31.79 MB        |

#### CsvHelper – Popular but heavier

| Method               | Rows     | Mean      | Gen0     | Gen1     | Gen2     | Allocated Memory |
|--------------------- |--------- |----------:|---------:|---------:|---------:|----------------:|
| ParseWithCsvHelper   | 50,000   | 41.25 ms  | 1917     | 1167     | 500      | 20.9 MB         |
| ParseWithCsvHelper   | 100,000  | 71.74 ms  | 3286     | 1857     | 857      | 41.74 MB        |

---

### 🚀 Key Takeaways

- **Tabulo is ~6–7x faster** than CsvHelper on large CSV files.
- Memory footprint is significantly lower, even with async pipelines.
- `ReadAsStreamAsync` allows **streaming large files efficiently** with cancellation support – ideal for ETL pipelines, APIs, or remote sources.
- `ReadAsStream()` provides **ultra-fast synchronous processing** for local CSV files.

## 🚀 Quick Start

Install via NuGet *(coming soon)*:

```bash
dotnet add package Tabulo
