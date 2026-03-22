namespace Tabulo.Generator.Parsers;

public static class DecimalParser
{
    public static string GenerateParserCode()
    {
       return """

                      private static decimal ParseDecimal(ReadOnlySpan<char> span)
                      {
                          long integer = 0;
                          long fraction = 0;
                          long divisor = 1;
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
                                  integer = integer * 10 + digit;
                              }
                              else
                              {
                                  fraction = fraction * 10 + digit;
                                  divisor *= 10;
                              }
                          }

                          decimal result = integer;

                          if (hasFraction)
                              result += (decimal)fraction / divisor;

                          return negative ? -result : result;
                      }
              """;
    }
}