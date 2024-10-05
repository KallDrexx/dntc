using Dntc.Common.Syntax.Expressions;

namespace Dntc.Common.Syntax.Statements;

/// <summary>
/// Represents a block of C code that can jump to one of the specified IL offsets provided. The expression
/// is expected to resolve into a number starting with zero, and will jump to the index of the jump
/// label. If the expression resolves to a number greater than or equal to the number of IL offsets 
/// provided, then execution will fall through to the next statement set.
/// </summary>
/// <param name="Value"></param>
/// <param name="IlOffsets"></param>
public record JumpTableStatementSet(CBaseExpression Value, IReadOnlyList<int> IlOffsets) : CStatementSet;