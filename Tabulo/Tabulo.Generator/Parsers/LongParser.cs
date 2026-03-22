namespace Tabulo.Generator.Parsers;

public static class LongParser
{
    public static string GenerateParserCode()
    {
        return """

                       private static long ParseLong(ReadOnlySpan<char> span)
                       {
                           long value = 0;
                           bool negative = false;
                           int i = 0;

                           if (span.Length > 0 && span[0] == '-')
                           {
                               negative = true;
                               i = 1;
                           }

                           for (; i < span.Length; i++)
                           {
                               value = value * 10 + (span[i] - '0');
                           }

                           return negative ? -value : value;
                       }
               """;
    }
}