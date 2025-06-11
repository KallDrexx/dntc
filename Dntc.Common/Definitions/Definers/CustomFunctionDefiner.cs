using Dntc.Attributes;
using Dntc.Common.Conversion;
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
                .Select(x => new DefinedMethod.Parameter(new IlTypeName(x.ParameterType.FullName), x.Name, x.IsConsideredReferenceType(), false))
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
                Utils.GetSourceFileName(ilNamespace), functionName, parameters, implementation != null)
        {
            _declaration = declaration;
            _implementation = implementation;
            _methodDefinition = methodDefinition;
            IsMacroDefinition = declaration.StartsWith("#define "); // kind of a hack
        }

        public override CustomCodeStatementSet? GetCustomDeclaration(ConversionCatalog catalog)
        {
            return new CustomCodeStatementSet(_declaration);
        }

        public override CustomCodeStatementSet? GetCustomImplementation(ConversionCatalog catalog)
        {
            return _implementation != null
                ? new CustomCodeStatementSet(_implementation)
                : null;
        }

        public override DefinedMethod MakeGenericInstance(
            IlMethodId methodId,
            IReadOnlyList<IlTypeName> genericArguments)
        {
            if (!IsMacroDefinition)
            {
                // For now, we only support making a generic instance of a custom function that is detected as
                // having a macro declaration (i.e. starts with #define).  This is because non-macros would need
                // their declarations, implementations (probably), and names customized based on the generic
                // arguments provided. That's complex, so we'll punt that until it's actually desired.
                var message = $"Custom function attribute on {_methodDefinition.FullName} does not have a macro " +
                              $"definition, and thus cannot have a generic instance made from it.";
                throw new InvalidOperationException(message);
            }

            // NOTE: This will cause the same macro to be declared multiple times. Not sure of a good
            // way to prevent that yet.

            // TODO: this is common code that should be generalized
            var newParameters = new List<Parameter>();
            foreach (var parameter in Parameters)
            {
                var genericIndex = (int?)null;
                for (var x = 0; x < _methodDefinition.GenericParameters.Count; x++)
                {
                    var generic = _methodDefinition.GenericParameters[x];
                    if (generic.FullName == parameter.Type.GetNonPointerOrRef().Value)
                    {
                        genericIndex = x;
                        break;
                    }
                }

                if (genericIndex != null)
                {
                    newParameters.Add(parameter with { Type = genericArguments[genericIndex.Value] });
                }
                else
                {
                    newParameters.Add(parameter);
                }
            }

            var returnType = ReturnType.GetNonPointerOrRef();
            for (var x = 0; x < _methodDefinition.GenericParameters.Count; x++)
            {
                if (_methodDefinition.GenericParameters[x].FullName == returnType.Value)
                {
                    returnType = genericArguments[x];
                }
            }

            if (ReturnType.IsPointer())
            {
                returnType = returnType.AsPointerType();
            }

            return new CustomDefinition(_declaration, _implementation, NativeName, _methodDefinition, methodId, returnType, Namespace, newParameters);
        }
    }
}