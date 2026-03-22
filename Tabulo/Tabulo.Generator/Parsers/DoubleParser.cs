namespace Tabulo.Generator.Parsers;

public static class DoubleParser
{
    public static string GenerateParseCode()
    {
        return """

                       private static double ParseDouble(ReadOnlySpan<char> span)
                       {
                           double result = 0;
                           double fraction = 0;
                           double divisor = 1;
                           bool negative = false;
                           bool hasFraction = false;

                           int i = 0;

                           if (span.Length > 0 && span[0] == '-')
                           {
                               negative = true;
                               i = 1;
                           }

                           for (; i < span.Length; i++)
                           {
                               var c = span[i];

                               if (c == '.')
                               {
                                   hasFraction = true;
                                   continue;
                               }

                               int digit = c - '0';

                               if (!hasFraction)
                               {
                                   result = result * 10 + digit;
                               }
                               else
                               {
                                   fraction = fraction * 10 + digit;
                                   divisor *= 10;
                               }
                           }

                           result += fraction / divisor;

                           return negative ? -result : result;
                       }
                       
               """;
    }
}