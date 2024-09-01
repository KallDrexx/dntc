using Dntc.Common.Definitions;

namespace Dntc.Common;

public class Catalog
{
    private readonly Dictionary<ClrTypeName, DefinedType> _types = new();
    private readonly Dictionary<ClrMethodId, DefinedMethod> _methods = new();

    public void AddType(DefinedType type)
    {
        if (_types.TryGetValue(type.ClrName, out var existingType))
        {
            // TODO: Add better duplication logic, and possibly allow overriding
            // in some circumstances
            var message = $"CLR type '{type.ClrName.Name} is already defined and cannot " +
                          $"be redefined";

            throw new InvalidOperationException(message);
        }

        _types[type.ClrName] = type;
    }

    public void AddMethod(DefinedMethod method)
    {
        if (_methods.TryGetValue(method.Id, out var existingMethod))
        {
            // TODO: Add better duplication logic, and possibly allow overriding
            var message = $"CLR method '{method.Id.Name}' is already defined and cannot " +
                          $"be redefined";

            throw new InvalidOperationException(message);
        }

        _methods[method.Id] = method;
    }

    public DefinedType? FindType(ClrTypeName clrName)
    {
        return _types.GetValueOrDefault(clrName);
    }

    public DefinedMethod? FindMethod(ClrMethodId methodId)
    {
        return _methods.GetValueOrDefault(methodId);
    }
}