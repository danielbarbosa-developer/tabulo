namespace Tabulo.Generator.Parsers;

public static class BoolParser
{
    public static string GenerateParseCode()
    {
        return """
                   // CsvBoolValuesAttribute opcional:
                   // [CsvBoolValues(TrueValue = "Y", FalseValue = "N")]
                   private static bool TryParseBool(ReadOnlySpan<char> span, out bool value, string? trueValue = null, string? falseValue = null)
                   {
                       value = false;

                       // fallback
                       if (trueValue == null) trueValue = "true";
                       if (falseValue == null) falseValue = "false";

                       if (span.Equals(trueValue.AsSpan(), StringComparison.OrdinalIgnoreCase))
                       {
                           value = true;
                           return true;
                       }
                       if (span.Equals(falseValue.AsSpan(), StringComparison.OrdinalIgnoreCase))
                       {
                           value = false;
                           return true;
                       }

                       // fallback 1/0
                       if (trueValue == "true" && falseValue == "false" && span.Length == 1)
                       {
                           if (span[0] == '1')
                           {
                               value = true;
                               return true;
                           }
                           if (span[0] == '0')
                           {
                               value = false;
                               return true;
                           }
                       }

                       return false;
                   }
               """;
    }
}