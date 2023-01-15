using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SourceGeneratorBuilder.Declarations
{
    public class AttributeDeclaration
    {
        public AttributeDeclaration(AttributeSyntax node)
        {
            this.node = node;
        }

        private readonly AttributeSyntax node;

        public string Name =>
            node.Name.ToString();
        public IEnumerable<ArgumentDeclaration> Arguments =>
            node.ArgumentList?.Arguments.Select(a => new ArgumentDeclaration(a)) ??
            Array.Empty<ArgumentDeclaration>();
    }
}
