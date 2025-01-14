using Dntc.Attributes;
using Mono.Cecil;

namespace Dntc.Common.Definitions.Definers;

/// <summary>
/// Creates a `NativeDefinedMethod` based on the method being passed in. Requires the
/// `NativeFunctionCallAttribute` on the method.
/// </summary>
public class NativeFunctionCallAttributeDefiner : IDotNetMethodDefiner
{
    public DefinedMethod? Define(MethodDefinition method)
    {
        var nativeFunctionCallAttribute = method.CustomAttributes
            .SingleOrDefault(x => x.AttributeType.FullName == typeof(NativeFunctionCallAttribute).FullName);

        if (nativeFunctionCallAttribute == null)
        {
            return null;
        }
        
        if (nativeFunctionCallAttribute.ConstructorArguments.Count < 1)
        {
            var message = $"NativeFunctionCallAttribute on '{method.FullName}' expected to have at least " +
                          $"1 constructor arguments but none were found";

            throw new InvalidOperationException(message);
        }

        var hasHeaderSpecified = nativeFunctionCallAttribute.ConstructorArguments.Count > 1 &&
                                 nativeFunctionCallAttribute.ConstructorArguments[1].Value != null;

        var headers = new List<HeaderName>();
        if (hasHeaderSpecified)
        {
            headers = nativeFunctionCallAttribute.ConstructorArguments[1]
                .Value
                .ToString()?
                .Split(',')
                .Select(x => new HeaderName(x))
                .ToList() ?? [];

        }

        var id = method.HasGenericParameters
            ? Utils.NormalizeGenericMethodId(method.FullName, method.GenericParameters)
            : new IlMethodId(method.FullName);

        return new NativeDefinedMethod(
            id,
            new IlTypeName(method.ReturnType.FullName),
            headers,
            new CFunctionName(nativeFunctionCallAttribute.ConstructorArguments[0].Value.ToString()!),
            new IlNamespace(method.DeclaringType.Namespace),
            method.Parameters.Select(x => new IlTypeName(x.ParameterType.FullName)).ToArray(),
            method);
    }
}