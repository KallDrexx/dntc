namespace Dntc.Common;

public readonly record struct IlTypeName(string Value)
{
    public override string ToString()
    {
        return Value;
    }

    public IlTypeName GetNonPointerOrRef()
    {
        return IsPointer() || IsReference()
            ? new IlTypeName(Value.Substring(0, Value.Length - 1))
            : this;
    }

    public bool IsPointer()
    {
        return Value.EndsWith('*');
    }

    public bool IsReference()
    {
        return Value.EndsWith('&');
    }

    public IlTypeName AsPointerType()
    {
        return IsPointer() ? this : new IlTypeName(Value + '*');
    }
}

public readonly record struct IlMethodId(string Value)
{
    public override string ToString()
    {
        return Value;
    }
}

public readonly record struct IlFieldId(string Value)
{
    public override string ToString()
    {
        return Value;
    }
}

public readonly record struct CTypeName(string Value)
{
    public override string ToString()
    {
        return Value;
    }
}

public readonly record struct CFieldName(string Value)
{
    public override string ToString()
    {
        return Value;
    }
}

public readonly record struct HeaderName(string Value)
{
    public override string ToString()
    {
        return Value;
    }
}

public readonly record struct IlNamespace(string Value)
{
    public override string ToString()
    {
        return Value;
    }
}

public readonly record struct CFunctionName(string Value)
{
    public override string ToString()
    {
        return Value;
    }
}

public readonly record struct CSourceFileName(string Value)
{
    public override string ToString()
    {
        return Value;
    }
}
