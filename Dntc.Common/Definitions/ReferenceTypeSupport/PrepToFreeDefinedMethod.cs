using Dntc.Common.Conversion;
using Dntc.Common.Syntax.Expressions;
using Dntc.Common.Syntax.Statements;

namespace Dntc.Common.Definitions.ReferenceTypeSupport;

public class PrepToFreeDefinedMethod : CustomDefinedMethod
{
    public DefinedType DefinedType { get; }
    
    public PrepToFreeDefinedMethod(DotNetDefinedType definedType)
        : base(
            ReferenceTypeConstants.PrepTypeToFreeMethodId(definedType.IlName),
            new IlTypeName(typeof(void).FullName!),
            definedType.Namespace,
            Utils.GetHeaderName(definedType.Namespace),
            Utils.GetSourceFileName(definedType.Namespace),
            FormNativeName(definedType.Definition.FullName),
            [
                new Parameter(definedType.IlName, "object", true),
            ])
    {
        DefinedType = definedType;
    }

    public PrepToFreeDefinedMethod(CustomDefinedType customDefinedType)
        : base(
            ReferenceTypeConstants.PrepTypeToFreeMethodId(customDefinedType.IlName),
            new IlTypeName(typeof(void).FullName!),
            new IlNamespace("Dntc.System"),
            customDefinedType.HeaderName ?? throw new NullReferenceException("customDefinedType.HeaderName"),
            customDefinedType.SourceFileName,
            FormNativeName(customDefinedType.NativeName.Value),
            [
                new Parameter(customDefinedType.IlName, "object", true),
            ])
    {
        DefinedType = customDefinedType;
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

        if (DefinedType is DotNetDefinedType dotNetType)
        {
            var referenceTypeFields = dotNetType.Definition
                .Fields
                .Where(x => !x.FieldType.IsValueType)
                .OrderBy(x => x.Name)
                .Select(x => new IlFieldId(x.FullName))
                .ToArray();

            var thisTypeInfo = catalog.Find(DefinedType.IlName);
            var objectVariable = new Variable(thisTypeInfo, "object", true);
            var objectVariableExpression = new VariableValueExpression(objectVariable);
            foreach (var field in referenceTypeFields)
            {
                var fieldInfo = catalog.Find(field);
                var fieldVariable = new Variable(fieldInfo.FieldTypeConversionInfo, fieldInfo.NameInC.Value, true);
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
                    new LiteralValueExpression($"&(object->base)", baseTypeInfo));

                statements.Add(new VoidExpressionStatementSet(methodCall));
            }
        }

        if (statements.Count == 0)
        {
            statements.Add(new CustomCodeStatementSet($"\t// No cleanup necessary\n"));
        }

        return new CompoundStatementSet(statements);
    }
}