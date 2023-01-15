using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceGeneratorBuilder.Declarations
{
    public class ArgumentDeclaration
    {
        public ArgumentDeclaration(AttributeArgumentSyntax node)
        {
            this.node = node;
        }

        private readonly AttributeArgumentSyntax node;

        public string Expression =>
            node.Expression.ToString();
    }
}
