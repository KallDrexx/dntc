﻿using Dntc.Common.Conversion;
using Dntc.Common.Syntax.Expressions;
using Dntc.Common.Syntax.Statements;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Dntc.Common.OpCodeHandling.Handlers;

public class MiscHandlers : IOpCodeHandlerCollection
{
    public IReadOnlyDictionary<Code, IOpCodeHandler> Handlers { get; } = new Dictionary<Code, IOpCodeHandler>
    {
        { Code.Initobj, new InitObjHandler() },
        { Code.Nop, new NopHandler() },
        { Code.Dup, new DupHandler() },
        { Code.Pop, new PopHandler() },
        { Code.Ret, new RetHandler() },
       
        // Constrained op code can be handled by a look behind with callvirt
        { Code.Constrained, new NopHandler()},
    };
    
    private class InitObjHandler : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(
            HandleContext context)
        {
            var typeDefinition = (TypeDefinition)context.CurrentInstruction.Operand;
            var conversionInfo = context.ConversionCatalog.Find(new IlTypeName(typeDefinition.FullName));
            var items = context.ExpressionStack.Pop(1);

            var left = new DereferencedValueExpression(items[0]);
            var right = new ZeroValuedObjectExpression(conversionInfo);
            var assignment = new AssignmentStatementSet(left, right);
            return new OpCodeHandlingResult(assignment);
        }
    }
    
    private class NopHandler : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(
            HandleContext context)
        {
            // do nothing
            return new OpCodeHandlingResult(null);
        }
    }
    
    private class DupHandler: IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(
            HandleContext context)
        {
            var items = context.ExpressionStack.Pop(1);
            context.ExpressionStack.Push(items[0] with { });
            context.ExpressionStack.Push(items[0]);

            return new OpCodeHandlingResult(null);
        }
    }
    
    private class PopHandler : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(
            HandleContext context)
        {
            var items = context.ExpressionStack.Pop(1);
            
            // this is usually the case of a method call without storing the 
            // result. So just make it its own statement
            var expression = new VoidExpressionStatementSet(items[0]);
            return new OpCodeHandlingResult(expression);
        }
    }
    
    private class RetHandler : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(
            HandleContext context)
        {
            if (context.ExpressionStack.Count > 1)
            {
                var message = $"Encountered return statement with {context.ExpressionStack.Count} items in it, but " +
                              $"only 0 or 1 is expected";
                throw new InvalidOperationException(message);
            }

            var innerExpression = context.ExpressionStack.Count == 1
                ? context.ExpressionStack.Pop(1)[0]
                : null;

            return new OpCodeHandlingResult(new ReturnStatementSet(innerExpression));
        }
    }
}