using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceGeneratorQuery.Declarations
{
    public class ArgumentDeclaration
    {
        public ArgumentDeclaration(AttributeArgumentSyntax node)
        {
            this.SyntaxNode = node;
        }

        public readonly AttributeArgumentSyntax SyntaxNode;

        public string Expression =>
            SyntaxNode.Expression.ToString();
    }
}
