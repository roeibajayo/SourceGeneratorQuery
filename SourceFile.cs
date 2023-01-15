using SourceGeneratorBuilder.Declarations;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SourceGeneratorBuilder
{
    public class SourceFile
    {
        internal readonly string EntryPath;

        public SourceFile(SyntaxTree syntaxTree, string entryPath)
        {
            this.SyntaxTree = syntaxTree;
            this.EntryPath = entryPath;
        }

        public readonly SyntaxTree SyntaxTree;

        private IEnumerable<NamespaceDeclarationSyntax> GetNamespaces()
        {
            return SyntaxTree.GetRoot().ChildNodes().OfType<NamespaceDeclarationSyntax>();
        }

        public IEnumerable<string> Usings
        {
            get
            {
                return SyntaxTree.GetRoot().ChildNodes().OfType<UsingDirectiveSyntax>()
                    .Select(x => x.Name.ToString());
            }
        }
        public IEnumerable<string> Namespaces
        {
            get
            {
                return GetNamespaces().Select(x => x.Name.ToString());
            }
        }
        public IEnumerable<TypeDeclaration> GetClasses()
        {
            return GetNamespaces().SelectMany(x =>
                x.ChildNodes().OfType<ClassDeclarationSyntax>()
                .Select(y => new TypeDeclaration(this, y)));
        }
        public IEnumerable<TypeDeclaration> GetAllClasses()
        {
            return SyntaxTree.GetRoot().DescendantNodes()
                .OfType<ClassDeclarationSyntax>()
                .Select(y => new TypeDeclaration(this, y));
        }
        public IEnumerable<TypeDeclaration> GetRecords()
        {
            return GetNamespaces().SelectMany(x =>
                x.ChildNodes().OfType<RecordDeclarationSyntax>()
                .Select(y => new TypeDeclaration(this, y)));
        }
        public IEnumerable<TypeDeclaration> GetAllRecords()
        {
            return SyntaxTree.GetRoot().DescendantNodes()
                .OfType<RecordDeclarationSyntax>()
                .Select(y => new TypeDeclaration(this, y));
        }
        public IEnumerable<TypeDeclaration> GetStructs()
        {
            return GetNamespaces().SelectMany(x =>
                x.ChildNodes().OfType<StructDeclarationSyntax>()
                .Select(y => new TypeDeclaration(this, y)));
        }
        public IEnumerable<TypeDeclaration> GetAllStructs()
        {
            return SyntaxTree.GetRoot().DescendantNodes()
                .OfType<StructDeclarationSyntax>()
                .Select(y => new TypeDeclaration(this, y));
        }
        public IEnumerable<TypeDeclaration> GetInterfaces()
        {
            return GetNamespaces().SelectMany(x =>
                x.ChildNodes().OfType<InterfaceDeclarationSyntax>()
                .Select(y => new TypeDeclaration(this, y)));
        }
        public IEnumerable<TypeDeclaration> GetAllInterfaces()
        {
            return SyntaxTree.GetRoot().DescendantNodes()
                .OfType<InterfaceDeclarationSyntax>()
                .Select(y => new TypeDeclaration(this, y));
        }
        public IEnumerable<TypeDeclaration> GetTypes()
        {
            return GetNamespaces().SelectMany(x =>
                x.ChildNodes().OfType<TypeDeclarationSyntax>()
                .Select(y => new TypeDeclaration(this, y)));
        }
        public IEnumerable<TypeDeclaration> GetAllTypes()
        {
            return SyntaxTree.GetRoot().DescendantNodes()
                .OfType<TypeDeclarationSyntax>()
                .Select(y => new TypeDeclaration(this, y));
        }

        public string Body =>
            SyntaxTree.GetText().ToString();
        public string FileName =>
            Path.GetFileName(SyntaxTree.FilePath);
        public string FilePath =>
            "./" + SyntaxTree.FilePath.Substring(EntryPath.Length + 1).Replace('\\', '/');


        public override string ToString()
        {
            return FileName;
        }
    }

    public static class SourceFileExtentions
    {
        public static IEnumerable<SourceFile> WithPath(this IEnumerable<SourceFile> source,
            string path, bool subDirectories = true)
        {
            path = path.TrimStart('.', '/').Replace('/', '\\');
            path = path.PadLeft(path.Length + 1, '/').PadLeft(path.Length + 2, '.');

            return source.Where(x => x.FilePath.StartsWith(path) &&
                    (subDirectories || !x.FilePath.Substring(path.Length + 1).Contains('\\')));
        }
        public static IEnumerable<SourceFile> WithClasses(this IEnumerable<SourceFile> source,
            Func<TypeDeclaration, bool> predicate, bool subClasses = false)
        {
            return source.Where(f => (subClasses ? f.GetAllClasses() : f.GetClasses()).Any(predicate));
        }
        public static IEnumerable<SourceFile> WithInterfaces(this IEnumerable<SourceFile> source,
            Func<TypeDeclaration, bool> predicate, bool subInterfaces = false)
        {
            return source.Where(f => (subInterfaces ? f.GetAllInterfaces() : f.GetInterfaces()).Any(predicate));
        }
        public static IEnumerable<SourceFile> WithStructs(this IEnumerable<SourceFile> source,
            Func<TypeDeclaration, bool> predicate, bool subStructs = false)
        {
            return source.Where(f => (subStructs ? f.GetAllStructs() : f.GetStructs()).Any(predicate));
        }
        public static IEnumerable<SourceFile> WithRecords(this IEnumerable<SourceFile> source,
            Func<TypeDeclaration, bool> predicate, bool subRecords = false)
        {
            return source.Where(f => (subRecords ? f.GetAllRecords() : f.GetRecords()).Any(predicate));
        }
        public static IEnumerable<TypeDeclaration> WithEnums(this IEnumerable<SourceFile> source,
            Func<ParameterDeclaration, bool> predicate, bool subEnums = false)
        {
            return source
                .SelectMany(x => x.GetAllTypes())
                .Where(f => (subEnums ? f.GetAllEnums() : f.GetEnums()).Any(predicate));
        }

        public static IEnumerable<TypeDeclaration> GetTypes(this IEnumerable<SourceFile> source,
            bool subTypes = false)
        {
            return source
                .SelectMany(f => subTypes ? f.GetAllTypes() : f.GetTypes());
        }
        public static IEnumerable<TypeDeclaration> GetTypes(this IEnumerable<SourceFile> source,
            Func<TypeDeclaration, bool> predicate, bool subTypes = false)
        {
            return GetTypes(source, subTypes)
                .Where(predicate);
        }
        public static IEnumerable<TypeDeclaration> GetClasses(this IEnumerable<SourceFile> source,
            bool subClasses = false)
        {
            return source
                .SelectMany(f => subClasses ? f.GetAllClasses() : f.GetClasses());
        }
        public static IEnumerable<TypeDeclaration> GetClasses(this IEnumerable<SourceFile> source,
            Func<TypeDeclaration, bool> predicate, bool subClasses = false)
        {
            return GetClasses(source, subClasses)
                .Where(predicate);
        }
        public static IEnumerable<TypeDeclaration> GetInterfaces(this IEnumerable<SourceFile> source,
            bool subInterfaces = false)
        {
            return source
                .SelectMany(f => subInterfaces ? f.GetAllInterfaces() : f.GetInterfaces());
        }
        public static IEnumerable<TypeDeclaration> GetInterfaces(this IEnumerable<SourceFile> source,
            Func<TypeDeclaration, bool> predicate, bool subInterfaces = false)
        {
            return GetInterfaces(source, subInterfaces)
                .Where(predicate);
        }
        public static IEnumerable<TypeDeclaration> GetStructs(this IEnumerable<SourceFile> source,
            bool subStructs = false)
        {
            return source
                .SelectMany(f => subStructs ? f.GetAllStructs() : f.GetStructs());
        }
        public static IEnumerable<TypeDeclaration> GetStructs(this IEnumerable<SourceFile> source,
            Func<TypeDeclaration, bool> predicate, bool subStructs = false)
        {
            return GetStructs(source, subStructs)
                .Where(predicate);
        }
        public static IEnumerable<TypeDeclaration> GetRecords(this IEnumerable<SourceFile> source,
            bool subRecords = false)
        {
            return source
                .SelectMany(f => subRecords ? f.GetAllRecords() : f.GetRecords());
        }
        public static IEnumerable<TypeDeclaration> GetRecords(this IEnumerable<SourceFile> source,
            Func<TypeDeclaration, bool> predicate, bool subRecords = false)
        {
            return GetRecords(source, subRecords)
                .Where(predicate);
        }
    }
}
