using Mono.Cecil;

namespace Dntc.Common.Definitions.Mutators;

/// <summary>
/// Converts usages of enums to be the enum's underlying type (e.g. int32).
/// </summary>
public class EnumDefinitionMutator : IFieldDefinitionMutator, IMethodDefinitionMutator
{
    public void Mutate(DefinedField field, FieldDefinition cecilField)
    {
        var resolvedFieldType = cecilField.FieldType.Resolve();
        if (resolvedFieldType == null || !cecilField.FieldType.Resolve().IsEnum)
        {
            return;
        }

        field.IlType = GetEnumType(cecilField.FieldType.Resolve());
    }

    private static IlTypeName GetEnumType(TypeDefinition enumType)
    {
        var field = enumType.Fields
            .Single(x => x.Name == "value__");

        return new IlTypeName(field.FieldType.FullName);
    }

    public void Mutate(DefinedMethod method, MethodDefinition cecilDefinition)
    {
        var resolvedReturnType = cecilDefinition.ReturnType.Resolve();

        if (resolvedReturnType != null && resolvedReturnType.IsEnum)
        {
            method.ReturnType = GetEnumType(cecilDefinition.ReturnType.Resolve());
        }

        if (method.Parameters.Any())
        {
            var newParameters = new List<DefinedMethod.Parameter>();
            foreach (var param in method.Parameters)
            {
                var cecilParam = cecilDefinition.Parameters
                    .FirstOrDefault(x => x.Name == param.Name);

                var cecilParamType = cecilParam?.ParameterType.Resolve();

                if (cecilParamType?.IsEnum != true || cecilParamType.FullName != param.Type.Value)
                {
                    newParameters.Add(param);
                    continue;
                }

                var newParam = param with { Type = GetEnumType(cecilParamType) };
                newParameters.Add(newParam);
            }

            method.Parameters = newParameters;
        }

        if (method.Locals.Any())
        {
            var newLocals = new List<DefinedMethod.Local>();
            for (var index = 0; index < method.Locals.Count; index++)
            {
                var currentLocal = method.Locals[index];
                var cecilLocal = cecilDefinition.Body.Variables[index];

                var resolvedType = cecilLocal.VariableType.Resolve();
                if (resolvedType?.IsEnum != true || resolvedType.FullName != currentLocal.Type.Value)
                {
                    newLocals.Add(currentLocal);
                    continue;
                }

                var newLocal = currentLocal with { Type = GetEnumType(resolvedType) };
                newLocals.Add(newLocal);
            }

            method.Locals = newLocals;
        }
    }
}