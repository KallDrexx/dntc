namespace Dntc.Common.Definitions;

public class NativeDefinedType : DefinedType
{
    public HeaderName? HeaderFile { get; }
    public CTypeName NativeName { get; }

    public NativeDefinedType(IlTypeName ilTypeName, HeaderName? headerFile, CTypeName nativeName)
    {
        HeaderFile = headerFile;
        NativeName = nativeName;
        IlName = ilTypeName;
        
        Fields = ArraySegment<Field>.Empty;
        Methods = ArraySegment<IlMethodId>.Empty;
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
        new NativeDefinedType(new IlTypeName("System.Single"), null, new CTypeName("float")),
        new NativeDefinedType(new IlTypeName("System.Double"), null, new CTypeName("double")),
    };

    private static NativeDefinedType StdIntType(string clrName, string nativeName)
    {
        return new NativeDefinedType(new IlTypeName(clrName), new HeaderName("<stdint.h>"), new CTypeName(nativeName));
    }
}