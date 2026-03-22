namespace Tabulo.Generator.Parsers;

public static class BoolParser
{
    public static string GenerateParseCode()
    {
        return """

                       private static bool ParseBool(ReadOnlySpan<char> span)
                       {
                           // true / false
                           if (span.Length == 4 &&
                               (span[0] == 't' || span[0] == 'T') &&
                               (span[1] == 'r' || span[1] == 'R') &&
                               (span[2] == 'u' || span[2] == 'U') &&
                               (span[3] == 'e' || span[3] == 'E'))
                               return true;

                           if (span.Length == 5 &&
                               (span[0] == 'f' || span[0] == 'F') &&
                               (span[1] == 'a' || span[1] == 'A') &&
                               (span[2] == 'l' || span[2] == 'L') &&
                               (span[3] == 's' || span[3] == 'S') &&
                               (span[4] == 'e' || span[4] == 'E'))
                               return false;

                           // 1 / 0
                           if (span.Length == 1)
                           {
                               if (span[0] == '1') return true;
                               if (span[0] == '0') return false;
                           }

                           return false;
                       }
               """;
    }
}