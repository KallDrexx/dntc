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
            new LiteralValueExpression("calloc", variableToAllocate.ResultingType, 0),
            [
                new MethodConversionInfo.Parameter(intType.IlName, "n", false, false),
                new MethodConversionInfo.Parameter(intType.IlName, "size", false, false)
            ],
            [
                countExpression ?? new LiteralValueExpression("1", intType, 0),
                new LiteralValueExpression($"sizeof({cTypeName.Value})", intType, 0)
            ],
            variableToAllocate.ResultingType,
            conversionCatalog);

        var assignment = new AssignmentStatementSet(variableToAllocate, callocCall);

        return assignment;
    }

    public CStatementSet FreeCall(CBaseExpression variableToFree, ConversionCatalog conversionCatalog)
    {
        var voidType = conversionCatalog.Find(new IlTypeName(typeof(void).FullName!));
        var freeCall = new MethodCallExpression(
            new LiteralValueExpression("free", voidType, 0),
            [new MethodConversionInfo.Parameter(variableToFree.ResultingType.IlName, "ptr", true, false)],
            [variableToFree],
            voidType,
            conversionCatalog);

        return new VoidExpressionStatementSet(freeCall);
    }
}