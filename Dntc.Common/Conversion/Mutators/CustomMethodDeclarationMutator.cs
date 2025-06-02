using Dntc.Common.Definitions;

namespace Dntc.Common.Conversion.Mutators;

public class CustomMethodDeclarationMutator : IMethodConversionMutator
{
    public void Mutate(MethodConversionInfo conversionInfo, DotNetDefinedMethod? method)
    {
        if (method?.CustomDeclaration != null)
        {
            conversionInfo.CustomDeclaration = method.CustomDeclaration.Declaration;
        }

        if (method?.CustomDeclaration?.ReferredBy != null)
        {
            conversionInfo.NameInC = method.CustomDeclaration.ReferredBy.Value;
        }
    }
}