using Microsoft.CodeAnalysis;
using SourceGeneratorQuery.Declarations;
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
                .NewQuery() // start new query
                .WithPath("./Social") // filter search on specific path
                .GetClasses()
                .WithName(x => x.EndsWith("Client")); // classes ends with 'Client'

            var syntaxNodes = classes.Select(x => x.SyntaxNode); // get the syntax nodes if you want

            // example of iteration
            foreach (var c in classes)
            {
                var publicMethodsWithMyAttributeAndStartsWithGet = c.GetMethods()
                            .WithAttribute("MyAttribute")
                            .WithPublic()
                            .WithName(name => name.StartsWith("Get"));

                var methodsSyntaxNodes = publicMethodsWithMyAttributeAndStartsWithGet.Select(x => x.SyntaxNode);

                var stringifyMethods = publicMethodsWithMyAttributeAndStartsWithGet
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
