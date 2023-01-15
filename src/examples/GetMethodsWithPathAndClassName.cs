using Microsoft.CodeAnalysis;
using SourceGeneratorBuilder;
using SourceGeneratorBuilder.Declarations;
using System.Diagnostics;
using System.Linq;

namespace SourceGeneratorQuery.examples
{
    /// <summary>
    /// Get all classes which ends with 'Client', and located in './Social'
    /// </summary>
    [Generator]
    public class GetMethodsWithPathAndClassName : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            var classes = context
                .NewQuery()
                .WithPath("./Social")
                .GetClasses()
                .WithName(x => x.EndsWith("Client"));

            foreach (var c in classes)
            {
                var methods = c.GetMethods()
                            .Where(x => x.IsPublic && (x.Name.StartsWith("Push") || x.Name.StartsWith("Get")));

                var stringify = methods
                    .Select(x => $"{string.Join(" ", x.Modifiers)} {x.Name}({string.Join(", ", x.Parameters.Select(p => $"{p.Type} {p.Name}"))})")
                    .ToArray();
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
#if DEBUG
            if (!Debugger.IsAttached)
            {
                Debugger.Launch();
            }
#endif
        }
    }
}
