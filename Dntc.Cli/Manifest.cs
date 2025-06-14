using Dntc.Common.Syntax.Statements.Generators;
using Newtonsoft.Json;

namespace Dntc.Cli;

/// <summary>
/// Describes how the transpile process should be performed, including relevant paths. All
/// paths should either be absolute or relative to the manifest file itself.
/// </summary>
public class Manifest
{
    /// <summary>
    /// Directory containing the .net project to build. If not specified then no project
    /// will be built.
    /// </summary>
    public string? DotNetProjectDirectory { get; set; } 
   
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
    public IReadOnlyList<string> AssembliesToLoad { get; set; } = Array.Empty<string>();

    /// <summary>
    /// The name of the assembly (in the AssemblyDirectory) that contains a transpiler
    /// plugin that should be activated.
    /// </summary>
    public string? PluginAssembly { get; set; }
    
    /// <summary>
    /// Full names of all methods to transpile
    /// </summary>
    public IReadOnlyList<string> MethodsToTranspile { get; set; } = Array.Empty<string>();

    /// <summary>
    /// Full identifiers for globals to explicitly compile, even if they are not referenced
    /// by other .net code.
    /// </summary>
    public IReadOnlyList<string> GlobalsToTranspile { get; set; } = Array.Empty<string>();
    
    /// <summary>
    /// Directory to place all generated header and source files in
    /// </summary>
    public string? OutputDirectory { get; set; }
   
    /// <summary>
    /// If specified, directs all transpiled code to a single source file with no 
    /// </summary>
    public string? SingleGeneratedSourceFileName { get; set; }

    public DebugInfoMode DebugInfoMode { get; set; } = DebugInfoMode.CLineSourceMaps;

    public static async Task<Manifest> ParseManifestAsync(string manifestFilePath)
    {
        string rawContents;
        try
        {
            rawContents = await File.ReadAllTextAsync(manifestFilePath);
        }
        catch (Exception exception)
        {
            throw new Exception($"Failed to read manifest file '{manifestFilePath}'", exception);
        }

        Manifest manifest;
        try
        {
            manifest = JsonConvert.DeserializeObject<Manifest>(rawContents) ?? new Manifest();
        }
        catch (Exception exception)
        {
            throw new Exception($"Failed to parse json from manifest file `{manifestFilePath}`", exception);
        }
        
        // Update all directories to be absolute paths, changing relative paths to be relative to the
        // manifest file for more predictable file placement.
        var manifestDirectory = Path.GetDirectoryName(manifestFilePath);
        manifest.DotNetProjectDirectory = ConvertToAbsolutePath(manifestDirectory!, manifest.DotNetProjectDirectory);
        manifest.OutputDirectory = ConvertToAbsolutePath(manifestDirectory!, manifest.OutputDirectory);
        manifest.AssemblyDirectory = ConvertToAbsolutePath(manifestDirectory!, manifest.AssemblyDirectory);
        
        return manifest;
    }

    private static string? ConvertToAbsolutePath(string manifestPath, string? filePath)
    {
        if (filePath == null || Path.IsPathRooted(filePath))
        {
            return filePath;
        }

        var combinedPath = Path.Combine(Path.GetFullPath(manifestPath), filePath);
        return Path.GetFullPath(combinedPath);
    }
}
