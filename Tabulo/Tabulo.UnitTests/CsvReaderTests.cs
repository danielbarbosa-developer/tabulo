using System.Text;
using AwesomeAssertions;

namespace Tabulo.UnitTests;

public class CsvReaderTests
{
    private string SampleCsv => 
        @"id,name,price
1,Notebook,3500.50
2,Mouse,150.00
3,Keyboard,250.00";
    
    private string FinancialCsv =>
        @"Id,AccountId,Description,Amount,FeeRate,IsCredit,TransactionDate
1,50001,Salary,5000.00,0.0,true,2025-03-01
2,50001,Netflix Subscription,-39.90,0.02,false,2025-03-05
3,50002,Freelance Project,1200.50,0.015,1,2025-03-10";
    
    [Fact]
    public void ReadAll_ShouldStreamAllLines()
    {
        using var reader = new StringReader(SampleCsv);
        var csv = new CsvReader<ProductDto>(reader);

        var list = csv.ReadAll().ToList();

        AssertSampleCsv(list);
    }

    private static void AssertSampleCsv(List<ProductDto> list)
    {
        list.Should().HaveCount(3);
        list.Select(x => x.Name).Should().ContainInOrder("Notebook", "Mouse", "Keyboard");
    }

    [Fact]
    public void ReadAll_ShouldParseFinancialTransactionsCorrectly()
    {
        using var reader = new StringReader(FinancialCsv);
        var csv = new CsvReader<FinancialTransactionDto>(reader);

        var list = csv.ReadAll().ToList();

        AssertFinancialCsv(list);
    }
    
    [Fact]
    public void ReadAllAsStream_ShouldStreamAllLines()
    {
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(SampleCsv));
        var csv = new CsvReader<ProductDto>(stream);

        var list = csv.ReadAsStream().ToList();

        AssertSampleCsv(list);
    }
    
    [Fact]
    public void ReadAllAsStream_ShouldParseFinancialTransactionsCorrectly()
    {
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(FinancialCsv));
        var csv = new CsvReader<FinancialTransactionDto>(stream);

        var list = csv.ReadAsStream().ToList();

        AssertFinancialCsv(list);
    }

    private static void AssertFinancialCsv(List<FinancialTransactionDto> list)
    {
        list.Should().HaveCount(3);

        list[0].Should().BeEquivalentTo(new
        {
            Id = 1,
            AccountId = 50001,
            Description = "Salary",
            Amount = 5000.00m,
            FeeRate = 0.0,
            IsCredit = true,
            TransactionDate = new DateTime(2025, 3, 1)
        });

        list[1].Should().BeEquivalentTo(new
        {
            Id = 2,
            AccountId = 50001,
            Description = "Netflix Subscription",
            Amount = -39.90m,
            FeeRate = 0.02,
            IsCredit = false,
            TransactionDate = new DateTime(2025, 3, 5)
        });

        list[2].Should().BeEquivalentTo(new
        {
            Id = 3,
            AccountId = 50002,
            Description = "Freelance Project",
            Amount = 1200.50m,
            FeeRate = 0.015,
            IsCredit = true,
            TransactionDate = new DateTime(2025, 3, 10)
        });
    }
}