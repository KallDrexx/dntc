using Dntc.Common.Conversion;
using Dntc.Common.Syntax.Statements;

namespace Dntc.Common.Syntax;

public class MethodBlock
{
    private readonly MethodConversionInfo _methodConversionInfo;
    private readonly MethodDeclaration _methodDeclaration;
    private readonly IReadOnlyList<int> _jumpOffsets;

    public IReadOnlyList<CStatementSet> Statements { get; }

    public MethodBlock(
        MethodConversionInfo method, 
        IReadOnlyList<CStatementSet> statements,
        MethodDeclaration declaration)
    {
        _methodConversionInfo = method;
        _methodDeclaration = declaration;
        Statements = MoveDeclarationsToBeginning(statements);

        _jumpOffsets = Statements
            .SelectMany(GetJumpOffsets)
            .OrderBy(x => x)
            .Distinct()
            .ToArray();
    }

    public async Task WriteAsync(StreamWriter writer)
    {
        // I know some attributes (like always_inline) require the attribute on the implementation and not 
        // just the declaration, so write the attribute here. I'm not sure if that's always the case.
        if (_methodConversionInfo.AttributeText != null)
        {
            await writer.WriteLineAsync(_methodConversionInfo.AttributeText);
        }
            
        await _methodDeclaration.WriteAsync(writer);
        await writer.WriteLineAsync(" {");

        var jumpOffsetIndex = 0;
        foreach (var statement in Statements)
        {
            while (jumpOffsetIndex < _jumpOffsets.Count && _jumpOffsets[jumpOffsetIndex] <= statement.LastIlOffset)
            {
                // It's rare that a jump label is in between the expressions used by statements, so
                // this should be fine. Ternaries are the main thing this will probably break on, but
                // that requires a more complex solution anyway that we'll handle later.
                await writer.WriteLineAsync(); // Blank line for visual separation
                await writer.WriteLineAsync($"{Utils.IlOffsetToLabel(_jumpOffsets[jumpOffsetIndex], _methodConversionInfo)}:");
                jumpOffsetIndex++;
            }

            await statement.WriteAsync(writer);
        }

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

    private static IReadOnlyList<CStatementSet> MoveDeclarationsToBeginning(IReadOnlyList<CStatementSet> statements)
    {
        // Some C compilers do not handle local declarations that are not at the beginning of the function well.
        // Sometimes mid-function declarations are not allowed, and sometimes you get into odd edge cases where a
        // local declaration is not allowed directly after a jump label. To prevent this, move all local declarations
        // to the top of the function.
        //
        // We also need to set all local declarations to have starting IL offsets at 0, otherwise it's still possible
        // that a jump label points to a local declaration, thus causing the local declaration after a goto label
        // compilation error.
        var declarations = new List<LocalDeclarationStatementSet>();
        var nonDeclarations = new List<CStatementSet>();
        var initialLineStatements = new List<CStatementSet>();

        // First, collect any LineStatementSets that appear before the first non-LineStatementSet
        int i = 0;
        while (i < statements.Count && statements[i] is LineStatementSet)
        {
            initialLineStatements.Add(statements[i]);
            i++;
        }

        // Then process the rest of the statements
        for (; i < statements.Count; i++)
        {
            var statement = statements[i];
            if (statement is CompoundStatementSet compound)
            {
                foreach (var inner in compound.Flatten())
                {
                    if (inner is LocalDeclarationStatementSet declaration)
                    {
                        declarations.Add(declaration with { StartingIlOffset = 0, LastIlOffset = 0 });
                    }
                    else
                    {
                        nonDeclarations.Add(inner);
                    }
                }
            }
            else if (statement is LocalDeclarationStatementSet declaration)
            {
                declarations.Add(declaration with { StartingIlOffset = 0, LastIlOffset = 0 });
            }
            else
            {
                nonDeclarations.Add(statement);
            }
        }

        // Combine in the order: initialLineStatements -> declarations -> other non-declarations
        return initialLineStatements.TakeLast(1).Concat(declarations).Concat(nonDeclarations).ToArray();
    }
}