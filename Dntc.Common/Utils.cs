using System.Text.RegularExpressions;
using Dntc.Attributes;
using Dntc.Common.Conversion;
using Dntc.Common.Definitions;
using Mono.Cecil;

namespace Dntc.Common;

public static class Utils
{
    public static string IlOffsetToLabel(int ilOffset, MethodConversionInfo method) => $"{method.NameInC}_IL_{ilOffset:x4}";

    public static string ThisArgumentName => "__this";

    public static string LocalName(int localIndex) => $"__local_{localIndex}";

    public static string LocalName(MethodDefinition method, int localIndex)
    {
        if (method.DebugInformation.TryGetName(method.Body.Variables[localIndex],
                out var name))
        {
            return name;
        }
        
        return LocalName(localIndex);
    }

    public static string ReturnVariableName() => "__return_value";

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

    public static IlNamespace GetNamespace(TypeReference type)
    {
        if (type is ArrayType arrayType)
        {
            type = arrayType.ElementType;
        }

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
        return typeInfo.IsPointer || typeInfo.IsReferenceType
            ? $"{typeInfo.NameInC}*"
            : typeInfo.NameInC.Value;
    }
    
    public static string NativeNameWithPointer(this TypeConversionInfo typeInfo)
    {
        return $"{typeInfo.NameInC}*";
    }

    public static string NativeTypeName(this Variable variable)
    {
        return variable.PointerDepth > 0
            ? variable.Type.NativeNameWithPointer()
            : variable.Type.NativeNameWithPossiblePointer(); // todo pointer to pointer (pointer to reference type)
    }

    public static CustomAttribute? GetCustomAttribute(Type attributeType, MethodDefinition method)
    {
        return method.CustomAttributes
            .SingleOrDefault(x => x.AttributeType.FullName == attributeType.FullName);
    }

    public static CustomAttribute? GetCustomAttribute(Type attributeType, FieldDefinition field)
    {
        return field.CustomAttributes
            .SingleOrDefault(x => x.AttributeType.FullName == attributeType.FullName);
    }

    public static bool IsConsideredReferenceType(this ParameterDefinition definition)
    {
        return definition.ParameterType.IsByReference ||
               definition.ParameterType.IsPointer ||
               !(definition.ParameterType.IsValueType || definition.ParameterType.IsGenericParameter ||
                 definition.ParameterType.IsFunctionPointer);
    }

    public static bool IsOverrideOf(this MethodDefinition method, MethodDefinition baseMethod)
    {
        // Method must be virtual and reuse slot (override keyword)
        if (!method.IsVirtual || !method.IsReuseSlot)
            return false;

        return method.SignatureCompatibleWith(baseMethod);
    }
    
    public static bool SignatureCompatibleWith(this MethodDefinition method, MethodDefinition other)
    {
        // Names must match
        if (method.Name != other.Name)
            return false;
        
        // Return types must be compatible
        if (method.ReturnType.FullName != other.ReturnType.FullName)
            return false;
        
        // Parameter counts must match
        if (method.Parameters.Count != other.Parameters.Count)
            return false;
        
        // Parameter types must match
        for (int i = 0; i < method.Parameters.Count; i++)
        {
            // TODO check if the parameters can be cast.
            if (method.Parameters[i].ParameterType.FullName != other.Parameters[i].ParameterType.FullName)
                return false;
        }
    
        return true;
    }
    
    public static bool IsSubclassOf(this TypeDefinition type, TypeDefinition baseType)
    {
        // Check direct inheritance chain
        var current = type;
        while (current.BaseType != null)
        {
            if (current.BaseType.FullName == baseType.FullName)
                return true;
            
            // Continue up the inheritance chain
            var resolved = current.BaseType.Resolve();
            if (resolved == null) 
                break;
            
            current = resolved;
        }
    
        return false;
    }
    
    public static bool SignatureCompatibleWith(
        this MethodConversionInfo method,
        MethodConversionInfo other,
        ConversionCatalog catalog)
    {
        // Names must match
        if (method.Name != other.Name)
            return false;
        
        // Return types must be compatible
        if (method.ReturnTypeInfo.IlName.Value != other.ReturnTypeInfo.IlName.Value)
            return false;
        
        // Parameter counts must match
        if (method.Parameters.Count != other.Parameters.Count)
            return false;
        
        // Parameter types must match
        for (var i = 0; i < method.Parameters.Count; i++)
        {
            var methodParamInfo = catalog.Find(method.Parameters[i].TypeName);
            var otherParamInfo = catalog.Find(other.Parameters[i].TypeName);
            if (methodParamInfo.OriginalTypeDefinition is DotNetDefinedType t1 &&
                otherParamInfo.OriginalTypeDefinition is DotNetDefinedType t2)
            {
                if (t1.IlName == t2.IlName)
                {
                    return true;
                }

                if (t1.Definition.IsSubclassOf(t2.Definition))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public static TypeReference? GetNonSystemBaseType(TypeDefinition type)
    {
        if (type.BaseType == null)
        {
            return null;
        }

        if (type.BaseType.Namespace.StartsWith("System"))
        {
            return null;
        }

        return type.BaseType;
    }
}