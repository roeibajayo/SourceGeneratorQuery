using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SourceGeneratorQuery.Declarations
{
    public class ParameterDeclaration
    {
        public ParameterDeclaration(ParameterSyntax node)
        {
            this.SyntaxNode = node;
        }
        public ParameterDeclaration(FieldDeclarationSyntax node)
        {
            this.SyntaxNode = node;
        }
        public ParameterDeclaration(PropertyDeclarationSyntax node)
        {
            this.SyntaxNode = node;
        }
        public ParameterDeclaration(MemberDeclarationSyntax node)
        {
            this.SyntaxNode = node;
        }
        public ParameterDeclaration(EnumDeclarationSyntax node)
        {
            this.SyntaxNode = node;
        }

        public readonly SyntaxNode SyntaxNode;

        public bool IsPublic => !(SyntaxNode is ParameterSyntax) &&
            Modifiers.Contains("public");
        public bool IsProtected => !(SyntaxNode is ParameterSyntax) &&
            Modifiers.Contains("protected");
        public bool IsInternal => !(SyntaxNode is ParameterSyntax) &&
            Modifiers.Contains("internal");
        public bool IsPrivate => !(SyntaxNode is ParameterSyntax) &&
            (Modifiers.Contains("private") ||
            !IsPublic && !IsProtected && !IsInternal);
        public bool IsReadonly => !(SyntaxNode is ParameterSyntax) &&
            Modifiers.Contains("readonly");
        public bool IsStatic => !(SyntaxNode is ParameterSyntax) &&
            Modifiers.Contains("static");
        public bool IsAbstract => !(SyntaxNode is ParameterSyntax) &&
            Modifiers.Contains("abstract");
        public IEnumerable<string> Modifiers
        {
            get
            {
                SyntaxTokenList modifiers = default;
                switch (SyntaxNode)
                {
                    case MemberDeclarationSyntax x:
                        modifiers = x.Modifiers;
                        break;
                    case ParameterSyntax x:
                        modifiers = x.Modifiers;
                        break;
                }
                return modifiers.Select(x => x.Text.ToLower());
            }
        }

        public IEnumerable<AttributeDeclaration> Attributes
        {
            get
            {
                SyntaxList<AttributeListSyntax> attributes = default;
                switch (SyntaxNode)
                {
                    case MemberDeclarationSyntax x:
                        attributes = x.AttributeLists;
                        break;
                    case ParameterSyntax x:
                        attributes = x.AttributeLists;
                        break;
                }
                return attributes
                    .SelectMany(x => x.Attributes.Select(a => new AttributeDeclaration(a)));
            }
        }
        public string Type
        {
            get
            {
                switch (SyntaxNode)
                {
                    case ParameterSyntax x:
                        return x.Type.ToString();
                    case FieldDeclarationSyntax x:
                        return x.Declaration.Type.ToString();
                    case PropertyDeclarationSyntax x:
                        return x.Type.ToString();
                    case EnumDeclarationSyntax x:
                        return x.Identifier.ToString();
                }

                throw new ArgumentException();
            }
        }
        public bool IsNullable
        {
            get
            {
                switch (SyntaxNode)
                {
                    case ParameterSyntax x:
                        return x.Type.IsNotNull;
                    case FieldDeclarationSyntax x:
                        return x.Declaration.Type.IsNotNull;
                    case PropertyDeclarationSyntax x:
                        return x.Type.IsNotNull;
                }

                return false;
            }
        }
        public string Name
        {
            get
            {
                switch (SyntaxNode)
                {
                    case ParameterSyntax x:
                        return x.Identifier.ToString();
                    case FieldDeclarationSyntax x:
                        return x.Declaration.Variables[0].Identifier.ToString();
                    case PropertyDeclarationSyntax x:
                        return x.Identifier.ToString();
                    case EnumDeclarationSyntax x:
                        return x.Identifier.ToString();
                }

                return null;
            }
        }
        public string DefaultValue
        {
            get
            {
                switch (SyntaxNode)
                {
                    case ParameterSyntax x:
                        return x.Default.Value?.ToString();
                    case FieldDeclarationSyntax x:
                        return x.Declaration.Variables[0].Initializer?.Value?.ToString();
                    case PropertyDeclarationSyntax x:
                        return x.Initializer?.Value?.ToString();
                }

                return null;
            }
        }
        public IEnumerable<KeyValuePair<string, string>> Values
        {
            get
            {
                switch (SyntaxNode)
                {
                    case EnumDeclarationSyntax x:
                        return x.Members.Select(m =>
                            new KeyValuePair<string, string>(m.Identifier.ToString(), m.EqualsValue?.Value.ToString()));
                }
                return Array.Empty<KeyValuePair<string, string>>();
            }
        }
    }
}
