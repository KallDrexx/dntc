using Dntc.Common.Conversion;
using Dntc.Common.Syntax.Statements;

namespace Dntc.Common.Syntax;

public class MethodCode
{
    private readonly List<CStatementSet> _statements = [];
    private readonly HashSet<int> _jumpOffsets = [];

    public IReadOnlyList<CStatementSet> Statements => _statements;
    public IReadOnlyList<int> JumpDestinationOffsets => _jumpOffsets.OrderBy(x => x).ToArray();
    
    public MethodConversionInfo MethodInfo { get; }

    public MethodCode(MethodConversionInfo conversionInfo)
    {
        MethodInfo = conversionInfo;
    }

    public void Add(CStatementSet statementSet)
    {
        _statements.Add(statementSet);

        switch (statementSet)
        {
            case GotoStatementSet gotoStatementSet:
                _jumpOffsets.Add(gotoStatementSet.IlOffset);
                break;
            
            case IfConditionJumpStatementSet ifConditionJumpStatementSet:
                _jumpOffsets.Add(ifConditionJumpStatementSet.IlOffset);
                break;
            
            case JumpTableStatementSet jumpTableStatementSet:
                foreach (var offset in jumpTableStatementSet.IlOffsets)
                {
                    _jumpOffsets.Add(offset);
                }

                break;
        }
    }
}