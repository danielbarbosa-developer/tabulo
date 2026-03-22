namespace Tabulo.UnitTests;

public class CsvReaderShould
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

        Assert.Equal(3, list.Count);
        Assert.Equal("Notebook", list[0].Name);
        Assert.Equal("Mouse", list[1].Name);
        Assert.Equal("Keyboard", list[2].Name);
    }
    
    [Fact]
    public void ReadAll_ShouldParseFinancialTransactionsCorrectly()
    {
        using var reader = new StringReader(FinancialCsv);
        var csv = new CsvReader<FinancialTransactionDto>(reader);

        var list = csv.ReadAll().ToList();

        Assert.Equal(3, list.Count);

        var first = list[0];

        Assert.Equal(1, first.Id);
        Assert.Equal(50001, first.AccountId);
        Assert.Equal("Salary", first.Description);
        Assert.Equal(5000.00m, first.Amount);
        Assert.Equal(0.0, first.FeeRate);
        Assert.True(first.IsCredit);
        Assert.Equal(new DateTime(2025, 3, 1), first.TransactionDate);

        var second = list[1];

        Assert.Equal(2, second.Id);
        Assert.Equal(50001, second.AccountId);
        Assert.Equal("Netflix Subscription", second.Description);
        Assert.Equal(-39.90m, second.Amount);
        Assert.Equal(0.02, second.FeeRate);
        Assert.False(second.IsCredit);
        Assert.Equal(new DateTime(2025, 3, 5), second.TransactionDate);

        var third = list[2];

        Assert.Equal(3, third.Id);
        Assert.Equal(50002, third.AccountId);
        Assert.Equal("Freelance Project", third.Description);
        Assert.Equal(1200.50m, third.Amount);
        Assert.Equal(0.015, third.FeeRate);
        Assert.True(third.IsCredit); // "1"
        Assert.Equal(new DateTime(2025, 3, 10), third.TransactionDate);
    }

}