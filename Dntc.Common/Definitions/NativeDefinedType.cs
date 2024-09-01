namespace Dntc.Common.Definitions;

public class NativeDefinedType : DefinedType
{
    public HeaderName? HeaderFile { get; }
    public CTypeName NativeName { get; }

    public NativeDefinedType(ClrTypeName clrTypeName, HeaderName? headerFile, CTypeName nativeName)
    {
        HeaderFile = headerFile;
        NativeName = nativeName;
        ClrName = clrTypeName;
        
        Fields = ArraySegment<Field>.Empty;
        Methods = ArraySegment<ClrMethodId>.Empty;
    }
    
    public static IEnumerable<NativeDefinedType> StandardTypes => new[]
    {
        StdIntType("System.SByte", "int8_t"),
        StdIntType("System.Int16", "int16_t"),
        StdIntType("System.Int32", "int32_t"),
        StdIntType("System.Int64", "int64_t"),
        StdIntType("System.Byte", "uint8_t"),
        StdIntType("System.UInt16", "uint16_t"),
        StdIntType("System.UInt32", "uint32_t"),
        StdIntType("System.UInt64", "uint64_t"),
        new NativeDefinedType(new ClrTypeName("System.Single"), null, new CTypeName("float")),
        new NativeDefinedType(new ClrTypeName("System.Double"), null, new CTypeName("double")),
    };

    private static NativeDefinedType StdIntType(string clrName, string nativeName)
    {
        return new NativeDefinedType(new ClrTypeName(clrName), new HeaderName("<stdint.h>"), new CTypeName(nativeName));
    }
}