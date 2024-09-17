using Dntc.Common.Conversion.OpCodeHandlers;
using Dntc.Common.Definitions;
using Dntc.Common.MethodAnalysis;

namespace Dntc.Common.Conversion;

public class CodeGenerator
{
    private readonly ConversionCatalog _conversionCatalog;
    private readonly KnownOpcodeHandlers _opcodeHandlers = new();

    public CodeGenerator(ConversionCatalog catalog)
    {
        _conversionCatalog = catalog;
    }

    public static string OffsetLabel(int offset) => $"il{offset:0000}";
    
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

    public async Task GenerateMethodDeclarationAsync(DotNetDefinedMethod method, StreamWriter writer)
    {
        var methodInfo = _conversionCatalog.Find(method.Id);
        var returnTypeInfo = _conversionCatalog.Find(method.ReturnType);

        await writer.WriteAsync($"{returnTypeInfo.NameInC.Value} {methodInfo.NameInC.Value}(");
        for (var x = 0; x < method.Parameters.Count; x++)
        {
            var param = method.Parameters[x];
            var prefix = x > 0 ? ", " : "";
            var paramInfo = _conversionCatalog.Find(param.Type);
            await writer.WriteAsync($"{prefix}{paramInfo.NameInC.Value} {param.Name}");
        }

        await writer.WriteLineAsync(");");
    }

    public async Task GenerateMethodImplementationAsync(DotNetDefinedMethod method, StreamWriter writer)
    {
        var methodInfo = _conversionCatalog.Find(method.Id);
        var returnTypeInfo = _conversionCatalog.Find(method.ReturnType);
        var methodVariables = new VariableCollection();
        var methodAnalysis = new MethodAnalyzer().Analyze(method);

        await writer.WriteAsync($"{returnTypeInfo.NameInC.Value} {methodInfo.NameInC.Value}(");
        for (var x = 0; x < method.Parameters.Count; x++)
        {
            var param = method.Parameters[x];
            var prefix = x > 0 ? ", " : "";
            var paramInfo = _conversionCatalog.Find(param.Type);
            
            var index = methodVariables.AddParameter(paramInfo, param.Name);
            if (index != x)
            {
                var message = $"Parameter ${x} was given an index of {index} in the name collection";
                throw new InvalidOperationException(message);
            }
            
            
            await writer.WriteAsync($"{prefix}{paramInfo.NameInC.Value} {param.Name}");
        }

        await writer.WriteLineAsync(") {");

        for (var x = 0; x < method.Locals.Count; x++)
        {
            var local = method.Locals[x];
            var type = _conversionCatalog.Find(local);
            var index = methodVariables.AddLocal(type);
            if (index != x)
            {
                var message = $"Local ${x} was given an index of {index} in the name collection";
                throw new InvalidOperationException(message);
            }

            await writer.WriteLineAsync($"\t{type.NameInC.Value} {methodVariables};");
        }

        await writer.WriteLineAsync();

        var context = new OpCodeHandlingContext(
            method.Parameters.Select(x => x.Name).ToArray(), 
            methodVariables,
            writer);
        
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
                await writer.WriteLineAsync($"{OffsetLabel(instruction.Offset)}");
            }
            
            await handler(context);
        }
        
        await writer.WriteLineAsync("}");
    }
}