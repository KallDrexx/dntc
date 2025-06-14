using Dntc.Common.Conversion;
using Dntc.Common.Definitions.CustomDefinedTypes;
using Dntc.Common.Syntax.Expressions;
using Dntc.Common.Syntax.Statements;

namespace Dntc.Common.Definitions.ReferenceTypeSupport;

public class PrepToFreeDefinedMethod : CustomDefinedMethod
{
    private readonly IMemoryManagementActions _memoryManagement;

    public DefinedType DefinedType { get; }
    
    public PrepToFreeDefinedMethod(DotNetDefinedType definedType, IMemoryManagementActions memoryManagement)
        : base(
            ReferenceTypeConstants.PrepTypeToFreeMethodId(definedType.IlName),
            new IlTypeName(typeof(void).FullName!),
            definedType.Namespace,
            Utils.GetHeaderName(definedType.Namespace),
            Utils.GetSourceFileName(definedType.Namespace),
            FormNativeName(definedType.Definition.FullName),
            [
                new Parameter(definedType.IlName, "object", true, false),
            ])
    {
        DefinedType = definedType;
        _memoryManagement = memoryManagement;
    }

    public PrepToFreeDefinedMethod(CustomDefinedType customDefinedType, IMemoryManagementActions memoryManagement)
        : base(
            ReferenceTypeConstants.PrepTypeToFreeMethodId(customDefinedType.IlName),
            new IlTypeName(typeof(void).FullName!),
            new IlNamespace("Dntc.System"),
            customDefinedType.HeaderName ?? throw new NullReferenceException("customDefinedType.HeaderName"),
            customDefinedType.SourceFileName,
            FormNativeName(customDefinedType.NativeName.Value),
            [
                new Parameter(customDefinedType.IlName, "object", true, false),
            ])
    {
        DefinedType = customDefinedType;
        _memoryManagement = memoryManagement;
    }

    public static CFunctionName FormNativeName(string typeNativeName) =>
        new(Utils.MakeValidCName($"{typeNativeName}__PrepForFree"));

    public override CustomCodeStatementSet? GetCustomDeclaration(ConversionCatalog catalog)
    {
        var typeName = catalog.Find(DefinedType.IlName);
        return new CustomCodeStatementSet($"void {NativeName}({typeName.NameInC}* object)");
    }

    public override CStatementSet? GetCustomImplementation(ConversionCatalog catalog)
    {
        var statements = new List<CStatementSet>();
        var thisTypeInfo = catalog.Find(DefinedType.IlName);
        var objectVariable = new Variable(thisTypeInfo, "object", 1);
        var objectVariableExpression = new VariableValueExpression(objectVariable);

        if (DefinedType is DotNetDefinedType dotNetType)
        {
            var referenceTypeFields = dotNetType.Definition
                .Fields
                .Where(x => !x.FieldType.IsValueType)
                .OrderBy(x => x.Name)
                .Select(x => new IlFieldId(x.FullName))
                .ToArray();

            foreach (var field in referenceTypeFields)
            {
                var fieldInfo = catalog.Find(field);
                var fieldVariable = new Variable(fieldInfo.FieldTypeConversionInfo, fieldInfo.NameInC.Value, 1);
                var fieldAccess = new FieldAccessExpression(objectVariableExpression, fieldVariable);
                statements.Add(new GcUntrackFunctionCallStatement(fieldAccess, catalog));
                statements.Add(new CustomCodeStatementSet("\n")); // Need to come up with a better white space strategy
            }

            var baseType = Utils.GetNonSystemBaseType(dotNetType.Definition);
            if (baseType != null)
            {
                var baseTypeInfo = catalog.Find(new IlTypeName(baseType.FullName));
                var methodCall = new MethodCallExpression(
                    ReferenceTypeConstants.PrepTypeToFreeMethodId(baseTypeInfo.IlName),
                    catalog,
                    new LiteralValueExpression($"&(object->base)", baseTypeInfo, 0));

                statements.Add(new VoidExpressionStatementSet(methodCall));
            }
        }

        // If this is an array, we need to free the items pointer.
        // TODO: This logic should be owned by the array defined type and not "hardcoded" here
        if (DefinedType is ArrayDefinedType arrayDefinedType)
        {
            var itemsPointer = arrayDefinedType.GetItemsAccessorExpression(objectVariableExpression, catalog);
            statements.Add(_memoryManagement.FreeCall(itemsPointer, catalog));
        }

        if (statements.Count == 0)
        {
            statements.Add(new CustomCodeStatementSet($"\t// No cleanup necessary\n"));
        }

        return new CompoundStatementSet(statements);
    }
}