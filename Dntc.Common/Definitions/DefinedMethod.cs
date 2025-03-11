﻿using Mono.Cecil;

namespace Dntc.Common.Definitions;

public abstract class DefinedMethod
{
    public record Parameter(IlTypeName Type, string Name, bool IsReference);

    public record Local(IlTypeName Type, bool IsReference);
    
    public IlMethodId Id { get; protected set; }
    public IlTypeName ReturnType { get; set; }
    public IlNamespace Namespace { get; protected set; }
    public IReadOnlyList<Parameter> Parameters { get; set; } = ArraySegment<Parameter>.Empty;
    public IReadOnlyList<Local> Locals { get; set; } = ArraySegment<Local>.Empty;
    public IReadOnlyList<FunctionPointerType> FunctionPointerTypes { get; protected set; } = ArraySegment<FunctionPointerType>.Empty;
    public bool IsMacroDefinition { get; protected set; }

    /// <summary>
    /// Headers that are referenced by this method but cannot be inferred from initial static analysis. This is
    /// mostly required for custom defined types, or headers due to customizations found during method analysis.
    /// </summary>
    public IReadOnlyList<HeaderName> ReferencedHeaders { get; protected set; } = ArraySegment<HeaderName>.Empty;
    
    public IReadOnlyList<IlTypeName> GetReferencedTypes => Locals.Select(x => x.Type)
        .Concat(Parameters.Select(x => x.Type))
        .Concat([ReturnType])
        .Concat(GetReferencedTypesInternal())
        .ToArray();

    public virtual DefinedMethod MakeGenericInstance(IlMethodId methodId, IReadOnlyList<IlTypeName> genericArguments)
    {
        var message = $"Generic method '{methodId}' cannot be made into a generic instance because " +
                      $"{GetType().FullName} does not support doing so.";

        throw new InvalidOperationException(message);
    }

    protected abstract IReadOnlyList<IlTypeName> GetReferencedTypesInternal();
}