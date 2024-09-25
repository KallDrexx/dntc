namespace Dntc.Common.Conversion;

public record EvaluationStackItem(string Text)
{
    public override string ToString()
    {
        return Text;
    }
}
