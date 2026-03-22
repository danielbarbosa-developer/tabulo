namespace Tabulo.Generator.Parsers;

public static class DateTimeParser
{
    public static string GenerateParserCode()
    {
        return """

                       private static DateTime ParseDateTime(ReadOnlySpan<char> span)
                       {
                           try{
                               int year =
                                   (span[0] - '0') * 1000 +
                                   (span[1] - '0') * 100 +
                                   (span[2] - '0') * 10 +
                                   (span[3] - '0');

                               int month =
                                   (span[5] - '0') * 10 +
                                   (span[6] - '0');

                               int day =
                                   (span[8] - '0') * 10 +
                                   (span[9] - '0');

                               int hour =
                                   (span[11] - '0') * 10 +
                                   (span[12] - '0');

                               int minute =
                                   (span[14] - '0') * 10 +
                                   (span[15] - '0');

                               int second =
                                   (span[17] - '0') * 10 +
                                   (span[18] - '0');

                               return new DateTime(year, month, day, hour, minute, second, DateTimeKind.Unspecified);
                           }
                           catch(Exception ex) {
                               return DateTime.Parse(span);
                           }
                       }
               """;
    }
}