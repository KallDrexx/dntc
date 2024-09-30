using Dntc.Common.Conversion.OpCodeHandlers;
using Dntc.Common.Definitions;
using Dntc.Common.MethodAnalysis;

namespace Dntc.Common.Conversion;

public class CodeGenerator
{
    private readonly ConversionCatalog _conversionCatalog;
    private readonly DefinitionCatalog _definitionCatalog;
    private readonly KnownOpcodeHandlers _opcodeHandlers = new();

    public CodeGenerator(ConversionCatalog catalog, DefinitionCatalog definitionCatalog)
    {
        _conversionCatalog = catalog;
        _definitionCatalog = definitionCatalog;
    }

    public static string OffsetLabel(int offset) => $"IL_{offset:X4}";
    
    public async Task GenerateStructAsync(DotNetDefinedType type, StreamWriter writer)
    {
        var conversionInfo = _conversionCatalog.Find(type.IlName);
        
        await writer.WriteLineAsync("typedef struct {");
        foreach (var field in type.Fields)
        {
            var fieldConversionInfo = _conversionCatalog.Find(field.Type);
            await writer.WriteLineAsync($"\t{fieldConversionInfo.NameInC.Value} {field.Name};");
        }
        
        await writer.WriteLineAsync($"}} {conversionInfo.NameInC.Value};");
    }

    public async Task GenerateFunctionPointerTypedef(DotNetFunctionPointerType fnPtr, StreamWriter writer)
    {
        var conversionInfo = _conversionCatalog.Find(fnPtr.IlName);
        var returnType = _conversionCatalog.Find(new IlTypeName(fnPtr.Definition.ReturnType.FullName));
        
        await writer.WriteAsync($"typedef {returnType.NameInC} (*{conversionInfo.NameInC})(");
        for (var x = 0; x < fnPtr.Definition.Parameters.Count; x++)
        {
            var param = fnPtr.Definition.Parameters[x];
            var paramType = _conversionCatalog.Find(new IlTypeName(param.ParameterType.FullName));

            if (x > 0)
            {
                await writer.WriteAsync(", ");
            }

            await writer.WriteAsync(paramType.NameInC.Value);
        }

        await writer.WriteLineAsync(");");
    }

    public async Task GenerateCustomDefinedHeaderData(CustomDefinedType customDefinedType, StreamWriter writer)
    {
        await customDefinedType.WriteHeaderContentsAsync(_conversionCatalog, writer);
    }

    public async Task GenerateMethodDeclarationAsync(DotNetDefinedMethod method, StreamWriter writer, bool hasImplementation = false)
    {
        var methodInfo = _conversionCatalog.Find(method.Id);
        var returnTypeInfo = _conversionCatalog.Find(method.ReturnType);

        await writer.WriteAsync($"{returnTypeInfo.NameInC.Value} {methodInfo.NameInC.Value}(");
        for (var x = 0; x < method.Parameters.Count; x++)
        {
            var param = method.Parameters[x];
            var prefix = x > 0 ? ", " : "";
            var pointerSymbol = param.IsReference ? "*" : "";
            var paramInfo = _conversionCatalog.Find(param.Type);
            await writer.WriteAsync($"{prefix}{paramInfo.NameInC.Value} {pointerSymbol}{param.Name}");
        }

        if (hasImplementation)
        {
            await writer.WriteLineAsync(") {");
        }
        else
        {
            await writer.WriteLineAsync(");");
        }
    }

    public async Task GenerateMethodImplementationAsync(DotNetDefinedMethod method, StreamWriter writer)
    {
        var methodVariables = new VariableCollection();
        var methodAnalysis = new MethodAnalyzer().Analyze(method);

        await GenerateMethodDeclarationAsync(method, writer, true);
        for (var x = 0; x < method.Parameters.Count; x++)
        {
            var param = method.Parameters[x];
            var paramInfo = _conversionCatalog.Find(param.Type);
            
            var index = methodVariables.AddParameter(paramInfo, param.Name, param.IsReference);
            if (index != x)
            {
                var message = $"Parameter ${x} was given an index of {index} in the name collection";
                throw new InvalidOperationException(message);
            }
        }

        for (var x = 0; x < method.Locals.Count; x++)
        {
            var local = method.Locals[x];
            var type = _conversionCatalog.Find(local.Type);
            var index = methodVariables.AddLocal(type, local.IsReference);
            if (index != x)
            {
                var message = $"Local ${x} was given an index of {index} in the name collection";
                throw new InvalidOperationException(message);
            }

            var pointerSymbol = local.IsReference ? "*" : "";
            await writer.WriteLineAsync($"\t{type.NameInC.Value} {pointerSymbol}{methodVariables.Locals[index].Name};");
        }

        await writer.WriteLineAsync();

        var context = new OpCodeHandlingContext(
            methodVariables,
            writer,
            _conversionCatalog,
            _definitionCatalog);
        
        foreach (var instruction in method.Definition.Body.Instructions)
        {
            var handler = _opcodeHandlers.Get(instruction.OpCode.Code);
            if (handler == null)
            {
                throw new NotSupportedException($"No handler for opcode '{instruction.OpCode}'");
            }

            context.Operand = instruction.Operand;

            if (methodAnalysis.BranchTargetOffsets.Contains(instruction.Offset))
            {
                // This instruction is a branch target, so we need to give it a label
                await writer.WriteLineAsync();
                await writer.WriteLineAsync($"{OffsetLabel(instruction.Offset)}:");
            }

            try
            {
                await handler(context);
            }
            catch (Exception exception)
            {
                var message = $"Exception handling op code '{instruction.OpCode.Code}' in method " +
                              $"'{method.Definition.FullName}' at {OffsetLabel(instruction.Offset)}";

                throw new Exception(message, exception);
            }
        }
        
        await writer.WriteLineAsync("}");
    }
}