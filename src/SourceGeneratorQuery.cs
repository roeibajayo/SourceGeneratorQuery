using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace SourceGeneratorQuery
{
    public class SourceGeneratorQuery
    {
        private readonly GeneratorExecutionContext context;
        private readonly string entryPath;

        public SourceGeneratorQuery(GeneratorExecutionContext context)
        {
            this.context = context;
            var objPath = context.Compilation.Assembly.Locations.Last().SourceTree.FilePath;
            entryPath = objPath.Substring(0, objPath.IndexOf("\\obj\\"));
        }

        public IEnumerable<SourceFile> NewQuery()
        {
            return context.Compilation.SyntaxTrees
                .Select(x => new SourceFile(x, entryPath));
        }
    }

    public static class SourceGeneratorQueryExtentions
    {
        public static IEnumerable<SourceFile> NewQuery(this GeneratorExecutionContext context) =>
            new SourceGeneratorQuery(context).NewQuery();
    }
}
