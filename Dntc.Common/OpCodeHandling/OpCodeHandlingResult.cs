using Dntc.Common.Syntax.Statements;

namespace Dntc.Common.OpCodeHandling;

public class OpCodeHandlingResult
{
    public CStatementSet? StatementSet { get; }
    
    public OpCodeHandlingResult(CStatementSet? statementSet)
    {
        StatementSet = statementSet;
    }
}