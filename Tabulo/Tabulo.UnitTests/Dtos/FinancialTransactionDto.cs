namespace Tabulo.UnitTests;

[CsvRecord]
public partial class FinancialTransactionDto
{
    public int Id { get; set; }
    public long AccountId { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public double FeeRate { get; set; }
    public bool IsCredit { get; set; }
    public DateTime TransactionDate { get; set; }
}