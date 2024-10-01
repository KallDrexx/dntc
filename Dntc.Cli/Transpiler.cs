using Dntc.Common;
using Dntc.Common.Conversion;
using Dntc.Common.Conversion.Planning;
using Dntc.Common.Definitions;
using Dntc.Common.Dependencies;
using Mono.Cecil;

namespace Dntc.Cli;

public class Transpiler
{
    private readonly Manifest _manifest;

    public Transpiler(Manifest manifest)
    {
        _manifest = manifest;
    }

    public async Task RunAsync()
    {
        var modules = GetModules();
        var definitionCatalog = new DefinitionCatalog();
        definitionCatalog.Add(NativeDefinedType.StandardTypes.Values);
        definitionCatalog.Add(NativeDefinedMethod.StandardMethods);
        definitionCatalog.Add(CustomDefinedMethod.StandardCustomMethods);
        definitionCatalog.Add(modules.SelectMany(x => x.Types)); // adding types via type definition automatically adds its methods

        var conversionCatalog = new ConversionCatalog(definitionCatalog);
        var implementationPlan = new ImplementationPlan(conversionCatalog);
        foreach (var methodId in _manifest.MethodsToTranspile)
        {
            var foundMethod = definitionCatalog.Get(new IlMethodId(methodId.Trim()));
            if (foundMethod == null)
            {
                var message = $"No method with the id '{methodId}' could be found in any of the loaded modules";
                throw new InvalidOperationException(message);
            }

            var graph = new DependencyGraph(definitionCatalog, foundMethod.Id);
            conversionCatalog.Add(graph);
            implementationPlan.AddMethodGraph(graph);
        }

        var fileGenerator = new FileGenerator(definitionCatalog, conversionCatalog);
        
        // Make sure the output folder is clean
        try
        {
            Directory.Delete(_manifest.OutputDirectory!, true);
        }
        catch (IOException)
        {
            // Ignore if the directory doesn't exist
        }
        
        Directory.CreateDirectory(_manifest.OutputDirectory!);
        foreach (var header in implementationPlan.Headers)
        {
            var fullHeaderPath = Path.Combine(_manifest.OutputDirectory!, header.Name.Value);
            await using var stream = File.OpenWrite(fullHeaderPath);
            await using var writer = new StreamWriter(stream);
            await fileGenerator.WriteHeaderFileAsync(header, writer);
        }

        foreach (var sourceFile in implementationPlan.SourceFiles)
        {
            var fullPath = Path.Combine(_manifest.OutputDirectory!, sourceFile.Name.Value);
            await using var stream = File.OpenWrite(fullPath);
            await using var writer = new StreamWriter(stream);
            await fileGenerator.WriteSourceFileAsync(sourceFile, writer);
        }
        
        Console.WriteLine($"Headers and source successfully written to {_manifest.OutputDirectory}");
    }
    
    public void Query()
    {
        Console.WriteLine("Assembly query results:");
        var modules = GetModules();
        
        foreach (var module in modules)
        {
            Console.WriteLine($"{module.FileName}:");
            
            var methods = new HashSet<MethodDefinition>();
            foreach (var type in module.Types)
            {
                FindTypesAndMethods(type, methods);
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

    private static void FindTypesAndMethods(TypeDefinition type, HashSet<MethodDefinition> methods)
    {
        foreach (var method in type.Methods)
        {
            methods.Add(method);
        }

        foreach (var nestedType in type.NestedTypes)
        {
            FindTypesAndMethods(nestedType, methods);
        }
    }

    private IReadOnlyList<ModuleDefinition> GetModules()
    {
        return _manifest.AssembliesToLoad
            .Select(assemlbyFile => Path.Combine(_manifest.AssemblyDirectory!, assemlbyFile))
            .Select(path => ModuleDefinition.ReadModule(path))
            .ToArray();
    }
}