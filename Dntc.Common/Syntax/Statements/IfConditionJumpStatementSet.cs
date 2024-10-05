using Dntc.Common.Syntax.Expressions;

namespace Dntc.Common.Syntax.Statements;

/// <summary>
/// If the expression evaluates to true, then jump to the specified IL offset. Otherwise, fall through
/// to the next statement set.
/// </summary>
public record IfConditionJumpStatementSet(CBaseExpression Expression, int IlOffset) : CStatementSet;