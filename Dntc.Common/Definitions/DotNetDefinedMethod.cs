using Mono.Cecil;

namespace Dntc.Common.Definitions;

public class DotNetDefinedMethod : DefinedMethod
{
    public MethodDefinition Definition { get; }

    public DotNetDefinedMethod(MethodDefinition definition)
    {
        Definition = definition;
        Id = new IlMethodId(definition.FullName);
        ReturnType = new IlTypeName(definition.ReturnType.FullName);
        Parameters = definition.Parameters
            .OrderBy(x => x.Index)
            .Select(x => new Parameter(new IlTypeName(x.ParameterType.FullName), x.Name))
            .ToArray();

        Locals = definition.Body
            .Variables
            .OrderBy(x => x.Index)
            .Select(x => new IlTypeName(x.VariableType.FullName))
            .ToArray();
    }
}