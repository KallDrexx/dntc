using System.Text;
using Dntc.Common.Conversion;
using Dntc.Common.OpCodeHandling;
using Dntc.Common.Syntax.Statements;
using Mono.Cecil;

namespace Dntc.Common.Definitions.CustomDefinedMethods;

/// <summary>
/// Writes the ReferenceType__Create() methods.
/// These methods are responsible for:
/// 1. allocating the memory that the reference type will be stored in.
/// 2. clearing the allocated memory.
/// 3. setting up the virtual table of the reference type. (ctors are called seperately)
/// </summary>
public class ReferenceTypeAllocationMethod : CustomDefinedMethod
{
    private readonly TypeDefinition _typeDefinition;

    public ReferenceTypeAllocationMethod(TypeDefinition typeDefinition) 
        :  base(new IlMethodId(typeDefinition.FullName + "__Create"),
            new IlTypeName(typeDefinition.FullName),
            Utils.GetNamespace(typeDefinition),
            Utils.GetHeaderName(Utils.GetNamespace(typeDefinition)),
            Utils.GetSourceFileName(Utils.GetNamespace(typeDefinition)),
            new CFunctionName(Utils.MakeValidCName(typeDefinition.FullName + "__Create")), [])
    {
        _typeDefinition = typeDefinition;

        foreach (var virtualMethod in _typeDefinition.Methods.Where(x => x.IsVirtual))
        {
            if (virtualMethod.IsReuseSlot)
            {
                InvokedMethods.Add(new InvokedMethod(new IlMethodId(virtualMethod.FullName)));
            }
        }
    }

    public sealed override List<InvokedMethod> InvokedMethods { get; } = [];

    public override CustomCodeStatementSet? GetCustomDeclaration()
    {
        var typeName = Utils.MakeValidCName(_typeDefinition.FullName);
        
        return new CustomCodeStatementSet($"{typeName}* {NativeName}(void)");
    }

    public override CustomCodeStatementSet? GetCustomImplementation(ConversionCatalog catalog)
    {
        var typeName = Utils.MakeValidCName(_typeDefinition.FullName);

        var sb = new StringBuilder();
        
        sb.AppendLine($@"    {typeName}* result = ({typeName}*) malloc(sizeof({typeName}));
	memset(result, 0, sizeof({typeName}));");
        
        foreach (var virtualMethod in _typeDefinition.Methods.Where(x => x.IsVirtual))
        {
            if (virtualMethod.IsReuseSlot)
            {
                var sourceMethod = catalog.Find(new IlMethodId(virtualMethod.FullName));
                var baseType = virtualMethod.DeclaringType.BaseType.Resolve();
                    
                var targetMethod = FindMatchingMethodInBaseTypes(catalog, sourceMethod, baseType);

                if (targetMethod != null)
                {
                    var thisExpression = targetMethod.Parameters[0];

                    sb.Append($"\t(({thisExpression.ConversionInfo.NameInC}*)result)->{targetMethod.NameInC} = (");

                    var methodInfo = targetMethod;
                    sb.Append($"{methodInfo.ReturnTypeInfo.NativeNameWithPossiblePointer()} (*)(");
                    
                    for (var x = 0; x < methodInfo.Parameters.Count; x++)
                    {
                        if (x > 0) sb.Append(", ");
                        var param = methodInfo.Parameters[x];
                        var paramType = param.ConversionInfo;

                        var pointerSymbol = param.IsReference ? "*" : "";
                        sb.Append($"{paramType.NameInC}{pointerSymbol}");
                    }
                        
                    sb.AppendLine($")){sourceMethod.NameInC};");
                }
            }
            else
            {
                var methodInfo = catalog.Find(new IlMethodId(virtualMethod.FullName));

                sb.AppendLine($"\tresult->{methodInfo.NameInC} = {methodInfo.NameInC};");
            }
        }

        sb.AppendLine("\treturn result;");

        return new CustomCodeStatementSet(sb.ToString());
    }
    
    private MethodConversionInfo? FindMatchingMethodInBaseTypes(ConversionCatalog catalog, MethodConversionInfo sourceMethod, TypeDefinition startingBaseType)
    {
        var currentBaseType = startingBaseType;
    
        while (currentBaseType != null)
        {
            var type = catalog.Find(new IlTypeName(currentBaseType.FullName));
        
            foreach (var method in type.OriginalTypeDefinition.Methods)
            {
                var methodInfo = catalog.Find(method);
            
                if (sourceMethod.Name != methodInfo.Name)
                    continue;
            
                if (sourceMethod.ReturnTypeInfo != methodInfo.ReturnTypeInfo)
                    continue;

                if (sourceMethod.Parameters.Count != methodInfo.Parameters.Count)
                    continue;

                bool parametersMatch = true;
                for (int i = 1; i < sourceMethod.Parameters.Count; i++)
                {
                    if (sourceMethod.Parameters[i].ConversionInfo != 
                        methodInfo.Parameters[i].ConversionInfo)
                    {
                        parametersMatch = false;
                        break;
                    }
                }

                if (parametersMatch)
                {
                    return methodInfo;
                }
            }
        
            // Move to the next base type in the hierarchy
            currentBaseType = currentBaseType.BaseType.Resolve();
        }
    
        // If we've gone through all base types and found nothing
        return null;
    }
}