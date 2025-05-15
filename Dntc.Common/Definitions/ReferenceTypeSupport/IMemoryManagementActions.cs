using Dntc.Common.Conversion;
using Dntc.Common.Syntax.Expressions;
using Dntc.Common.Syntax.Statements;

namespace Dntc.Common.Definitions.ReferenceTypeSupport;

/// <summary>
/// Provides C statements that allow for memory allocation and deallocation capabilities
/// </summary>
public interface IMemoryManagementActions
{
    /// <summary>
    /// Headers that contain the necessary declarations for the functions
    /// </summary>
    IReadOnlyList<HeaderName> RequiredHeaders { get; }

    /// <summary>
    /// Provides the statements to perform an allocation. It is assumed that the statement sets will
    /// zero out all memory for the allocated variable.
    /// </summary>
    /// <param name="variableToAllocate">The variable to allocate memory into</param>
    /// <param name="cTypeName">The name of the type to allocate to. Usually used for sizeof() calls</param>
    /// <param name="conversionCatalog">Conversion catalog to get type conversion info structures</param>
    CStatementSet AllocateCall(
        Variable variableToAllocate,
        LiteralValueExpression cTypeName,
        ConversionCatalog conversionCatalog);

    /// <summary>
    /// Provides statements to perform a deallocation for the provided variable. It should be assumed that
    /// the variable's pointer could be null-ed out.
    /// </summary>
    CStatementSet FreeCall(Variable variableToFree, ConversionCatalog conversionCatalog);
}