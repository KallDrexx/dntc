using System.Text;
using Dntc.Common.Conversion;
using Dntc.Common.Definitions.ReferenceTypeSupport;
using Dntc.Common.OpCodeHandling;
using Dntc.Common.Syntax.Expressions;
using Dntc.Common.Syntax.Statements;
using Mono.Cecil;

namespace Dntc.Common.Definitions.CustomDefinedMethods;

/// <summary>
/// Writes the ReferenceType__Create() methods.
/// These methods are responsible for:
/// 1. allocating the memory that the reference type will be stored in.
/// 2. clearing the allocated memory.
/// 3. setting up the virtual table of the reference type. (ctors are called separately)
/// </summary>
public class ReferenceTypeAllocationMethod : CustomDefinedMethod
{
    public TypeReference TypeReference { get; }
    private readonly IMemoryManagementActions _memoryManagement;

    public ReferenceTypeAllocationMethod(IMemoryManagementActions memoryManagement, TypeReference typeReference)
        :  base(
            FormIlMethodId(typeReference),
            new IlTypeName(typeReference.FullName),
            Utils.GetNamespace(typeReference),
            Utils.GetHeaderName(Utils.GetNamespace(typeReference)),
            Utils.GetSourceFileName(Utils.GetNamespace(typeReference)),
            FormNativeName(typeReference.FullName),
            [])
    {
        TypeReference = typeReference;
        _memoryManagement = memoryManagement;
        ReferencedHeaders = memoryManagement.RequiredHeaders;

        if (typeReference is TypeDefinition definition)
        {
            foreach (var virtualMethod in definition.Methods.Where(x => x.IsVirtual))
            {
                if (virtualMethod.IsReuseSlot)
                {
                    InvokedMethods.Add(new InvokedMethod(new IlMethodId(virtualMethod.FullName)));
                }
            }
        }
    }

    public static IlMethodId FormIlMethodId(TypeReference typeReference) => new($"{typeReference.FullName}__Create");

    public static CFunctionName FormNativeName(string prefix) => new(Utils.MakeValidCName($"{prefix}__Create"));

    public sealed override List<InvokedMethod> InvokedMethods { get; } = [];

    public override CustomCodeStatementSet? GetCustomDeclaration(ConversionCatalog catalog)
    {
        var returnType = catalog.Find(ReturnType);
        return new CustomCodeStatementSet($"{returnType.NameInC}* {NativeName}(void)");
    }

    public override CStatementSet? GetCustomImplementation(ConversionCatalog catalog)
    {
        var typeInfo = catalog.Find(new IlTypeName(TypeReference.FullName));
        var typeNameExpression = new LiteralValueExpression(typeInfo.NameInC.Value, typeInfo, 0);
        var variable = new Variable(typeInfo, "result", 1);
        var variableExpression = new VariableValueExpression(variable);
        var statements = new List<CStatementSet>
        {
            new LocalDeclarationStatementSet(variable),
            _memoryManagement.AllocateCall(variableExpression, typeNameExpression, catalog)
        };

        AssignPrepPointer(catalog, variable, statements);

        if (TypeReference is TypeDefinition definition)
        {
            var sb = new StringBuilder();
            foreach (var virtualMethod in definition.Methods.Where(x => x.IsVirtual))
            {
                if (virtualMethod.IsReuseSlot)
                {
                    var sourceMethod = catalog.Find(new IlMethodId(virtualMethod.FullName));
                    var baseType = virtualMethod.DeclaringType.BaseType.Resolve();

                    var targetMethod = FindMatchingMethodInBaseTypes(catalog, sourceMethod, baseType);

                    if (targetMethod != null)
                    {
                        var thisExpression = targetMethod.Parameters[0];
                        var thisExpressionInfo = catalog.Find(thisExpression.TypeName);

                        sb.Clear();
                        sb.Append($"\t(({thisExpressionInfo.NameInC}*)result)->{targetMethod.NameInC} = (");

                        var methodInfo = targetMethod;
                        sb.Append($"{methodInfo.ReturnTypeInfo.NativeNameWithPossiblePointer()} (*)(");

                        for (var x = 0; x < methodInfo.Parameters.Count; x++)
                        {
                            if (x > 0) sb.Append(", ");
                            var param = methodInfo.Parameters[x];
                            var paramInfo = catalog.Find(param.TypeName);
                            var paramType = paramInfo;

                            var pointerSymbol = param.IsReference ? "*" : "";
                            sb.Append($"{paramType.NameInC}{pointerSymbol}");
                        }

                        sb.AppendLine($")){sourceMethod.NameInC};");
                        statements.Add(new CustomCodeStatementSet(sb.ToString()));
                    }
                }
                else
                {
                    var methodInfo = catalog.Find(new IlMethodId(virtualMethod.FullName));
                    var customStatement = new CustomCodeStatementSet(
                        $"\tresult->{methodInfo.NameInC} = {methodInfo.NameInC};");

                    statements.Add(customStatement);
                }
            }
        }

        statements.Add(new ReturnStatementSet(new VariableValueExpression(variable)));

        return new CompoundStatementSet(statements);
    }

    private MethodConversionInfo? FindMatchingMethodInBaseTypes(ConversionCatalog catalog, MethodConversionInfo sourceMethod, TypeDefinition startingBaseType)
    {
        var currentBaseType = startingBaseType;
    
        while (currentBaseType != null)
        {
            foreach (var method in currentBaseType.Methods.Where(x=>x.IsVirtual && x.IsNewSlot))
            {
                if (catalog.TryFind(new IlMethodId(method.FullName), out var c))
                {
                    if (sourceMethod.SignatureCompatibleWith(c, catalog))
                    {
                        return c;
                    }
                }
            }
        
            // Move to the next base type in the hierarchy
            currentBaseType = currentBaseType.BaseType.Resolve();
        }
    
        // If we've gone through all base types and found nothing
        return null;
    }

    private void AssignPrepPointer(ConversionCatalog catalog, Variable variable, List<CStatementSet> statements)
    {
        // Set the prep for free function pointer to the correct function
        // We can't use expressions since we would need a custom type info for the pointer type
        var referenceBaseTypeInfo = catalog.Find(ReferenceTypeConstants.ReferenceTypeBaseId);
        var prepFnInfo = catalog.Find(
            ReferenceTypeConstants.PrepTypeToFreeMethodId(
                new IlTypeName(TypeReference.FullName)));

        statements.Add(
            new CustomCodeStatementSet(
                $"\t((({referenceBaseTypeInfo.NameInC}*){variable.Name})->PrepForFree) = (void (*)(void*)){prepFnInfo.NameInC};\n"));
    }
}