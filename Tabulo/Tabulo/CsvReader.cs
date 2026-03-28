using System.Text;

namespace Tabulo;

using System.Collections.Generic;
using System.IO;

public class CsvReader<T> where T : ICsvParser<T>, new()
{
    private readonly TextReader _reader;
    private readonly Stream _stream;
    private readonly Encoding _encoding;
    private readonly int _bufferSize;

    public CsvReader(TextReader reader)
    {
        _reader = reader;
    }

    public CsvReader(Stream stream, Encoding? encoding = null, int bufferSize = 64 * 1024)
    {
        _stream = stream;
        _encoding = encoding ?? Encoding.UTF8;
        _bufferSize = bufferSize;
    }

    public IEnumerable<T> ReadAll(bool skipInvalid = true, Action<string>? onError = null )
        => CsvParser<T>.Instance.ParseStream(_reader, skipInvalid, onError);

    public IEnumerable<T> ReadAsStream(bool skipInvalid = true, Action<string>? onError = null)
    {
        using var reader = new StreamReader(_stream, _encoding, detectEncodingFromByteOrderMarks: true, bufferSize: _bufferSize, leaveOpen: true);

        foreach (var item in CsvParser<T>.Instance.ParseStream(reader, skipInvalid, onError))
            yield return item;
    }
}

public static class CsvParser<T> where T : ICsvParser<T>, new()
{
    public static ICsvParser<T> Instance { get; } = new T();
}