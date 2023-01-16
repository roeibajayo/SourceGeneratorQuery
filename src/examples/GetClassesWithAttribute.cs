using Microsoft.CodeAnalysis;
using SourceGeneratorQuery.Declarations;
using System;
using System.Diagnostics;
using System.Linq;

namespace SourceGeneratorQuery.examples
{
    [Generator]
    public class GetClassesWithAttribute : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            var classes = context
                .NewQuery() // start new query
                .GetClasses(subClasses: true) // get all classes
                .WithAttribute("MyAttributeName"); // filter classes by attribute name

            var paths = classes.Select(c => c.SourceFile); // return filepath, eg. "./My/Path/To/File"

            var syntaxNodes = classes.Select(x => x.SyntaxNode);
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
