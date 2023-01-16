using Microsoft.CodeAnalysis;
using SourceGeneratorQuery.Declarations;
using System.Diagnostics;
using System.Linq;

namespace SourceGeneratorQuery.examples
{
    [Generator]
    public class GetAllEnums : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            var enums = context
                .NewQuery() // start new query
                .GetTypes(subTypes: true) // get all classes, interface, etc..
                .GetEnums(subEnums: true); // get enums

            var syntaxNodes = enums.Select(x => x.SyntaxNode);
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
