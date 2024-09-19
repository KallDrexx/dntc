namespace Dntc.Common.Conversion;

public class Variable
{
    public TypeConversionInfo Type { get; }
    public string Name { get; }
    
    public Variable(TypeConversionInfo type, string name)
    {
        Type = type;
        Name = name;
    }
}