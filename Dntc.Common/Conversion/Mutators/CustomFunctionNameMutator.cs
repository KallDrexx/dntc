using Dntc.Attributes;
using Dntc.Common.Definitions;

namespace Dntc.Common.Conversion.Mutators;

public class CustomFunctionNameMutator : IMethodConversionMutator
{
    public void Mutate(MethodConversionInfo conversionInfo, DotNetDefinedMethod? method)
    {
        var customNameAttribute = method?.Definition
            .CustomAttributes
            .FirstOrDefault(x => x.AttributeType.FullName == typeof(CustomFunctionNameAttribute).FullName);

        if (customNameAttribute == null)
        {
            return;
        }
        
        var functionName = customNameAttribute.ConstructorArguments[0].Value.ToString()!;
        conversionInfo.NameInC = new CFunctionName(Utils.MakeValidCName(functionName));
    }
}