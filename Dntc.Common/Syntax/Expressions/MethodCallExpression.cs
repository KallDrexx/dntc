using Dntc.Common.Conversion;

namespace Dntc.Common.Syntax.Expressions;

public record MethodCallExpression(MethodConversionInfo MethodInfo, IReadOnlyList<CBaseExpression> Parameters)
    : CBaseExpression(false)
{
    // Note: Right now method can only return value types. That may change depending on how
    // reference types end up being handled
    
    public override async ValueTask WriteCodeStringAsync(StreamWriter writer)
    {
        await writer.WriteAsync($"{MethodInfo.NameInC}(");
        
        for (var x = 0; x < Parameters.Count; x++)
        {
            if (x > 0) await writer.WriteAsync(", ");
            
            var param = Parameters[x];
            await param.WriteCodeStringAsync(writer);
        }

        await writer.WriteAsync(")");
    }
} 


    