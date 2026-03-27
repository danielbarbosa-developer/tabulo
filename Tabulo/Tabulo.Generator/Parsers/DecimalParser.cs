namespace Tabulo.Generator.Parsers;

public static class DecimalParser
{
    public static string GenerateParserCode()
    {
        return """
                   private static bool TryParseDecimal(ReadOnlySpan<char> span, out decimal value)
                   {
                       value = 0;
                       if (span.Length == 0)
                           return false;

                       bool negative = false;
                       bool hasFraction = false;
                       long integer = 0;
                       long fraction = 0;
                       long divisor = 1;

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

                           if (c == '.')
                           {
                               if (hasFraction)
                                   return false;
                               hasFraction = true;
                               continue;
                           }

                           if (c < '0' || c > '9')
                               return false;

                           int digit = c - '0';

                           if (!hasFraction)
                               integer = integer * 10 + digit;
                           else
                           {
                               fraction = fraction * 10 + digit;
                               divisor *= 10;
                           }
                       }

                       decimal result = integer;
                       if (hasFraction)
                           result += (decimal)fraction / divisor;

                       value = negative ? -result : result;
                       return true;
                   }
               """;
    }
}