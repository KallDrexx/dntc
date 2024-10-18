using Dntc.Common.Definitions;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Dntc.Common.OpCodeHandling;

public static class VirtualCallConverter
{
    /// <summary>
    /// Converts the method being called by a `callVirt` call to the appropriate method id
    /// corresponding to the concrete class of the method to be called. 
    /// </summary>
    public static IlMethodId Convert(Instruction callVirtInstruction, DotNetDefinedMethod containedMethod)
    {
        if (callVirtInstruction.Operand is not MethodReference methodReference)
        {
            throw new NotSupportedException(callVirtInstruction.Operand.GetType().FullName);
        }
        
        // First check if we have a constraint
        if (callVirtInstruction.Previous.OpCode.Code == Code.Constrained)
        {
            if (callVirtInstruction.Previous.Operand is GenericParameter genericParameter)
            {
                if (!containedMethod.GenericArgumentTypes.TryGetValue(genericParameter.FullName, out var ilType))
                {
                    var message = $"No generic argument defined for type '{genericParameter.FullName}' on method " +
                                  $"'{containedMethod.Id}'";
                    throw new InvalidOperationException(message);
                }

                var methodId = methodReference.FullName
                    .Replace(methodReference.DeclaringType.FullName, ilType.Value);

                return new IlMethodId(methodId);
            }
        }

        return new IlMethodId(methodReference.FullName);
    }
}