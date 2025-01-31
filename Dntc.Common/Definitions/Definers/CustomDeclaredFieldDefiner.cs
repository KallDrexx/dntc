using System.ComponentModel.Design;
using Dntc.Attributes;
using Dntc.Common.Syntax.Statements;
using Mono.Cecil;

namespace Dntc.Common.Definitions.Definers;

/// <summary>
/// Creates a custom field definition if the field has a `CustomDeclaration` attribute attached.
/// </summary>
public class CustomDeclaredFieldDefiner : IDotNetFieldDefiner
{
    public DefinedField? Define(FieldDefinition field)
    {
        var attribute = field.CustomAttributes
            .SingleOrDefault(x => x.AttributeType.FullName == typeof(CustomDeclarationAttribute).FullName);

        if (attribute == null)
        {
            return null;
        }

        if (attribute.ConstructorArguments.Count != 3)
        {
            var message = $"CustomDeclarationAttribute on field {field.FullName} was expected to have 3 arguments, " +
                          $"but it instead had {attribute.ConstructorArguments.Count}";
            
            throw new InvalidOperationException(message);
        }

        var declaration = (string)attribute.ConstructorArguments[0].Value;
        var referredBy = (string?)attribute.ConstructorArguments[1].Value;
        var header = (string?)attribute.ConstructorArguments[2].Value;
        var declaringNamespace = Utils.GetNamespace(field.DeclaringType);

        referredBy ??= field.IsStatic
            ? $"{field.DeclaringType.FullName}_{field.Name}"
            : field.Name;

        List<HeaderName> referencedHeaders = header != null ? [new HeaderName(header)] : [];

        return new CustomDeclaredFieldDefinition(
            field,
            declaration,
            Utils.GetHeaderName(declaringNamespace),
            Utils.GetSourceFileName(declaringNamespace),
            new CFieldName(referredBy),
            new IlFieldId(field.FullName),
            new IlTypeName(field.FieldType.FullName),
            field.IsStatic,
            referencedHeaders);
    }

    private class CustomDeclaredFieldDefinition : CustomDefinedField
    {
        private readonly string _declaration;
        
        public CustomDeclaredFieldDefinition(
            FieldDefinition originalField,
            string declaration,
            HeaderName? declaredInHeader, 
            CSourceFileName? declaredInSourceFileName, 
            CFieldName nativeName, 
            IlFieldId name, 
            IlTypeName type, 
            bool isGlobal, 
            IReadOnlyList<HeaderName>? referencedHeaders = null) 
            : base(originalField, declaredInHeader, declaredInSourceFileName, nativeName, name, type, isGlobal, referencedHeaders)
        {
            _declaration = declaration;
        }

        public override CustomCodeStatementSet GetCustomDeclaration()
        {
            return new CustomCodeStatementSet(_declaration);
        }
    }
}