using Dntc.Common.Syntax.Statements;
using Mono.Cecil;

namespace Dntc.Common.Definitions.CustomDefinedMethods;

public class CreateDefinedMethod : CustomDefinedMethod
{
    private readonly TypeDefinition _typeDefinition;

    public CreateDefinedMethod(TypeDefinition typeDefinition) 
        :  base(new IlMethodId(typeDefinition.FullName + "__Create"),
            new IlTypeName(typeDefinition.FullName),
            Utils.GetNamespace(typeDefinition),
            Utils.GetHeaderName(Utils.GetNamespace(typeDefinition)),
            Utils.GetSourceFileName(Utils.GetNamespace(typeDefinition)),
            new CFunctionName(Utils.MakeValidCName(typeDefinition.FullName + "__Create")), [])
    {
        _typeDefinition = typeDefinition;
    }

    public override CustomCodeStatementSet? GetCustomDeclaration()
    {
        var typeName = Utils.MakeValidCName(_typeDefinition.FullName);
        
        return new CustomCodeStatementSet($@"
    {typeName}* {NativeName}(void)");
    }

    public override CustomCodeStatementSet? GetCustomImplementation()
    {
        var typeName = Utils.MakeValidCName(_typeDefinition.FullName);
        
        return new CustomCodeStatementSet($@"
	{typeName}* result = ({typeName}*) malloc(sizeof({typeName}));
	memset(result, 0, sizeof({typeName}));
	return result;");
        
        ////((HelloWorld_ConsoleBase*)result)->HelloWorld_ConsoleBase_VirtualMethod = (void (*)(HelloWorld_ConsoleBase*))HelloWorld_Console_VirtualMethod;
    }
}