using Mono.Cecil;

namespace Dntc.Cli;

public static class AssemblyQuery
{
    public static void Run(Manifest manifest)
    {
        Console.WriteLine("Assembly query results:");
        foreach (var assemblyFile in manifest.AssembliesToLoad)
        {
            var path = Path.Combine(manifest.AssemblyDirectory!, assemblyFile);
            Console.WriteLine($"{path}:");

            var module = ModuleDefinition.ReadModule(path);
            var methods = new HashSet<MethodDefinition>();
            foreach (var type in module.Types)
            {
                FindMethods(type, methods);
            }

            var orderedMethods = methods
                .OrderBy(x => x.DeclaringType.FullName)
                .ThenBy(x => x.Name)
                .ToArray();

            foreach (var method in orderedMethods)
            {
                Console.WriteLine($"\t- {method.FullName}");
            }
        }
    }

    private static void FindMethods(TypeDefinition type, HashSet<MethodDefinition> methods)
    {
        foreach (var method in type.Methods)
        {
            methods.Add(method);
        }

        foreach (var innerType in type.NestedTypes)
        {
            FindMethods(innerType, methods);
        }
    }
}