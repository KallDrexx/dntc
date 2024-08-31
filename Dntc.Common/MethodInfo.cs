using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Dntc.Common;

public class MethodInfo
{
    public record MethodParameter(ClrTypeName Type, string Name);
    
    internal MethodInfo(MethodDefinition definition)
    {
        ClrShortName = new ClrMethodShortName(definition.Name);
        ClrFullName = new ClrMethodFullName(definition.FullName);
        DeclaringClrType = new ClrTypeName(definition.DeclaringType.FullName);
        ReturnedClrType = new ClrTypeName(definition.ReturnType.FullName);
        Parameters = definition.Parameters
            .OrderBy(x => x.Index)
            .Select(x => new MethodParameter(new ClrTypeName(x.ParameterType.FullName), x.Name))
            .ToArray();

        Instructions = definition.Body.Instructions.ToArray();

        Locals = definition.Body
            .Variables
            .OrderBy(x => x.Index)
            .Select(x => new ClrTypeName(x.VariableType.FullName))
            .ToArray();
    }
    
    public ClrMethodShortName ClrShortName { get; }
    public ClrTypeName DeclaringClrType { get; }
    public ClrMethodFullName ClrFullName { get; }
    
    public ClrTypeName ReturnedClrType { get; }
    public IReadOnlyList<MethodParameter> Parameters { get; }
    public IReadOnlyList<Instruction> Instructions { get; }
    public IReadOnlyList<ClrTypeName> Locals { get; }
}