using System.Text.RegularExpressions;
using Dntc.Attributes;
using Dntc.Common.Conversion;
using Mono.Cecil;

namespace Dntc.Common;

public static class Utils
{
    public static string IlOffsetToLabel(int ilOffset) => $"IL_{ilOffset:x4}";

    public static string LocalName(int localIndex) => $"__local_{localIndex}";

    public static CSourceFileName GetSourceFileName(IlNamespace csharpNamespace)
    {
        return new CSourceFileName($"{MakeValidCName(csharpNamespace.Value)}.c");
    }

    public static HeaderName GetHeaderName(IlNamespace csharpNamespace)
    {
        return new HeaderName($"{MakeValidCName(csharpNamespace.Value)}.h");
    }

    public static string MakeValidCName(string name)
    {
        return name.Replace('<', '_')
            .Replace('>', '_')
            .Replace(".", "_")
            .Replace("/", "_") // Instance methods have the type name with a slash in it
            .Replace("::", "_");
    }

    public static (CSourceFileName, HeaderName)? GetCustomFileName(
        IEnumerable<CustomAttribute> customAttributes, 
        string attachedItemName)
    {
        var attribute = customAttributes
            .SingleOrDefault(x => x.AttributeType.FullName == typeof(CustomFileNameAttribute).FullName);

        if (attribute == null)
        {
            return null;
        }
        
        if (attribute.ConstructorArguments.Count != 2)
        {
            var message =
                $"Expected {attachedItemName}'s {typeof(CustomFileNameAttribute).FullName}'s " +
                $"specification 2 have 2 arguments, but {attribute.ConstructorArguments.Count} were present";

            throw new InvalidOperationException(message);
        }
        
        var sourceFileName = attribute.ConstructorArguments[0].Value.ToString();
        var headerName = attribute.ConstructorArguments[1].Value.ToString();
        
        if (sourceFileName == null || headerName == null)
        {
            var message = $"{attachedItemName}'s {typeof(CustomFileNameAttribute).FullName}'s had a null source " +
                          $"file name and/or a null header file name";
            
            throw new InvalidOperationException(message);
        }

        return new(new CSourceFileName(sourceFileName), new HeaderName(headerName));
    }

    public static CSourceFileName ToSourceFileName(HeaderName headerName)
    {
        var headerNameString = headerName.Value;
        if (headerNameString.EndsWith(".h"))
        {
            headerNameString = headerNameString[..^1] + 'c';
        }

        return new CSourceFileName(headerNameString);
    }

    public static IlMethodId NormalizeGenericMethodId(
        string signature,
        Mono.Collections.Generic.Collection<GenericParameter> parameters)
    {
        for (var x = 0; x < parameters.Count; x++)
        {
            var name = parameters[x].FullName;
            // We need to replace any part of the method signature that's referring to a
            // named generic parameter and replace it with the index. There's probably a 
            // better way to do this, but I think this will work?
            //
            // The regex will match any parameters that consist of the name. The type name must
            // start with either a parenthesis (first param) or comma (not first param). It must 
            // also end with a parenthesis (last param) or comma. Use regex groupings to keep
            // the preceding and trailing characters as expected.
            var parameterPattern = new Regex($"([\\(,]){name}([,\\) \\&\\*])");
            var returnTypePattern = new Regex($"^{name}([ \\*])");
            signature = parameterPattern.Replace(signature, $"$1!!{x}$2");
            signature = returnTypePattern.Replace(signature, $"!!{x}$1");
        }

        return new IlMethodId(signature);
    }

    public static IlNamespace GetNamespace(TypeDefinition type)
    {
        while (type.DeclaringType != null)
        {
            type = type.DeclaringType;
        }

        if (string.IsNullOrEmpty(type.Namespace))
        {
            var message = $"Root type {type.FullName} did not have a namespace";
            throw new InvalidOperationException(message);
        }

        return new IlNamespace(type.Namespace);
    }

    public static string NativeNameWithPossiblePointer(this TypeConversionInfo typeInfo)
    {
        return typeInfo.IsPointer
            ? $"{typeInfo.NameInC}*"
            : typeInfo.NameInC.Value;
    }
}