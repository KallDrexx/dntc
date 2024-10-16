using Mono.Cecil;

namespace Dntc.Common.MethodAnalysis;

public record InvokedMethod(IlMethodId MethodId);

public record GenericInvokedMethod(
    IlMethodId MethodId, 
    IlMethodId OriginalMethodId, 
    IReadOnlyList<IlTypeName> GenericArguments) : InvokedMethod(MethodId);
    