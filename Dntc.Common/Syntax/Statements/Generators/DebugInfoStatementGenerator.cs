using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Dntc.Common.Syntax.Statements.Generators;

public enum DebugInfoMode
{
    CLineSourceMaps,
    LineNumberComments,
    None,
}

public class DebugInfoStatementGenerator(DebugInfoMode mode) : IStatementGenerator
{
    private EndLineStatementSet? _movedStatementSet;
    
    public IEnumerable<CStatementSet> Before(List<CStatementSet> statements, MethodDefinition definition,
        Instruction methodInstruction)
    {
        if (mode != DebugInfoMode.None)
        {
            var sequencePoint = CecilUtils.GetSequencePoint(definition, methodInstruction);

            var lastLineStatement = statements.OfType<LineStatementSet>().LastOrDefault();

            if (lastLineStatement != null && lastLineStatement.SequencePoint == sequencePoint)
            {
                // The sequence point matches the last line statement so we have multiple CStatements
                // representing the same line in the original C# code. We will remove the 
                // endLineStatement so that all these CStatements will get bunched onto the same line.
                var lastEndLineStatement = statements.OfType<EndLineStatementSet>().LastOrDefault();
                if (lastEndLineStatement != null)
                {
                    statements.Remove(lastEndLineStatement);
                }
            }
            else if (sequencePoint is { IsHidden: false })
            {
                // This is a new non-hidden sequence point so we will add a new line statement
                yield return new LineStatementSet(mode, methodInstruction.Offset, sequencePoint);
            }
            else
            {
                // This is a hidden sequence point meaning we need to move the last endLineStatement
                // to keep moving the endLineStatment until we find a non-hidden sequence point.
                // The endLineStatement will be re-added in the call to After().
                var lastEndLineStatement = statements.OfType<EndLineStatementSet>().LastOrDefault();
                if (lastEndLineStatement != null)
                {
                    statements.Remove(lastEndLineStatement);
                    _movedStatementSet = lastEndLineStatement;
                }
            }
        }
    }

    public IEnumerable<CStatementSet> After(List<CStatementSet> statements, MethodDefinition definition,
        Instruction methodInstruction)
    {
        if (mode != DebugInfoMode.None)
        {
            if (_movedStatementSet != null)
            {
                yield return _movedStatementSet;
                _movedStatementSet = null;
            }
            else
            {
                var sequencePoint = CecilUtils.GetSequencePoint(definition, methodInstruction);

                if (sequencePoint is { IsHidden: false })
                {
                    yield return new EndLineStatementSet(mode, methodInstruction.Offset);
                }
            }
        }
    }
}