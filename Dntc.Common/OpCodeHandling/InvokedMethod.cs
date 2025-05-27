using Dntc.Common.Definitions;
using Dntc.Common.Syntax.Expressions;

namespace Dntc.Common.OpCodeHandling;

public record InvokedMethod(IlMethodId MethodId);

public record GenericInvokedMethod(
    IlMethodId MethodId, 
    IlMethodId OriginalMethodId, 
    IReadOnlyList<IlTypeName> GenericArguments) : InvokedMethod(MethodId);

public record CustomInvokedMethod(CustomDefinedMethod InvokedMethod) : InvokedMethod(InvokedMethod.Id);
    