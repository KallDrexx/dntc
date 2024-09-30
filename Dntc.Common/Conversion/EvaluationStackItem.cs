namespace Dntc.Common.Conversion;

public class EvaluationStackItem
{
    private readonly string _text;
    
    public bool IsPointer { get; }
    public string RawText => _text;

    public string WithAccessor => IsPointer ? $"{_text}->" : $"{_text}.";
    public string Dereferenced => IsPointer ? $"*{_text}" : _text;
    public string ReferenceTo => IsPointer ? $"{_text}" : $"&{_text}";

    public EvaluationStackItem(string text, bool isPointer)
    {
        _text = text;
        IsPointer = isPointer;
    }
    
    public override string ToString()
    {
        return _text;
    }

    public EvaluationStackItem Clone()
    {
        return new EvaluationStackItem(_text, IsPointer);
    }
}
