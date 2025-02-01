namespace Dntc.Common.Definitions;

public class NativeDefinedType : DefinedType
{
    public HeaderName? HeaderFile { get; }
    public CTypeName NativeName { get; }

    public NativeDefinedType(IlTypeName ilTypeName, HeaderName? headerFile, CTypeName nativeName,
        IReadOnlyList<Field> fields)
    {
        HeaderFile = headerFile;
        NativeName = nativeName;
        IlName = ilTypeName;

        InstanceFields = fields;
        Methods = Array.Empty<IlMethodId>();
    }

    public static IReadOnlyDictionary<Type, NativeDefinedType> StandardTypes { get; } =
        new Dictionary<Type, NativeDefinedType>
        {
            { typeof(sbyte), StdIntType(typeof(sbyte).FullName!, "int8_t") },
            { typeof(short), StdIntType(typeof(short).FullName!, "int16_t") },
            { typeof(int), StdIntType(typeof(int).FullName!, "int32_t") },
            { typeof(long), StdIntType(typeof(long).FullName!, "int64_t") },
            { typeof(byte), StdIntType(typeof(byte).FullName!, "uint8_t") },
            { typeof(ushort), StdIntType(typeof(ushort).FullName!, "uint16_t") },
            { typeof(uint), StdIntType(typeof(uint).FullName!, "uint32_t") },
            { typeof(ulong), StdIntType(typeof(ulong).FullName!, "uint64_t") },
            {
                typeof(float),
                new NativeDefinedType(new IlTypeName(typeof(float).FullName!), null, new CTypeName("float"), [])
            },
            {
                typeof(double),
                new NativeDefinedType(new IlTypeName(typeof(double).FullName!), null, new CTypeName("double"), [])
            },
            {
                typeof(bool),
                new NativeDefinedType(new IlTypeName(typeof(bool).FullName!), new HeaderName("<stdbool.h>"),
                    new CTypeName("bool"), [])
            },
            {
                typeof(void),
                new NativeDefinedType(new IlTypeName(typeof(void).FullName!), null, new CTypeName("void"), [])
            },
            {
                typeof(string),
                new NativeDefinedType(new IlTypeName(typeof(string).FullName!), null, new CTypeName("char*"), [])
            },
            {
                typeof(char),
                new NativeDefinedType(new IlTypeName(typeof(char).FullName!), null, new CTypeName("char"), [])
            },
            {
                // Since C doesn't have any reflection, there's no native variation for this. However, this still
                // needs a definition for support when `typeof()` is used to fill in the literal name of the type.
                typeof(Type),
                new NativeDefinedType(new IlTypeName(typeof(Type).FullName!), null, new CTypeName(""), [])
            }
        };

    private static NativeDefinedType StdIntType(string clrName, string nativeName)
    {
        return new NativeDefinedType(new IlTypeName(clrName), new HeaderName("<stdint.h>"), new CTypeName(nativeName),
            []);
    }
}