using Dntc.Common.Conversion;
using Dntc.Common.Syntax.Expressions;
using Dntc.Common.Syntax.Statements;

namespace Dntc.Common.Definitions.ReferenceTypeSupport;

/// <summary>
/// Provides capabilities for standard C memory management functions
/// </summary>
public class StandardMemoryManagementActions : IMemoryManagementActions
{
    public IReadOnlyList<HeaderName> RequiredHeaders => [new("<stdlib.h>")];

    public CStatementSet AllocateCall(
        Variable variableToAllocate,
        LiteralValueExpression cTypeName,
        ConversionCatalog conversionCatalog)
    {
        var intType = conversionCatalog.Find(new IlTypeName(typeof(int).FullName!));
        var variableValueExpression = new VariableValueExpression(variableToAllocate);
        var callocCall = new MethodCallExpression(
            new LiteralValueExpression("calloc", variableToAllocate.Type),
            [
                new MethodConversionInfo.Parameter(intType, "n", false),
                new MethodConversionInfo.Parameter(intType, "size", false)
            ],
            [
                new LiteralValueExpression("1", intType),
                new LiteralValueExpression($"sizeof({cTypeName.Value})", intType)
            ],
            variableToAllocate.Type);

        var assignment = new AssignmentStatementSet(variableValueExpression, callocCall);

        return assignment;
    }

    public CStatementSet FreeCall(Variable variableToFree, ConversionCatalog conversionCatalog)
    {
        var voidType = conversionCatalog.Find(new IlTypeName(typeof(void).FullName!));
        var freeCall = new MethodCallExpression(
            new LiteralValueExpression("free", voidType),
            [new MethodConversionInfo.Parameter(variableToFree.Type, "ptr", true)],
            [new VariableValueExpression(variableToFree)],
            voidType);

        return new VoidExpressionStatementSet(freeCall);
    }
}