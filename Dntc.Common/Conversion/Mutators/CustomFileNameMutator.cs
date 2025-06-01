using Dntc.Common.Definitions;
using Mono.Cecil;

namespace Dntc.Common.Conversion.Mutators;

public class CustomFileNameMutator : ITypeConversionMutator, IMethodConversionMutator, IFieldConversionMutator
{
    public void Mutate(TypeConversionInfo conversionInfo)
    {
        if (conversionInfo.OriginalTypeDefinition is not DotNetDefinedType type)
        {
            return;
        }

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

    public void Mutate(MethodConversionInfo conversionInfo, DotNetDefinedMethod? method)
    {
        if (method == null)
        {
            return;
        }

        var customNaming = Utils.GetCustomFileName(method.Definition.CustomAttributes, method.Id.Value);
        if (customNaming == null)
        {
            return;
        }

        conversionInfo.SourceFileName = customNaming.Value.Item1;

        if (conversionInfo.Header != null)
        {
            // Only add the header if the header value isn't already null. A null header usually means
            // this type didn't want to be declared in a header.
            conversionInfo.Header = customNaming.Value.Item2;
        }
    }

    public IReadOnlySet<IlTypeName> RequiredTypes => new HashSet<IlTypeName>();

    public void Mutate(FieldConversionInfo conversionInfo, FieldDefinition field)
    {
        var customNaming = Utils.GetCustomFileName(field.CustomAttributes, field.FullName);
        if (customNaming == null)
        {
            return;
        }

        conversionInfo.SourceFileName = customNaming.Value.Item1;
        if (conversionInfo.Header != null)
        {
            // Only add the header if the header value isn't already null. A null header usually means
            // this type didn't want to be declared in a header.
            conversionInfo.Header = customNaming.Value.Item2;
        }
    }
}