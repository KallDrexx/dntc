namespace Dntc.Common.Conversion;

public class LocalNameCollection
{
    private readonly List<string> _localNames = new();

    public string this[int i] => _localNames[i];

    public int Count => _localNames.Count;

    public int Add()
    {
        var nextIndex = _localNames.Count;
        _localNames.Add($"__local_{nextIndex}");

        return nextIndex;
    }
}