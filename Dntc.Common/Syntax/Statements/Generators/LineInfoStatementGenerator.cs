using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Dntc.Common.Syntax.Statements.Generators;

public enum LineInfoMode
{
    Directives,
    Comments,
    None,
}

public class LineInfoStatementGenerator(LineInfoMode mode) : IStatementGenerator
{
    private EndLineStatementSet? _movedStatementSet;
    
    public IEnumerable<CStatementSet> Before(List<CStatementSet> statements, MethodDefinition definition,
        Instruction methodInstruction)
    {
        if (mode != LineInfoMode.None)
        {
            var sequencePoint = CecilUtils.GetSequencePoint(definition, methodInstruction);

            var lastLineStatement = statements.OfType<LineStatementSet>().LastOrDefault();

            if (lastLineStatement != null && lastLineStatement.SequencePoint == sequencePoint)
            {
                var lastEndLineStatement = statements.OfType<EndLineStatementSet>().LastOrDefault();
                if (lastEndLineStatement != null)
                {
                    statements.Remove(lastEndLineStatement);
                }
            }
            else if (sequencePoint is { IsHidden: false })
            {
                yield return new LineStatementSet(mode, methodInstruction.Offset, sequencePoint);
            }
            else
            {
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
        if (mode != LineInfoMode.None)
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