using Dntc.Common.Conversion;
using Dntc.Common.Syntax.Expressions;
using Dntc.Common.Syntax.Statements;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Dntc.Common.OpCodeHandling.Handlers;

public class CallHandlers : IOpCodeHandlerCollection
{
    public IReadOnlyDictionary<Code, IOpCodeHandler> Handlers { get; }
    
    private class CallHandler : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(
            Instruction currentInstruction, 
            ExpressionStack expressionStack,
            MethodConversionInfo currentMethod, 
            ConversionCatalog conversionCatalog)
        {
            var methodReference = (MethodReference)currentInstruction.Operand;
            var methodId = new IlMethodId(methodReference.FullName);
            var conversionInfo = conversionCatalog.Find(methodId);

            var arguments = expressionStack.Pop(conversionInfo.Parameters.Count)
                .Reverse() // Items are passed into the method in the reverse order they are popped
                .ToArray();

            return FormResult(conversionInfo, arguments, expressionStack);
        }
    }
    
    private class CallIHandler : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(
            Instruction currentInstruction, 
            ExpressionStack expressionStack,
            MethodConversionInfo currentMethod, 
            ConversionCatalog conversionCatalog)
        {
            var callSite = (CallSite)currentInstruction.Operand;
            
            // Top of the stack contains the function name to call, followed by the arguments in 
            // reverse calling order
            var allItems = expressionStack.Pop(callSite.Parameters.Count + 1);

            throw new NotImplementedException();
        }
    }

    private static OpCodeHandlingResult FormResult(
        MethodConversionInfo conversionInfo,
        IReadOnlyList<CBaseExpression> arguments,
        ExpressionStack expressionStack)
    {
        // If the method returns void, then a statement needs to be generated for it.
        // Otherwise, some other opcode will cause it to have an expression generated.
        var callExpression = new MethodCallExpression(conversionInfo, arguments);
        if (conversionInfo.ReturnTypeInfo.IlName.Value == typeof(void).FullName)
        {
            return new OpCodeHandlingResult(new VoidExpressionStatementSet(callExpression));
        }

        expressionStack.Push(callExpression);
        return new OpCodeHandlingResult(null);
    }
}