﻿namespace Dntc.Common.Syntax.Expressions;

/// <summary>
/// Applies an operator (such as `+`, `>`, `==`) to two expressions for evaluation.
/// </summary>
public record TwoExpressionEvalExpression(CBaseExpression Left, string Operator, CBaseExpression Right) 
    : CBaseExpression(false)
{
    // NOTE: I don't think any math operations would end up doing pointer arithmetic, but not completely sure.
    public override async ValueTask WriteCodeString(StreamWriter writer)
    {
        await writer.WriteAsync("(");
        await Left.WriteCodeString(writer);
        await writer.WriteAsync($" {Operator} ");
        await Right.WriteCodeString(writer);
        await writer.WriteAsync(")");
    }
}