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
/// 3. setting up the virtual table of the reference type. (ctors are called seperately)
/// </summary>
public class ReferenceTypeAllocationMethod : CustomDefinedMethod
{
    private readonly TypeDefinition _typeDefinition;
    private readonly IMemoryManagementActions _memoryManagement;

    public ReferenceTypeAllocationMethod(IMemoryManagementActions memoryManagement, TypeDefinition typeDefinition)
        :  base(new IlMethodId(typeDefinition.FullName + "__Create"),
            new IlTypeName(typeDefinition.FullName),
            Utils.GetNamespace(typeDefinition),
            Utils.GetHeaderName(Utils.GetNamespace(typeDefinition)),
            Utils.GetSourceFileName(Utils.GetNamespace(typeDefinition)),
            new CFunctionName(Utils.MakeValidCName(typeDefinition.FullName + "__Create")), [])
    {
        _typeDefinition = typeDefinition;
        _memoryManagement = memoryManagement;
        ReferencedHeaders = memoryManagement.RequiredHeaders;

        foreach (var virtualMethod in _typeDefinition.Methods.Where(x => x.IsVirtual))
        {
            if (virtualMethod.IsReuseSlot)
            {
                InvokedMethods.Add(new InvokedMethod(new IlMethodId(virtualMethod.FullName)));
            }
        }
    }

    public sealed override List<InvokedMethod> InvokedMethods { get; } = [];

    public override CustomCodeStatementSet? GetCustomDeclaration(ConversionCatalog catalog)
    {
        var typeName = Utils.MakeValidCName(_typeDefinition.FullName);
        
        return new CustomCodeStatementSet($"{typeName}* {NativeName}(void)");
    }

    public override CStatementSet? GetCustomImplementation(ConversionCatalog catalog)
    {
        var typeInfo = catalog.Find(new IlTypeName(_typeDefinition.FullName));
        var typeNameExpression = new LiteralValueExpression(typeInfo.NameInC.Value, typeInfo);
        var variable = new Variable(typeInfo, "result", true);
        var statements = new List<CStatementSet>
        {
            new LocalDeclarationStatementSet(variable),
            _memoryManagement.AllocateCall(variable, typeNameExpression, catalog)
        };

        // Set the prep for free function pointer to the correct function
        var referenceBaseTypeInfo = catalog.Find(ReferenceTypeConstants.ReferenceTypeBaseId);
        var voidType = catalog.Find(new IlTypeName(typeof(void).FullName!));
        var cast = new CastExpression(new VariableValueExpression(variable), referenceBaseTypeInfo, true);
        var fnPointerAccess = new FieldAccessExpression(
            cast,
            new Variable(voidType, ReferenceTypeConstants.PrepForFreeFnPointerName.Value, false));

        var prepFnInfo = catalog.Find(
            ReferenceTypeConstants.PrepTypeToFreeMethodId(
                new IlTypeName(_typeDefinition.FullName)));

        var prepFnExpression = new LiteralValueExpression(prepFnInfo.NameInC.Value, voidType);
        var assignment = new AssignmentStatementSet(fnPointerAccess, prepFnExpression);
        statements.Add(assignment);

        var sb = new StringBuilder();
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
                    $"\tresult->{methodInfo.NameInC} = {methodInfo.NameInC};)");

                statements.Add(customStatement);
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
}