namespace Tabulo;

using System.Collections.Generic;
using System.IO;

public class CsvReader<T> where T : ICsvParser<T>, new()
{
    private readonly TextReader _reader;

    public CsvReader(TextReader reader)
    {
        _reader = reader;
    }

    public IEnumerable<T> ReadAll()
        => CsvParser<T>.Instance.ParseStream(_reader);

    public List<T> ReadToList()
        => new List<T>(ReadAll());
}

public static class CsvParser<T> where T : ICsvParser<T>, new()
{
    public static ICsvParser<T> Instance { get; } = new T();
}