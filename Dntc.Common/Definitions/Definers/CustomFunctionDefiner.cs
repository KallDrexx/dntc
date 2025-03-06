using Dntc.Attributes;
using Dntc.Common.Syntax.Statements;
using Mono.Cecil;

namespace Dntc.Common.Definitions.Definers;

public class CustomFunctionDefiner : IDotNetMethodDefiner
{
    public DefinedMethod? Define(MethodDefinition method)
    {
        var attribute = Utils.GetCustomAttribute(typeof(CustomFunctionAttribute), method);
        if (attribute == null)
        {
            return null;
        }

        var declaration = attribute.ConstructorArguments[0].Value.ToString()!;
        var implementation = attribute.ConstructorArguments[1].Value?.ToString();
        var name = attribute.ConstructorArguments[2].Value.ToString()!;

        // TODO: Generic methods need their ids normalized. There should be an easier way
        // to do this so you can't forget to do so in a custom definer.
        var methodId = method.HasGenericParameters
            ? Utils.NormalizeGenericMethodId(method.FullName, method.GenericParameters)
            : new IlMethodId(method.FullName);

        return new CustomDefinition(
            declaration,
            implementation,
            new CFunctionName(name),
            method,
            methodId,
            new IlTypeName(method.ReturnType.FullName),
            Utils.GetNamespace(method.DeclaringType),
            method.Parameters
                .Select(x => new DefinedMethod.Parameter(new IlTypeName(x.ParameterType.FullName), x.Name, true))
                .ToArray());
    }

    private class CustomDefinition : CustomDefinedMethod
    {
        private readonly string _declaration;
        private readonly string? _implementation;
        private readonly MethodDefinition _methodDefinition;

        public CustomDefinition(
            string declaration,
            string? implementation,
            CFunctionName functionName,
            MethodDefinition methodDefinition,
            IlMethodId methodId,
            IlTypeName returnType,
            IlNamespace ilNamespace,
            IReadOnlyList<Parameter> parameters)
            : base(methodId, returnType, ilNamespace, Utils.GetHeaderName(ilNamespace),
                Utils.GetSourceFileName(ilNamespace), functionName, parameters)
        {
            _declaration = declaration;
            _implementation = implementation;
            _methodDefinition = methodDefinition;
            IsMacroDefinition = declaration.StartsWith("#define "); // kind of a hack
        }

        public override CustomCodeStatementSet? GetCustomDeclaration()
        {
            return new CustomCodeStatementSet(_declaration);
        }

        public override CustomCodeStatementSet? GetCustomImplementation()
        {
            return _implementation != null
                ? new CustomCodeStatementSet(_implementation)
                : null;
        }
    }
}