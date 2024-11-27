using Dntc.Common.OpCodeHandling;
using Mono.Cecil;

namespace Dntc.Common.Definitions;

public class DotNetDefinedMethod : DefinedMethod
{
    private readonly List<InvokedMethod> _invokedMethods = [];
    private readonly HashSet<IlTypeName> _referencedTypes = [];
    private readonly HashSet<IlFieldId> _referencedGlobals = [];
    private bool _hasBeenAnalyzed;
    
    public MethodDefinition Definition { get; }
    public IReadOnlyList<FunctionPointerType> FunctionPointerTypes { get; }
    public IReadOnlyList<TypeReference> ReferencedArrayTypes { get; }
    public IReadOnlyDictionary<string, IlTypeName> GenericArgumentTypes { get; } = new Dictionary<string, IlTypeName>();

    // We don't want to bother analyzing methods we aren't going to transpile, so only analyze
    // if we enter a code path looking for globals, types, and other methods this method utilizes.
    public List<InvokedMethod> InvokedMethods
    {
        get
        {
            Analyze();
            return _invokedMethods;
        }
    }

    public IReadOnlySet<IlTypeName> ReferencedTypes
    {
        get
        {
            Analyze();
            return _referencedTypes;
        }
    }

    public IReadOnlySet<IlFieldId> ReferencedGlobals
    {
        get
        {
            Analyze();
            return _referencedGlobals;
        }
    }
    
    public DotNetDefinedMethod(MethodDefinition definition)
    {
        Definition = definition;
        Id = new IlMethodId(definition.FullName);
        ReturnType = new IlTypeName(definition.ReturnType.FullName);

        var parameters = definition.Parameters
            .OrderBy(x => x.Index)
            .Select(GenerateParameter)
            .ToList();

        if (!definition.IsStatic)
        {
            // If this is an instance method, then the first parameter is always the declaring type
            parameters.Insert(0, new Parameter(new IlTypeName(definition.DeclaringType.FullName), "__this", true));
        }

        Parameters = parameters;
        ReferencedArrayTypes = definition.Parameters
            .Select(x => x.ParameterType)
            .Where(x => x.IsArray)
            .Distinct()
            .ToArray();

        Locals = definition.Body != null
            ? definition.Body
                .Variables
                .OrderBy(x => x.Index)
                .Select(x => new Local(new IlTypeName(x.VariableType.FullName), x.VariableType.IsByReference))
                .ToArray()
            : [];

        // Nested types don't have a namespace on them, so we need to go to the root
        var rootDeclaringType = definition.DeclaringType;
        while (rootDeclaringType.DeclaringType != null)
        {
            rootDeclaringType = rootDeclaringType.DeclaringType;
        }

        Namespace = new IlNamespace(rootDeclaringType.Namespace);

        FunctionPointerTypes = definition.Parameters
            .Select(x => x.ParameterType)
            .OfType<FunctionPointerType>()
            .Concat(definition.Body?.Variables.Select(x => x.VariableType).OfType<FunctionPointerType>() ?? [])
            .ToArray();
    }

    private DotNetDefinedMethod(
        MethodDefinition method, 
        IlMethodId methodId,
        IReadOnlyList<IlTypeName> genericArgumentTypes) : this(method)
    {
        Id = methodId;

        GenericArgumentTypes = genericArgumentTypes
            .Select((arg, index) => new { ParamName = Definition.GenericParameters[index].FullName, ArgName = arg })
            .ToDictionary(x => x.ParamName, x => x.ArgName);
        
        // Replace any referenced generic types with the corresponding argument type
        var genericParameters = Definition.GenericParameters
            .Select((param, index) => new { Name = param.FullName, Index = index })
            .ToDictionary(x => x.Name, x => x.Index);

        if (genericParameters.TryGetValue(ReturnType.Value, out var typeIndex))
        {
            ReturnType = genericArgumentTypes[typeIndex];
        }

        Parameters = Parameters
            .Select(x =>
            {
                if (genericParameters.TryGetValue(x.Type.Value, out typeIndex))
                {
                    return x with { Type = genericArgumentTypes[typeIndex] };
                }

                return x;
            })
            .ToArray();

        Locals = Locals
            .Select(x =>
            {
                if (genericParameters.TryGetValue(x.Type.Value, out typeIndex))
                {
                    return x with { Type = genericArgumentTypes[typeIndex] };
                }

                return x;
            }).ToArray();
    }

    public DotNetDefinedMethod MakeGenericInstance(IlMethodId methodId, IReadOnlyList<IlTypeName> genericArguments)
    {
        return new DotNetDefinedMethod(Definition, methodId, genericArguments);
    }

    private static Parameter GenerateParameter(ParameterDefinition definition)
    {
        if (definition.ParameterType.IsArray)
        {
            // This probably needs to be redone to pass by reference, but it's just a size_t and a pointer for now
            return new Parameter(new IlTypeName(definition.ParameterType.FullName), definition.Name, false);
        }

        return new Parameter(
            new IlTypeName(definition.ParameterType.GetElementType().FullName),
            definition.Name,
            definition.ParameterType.IsByReference);
    }

    protected override IReadOnlyList<IlTypeName> GetReferencedTypesInternal() =>
        ReferencedArrayTypes.Select(x => new IlTypeName(x.FullName))
            .Concat(GenericArgumentTypes.Values)
            .ToArray();

    private void Analyze()
    {
        if (_hasBeenAnalyzed)
        {
            return;
        }

        var referencedHeaders = new HashSet<HeaderName>();
        foreach (var instruction in Definition.Body.Instructions)
        {
            if (!KnownOpCodeHandlers.OpCodeHandlers.TryGetValue(instruction.OpCode.Code, out var handler))
            {
                var message = $"No handler for op code '{instruction.OpCode.Code}'";
                throw new InvalidOperationException(message);
            }

            var results = handler.Analyze(new AnalyzeContext(instruction, this));
            if (results.CalledMethod != null)
            {
                _invokedMethods.Add(results.CalledMethod);
            }

            foreach (var referencedType in results.ReferencedTypes)
            {
                _referencedTypes.Add(referencedType);
            }

            foreach (var header in results.ReferencedHeaders)
            {
                referencedHeaders.Add(header);
            }

            if (results.ReferencedGlobal != null)
            {
                _referencedGlobals.Add(new IlFieldId(results.ReferencedGlobal.FullName));
            }
        }

        ReferencedHeaders = ReferencedHeaders.Union(referencedHeaders).ToArray();
        _hasBeenAnalyzed = true;
    }
}