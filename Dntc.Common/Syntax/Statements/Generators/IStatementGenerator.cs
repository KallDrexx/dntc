using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Dntc.Common.Syntax.Statements.Generators;

public interface IStatementGenerator
{
    IEnumerable<CStatementSet> Before(List<CStatementSet> statements, MethodDefinition definition,
        Instruction methodInstruction);
    
    IEnumerable<CStatementSet> After(List<CStatementSet> statements, MethodDefinition definition,
        Instruction methodInstruction);
}