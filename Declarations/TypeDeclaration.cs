using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SourceGeneratorBuilder.Declarations
{
    public class TypeDeclaration
    {
        private readonly TypeDeclarationSyntax node;

        public TypeDeclaration(SourceFile sourceFile, TypeDeclarationSyntax node)
        {
            SourceFile = sourceFile;
            this.node = node;
        }

        public readonly SourceFile SourceFile;

        public bool IsPublic =>
            Modifiers.Contains("public");
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
        public IEnumerable<string> Modifiers =>
            node.Modifiers.Select(x => x.Text.ToLower());


        public IEnumerable<AttributeDeclaration> Attributes =>
            node.AttributeLists.SelectMany(x =>
                x.Attributes.Select(a => new AttributeDeclaration(a)));
        public string Name =>
            node.Identifier.Text;
        public IEnumerable<string> GetBaseTypes()
        {
            return node.BaseList?.Types
                .Select(x => x.Type.ToString()) ??
                Array.Empty<string>();
        }

        public IEnumerable<MethodDeclaration> GetConstructors()
        {
            return node.ChildNodes().OfType<ConstructorDeclarationSyntax>()
                .Select(y => new MethodDeclaration(y));
        }
        public IEnumerable<TypeDeclaration> GetClasses()
        {
            return node.ChildNodes().OfType<ClassDeclarationSyntax>()
                .Select(y => new TypeDeclaration(SourceFile, y));
        }
        public IEnumerable<TypeDeclaration> GetAllClasses()
        {
            return node.DescendantNodes().OfType<ClassDeclarationSyntax>()
                .Select(y => new TypeDeclaration(SourceFile, y));
        }
        public IEnumerable<ParameterDeclaration> GetFields()
        {
            return node.ChildNodes().OfType<FieldDeclarationSyntax>()
                .Select(y => new ParameterDeclaration(y));
        }
        public IEnumerable<ParameterDeclaration> GetProperties()
        {
            return node.ChildNodes().OfType<PropertyDeclarationSyntax>()
                .Select(y => new ParameterDeclaration(y));
        }
        public IEnumerable<MethodDeclaration> GetMethods()
        {
            return node.ChildNodes().OfType<MethodDeclarationSyntax>()
                .Select(y => new MethodDeclaration(y));
        }
        public IEnumerable<TypeDeclaration> GetInterfaces()
        {
            return node.ChildNodes().OfType<InterfaceDeclarationSyntax>()
                .Select(y => new TypeDeclaration(SourceFile, y));
        }
        public IEnumerable<TypeDeclaration> GetAllInterfaces()
        {
            return node.DescendantNodes().OfType<InterfaceDeclarationSyntax>()
                .Select(y => new TypeDeclaration(SourceFile, y));
        }
        public IEnumerable<TypeDeclaration> GetRecords()
        {
            return node.ChildNodes().OfType<RecordDeclarationSyntax>()
                .Select(y => new TypeDeclaration(SourceFile, y));
        }
        public IEnumerable<TypeDeclaration> GetAllRecords()
        {
            return node.DescendantNodes()
                .OfType<RecordDeclarationSyntax>()
                .Select(y => new TypeDeclaration(SourceFile, y));
        }
        public IEnumerable<TypeDeclaration> GetStructs()
        {
            return node.ChildNodes().OfType<StructDeclarationSyntax>()
                .Select(y => new TypeDeclaration(SourceFile, y));
        }
        public IEnumerable<TypeDeclaration> GetAllStructs()
        {
            return node.DescendantNodes()
                .OfType<StructDeclarationSyntax>()
                .Select(y => new TypeDeclaration(SourceFile, y));
        }
        public IEnumerable<ParameterDeclaration> GetEnums()
        {
            return node.ChildNodes().OfType<EnumDeclarationSyntax>()
                .Select(y => new ParameterDeclaration(y));
        }
        public IEnumerable<ParameterDeclaration> GetAllEnums()
        {
            return node.DescendantNodes()
                .OfType<EnumDeclarationSyntax>()
                .Select(y => new ParameterDeclaration(y));
        }
        public IEnumerable<TypeDeclaration> GetTypes()
        {
            return node.ChildNodes().OfType<TypeDeclarationSyntax>()
                .Select(y => new TypeDeclaration(SourceFile, y));
        }
        public IEnumerable<TypeDeclaration> GetAllTypes()
        {
            return node.DescendantNodes()
                .OfType<TypeDeclarationSyntax>()
                .Select(y => new TypeDeclaration(SourceFile, y));
        }
    }

    public static class TypeDeclarationExtentions
    {
        public static IEnumerable<TypeDeclaration> WithMethods(this IEnumerable<TypeDeclaration> source,
            Func<MethodDeclaration, bool> predicate)
        {
            return source.Where(x => x.GetMethods().Any(m => predicate(m)));
        }
        public static IEnumerable<TypeDeclaration> WithName(this IEnumerable<TypeDeclaration> source,
            Func<string, bool> predicate)
        {
            return source.Where(x => predicate(x.Name));
        }
        public static IEnumerable<TypeDeclaration> WithAttribute(this IEnumerable<TypeDeclaration> source,
            Func<AttributeDeclaration, bool> predicate)
        {
            return source.Where(x => x.Attributes.Any(a => predicate(a)));
        }
        public static IEnumerable<TypeDeclaration> WithAttribute(this IEnumerable<TypeDeclaration> source,
            string name)
        {
            return source.Where(x => x.Attributes.Any(a => a.Name.Equals(name, StringComparison.OrdinalIgnoreCase)));
        }
        public static IEnumerable<TypeDeclaration> WithBaseTypes(this IEnumerable<TypeDeclaration> source,
            Func<string, bool> predicate)
        {
            return source.Where(x => x.GetBaseTypes().Any(predicate));
        }
        public static IEnumerable<TypeDeclaration> WithBaseType(this IEnumerable<TypeDeclaration> source,
            string name)
        {
            return source.Where(x => x.GetBaseTypes().Any(b => b.Equals(name, StringComparison.OrdinalIgnoreCase)));
        }
        public static IEnumerable<TypeDeclaration> WithPublic(this IEnumerable<TypeDeclaration> source)
        {
            return source.Where(x => x.IsPublic);
        }
        public static IEnumerable<TypeDeclaration> WithProtected(this IEnumerable<TypeDeclaration> source)
        {
            return source.Where(x => x.IsProtected);
        }
        public static IEnumerable<TypeDeclaration> WithInternal(this IEnumerable<TypeDeclaration> source)
        {
            return source.Where(x => x.IsInternal);
        }
        public static IEnumerable<TypeDeclaration> WithPrivate(this IEnumerable<TypeDeclaration> source)
        {
            return source.Where(x => x.IsPrivate);
        }
        public static IEnumerable<TypeDeclaration> WithReadonly(this IEnumerable<TypeDeclaration> source)
        {
            return source.Where(x => x.IsReadonly);
        }
        public static IEnumerable<TypeDeclaration> WithStatic(this IEnumerable<TypeDeclaration> source)
        {
            return source.Where(x => x.IsStatic);
        }
        public static IEnumerable<TypeDeclaration> WithAbstract(this IEnumerable<TypeDeclaration> source)
        {
            return source.Where(x => x.IsAbstract);
        }

        public static IEnumerable<MethodDeclaration> GetMethods(this IEnumerable<TypeDeclaration> source,
            Func<MethodDeclaration, bool> predicate)
        {
            return source
                .SelectMany(x => x.GetMethods())
                .Where(predicate);
        }
        public static IEnumerable<TypeDeclaration> GetTypes(this IEnumerable<TypeDeclaration> source,
            bool subTypes = false)
        {
            return source
                .SelectMany(f => subTypes ? f.GetAllTypes() : f.GetTypes());
        }
        public static IEnumerable<TypeDeclaration> GetTypes(this IEnumerable<TypeDeclaration> source,
            Func<TypeDeclaration, bool> predicate, bool subTypes = false)
        {
            return source.GetTypes(subTypes)
                .Where(predicate);
        }
        public static IEnumerable<TypeDeclaration> GetClasses(this IEnumerable<TypeDeclaration> source,
            bool subClasses = false)
        {
            return source
                .SelectMany(f => subClasses ? f.GetAllClasses() : f.GetClasses());
        }
        public static IEnumerable<TypeDeclaration> GetClasses(this IEnumerable<TypeDeclaration> source,
            Func<TypeDeclaration, bool> predicate, bool subClasses = false)
        {
            return source.GetClasses(subClasses)
                .Where(predicate);
        }
        public static IEnumerable<TypeDeclaration> GetInterfaces(this IEnumerable<TypeDeclaration> source,
            bool subInterfaces = false)
        {
            return source
                .SelectMany(f => subInterfaces ? f.GetAllInterfaces() : f.GetInterfaces());
        }
        public static IEnumerable<TypeDeclaration> GetInterfaces(this IEnumerable<TypeDeclaration> source,
            Func<TypeDeclaration, bool> predicate, bool subInterfaces = false)
        {
            return source.GetInterfaces(subInterfaces)
                .Where(predicate);
        }
        public static IEnumerable<TypeDeclaration> GetStructs(this IEnumerable<TypeDeclaration> source,
            bool subStructs = false)
        {
            return source
                .SelectMany(f => subStructs ? f.GetAllStructs() : f.GetStructs());
        }
        public static IEnumerable<TypeDeclaration> GetStructs(this IEnumerable<TypeDeclaration> source,
            Func<TypeDeclaration, bool> predicate, bool subStructs = false)
        {
            return source.GetStructs(subStructs)
                .Where(predicate);
        }
        public static IEnumerable<TypeDeclaration> GetRecords(this IEnumerable<TypeDeclaration> source,
            bool subRecords = false)
        {
            return source
                .SelectMany(f => subRecords ? f.GetAllRecords() : f.GetRecords());
        }
        public static IEnumerable<TypeDeclaration> GetRecords(this IEnumerable<TypeDeclaration> source,
            Func<TypeDeclaration, bool> predicate, bool subRecords = false)
        {
            return source.GetRecords(subRecords)
                .Where(predicate);
        }
        public static IEnumerable<ParameterDeclaration> GetEnums(this IEnumerable<TypeDeclaration> source,
            bool subRecords = false)
        {
            return source
                .SelectMany(f => subRecords ? f.GetAllEnums() : f.GetEnums());
        }
        public static IEnumerable<ParameterDeclaration> GetEnums(this IEnumerable<TypeDeclaration> source,
            Func<ParameterDeclaration, bool> predicate, bool subRecords = false)
        {
            return source.GetEnums(subRecords)
                .Where(predicate);
        }
    }
}
