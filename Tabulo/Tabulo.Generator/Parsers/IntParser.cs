namespace Tabulo.Generator.Parsers;

public static class IntParser
{
    public static string GenerateParserCode()
    {
        return """
                   private static bool TryParseInt(ReadOnlySpan<char> span, out int value)
                   {
                       value = 0;
                       if (span.Length == 0)
                           return false;

                       bool negative = false;
                       int i = 0;

                       if (span[0] == '-')
                       {
                           negative = true;
                           i = 1;
                           if (span.Length == 1) 
                               return false;
                       }

                       for (; i < span.Length; i++)
                       {
                           char c = span[i];
                           if (c < '0' || c > '9')
                               return false;
                           value = value * 10 + (c - '0');
                       }

                       if (negative)
                           value = -value;

                       return true;
                   }
               """;
    }
}