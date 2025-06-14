using System.Runtime.Loader;
using Dntc.Common;
using Dntc.Common.Conversion;
using Dntc.Common.Conversion.Mutators;
using Dntc.Common.Definitions;
using Dntc.Common.Definitions.Definers;
using Dntc.Common.Definitions.Mutators;
using Dntc.Common.Definitions.ReferenceTypeSupport;
using Dntc.Common.Definitions.ReferenceTypeSupport.SimpleReferenceCounting;
using Dntc.Common.Dependencies;
using Dntc.Common.Planning;
using Dntc.Common.Syntax.Statements.Generators;
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
        var plugins = LoadPlugins();
        var modules = GetModules();
        var charArrayType = modules.First().ImportReference(typeof(char[]));
        var memoryManagement = new StandardMemoryManagementActions();
        
        var transpilerPipeline = new TranspilerContext(memoryManagement);
        var definerPipeline = transpilerPipeline.Definers;
        var conversionInfoCreator = transpilerPipeline.ConversionInfoCreator;
        var definitionCatalog = transpilerPipeline.DefinitionCatalog;
        var conversionCatalog = transpilerPipeline.ConversionCatalog;
        
        definerPipeline.Append(new NativeFieldAttributeDefiner());
        definerPipeline.Append(new NativeFunctionCallAttributeDefiner());
        definerPipeline.Append(new NativeTypeDefiner());
        definerPipeline.Append(new CustomDeclaredFieldDefiner());
        definerPipeline.Append(new CustomFunctionDefiner());

        definerPipeline.AppendFieldMutator(new StaticallySizedArrayMutator(definitionCatalog));
        definerPipeline.AppendFieldMutator(new HeapAllocatedArrayMutator(definitionCatalog));
        definerPipeline.AppendFieldMutator(new EnumDefinitionMutator());

        definerPipeline.AppendMethodMutator(new EnumDefinitionMutator());

        conversionInfoCreator.AddTypeMutator(new IgnoredInHeadersMutator());
        conversionInfoCreator.AddTypeMutator(new CustomFileNameMutator());
        conversionInfoCreator.AddTypeMutator(new ArrayLateNameBindingMutator(definitionCatalog, conversionInfoCreator));
       
        conversionInfoCreator.AddMethodMutator(new WithAttributeMutator());
        conversionInfoCreator.AddMethodMutator(new CustomFileNameMutator());
        conversionInfoCreator.AddMethodMutator(new CustomFunctionNameMutator());
        conversionInfoCreator.AddMethodMutator(new CustomMethodDeclarationMutator());
        conversionInfoCreator.AddMethodMutator(new IgnoredInHeadersMutator());
        conversionInfoCreator.AddMethodMutator(new ArrayLateNameBindingMutator(definitionCatalog, conversionInfoCreator));

        conversionInfoCreator.AddFieldMutator(new WithAttributeMutator());
        conversionInfoCreator.AddFieldMutator(new InitialValueMutator(conversionCatalog));
        conversionInfoCreator.AddFieldMutator(new CustomFileNameMutator());
        conversionInfoCreator.AddFieldMutator(new CustomFieldNameMutator());
        conversionInfoCreator.AddFieldMutator(new IgnoredInHeadersMutator());
        conversionInfoCreator.AddFieldMutator(new NonPointerStringMutator());
        conversionInfoCreator.AddFieldMutator(new StaticallySizedArrayFieldMutator(charArrayType, conversionCatalog));
        
        if (plugins.All(x => !x.BypassBuiltInNativeDefinitions))
        {
            definitionCatalog.Add(NativeDefinedType.StandardTypes.Values);
            definitionCatalog.Add(NativeDefinedMethod.StandardMethods);
            definitionCatalog.Add(CustomDefinedMethod.StandardCustomMethods);
        }

        foreach (var plugin in plugins)
        {
            plugin.Customize(transpilerPipeline);
        }

        foreach (var requiredType in conversionInfoCreator.RequiredTypes)
        {
            conversionCatalog.Add(requiredType);
        }

        var referenceTypeBaseDefinition = new ReferenceTypeBaseDefinedType();
        definitionCatalog.Add([referenceTypeBaseDefinition]);
        definitionCatalog.Add([new ReferenceTypeBaseField()]);

        var refCountImplementation = new SimpleRefCountImplementation(memoryManagement);
        refCountImplementation.UpdateCatalog(definitionCatalog);
        refCountImplementation.AddFieldsToReferenceTypeBase(referenceTypeBaseDefinition);

        var planConverter = new PlannedFileConverter(conversionCatalog, definitionCatalog, false, memoryManagement);
        planConverter.AddInstructionGenerator(new DebugInfoStatementGenerator(_manifest.DebugInfoMode));
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

            var graph = new DependencyGraph(definitionCatalog, foundMethod.Id, memoryManagement);
            conversionCatalog.Add(graph);
            implementationPlan.AddMethodGraph(graph);
        }

        foreach (var globalId in _manifest.GlobalsToTranspile)
        {
            var foundGlobal = definitionCatalog.Get(new IlFieldId(globalId));
            if (foundGlobal == null)
            {
                var message = $"No global with the id '{globalId}' could be found in any of the loaded modules";
                throw new InvalidOperationException(message);
            }

            var graph = new DependencyGraph(definitionCatalog, foundGlobal.IlName, memoryManagement);
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
            var globals = new HashSet<FieldDefinition>();
            foreach (var type in module.Types)
            {
                FindTranspilableDefinitions(type, methods, globals);
            }

            var orderedMethods = methods
                .OrderBy(x => x.DeclaringType.FullName)
                .ThenBy(x => x.Name)
                .ToArray();

            var orderedGlobals = globals
                .OrderBy(x => x.DeclaringType.FullName)
                .ThenBy(x => x.Name)
                .ToArray();

            Console.WriteLine("\tMethods:");
            foreach (var method in orderedMethods)
            {
                Console.WriteLine($"\t\t- {method.FullName}");
            }
            
            Console.WriteLine("\tGlobals:");
            foreach (var global in orderedGlobals)
            {
                Console.WriteLine($"\t\t- {global.FullName}");
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

    private static void FindTranspilableDefinitions(
        TypeDefinition type, 
        HashSet<MethodDefinition> methods, 
        HashSet<FieldDefinition> globals)
    {
        foreach (var method in type.Methods)
        {
            methods.Add(method);
        }

        foreach (var field in type.Fields.Where(x => x.IsStatic))
        {
            globals.Add(field);
        }

        foreach (var nestedType in type.NestedTypes)
        {
            FindTranspilableDefinitions(nestedType, methods, globals);
        }
    }

    private IReadOnlyList<ModuleDefinition> GetModules()
    {
        var parameters = new ReaderParameters
        {
            ReadSymbols = true,
        };

        return _manifest.AssembliesToLoad
            .Select(assemblyFile => Path.Combine(_manifest.AssemblyDirectory!, assemblyFile))
            .Select(x => ModuleDefinition.ReadModule(x, parameters))
            .ToArray();
    }

    private IReadOnlyList<ITranspilerPlugin> LoadPlugins()
    {
        if (_manifest.PluginAssembly == null)
        {
            return [];
        }

        var pluginAssemblyPath = Path.Combine(_manifest.AssemblyDirectory!, _manifest.PluginAssembly);
        var context = new AssemblyLoadContext(null);
        var assembly = context.LoadFromAssemblyPath(pluginAssemblyPath);
        return assembly.GetTypes()
            .Where(x => x.IsAssignableTo(typeof(ITranspilerPlugin)))
            .Select(Activator.CreateInstance)
            .Cast<ITranspilerPlugin>()
            .ToArray();
    }
}