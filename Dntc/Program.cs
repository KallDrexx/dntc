﻿using Dntc.Common;
using Dntc.Common.Conversion;
using Dntc.Common.Conversion.Planning;
using Dntc.Common.Definitions;
using Dntc.Common.Dependencies;
using Dntc.Common.MethodAnalysis;
using Mono.Cecil;

var module = ModuleDefinition.ReadModule("TestInputs/TestSetups.dll");
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

var foundMethod = catalog.Find(new IlMethodId("System.Int32 TestSetups.SimpleFunctions::IfTest(System.Int32)"));
if (foundMethod == null)
{
    throw new InvalidOperationException("CLR method not found");
}

var analysisResults = new MethodAnalyzer().Analyze((DotNetDefinedMethod)foundMethod);

var graph = new DependencyGraph(catalog, foundMethod.Id);
var conversionCatalog = new ConversionCatalog(catalog, graph);
var plan = new ImplementationPlan(conversionCatalog, graph);
var headerGenerator = new FileGenerator(catalog, conversionCatalog);

foreach (var header in plan.Headers)
{
    var contents = new MemoryStream();
    await using (var writer = new StreamWriter(contents, leaveOpen: true))
    {
        await headerGenerator.WriteHeaderFileAsync(header, writer);
    }
    
    Console.WriteLine($"Header: {header.Name.Value}");
    contents.Seek(0, SeekOrigin.Begin);

    using (var reader = new StreamReader(contents))
    {
        Console.Write(await reader.ReadToEndAsync());
    }
}

Console.WriteLine();
foreach (var sourceFile in plan.SourceFiles)
{
    var contents = new MemoryStream();
    await using (var writer = new StreamWriter(contents, leaveOpen: true))
    {
        await headerGenerator.WriteSourceFileAsync(sourceFile, writer);
    }
    
    Console.WriteLine($"Source File: {sourceFile.Name.Value}");
    contents.Seek(0, SeekOrigin.Begin);

    using (var reader = new StreamReader(contents))
    {
        Console.Write(await reader.ReadToEndAsync());
    }
}