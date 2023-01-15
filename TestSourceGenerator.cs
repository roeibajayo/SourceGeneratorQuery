using SourceGeneratorBuilder.Declarations;
using Microsoft.CodeAnalysis;
using System.Diagnostics;
using System.Linq;

namespace SourceGeneratorBuilder
{
    [Generator]
    public class TestSourceGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            var files = context
                .NewQuery()
                .WithPath("./Social")
                .GetClasses()
                    .WithName(x => x.EndsWith("Client"))
                    .First()
                    .GetMethods()
                        .Where(x => x.IsPublic && (x.Name.StartsWith("Push") || x.Name.StartsWith("Get")))
                        .ToArray();

            var methods = files
                .Select(x => $"{string.Join(" ", x.Modifiers)} {x.Name}({string.Join(", ", x.Parameters.Select(p => $"{p.Type} {p.Name}"))})")
                .ToArray();
        }

        public void Initialize(GeneratorInitializationContext context)
        {

#if DEBUG
            if (!Debugger.IsAttached)
            {
                Debugger.Launch();
            }
#endif

            // No initialization required for this one
        }
    }
}
