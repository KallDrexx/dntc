using Dntc.Common.Conversion;
using Dntc.Common.Syntax.Expressions;
using Dntc.Common.Syntax.Statements;

namespace Dntc.Common.Definitions.ReferenceTypeSupport;

public class PrepToFreeDefinedMethod : CustomDefinedMethod
{
    private readonly DotNetDefinedType _dotNetType;
    
    public PrepToFreeDefinedMethod(DotNetDefinedType dotNetType)
        : base(
            ReferenceTypeConstants.PrepTypeToFreeMethodId(dotNetType.IlName),
            new IlTypeName(typeof(void).FullName!),
            dotNetType.Namespace,
            Utils.GetHeaderName(dotNetType.Namespace),
            Utils.GetSourceFileName(dotNetType.Namespace),
            new CFunctionName(Utils.MakeValidCName($"{dotNetType.Definition.FullName}__PrepForFree")),
            [
                new Parameter(dotNetType.IlName, "object", true),
            ])
    {
        _dotNetType = dotNetType;
    }

    public override CustomCodeStatementSet? GetCustomDeclaration(ConversionCatalog catalog)
    {
        var typeName = catalog.Find(_dotNetType.IlName);
        return new CustomCodeStatementSet($"void {NativeName}({typeName.NameInC}* object)");
    }

    public override CStatementSet? GetCustomImplementation(ConversionCatalog catalog)
    {
        var statements = new List<CStatementSet>();
        var referenceTypeFields = _dotNetType.Definition
            .Fields
            .Where(x => !x.FieldType.IsValueType)
            .OrderBy(x => x.Name)
            .Select(x => new IlFieldId(x.FullName))
            .ToArray();

        var untrackMethod = catalog.Find(ReferenceTypeConstants.GcUntrackMethodId);
        foreach (var field in referenceTypeFields)
        {
            var fieldInfo = catalog.Find(field);
            statements.Add(
                new CustomCodeStatementSet(
                    $"\t{untrackMethod.NameInC}(({ReferenceTypeConstants.ReferenceTypeBaseName}**)&object->{fieldInfo.NameInC});"));

            statements.Add(new CustomCodeStatementSet("\n")); // Need to come up with a better white space strategy
        }

        var baseType = Utils.GetNonSystemBaseType(_dotNetType.Definition);
        if (baseType != null)
        {
            var baseTypeInfo = catalog.Find(new IlTypeName(baseType.FullName));
            var voidType = catalog.Find(new IlTypeName(typeof(void).FullName!));
            var prepMethod = catalog.Find(ReferenceTypeConstants.PrepTypeToFreeMethodId(baseTypeInfo.IlName));
            var fnExpression = new LiteralValueExpression(prepMethod.NameInC.Value, voidType);
            var methodCall = new MethodCallExpression(
                fnExpression,
                prepMethod.Parameters,
                [
                    new LiteralValueExpression($"&(object->base)", baseTypeInfo),
                ],
                voidType,
                catalog);

            statements.Add(new VoidExpressionStatementSet(methodCall));
        }

        if (statements.Count == 0)
        {
            statements.Add(new CustomCodeStatementSet($"\t// No cleanup necessary\n"));
        }

        return new CompoundStatementSet(statements);
    }
}