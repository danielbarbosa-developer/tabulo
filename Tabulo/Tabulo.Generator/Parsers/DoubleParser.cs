namespace Tabulo.Generator.Parsers;

public static class DoubleParser
{
    public static string GenerateParseCode()
    {
        return """
                   private static bool TryParseDouble(ReadOnlySpan<char> span, out double value)
                   {
                       value = 0;
                       if (span.Length == 0)
                           return false;

                       bool negative = false;
                       bool hasFraction = false;
                       double result = 0;
                       double fraction = 0;
                       double divisor = 1;
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
                               result = result * 10 + digit;
                           else
                           {
                               fraction = fraction * 10 + digit;
                               divisor *= 10;
                           }
                       }

                       result += fraction / divisor;
                       value = negative ? -result : result;
                       return true;
                   }
               """;
    }
}