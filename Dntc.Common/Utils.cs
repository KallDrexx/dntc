using Dntc.Common.Conversion;
using Dntc.Common.Definitions;

namespace Dntc.Common;

public static class Utils
{
    public static string IlOffsetToLabel(int ilOffset) => $"IL_{ilOffset:x4}";

    public static string LocalName(int localIndex) => $"__local_{localIndex}";

    public static string StaticFieldName(TypeConversionInfo containingType, DefinedType.Field field)
    {
        if (!field.isStatic)
        {
            var message = $"Field '{containingType.IlName}.{field.Name} is not static";
            throw new NotSupportedException(message);
        }

        return $"{containingType.NameInC}_{MakeValidCName(field.Name)}";
    }

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
            .Replace("/", "_"); // Instance methods have the type name with a slash in it
    }
}