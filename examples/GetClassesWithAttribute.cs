using Microsoft.CodeAnalysis;
using SourceGeneratorBuilder;
using SourceGeneratorBuilder.Declarations;
using System.Diagnostics;
using System.Linq;

namespace SourceGeneratorQuery.examples
{
    [Generator]
    public class GetClassesWithAttribute : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            var files = context
                .NewQuery()
                .GetClasses()
                .WithAttribute("MyAttributeName") // return classes
                .Select(c => c.SourceFile); // return filepath, eg. "./My/Path/To/File"
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
