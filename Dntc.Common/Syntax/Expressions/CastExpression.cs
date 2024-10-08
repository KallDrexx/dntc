﻿namespace Dntc.Common.Syntax.Expressions;

/// <summary>
/// Casts the result of the expression to the specified type.
/// </summary>
public record CastExpression(CBaseExpression Expression, string CastTo) : CBaseExpression(false)
{
    // NOTE: Not sure if we need to determine if the type we are casting to is a pointer or not. This
    // all depends on how reference types end up looking.

    public override async ValueTask WriteCodeStringAsync(StreamWriter writer)
    {
        await writer.WriteAsync($"(({CastTo})");
        await Expression.WriteCodeStringAsync(writer);
        await writer.WriteAsync(")");
    }
}