﻿using Dntc.Common.Conversion;
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
        Statements = statements;

        _jumpOffsets = statements
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
}