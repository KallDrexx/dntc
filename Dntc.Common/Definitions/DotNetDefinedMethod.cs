using Mono.Cecil;

namespace Dntc.Common.Definitions;

public class DotNetDefinedMethod : DefinedMethod
{
    public MethodDefinition Definition { get; }

    public DotNetDefinedMethod(MethodDefinition definition)
    {
        Definition = definition;
        Id = new ClrMethodId(definition.FullName);
        ReturnType = new ClrTypeName(definition.ReturnType.FullName);
        Parameters = definition.Parameters
            .OrderBy(x => x.Index)
            .Select(x => new Parameter(new ClrTypeName(x.ParameterType.FullName), x.Name))
            .ToArray();

        Locals = definition.Body
            .Variables
            .OrderBy(x => x.Index)
            .Select(x => new ClrTypeName(x.VariableType.FullName))
            .ToArray();
    }
}