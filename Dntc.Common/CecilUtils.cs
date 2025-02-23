using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Dntc.Common;

/// <summary>
/// Utilities for Mono.cecil functionality
/// </summary>
public static class CecilUtils
{
    /// <summary>
    /// Gets the relevant debugging sequence point for the specified instruction
    /// </summary>
    public static SequencePoint? GetSequencePoint(MethodDefinition method, Instruction instruction)
    {
        var mapping = method.DebugInformation.GetSequencePointMapping();

        var currentPoint = (SequencePoint?)null;
        foreach (var (mappedInstruction, point) in mapping.OrderBy(x => x.Key.Offset))
        {
            if (mappedInstruction.Offset > instruction.Offset)
            {
                break;
            }

            currentPoint = point;
        }

        return currentPoint;
    }

    /// <summary>
    /// Returns a string for the provided sequence point for logging purposes.
    /// </summary>
    public static string LoggedSequencePointInfo(SequencePoint? sequencePoint)
    {
        return sequencePoint == null
            ? string.Empty
            : $"in {sequencePoint.Document.Url}:line {sequencePoint.StartLine}";
    }
}