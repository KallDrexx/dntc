using Dntc.Attributes;
using Mono.Cecil;

namespace Dntc.Common.OpCodeHandling;

public record NativeGlobalOnTranspileInfo
{
    public string NativeName { get; }
    public HeaderName? HeaderName { get; }

    private NativeGlobalOnTranspileInfo(string nativeName, string? headerName)
    {
        NativeName = nativeName;
        HeaderName = headerName != null ? new HeaderName(headerName) : null;
    }

    public static NativeGlobalOnTranspileInfo? FromAttributes(
        IEnumerable<CustomAttribute> customAttributes, 
        string fieldName)
    {
        var attribute = customAttributes
            .SingleOrDefault(x => x.AttributeType.FullName == typeof(NativeGlobalAttribute).FullName);

        if (attribute == null)
        {
            return null;
        }

        if (attribute.ConstructorArguments.Count != 2)
        {
            var message =
                $"Expected {fieldName}'s {typeof(NativeGlobalAttribute).FullName}'s " +
                $"specification 2 have 2 arguments, but only 1 was present";

            throw new InvalidOperationException(message);
        }

        var nativeName = attribute.ConstructorArguments[0].Value.ToString();
        var header = attribute.ConstructorArguments[1].Value.ToString();
        
        if (nativeName == null)
        {
            var message = $"{fieldName}'s {typeof(NativeGlobalAttribute).FullName}'s had a null native name";
            throw new InvalidOperationException(message);
        }

        return new NativeGlobalOnTranspileInfo(nativeName, header);
    }
}
