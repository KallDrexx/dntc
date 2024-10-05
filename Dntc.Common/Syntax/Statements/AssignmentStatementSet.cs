using Dntc.Common.Syntax.Expressions;

namespace Dntc.Common.Syntax.Statements;

public record AssignmentStatementSet(CBaseExpression Left, CBaseExpression Right) : CStatementSet;
