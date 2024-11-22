using Dntc.Common;
using Dntc.Common.Conversion;
using Dntc.Common.Definitions;
using Dntc.Common.Definitions.CustomDefinedMethods;
using Dntc.Common.Dependencies;
using Dntc.Common.Planning;
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
        var definitionCatalog = new DefinitionCatalog();
        var conversionCatalog = new ConversionCatalog(definitionCatalog);
        var planConverter = new PlannedFileConverter(conversionCatalog, definitionCatalog, false);
        
        var modules = GetModules();
        definitionCatalog.Add(NativeDefinedType.StandardTypes.Values);
        definitionCatalog.Add(NativeDefinedMethod.StandardMethods);
        definitionCatalog.Add(CustomDefinedMethod.StandardCustomMethods);
        definitionCatalog.Add(modules.SelectMany(x => x.Types)); // adding types via type definition automatically adds its methods

        var implementationPlan = new ImplementationPlan(conversionCatalog, definitionCatalog);
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

        var headers = implementationPlan.Headers;
        var sourceFiles = implementationPlan.SourceFiles;

        if (_manifest.SingleGeneratedSourceFileName != null)
        {
            var mergedSourceFile = PlannedSourceFile.CreateMerged(
                new CSourceFileName(_manifest.SingleGeneratedSourceFileName),
                headers,
                sourceFiles);

            headers = [];
            sourceFiles = [mergedSourceFile];
        }
        else
        {
            // Only clean the output directory if we aren't creating a single file. When generating a single
            // file, it's probably the case where different files exist in a directory from multiple 
            // transpiler calls, or it's a `main.c` and other files exist for cmake and other utilities.
            // So in that case we are less sure the remaining files in the directory are free to delete. 
            //
            // Also, when transpiling to a single file it's a lot easier to know when a file has been left 
            // behind and is no longer relevant, which is the main reason we are cleaning the output directory
            // anyway.
            CleanOutputDirectory();
        }

        await WriteHeaderAndSourceFiles(headers, sourceFiles, planConverter);

        if (_manifest.SingleGeneratedSourceFileName != null)
        {
            var path = Path.Combine(_manifest.OutputDirectory!, _manifest.SingleGeneratedSourceFileName);
            Console.WriteLine($"Source successfully written to {path}");
        }
        else
        {
            Console.WriteLine($"Headers and source successfully written to {_manifest.OutputDirectory}");
        }
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

    private void CleanOutputDirectory()
    {
        // Make sure the output folder is clean. We don't want old header and source files to be around
        // if the code has changed to not require them anymore. Only delete the folder if it exists and
        // only contains header and source files. 
        if (Directory.Exists(_manifest.OutputDirectory))
        {
            var containsOtherFiles = Directory
                .GetFiles(_manifest.OutputDirectory)
                .Where(x => !x.EndsWith(".c"))
                .Any(x => !x.EndsWith(".h"));

            if (containsOtherFiles)
            {
                Console.WriteLine("WARNING: Not deleting files in output directory, as non header and non-source " +
                                  "files are present");
            }
            else
            {
                Directory.Delete(_manifest.OutputDirectory, true);
                Directory.CreateDirectory(_manifest.OutputDirectory!);
            }
        }
        else
        {
            Directory.CreateDirectory(_manifest.OutputDirectory!);
        }
    }

    private async Task WriteHeaderAndSourceFiles(
        IEnumerable<PlannedHeaderFile> headers,
        IEnumerable<PlannedSourceFile> sourceFiles,
        PlannedFileConverter planConverter)
    {
        foreach (var header in headers)
        {
            var fullHeaderPath = Path.Combine(_manifest.OutputDirectory!, header.Name.Value);
            
            // In case we didn't clear the directory, we definitely need to make sure
            // that the file is deleted before we write to it. It will never work to just
            // append to it.
            File.Delete(fullHeaderPath);
            await using var stream = File.OpenWrite(fullHeaderPath);
            await using var writer = new StreamWriter(stream);

            var headerFile = planConverter.Convert(header);
            await headerFile.WriteAsync(writer);
        }

        foreach (var sourceFile in sourceFiles)
        {
            var fullPath = Path.Combine(_manifest.OutputDirectory!, sourceFile.Name.Value);
            
            // In case we didn't clear the directory, we definitely need to make sure
            // that the file is deleted before we write to it. It will never work to just
            // append to it.
            File.Delete(fullPath);
            await using var stream = File.OpenWrite(fullPath);
            await using var writer = new StreamWriter(stream);

            var source = planConverter.Convert(sourceFile);
            await source.WriteAsync(writer);
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