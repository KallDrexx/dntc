using Dntc.Common.Conversion;

namespace Dntc.Common.Syntax.Expressions;

public record MethodCallExpression(CBaseExpression FnExpression, IReadOnlyList<CBaseExpression> Parameters)
    : CBaseExpression(false)
{
    // Note: Right now method can only return value types. That may change depending on how
    // reference types end up being handled

    public MethodCallExpression(MethodConversionInfo method, IReadOnlyList<CBaseExpression> parameters)
        : this(new LiteralValueExpression(method.NameInC.Value), parameters) { }

    public override async ValueTask WriteCodeStringAsync(StreamWriter writer)
    {
        await FnExpression.WriteCodeStringAsync(writer);
        await writer.WriteAsync("(");
        
        for (var x = 0; x < Parameters.Count; x++)
        {
            if (x > 0) await writer.WriteAsync(", ");
            
            var param = Parameters[x];
            await param.WriteCodeStringAsync(writer);
        }

        await writer.WriteAsync(")");
    }
} 


    