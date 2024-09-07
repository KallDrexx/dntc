using Dntc.Common;
using Dntc.Common.Conversion;
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

var foundType = catalog.FindType(new IlTypeName("TestSetups.SimpleFunctions"));
if (foundType == null)
{
    throw new InvalidOperationException("CLR type not found");
}

var foundMethod = catalog.FindMethod(new IlMethodId("System.Int32 TestSetups.SimpleFunctions::IntAdd(System.Int32,System.Int32)"));
if (foundMethod == null)
{
    throw new InvalidOperationException("CLR method not found");
}

var typeConversion = new TypeConversionInfo(foundType);
Console.WriteLine($"Type: {typeConversion.IlName.Name}");
Console.WriteLine($"Header: {typeConversion.Header?.Name}");
Console.WriteLine($"IsPredeclared: {typeConversion.IsPredeclared}");
Console.WriteLine($"C Name: {typeConversion.NameInC.Name}");
Console.WriteLine();


var methodConversion = new MethodConversionInfo(foundMethod);
Console.WriteLine($"Method: {methodConversion.MethodId.Name}");
Console.WriteLine($"Header: {methodConversion.Header.Name}");
Console.WriteLine($"Implementation File: {methodConversion.ImplementationFile?.Name}");
Console.WriteLine($"IsPredeclared: {methodConversion.IsPredeclared}");
Console.WriteLine($"C Name: {methodConversion.NameInC.Name}");
Console.WriteLine();


