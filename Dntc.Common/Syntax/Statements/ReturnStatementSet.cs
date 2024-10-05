using Dntc.Common.Syntax.Expressions;

namespace Dntc.Common.Syntax.Statements;

/// <summary>
/// Represents a return statement that optionally may have a value that gets returned with it.
/// </summary>
/// <param name="ReturnedExpression"></param>
public record ReturnStatementSet(CBaseExpression? ReturnedExpression) : CStatementSet;