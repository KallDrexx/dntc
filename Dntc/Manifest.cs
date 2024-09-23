namespace Dntc;

public class Manifest
{
    /// <summary>
    /// Directory containing the .net project to build. If not specified then no project
    /// will be built.
    /// </summary>
    public string? DotNetProjectDir { get; set; } 
   
    /// <summary>
    /// If true and a .net project has been specified, then the .net project will be
    /// built in debug mode. Default is building in Release mode.
    /// </summary>
    public bool BuildInDebugMode { get; set; }
    
    /// <summary>
    /// Directory with .net assemblies
    /// </summary>
    public string? AssemblyDirectory { get; set; }
    
    /// <summary>
    /// List of dll files to load from the assmebly directory
    /// </summary>
    public IReadOnlyList<string> AssembliesToLoad { get; set; } = ArraySegment<string>.Empty;
    
    /// <summary>
    /// Full names of all methods to transpile
    /// </summary>
    public IReadOnlyList<string> MethodsToTranspile { get; set; } = ArraySegment<string>.Empty;
}
