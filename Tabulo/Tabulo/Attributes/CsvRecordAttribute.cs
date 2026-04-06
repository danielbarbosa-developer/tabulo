namespace Tabulo;

[AttributeUsage(AttributeTargets.Class)]
public class CsvRecordAttribute : Attribute
{
    public char Delimiter { get; set; } = ',';
}