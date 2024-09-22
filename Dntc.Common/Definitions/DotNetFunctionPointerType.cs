using Mono.Cecil;

namespace Dntc.Common.Definitions;

public class DotNetFunctionPointerType : DefinedType
{
    public FunctionPointerType Definition { get; }
    
    public DotNetFunctionPointerType(FunctionPointerType functionPointer)
    {
        Definition = functionPointer;
        IlName = new IlTypeName(functionPointer.FullName);
    }
}