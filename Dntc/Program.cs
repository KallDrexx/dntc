using Dntc.Common;
using Dntc.Common.Definitions;
using Dntc.Common.Dependencies;
using Mono.Cecil;

var module = ModuleDefinition.ReadModule("TestInputs/SimpleFunctions.dll");
var catalog = new Catalog();
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

var foundType = catalog.FindType(new ClrTypeName("TestSetups.SimpleFunctions"));
if (foundType == null)
{
    throw new InvalidOperationException("CLR type not found");
}

Console.WriteLine($"Type {foundType.ClrName.Name}");
Console.WriteLine($"\tFields: {foundType.Fields.Count}");
foreach (var field in foundType.Fields)
{
    Console.WriteLine($"\t\t{field.Name}: {field.Type.Name}");
}

Console.WriteLine($"\tMethods: {foundType.Methods.Count}");
foreach (var methodId in foundType.Methods)
{
    Console.WriteLine($"\t\t{methodId.Name}");

    var method = catalog.FindMethod(methodId);
    if (method == null)
    {
        throw new InvalidOperationException($"No method in catalog with id '{methodId.Name}");
    }
    
    Console.WriteLine($"\t\t\tReturn Type: {method.ReturnType.Name}");
    for (var x = 0; x < method.Parameters.Count; x++)
    {
        var param = method.Parameters[x];
        Console.WriteLine($"\t\t\tParameter #{x:000}: {param.Name} ({param.Type.Name})");
    }

    for (var x = 0; x < method.Locals.Count; x++)
    {
        var local = method.Locals[x];
        Console.WriteLine($"\t\t\tLocal #{x:000}: {local.Name}");
    }
    
    Console.WriteLine("\t\t\tValidation Errors:");
    var errors = CatalogValidator.IsMethodImplementable(catalog, methodId);
    if (errors.Count == 0)
    {
        Console.WriteLine("\t\t\t\tNone!");
    }
    else
    {
        foreach (var error in errors)
        {
            Console.WriteLine($"\t\t\t\t{error}");
        }
    }
    
    void RenderGraphNode(DependencyGraph.Node node, int indent)
    {
        for (var x = 0; x < indent; x++)
        {
            Console.Write("\t");
        }
        
        switch (node)
        {
            case DependencyGraph.MethodNode methodNode:
                Console.WriteLine(methodNode.MethodId.Name);
                break;
            
            case DependencyGraph.TypeNode typeNode:
                Console.WriteLine(typeNode.TypeName.Name);
                break;
            
            default:
                throw new NotSupportedException(node.GetType().FullName);
        }

        foreach (var child in node.Children)
        {
            RenderGraphNode(child, indent + 1);
        }
    }
    
    Console.WriteLine();
    Console.WriteLine("\t\t\tDependency Graph");
    var graph = new DependencyGraph(catalog, method.Id);
    RenderGraphNode(graph.Root, 4);
}

