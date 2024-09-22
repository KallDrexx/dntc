﻿namespace Dntc.Common.Definitions;

public abstract class DefinedMethod
{
    public record Parameter(IlTypeName Type, string Name);
    
    public IlMethodId Id { get; protected set; }
    public IlTypeName ReturnType { get; protected set; }
    public IlNamespace Namespace { get; protected set; }
    public IReadOnlyList<Parameter> Parameters { get; protected set; } = ArraySegment<Parameter>.Empty;
    public IReadOnlyList<IlTypeName> Locals { get; protected set; } = ArraySegment<IlTypeName>.Empty;
}