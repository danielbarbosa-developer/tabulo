namespace Tabulo;

[AttributeUsage(AttributeTargets.Property)]
public class CsvColumnAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}