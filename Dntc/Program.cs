﻿using Dntc.Common.Definitions;
using Mono.Cecil;

namespace Dntc;

public static class Program
{
    public static int Main(string[] args)
    {
        if (args.Length == 0)
        {
            ShowHelp();
            return 1;
        }

        switch (args[0].ToLower())
        {
            case "listmethods":
                if (args.Length == 1)
                {
                    Console.WriteLine("Filename for assembly missing");
                    ShowHelp();

                    return 1;
                }

                ExecuteListMethods(args[1]);
                return 0;
            
            default:
                ShowHelp();
                return 1;
        }
    }

    private static void ShowHelp()
    {
        Console.WriteLine("Dntc - Dot Net To C transpiler");
        Console.WriteLine("Usage: dntc <command> <arguments>");
        Console.WriteLine();
        Console.WriteLine("  listMethods <dll> - Lists all methods in the specified dll");
        Console.WriteLine("  help - Displays this help");
    }

    private static void ExecuteListMethods(string? fileName)
    {
        var module = ModuleDefinition.ReadModule(fileName);
        foreach (var type in module.Types)
        {
            foreach (var method in type.Methods)
            {
                Console.WriteLine($"* {method.FullName}");
            }
        }
    }
    
}

// var module = ModuleDefinition.ReadModule("TestInputs/TestSetups.dll");
// var catalog = new DefinitionCatalog();
// foreach (var type in NativeDefinedType.StandardTypes.Values)
// {
//     catalog.Add(type);
// }
//
// foreach (var type in module.Types)
// {
//     catalog.Add(type);
// }
//
// var foundType = catalog.Get(new IlTypeName("TestSetups.SimpleFunctions"));
// if (foundType == null)
// {
//     throw new InvalidOperationException("CLR type not found");
// }
//
// var foundMethod = catalog.Get(new IlMethodId("System.Int32 TestSetups.SimpleFunctions::FnPointerTest(method System.Int32 *(System.Int32,System.Int32),System.Int32,System.Int32)"));
// if (foundMethod == null)
// {
//     throw new InvalidOperationException("CLR method not found");
// }
//
// // var analysisResults = new MethodAnalyzer().Analyze((DotNetDefinedMethod)foundMethod);
//
// var graph = new DependencyGraph(catalog, foundMethod.Id);
// var conversionCatalog = new ConversionCatalog(catalog, graph);
// var plan = new ImplementationPlan(conversionCatalog, graph);
// var headerGenerator = new FileGenerator(catalog, conversionCatalog);
//
// foreach (var header in plan.Headers)
// {
//     var contents = new MemoryStream();
//     await using (var writer = new StreamWriter(contents, leaveOpen: true))
//     {
//         await headerGenerator.WriteHeaderFileAsync(header, writer);
//     }
//     
//     Console.WriteLine($"Header: {header.Name.Value}");
//     contents.Seek(0, SeekOrigin.Begin);
//
//     using (var reader = new StreamReader(contents))
//     {
//         Console.Write(await reader.ReadToEndAsync());
//     }
// }
//
// Console.WriteLine();
// foreach (var sourceFile in plan.SourceFiles)
// {
//     var contents = new MemoryStream();
//     await using (var writer = new StreamWriter(contents, leaveOpen: true))
//     {
//         await headerGenerator.WriteSourceFileAsync(sourceFile, writer);
//     }
//     
//     Console.WriteLine($"Source File: {sourceFile.Name.Value}");
//     contents.Seek(0, SeekOrigin.Begin);
//
//     using (var reader = new StreamReader(contents))
//     {
//         Console.Write(await reader.ReadToEndAsync());
//     }
// }