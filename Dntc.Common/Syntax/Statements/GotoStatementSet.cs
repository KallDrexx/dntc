namespace Dntc.Common.Syntax.Statements;

/// <summary>
/// Statement that jumps to the specified IL instruction offset
/// </summary>
/// <param name="IlOffset"></param>
public record GotoStatementSet(int IlOffset) : CStatementSet;