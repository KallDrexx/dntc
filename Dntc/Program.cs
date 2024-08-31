using Dntc.Common;
using Mono.Cecil;

var catalog = new Catalog();
var module = ModuleDefinition.ReadModule("TestInputs/SimpleFunctions.dll");
catalog.AddModule(module);

foreach (var typeName in catalog.DefinedTypes)
{
    var typeInfo = catalog.FindType(typeName);
    Console.WriteLine($"{typeInfo!.ClrName.Name}");

    Console.WriteLine($"\tMethods found: {typeInfo.Methods.Count}");
    foreach (var methodName in typeInfo.Methods)
    {
        var method = catalog.FindMethod(methodName);
        if (method == null)
        {
            var message = $"No method found with {methodName.Name} on type {typeName.Name}";
            throw new NullReferenceException(message);
        }

        Console.Write($"\t\t{typeName.Name}.{methodName.Name}(");
        for (var x = 0; x < method.Parameters.Count; x++)
        {
            if (x > 0) Console.Write(", ");
            var param = method.Parameters[x];
            Console.Write($"{param.Type.Name} {param.Name}");
        }

        Console.WriteLine(")");
        
        Console.WriteLine("\t\tLocal Variables:");
        for (var x = 0; x < method.Locals.Count; x++)
        {
            Console.WriteLine($"\t\t\t{x:0000} - {method.Locals[x].Name}");
        }

        Console.WriteLine("\t\tInstructions:");

        foreach (var instruction in method.Instructions)
        {
            Console.Write($"\t\t\t{instruction.Offset:0000}: {instruction.OpCode} ");
            switch (instruction.Operand)
            {
                case null:
                    break;

                default:
                    Console.Write($"{instruction.Operand} ({instruction.Operand?.GetType().FullName})");
                    break;
            }

            Console.WriteLine();
        }

        Console.WriteLine();
    }
}