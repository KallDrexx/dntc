using Dntc.Common.Syntax.Statements;

namespace Dntc.Common.OpCodeHandling;

public class OpCodeHandlingResult
{
    public CStatementSet? StatementSet { get; }
    
    /// <summary>
    /// Represents that this op code requires the current stack item to be
    /// stored in a checkpoint until the specified IL offset.
    /// </summary>
    public int? CheckpointUntilTargetOffset { get; }
    
    public OpCodeHandlingResult(CStatementSet? statementSet, int? checkpointUntilTargetOffset = null)
    {
        StatementSet = statementSet;
        CheckpointUntilTargetOffset = checkpointUntilTargetOffset;
    }
}