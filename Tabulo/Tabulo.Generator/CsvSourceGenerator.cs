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
        return property.Name; // fallback to property name
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
        private readonly Dictionary<string, int> _columnMap = new();

        public void SetColumnMap(Dictionary<string, int> map)
        {{
            _columnMap.Clear();
            foreach (var kvp in map) _columnMap[kvp.Key] = kvp.Value;
        }}

        public {className} ParseLine(ReadOnlySpan<char> line)
        {{
            var parts = line.ToString().Split(',');

            return new {className}
            {{");

        for (int i = 0; i < props.Count; i++)
        {
            var p = props[i];
            string parse = p.Type switch
            {
                "int" => $"int.Parse(parts[_columnMap[\"{p.ColumnName}\"]])",
                "decimal" => $"decimal.Parse(parts[_columnMap[\"{p.ColumnName}\"]], System.Globalization.CultureInfo.InvariantCulture)",
                _ => $"parts[_columnMap[\"{p.ColumnName}\"]]"
            };

            sb.AppendLine($"                {p.Name} = {parse},");
        }

        sb.AppendLine(@"
            };
        }

        public IEnumerable<" + className + @"> ParseStream(TextReader reader)
        {
            string? headerLine = reader.ReadLine();
            if (headerLine == null) yield break;

            var headers = headerLine.Split(',');
            for (int i = 0; i < headers.Length; i++)
            {
                _columnMap[headers[i].Trim()] = i;
            }

            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                yield return ParseLine(line.AsSpan());
            }
        }
    }
}");

        return sb.ToString();
    }
}