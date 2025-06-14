using Dntc.Common.Conversion;
using Dntc.Common.Definitions.ReferenceTypeSupport;
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
        { Code.Ldtoken, new LdTokenHandler() },
        { Code.Neg, new NegHandler() },

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

            var left = new AdjustPointerDepthExpression(items[0], 0);
            var right = new ZeroValuedObjectExpression(conversionInfo);
            var assignment = new AssignmentStatementSet(left, right);
            return new OpCodeHandlingResult(assignment);
        }

        public OpCodeAnalysisResult Analyze(AnalyzeContext context)
        {
            return new OpCodeAnalysisResult();
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

        public OpCodeAnalysisResult Analyze(AnalyzeContext context)
        {
            return new OpCodeAnalysisResult();
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

        public OpCodeAnalysisResult Analyze(AnalyzeContext context)
        {
            return new OpCodeAnalysisResult();
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

        public OpCodeAnalysisResult Analyze(AnalyzeContext context)
        {
            return new OpCodeAnalysisResult();
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

            var statements = new List<CStatementSet>();
            var innerExpression = context.ExpressionStack.Count == 1
                ? context.ExpressionStack.Pop(1)[0]
                : null;

            var originalInnerExpression = innerExpression;
            if (innerExpression != null)
            {
                // If we are returning a value, we need to first save that value to a temporary value. This is
                // needed because the inner expression might use a reference type as an argument. So we need
                // to save the value to the temporary variable before we untrack it, otherwise we'll end up
                // with a use after free bug.
                var returnType = context.ConversionCatalog.Find(context.CurrentDotNetMethod.ReturnType);
                var tempVariable = new Variable(returnType, Utils.ReturnVariableName(), returnType.IsPointer ? 1 : 0);
                var tempVariableExpression = new VariableValueExpression(tempVariable);
                var assignment = new AssignmentStatementSet(tempVariableExpression, innerExpression);

                statements.Add(assignment);
                innerExpression = tempVariableExpression;
            }

            // If any locals are .net reference types, we need to untrack them
            var innerExpressionContainsVariable = false;
            foreach (var variable in context.ReferenceTypeVariables)
            {
                if (originalInnerExpression is VariableValueExpression variableValueExpression &&
                    variableValueExpression.Variable == variable)
                {
                    // Since we are returning the variable, there's no reason to track + untrack it
                    // as part of the return process.
                    innerExpressionContainsVariable = true;
                    continue;
                }

                statements.Add(
                    new GcUntrackFunctionCallStatement(
                        new VariableValueExpression(variable),
                        context.ConversionCatalog));
            }

            // Since the start of the method tracked reference type arguments, we now need to untrack them.
            foreach (var parameter in context.CurrentMethodConversion.Parameters)
            {
                var paramType = context.ConversionCatalog.Find(parameter.TypeName);
                if (paramType.IsReferenceType && parameter.Name != Utils.ThisArgumentName && parameter.IsReference)
                {
                    // Skip GC untracking for ref reference type parameters - they represent addresses 
                    // of caller variables, not references that need untracking
                    if (parameter.IsReferenceTypeByRef && paramType.IsReferenceType)
                    {
                        continue;
                    }
                    
                    var variable = new VariableValueExpression(
                        new Variable(paramType, parameter.Name, 1));

                    statements.Add(new GcUntrackFunctionCallStatement(variable, context.ConversionCatalog));
                }
            }

            // If the value being returned is a .net reference type, then we need to mark it as tracked so we
            // don't accidentally free it. Don't bother doing this though if we are returning a local, since
            // it should already be tracked, and we explicitly are not untracking it as part of the pre-return
            // gc cleanup.
            if (originalInnerExpression?.ResultingType.IsReferenceType == true && !innerExpressionContainsVariable)
            {
                // This needs to be added first, so it comes before any untrack calls. Otherwise, it might be
                // freed before we are able to track it and return it.
                statements.Insert(0, new GcTrackFunctionCallStatement(originalInnerExpression, context.ConversionCatalog));
            }

            statements.Add(new ReturnStatementSet(innerExpression));
            return new OpCodeHandlingResult(new CompoundStatementSet(statements));
        }

        public OpCodeAnalysisResult Analyze(AnalyzeContext context)
        {
            return new OpCodeAnalysisResult();
        }
    }

    private class LdTokenHandler : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(HandleContext context)
        {
            switch (context.CurrentInstruction.Operand)
            {
                case TypeReference typeReference:
                {
                    var tokenInfo = context.ConversionCatalog.Find(new IlTypeName(typeReference.FullName));
                    var returnTypeInfo = context.ConversionCatalog.Find(new IlTypeName(typeof(Type).FullName!));
                    var expression = new LiteralValueExpression(tokenInfo.NameInC.Value, returnTypeInfo, 0);
                    context.ExpressionStack.Push(expression);
                    break;
                }

                default:
                    throw new NotSupportedException(context.CurrentInstruction.Operand.GetType().FullName);
            }

            return new OpCodeHandlingResult(null);
        }

        public OpCodeAnalysisResult Analyze(AnalyzeContext context)
        {
            var types = new HashSet<IlTypeName>();
            switch (context.CurrentInstruction.Operand)
            {
                case TypeReference typeReference:
                    types.Add(new IlTypeName(typeReference.FullName));
                    break;

                default:
                    throw new NotSupportedException(context.CurrentInstruction.Operand.GetType().FullName);
            }

            return new OpCodeAnalysisResult
            {
                ReferencedTypes = types,
            };
        }
    }

    private class NegHandler : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(HandleContext context)
        {
            var items = context.ExpressionStack.Pop(1);
            var newExpression = new NegateExpression(items[0], false);
            context.ExpressionStack.Push(newExpression);

            return new OpCodeHandlingResult(null);
        }

        public OpCodeAnalysisResult Analyze(AnalyzeContext context)
        {
            return new OpCodeAnalysisResult();
        }
    }
}