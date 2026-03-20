namespace Tabulo.Generator;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Text;
using System.Linq;

[Generator]
public class CsvSourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var classDeclarations = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (s, _) => s is ClassDeclarationSyntax,
                transform: static (ctx, _) => (ClassDeclarationSyntax)ctx.Node)
            .Where(static m => m is not null);

        var compilationAndClasses = context.CompilationProvider.Combine(classDeclarations.Collect());

        context.RegisterSourceOutput(compilationAndClasses, static (spc, source) => Execute(source.Left, source.Right, spc));
    }

    static void Execute(Compilation compilation, ImmutableArray<ClassDeclarationSyntax> classes, SourceProductionContext context)
    {
        foreach (var classDecl in classes)
        {
            var model = compilation.GetSemanticModel(classDecl.SyntaxTree);
            var symbol = model.GetDeclaredSymbol(classDecl) as INamedTypeSymbol;

            if (symbol == null)
                continue;

            var hasAttribute = symbol.GetAttributes()
                .Any(a => a.AttributeClass?.Name == "CsvRecordAttribute");

            if (!hasAttribute)
                continue;

            var className = symbol.Name;
            var namespaceName = symbol.ContainingNamespace.ToDisplayString();

            var properties = symbol.GetMembers()
                .OfType<IPropertySymbol>()
                .Select(p => new CsvProperty
                {
                    Name = p.Name,
                    Type = p.Type.ToDisplayString(),
                    ColumnName = GetColumnName(p)
                })
                .ToList();

            var source = GenerateParser(namespaceName, className, properties);

            context.AddSource($"{className}CsvParser.g.cs", source);
        }
    }

    static string? GetColumnName(IPropertySymbol property)
    {
        var attr = property.GetAttributes()
            .FirstOrDefault(a => a.AttributeClass?.Name == "CsvColumnAttribute");
        if (attr != null && attr.ConstructorArguments.Length > 0)
        {
            return attr.ConstructorArguments[0].Value as string;
        }
        return property.Name;
    }

   private static string GenerateParser(string namespaceName, string className, List<CsvProperty> props)
{
    var sb = new StringBuilder();
    
    sb.AppendLine($@"
using System;
using System.IO;
using System.Collections.Generic;
using Tabulo;

namespace {namespaceName}
{{
    public partial class {className} : ICsvParser<{className}>
    {{
");
    
    foreach (var p in props)
    {
        sb.AppendLine($"        private int _idx_{p.Name};");
    }

    sb.AppendLine($@"
        public {className} ParseLine(ReadOnlySpan<char> line)
        {{
            Span<Range> ranges = stackalloc Range[{props.Count + 4}];
            int count = Split(line, ranges);

            return new {className}
            {{");

    foreach (var p in props)
    {
        string parse = p.Type switch
        {
            "int" => $"ParseInt(line[ranges[_idx_{p.Name}]])",
            "decimal" => $"ParseDecimal(line[ranges[_idx_{p.Name}]])",
            _ => $"line[ranges[_idx_{p.Name}]].ToString()"
        };

        sb.AppendLine($"                {p.Name} = {parse},");
    }

    sb.AppendLine(@"
            };
        }");

    sb.AppendLine($@"
        public IEnumerable<{className}> ParseStream(TextReader reader)
        {{
            var header = reader.ReadLine();
            if (header == null) yield break;

            var span = header.AsSpan();
            Span<Range> ranges = stackalloc Range[{props.Count + 4}];
            int count = Split(span, ranges);

            for (int i = 0; i < count; i++)
            {{
                var col = span[ranges[i]].Trim();

");

    foreach (var p in props)
    {
        sb.AppendLine($@"
                if (col.SequenceEqual(""{p.ColumnName}"".AsSpan()))
                    _idx_{p.Name} = i;");
    }

    sb.AppendLine(@"
            }

            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                yield return ParseLine(line.AsSpan());
            }
        }");

    sb.AppendLine(@"
        private static int Split(ReadOnlySpan<char> line, Span<Range> ranges)
        {
            int count = 0;
            int start = 0;

            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == ',')
                {
                    ranges[count++] = start..i;
                    start = i + 1;
                }
            }

            ranges[count++] = start..line.Length;
            return count;
        }");

    sb.AppendLine(@"
        private static int ParseInt(ReadOnlySpan<char> span)
        {
            int value = 0;
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
        }");

    sb.AppendLine(@"
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
        }");

    sb.AppendLine(@"
    }
}");

    return sb.ToString();
}
}