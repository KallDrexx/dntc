using Mono.Cecil;

namespace Dntc.Common.Definitions.Mutators;

/// <summary>
/// Converts usages of enums to be the enum's underlying type (e.g. int32).
/// </summary>
public class EnumDefinitionMutator : IFieldDefinitionMutator, IMethodDefinitionMutator
{
    public void Mutate(DefinedField field, FieldDefinition cecilField)
    {
        if (!cecilField.FieldType.Resolve().IsEnum)
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
        if (cecilDefinition.ReturnType is not GenericParameter &&
            !cecilDefinition.ReturnType.ContainsGenericParameter &&
            cecilDefinition.ReturnType is not FunctionPointerType &&
            cecilDefinition.ReturnType.Resolve().IsEnum)
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

                if (cecilParam == null ||
                    cecilParam.ParameterType.ContainsGenericParameter ||
                    cecilParam.ParameterType is GenericParameter ||
                    cecilParam.ParameterType is FunctionPointerType)
                {
                    newParameters.Add(param);
                    continue;
                }

                if (cecilParam.ParameterType.FullName != param.Type.Value)
                {
                    // Might have already been modified, so don't touch it
                    newParameters.Add(param);
                    continue;
                }

                var type = cecilParam.ParameterType.Resolve();
                if (!type.IsEnum)
                {
                    newParameters.Add(param);
                    continue;
                }

                var newParam = param with { Type = GetEnumType(type) };
                newParameters.Add(newParam);
            }

            method.Parameters = newParameters;
        }
    }
}