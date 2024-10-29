namespace Dntc.Common.OpCodeHandling;

public record InvokedMethod(IlMethodId MethodId);

public record GenericInvokedMethod(
    IlMethodId MethodId, 
    IlMethodId OriginalMethodId, 
    IReadOnlyList<IlTypeName> GenericArguments) : InvokedMethod(MethodId);
    