namespace Tabulo;

public interface ICsvParser<T>
{
    bool TryParseLine(ReadOnlySpan<char> line, out T result);
    IEnumerable<T> ParseStream(TextReader reader, bool skipInvalid = true, Action<string>? onError = null);
}