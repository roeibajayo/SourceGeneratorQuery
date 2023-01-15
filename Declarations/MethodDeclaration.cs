using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceGeneratorBuilder.Declarations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SourceGeneratorBuilder.Declarations
{
    public class MethodDeclaration
    {
        public MethodDeclaration(BaseMethodDeclarationSyntax node)
        {
            this.node = node;
        }

        private readonly BaseMethodDeclarationSyntax node;

        public bool IsPublic
        {
            get
            {
                return Modifiers.Contains("public");
            }
            //set
            //{
            //    var publicModifiers = node.Modifiers.FirstOrDefault(x => x.Text == "public");
            //    if (publicModifiers != null)
            //    {                    
            //        return node.WithModifiers(node.Modifiers.Remove(publicModifiers));
            //    }
            //    else
            //    {
            //        return node;
            //    }
            //}
        }
        public bool IsProtected =>
            Modifiers.Contains("protected");
        public bool IsInternal =>
            Modifiers.Contains("internal");
        public bool IsPrivate => Modifiers.Contains("private") ||
            !IsPublic && !IsProtected && !IsInternal;
        public bool IsReadonly =>
            Modifiers.Contains("readonly");
        public bool IsStatic =>
            Modifiers.Contains("static");
        public bool IsAbstract =>
            Modifiers.Contains("abstract");
        public bool IsAsync =>
            Modifiers.Contains("async");
        public IEnumerable<string> Modifiers =>
            node.Modifiers.Select(x => x.Text);

        public string Name
        {
            get
            {
                switch (node)
                {
                    case MethodDeclarationSyntax method:
                        return method.Identifier.ToString();
                }
                switch (node)
                {
                    case ConstructorDeclarationSyntax method:
                        return method.Identifier.ToString();
                }

                throw new ArgumentNullException();
            }
            set
            {
                switch (node)
                {
                    case MethodDeclarationSyntax method:
                        method.WithIdentifier(SyntaxFactory.Identifier(""));
                        break;
                }
                throw new NotImplementedException();
            }
        }
        public string ReturnType
        {
            get
            {
                switch (node)
                {
                    case MethodDeclarationSyntax method:
                        return method.ReturnType.ToString();
                }
                return "void";
            }
        }
        public bool ReturnTypeIsNullable
        {
            get
            {
                switch (node)
                {
                    case MethodDeclarationSyntax method:
                        return method.ReturnType.IsNotNull;
                }
                return false;
            }
        }
        public IEnumerable<AttributeDeclaration> Attributes => node.AttributeLists
            .SelectMany(x => x.Attributes.Select(a => new AttributeDeclaration(a)));
        public IEnumerable<ParameterDeclaration> Parameters =>
            node.ParameterList.Parameters.Select(p => new ParameterDeclaration(p));
        public string Body =>
            node.Body.GetText().ToString();
    }
}
