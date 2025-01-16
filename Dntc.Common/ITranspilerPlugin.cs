namespace Dntc.Common;

/// <summary>
/// Interface transpiler plugins implement to customize the transpilation process.
/// </summary>
public interface ITranspilerPlugin
{
    /// <summary>
    /// If true, the plugin expects the transpiler **NOT** add in it the standard
    /// native methods, types, and globals before the transpiler context is
    /// provided for customization. This means things like `System.Int32` will not
    /// be defined unless the plugin manually defines them.
    /// </summary>
    bool BypassBuiltInNativeDefinitions { get; }
    
    /// <summary>
    /// Allows the plugin to customize the transpilation context (e.g. add mutators,
    /// definers, add custom types, etc...).
    /// </summary>
    void Customize(TranspilerContext context);
}