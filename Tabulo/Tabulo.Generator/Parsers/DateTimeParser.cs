namespace Tabulo.Generator.Parsers;

public static class DateTimeParser
{
    public static string GenerateParserCode()
    {
        return """

                       private static bool TryParseDateTime(ReadOnlySpan<char> span, out DateTime value)
                       {
                           return DateTime.TryParse(span, out value);
                       }
               """;
    }
}