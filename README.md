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

Parsing **5,000 CSV rows** compared to CsvHelper:

| Method                | Mean       | Allocated Memory |
|----------------------|-----------:|----------------:|
| ParseWithTabulo      | 576.7 µs   | 1.34 MB         |
| ParseWithCsvHelper   | 3.767 ms   | 2.17 MB         |

👉 **~6x faster than CsvHelper**, with less memory usage.

---

## 🚀 Quick Start

Install via NuGet *(coming soon)*:

```bash
dotnet add package Tabulo
