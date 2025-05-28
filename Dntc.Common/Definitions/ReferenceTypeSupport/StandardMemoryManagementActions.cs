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
        CBaseExpression variableToAllocate,
        LiteralValueExpression cTypeName,
        ConversionCatalog conversionCatalog,
        CBaseExpression? countExpression = null)
    {
        var intType = conversionCatalog.Find(new IlTypeName(typeof(int).FullName!));
        var callocCall = new MethodCallExpression(
            new LiteralValueExpression("calloc", variableToAllocate.ResultingType),
            [
                new MethodConversionInfo.Parameter(intType.IlName, "n", false),
                new MethodConversionInfo.Parameter(intType.IlName, "size", false)
            ],
            [
                countExpression ?? new LiteralValueExpression("1", intType),
                new LiteralValueExpression($"sizeof({cTypeName.Value})", intType)
            ],
            variableToAllocate.ResultingType,
            conversionCatalog);

        var assignment = new AssignmentStatementSet(variableToAllocate, callocCall);

        return assignment;
    }

    public CStatementSet FreeCall(Variable variableToFree, ConversionCatalog conversionCatalog)
    {
        var voidType = conversionCatalog.Find(new IlTypeName(typeof(void).FullName!));
        var freeCall = new MethodCallExpression(
            new LiteralValueExpression("free", voidType),
            [new MethodConversionInfo.Parameter(variableToFree.Type.IlName, "ptr", true)],
            [new VariableValueExpression(variableToFree)],
            voidType,
            conversionCatalog);

        return new VoidExpressionStatementSet(freeCall);
    }
}