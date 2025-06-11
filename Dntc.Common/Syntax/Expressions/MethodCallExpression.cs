using Dntc.Common.Conversion;

namespace Dntc.Common.Syntax.Expressions;

public record MethodCallExpression : CBaseExpression
{
    private CBaseExpression FnExpression { get; set; }
    private IReadOnlyList<MethodConversionInfo.Parameter> Parameters { get; }
    private IReadOnlyList<CBaseExpression> Arguments { get; }
    private TypeConversionInfo ReturnType { get; }
    private ConversionCatalog Catalog { get; }
    public bool IsVirtualCall { get; set;  }

    public MethodCallExpression(
        IlMethodId method,
        ConversionCatalog catalog,
        params CBaseExpression[] arguments) : base(0)
    {
        Catalog = catalog;
        Arguments = arguments;

        var functionInfo = catalog.Find(method);
        FnExpression = new LiteralValueExpression(functionInfo.NameInC.Value, functionInfo.ReturnTypeInfo, 0);
        Parameters = functionInfo.Parameters;
        ReturnType = functionInfo.ReturnTypeInfo;
        IsVirtualCall = false;
    }

    public MethodCallExpression(
        CBaseExpression fnExpression,
        IReadOnlyList<MethodConversionInfo.Parameter> parameters,
        IReadOnlyList<CBaseExpression> arguments,
        TypeConversionInfo returnType,
        ConversionCatalog catalog,
        bool isVirtualCall = false
    ) : base(returnType.IsPointer ? 1 : 0)
    {
        FnExpression = fnExpression;
        Parameters = parameters;
        Arguments = arguments;
        Catalog = catalog;
        IsVirtualCall = isVirtualCall;
        ReturnType = returnType;
    }

    public override TypeConversionInfo ResultingType => ReturnType;

    public override async ValueTask WriteCodeStringAsync(StreamWriter writer)
    {
        if (IsVirtualCall)
        {
            var thisExpression = Arguments[0];
            var targetExpression = Parameters[0];
            var targetExpressionInfo = Catalog.Find(targetExpression.TypeName);
            await writer.WriteAsync($"(({targetExpressionInfo.NativeNameWithPointer()})");
            await thisExpression.WriteCodeStringAsync(writer);
            await writer.WriteAsync(")->");
            await FnExpression.WriteCodeStringAsync(writer);

            await writer.WriteAsync("(");
            await WriteParametersAsync(writer);
            await writer.WriteAsync(")");
        }
        else
        {
            await FnExpression.WriteCodeStringAsync(writer);
            
            await writer.WriteAsync("(");
            await WriteParametersAsync(writer);
            await writer.WriteAsync(")");
        }
    }

    private async ValueTask WriteParametersAsync(StreamWriter writer, int startIndex = 0)
    {
        for (var x = startIndex; x < Arguments.Count; x++)
        {
            if (x > 0) await writer.WriteAsync(", ");

            var paramInfo = Catalog.Find(Parameters[x].TypeName);
            if (Parameters[x].IsReference)
            {
                // For ref reference types: cast to double pointer and take address
                if (Parameters[x].IsReferenceTypeByRef && paramInfo.IsReferenceType)
                {
                    await writer.WriteAsync($"({paramInfo.NameInC}**)&");
                }
                else if (paramInfo.NameInC != Arguments[x].ResultingType.NameInC)
                {
                    await writer.WriteAsync($"({paramInfo.NameInC}*)");
                }
            }

            var param = Arguments[x];

            await param.WriteCodeStringAsync(writer);
        }
    }

    public override CBaseExpression? ReplaceExpression(CBaseExpression search, CBaseExpression replacement)
    {
        var newFn = ReplaceExpression(FnExpression, search, replacement);
        return newFn != null ? this with { FnExpression = newFn } : null;
    }
}