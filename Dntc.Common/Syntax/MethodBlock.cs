using Dntc.Common.Conversion;
using Dntc.Common.Syntax.Statements;

namespace Dntc.Common.Syntax;

public class MethodBlock
{
    private readonly MethodConversionInfo _methodConversionInfo;
    private readonly MethodDeclaration _methodDeclaration;
    private readonly IReadOnlyList<CStatementSet> _statements;
    private readonly IReadOnlyList<int> _jumpOffsets;

    public MethodBlock(MethodConversionInfo method, IReadOnlyList<CStatementSet> statements)
    {
        _methodConversionInfo = method;
        _methodDeclaration = new MethodDeclaration(method);
        _statements = statements
            .OrderBy(x => x.StartingIlOffset)
            .ToArray();

        _jumpOffsets = statements
            .SelectMany(GetJumpOffsets)
            .ToArray();
    }

    public async Task WriteAsync(StreamWriter writer)
    {
        await _methodDeclaration.WriteAsync(writer);
        await writer.WriteLineAsync("{ ");
        
        


        await writer.WriteLineAsync("}");
    }

    private static IReadOnlyList<int> GetJumpOffsets(CStatementSet statementSet)
    {
        switch (statementSet)
        {
            case GotoStatementSet gotoStatementSet:
                return [gotoStatementSet.IlOffset];
            
            case IfConditionJumpStatementSet ifConditionJumpStatementSet:
                return [ifConditionJumpStatementSet.IlOffset];
            
            case JumpTableStatementSet jumpTableStatementSet:
                return jumpTableStatementSet.IlOffsets;
            
            default:
                return [];
        }
    }
}