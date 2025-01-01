using Dntc.Common.Definitions;

namespace Dntc.Common.Conversion.Mutators;

public class CustomNameTypeMutator : ITypeConversionMutator
{
    public void Mutate(TypeConversionInfo conversionInfo, DotNetDefinedType type)
    {
        var customNaming = Utils.GetCustomFileName(type.Definition.CustomAttributes, type.IlName.Value);
        if (customNaming == null)
        {
            return;
        }
        
        // TODO: Figure out a better way to do this without forcing it to be after the Ignored in header mutator
        // This has to go after the ignore in header one as the ignore in header will simply swap the 
        // header and source file fields, and that may cause it to have the unintended name.
        
        if (conversionInfo.Header != null)
        {
            conversionInfo.Header = customNaming.Value.Item2;
        }

        if (conversionInfo.SourceFileName != null)
        {
            conversionInfo.SourceFileName = customNaming.Value.Item1;
        }
    }
}