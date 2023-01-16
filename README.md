# SourceGeneratorQuery
C# class library that helps you query the GeneratorExecutionContext, and also adds LINQ support.

You can read about Source Generators on [Microsoft's site](https://learn.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/source-generators-overview). 

Installation
---
This library is distributed via NuGet.

> PM> Install-Package [SourceGeneratorQuery](https://www.nuget.org/packages/SourceGeneratorQuery)

Quick Start
---
```csharp
//..
        public void Execute(GeneratorExecutionContext context)
        {
            var classes = context
                .NewQuery() // start new query
                .WithPath("./Social") // filter search on specific path
                .GetClasses()
                .WithName(x => x.EndsWith("Client")); // classes ends with 'Client'

            var syntaxNodes = classes.Select(x => x.SyntaxNode); // get the syntax nodes if you want

            // example of iteration
            foreach (var c in classes)
            {
                var publicMethodsWithMyAttributeAndStartsWithGet = c.GetMethods()
                            .WithAttribute("MyAttribute")
                            .WithPublic()
                            .WithName(name => name.StartsWith("Get"));

                var methodsSyntaxNodes = publicMethodsWithMyAttributeAndStartsWithGet.Select(x => x.SyntaxNode);

                var stringifyMethods = publicMethodsWithMyAttributeAndStartsWithGet
                    .Select(x => $"{string.Join(" ", x.Modifiers)} {x.Name}({string.Join(", ", x.Parameters.Select(p => $"{p.Type} {p.Name}"))})")
                    .ToArray();
            }
        }
//..
```

Examples
---
See <code>/src/examples</code> folder for more examples.