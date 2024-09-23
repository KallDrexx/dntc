using Newtonsoft.Json;

namespace Dntc.Cli;

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
    /// Full names of all methods to transpile
    /// </summary>
    public IReadOnlyList<string> MethodsToTranspile { get; set; } = Array.Empty<string>();
    
    /// <summary>
    /// Directory to place all generated header and source files in
    /// </summary>
    public string? OutputDirectory { get; set; }

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

        try
        {
            return JsonConvert.DeserializeObject<Manifest>(rawContents) ?? new Manifest();
        }
        catch (Exception exception)
        {
            throw new Exception($"Failed to parse json from manifest file `{manifestFilePath}`", exception);
        }
    }
}
