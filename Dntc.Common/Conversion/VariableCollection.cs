namespace Dntc.Common.Conversion;

internal class VariableCollection
{
    private readonly List<Variable> _locals = new();
    private readonly List<Variable> _parameters = new();

    public IReadOnlyList<Variable> Locals => _locals;
    public IReadOnlyList<Variable> Parameters => _parameters;

    public int AddLocal(TypeConversionInfo typeInfo)
    {
        var nextIndex = _locals.Count;
        var name = $"__local_{nextIndex}";
        
        _locals.Add(new Variable(typeInfo, name));

        return nextIndex;
    }

    public int AddParameter(TypeConversionInfo typeInfo, string name)
    {
        var nextIndex = _parameters.Count;
        _parameters.Add(new Variable(typeInfo, name));

        return nextIndex;
    }
}
