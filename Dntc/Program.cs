using Dntc.Common;
using Dntc.Common.Conversion;
using Dntc.Common.Conversion.Planning;
using Dntc.Common.Definitions;
using Dntc.Common.Dependencies;
using Mono.Cecil;

var module = ModuleDefinition.ReadModule("TestInputs/SimpleFunctions.dll");
var catalog = new DefinitionCatalog();
foreach (var type in NativeDefinedType.StandardTypes)
{
    catalog.AddType(type);
}

foreach (var type in module.Types.Where(x => x.Name != "<Module>"))
{
    var definedType = new DotNetDefinedType(type);
    catalog.AddType(definedType);

    foreach (var method in type.Methods)
    {
        var definedMethod = new DotNetDefinedMethod(method);
        catalog.AddMethod(definedMethod);
    }
}

var foundType = catalog.Find(new IlTypeName("TestSetups.SimpleFunctions"));
if (foundType == null)
{
    throw new InvalidOperationException("CLR type not found");
}

var foundMethod = catalog.Find(new IlMethodId("System.Int32 TestSetups.SimpleFunctions::IntAdd(System.Int32,System.Int32)"));
if (foundMethod == null)
{
    throw new InvalidOperationException("CLR method not found");
}

var graph = new DependencyGraph(catalog, foundMethod.Id);
var plan = new ImplementationPlan(catalog, graph);

Console.WriteLine("Headers:");
foreach (var header in plan.Headers)
{
    Console.WriteLine($"\t{header.Name.Value}");
    Console.WriteLine($"\tReferenced Headers:");
    foreach (var referencedHeader in header.ReferencedHeaders)
    {
        Console.WriteLine($"\t\t{referencedHeader.Value}");
    }
    
    Console.WriteLine();
    Console.WriteLine($"\tTypes:");
    foreach (var type in header.DeclaredTypes)
    {
        Console.WriteLine($"\t\t{type.NameInC.Value} ({type.IlName.Value})");
    }
    
    Console.WriteLine();
    Console.WriteLine("\tMethods:");
    foreach (var method in header.DeclaredMethods)
    {
        Console.WriteLine($"\t\t{method.NameInC.Value} ({method.MethodId.Value})");
    }
    
    Console.WriteLine();
}


