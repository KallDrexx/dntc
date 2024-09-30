namespace Dntc.Common.Conversion;

public class EvaluationStackItem
{
    private readonly string _text;
    
    public bool IsPointer { get; }
    public string RawText => _text;

    public EvaluationStackItem(string text, bool isPointer)
    {
        _text = text;
        IsPointer = isPointer;
    }
    
    public override string ToString()
    {
        return _text;
    }

    public string TextWithAccessor => IsPointer ? $"{_text}->" : $"{_text}.";
    public string TextDerefed => IsPointer ? $"*{_text}" : _text;
    public string TextReference => IsPointer ? $"&{_text}" : _text;
}
