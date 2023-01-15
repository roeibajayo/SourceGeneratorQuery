using Microsoft.CodeAnalysis;
using SourceGeneratorBuilder;
using SourceGeneratorBuilder.Declarations;
using System.Diagnostics;

namespace SourceGeneratorQuery.examples
{
    [Generator]
    public class GetAllEnums : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            var enums = context
                .NewQuery()
                .GetTypes()
                .GetEnums();
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
