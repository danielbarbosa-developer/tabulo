namespace Tabulo;

public interface ICsvParser<T>
{
    T ParseLine(ReadOnlySpan<char> line);
    IEnumerable<T> ParseStream(TextReader reader);
}