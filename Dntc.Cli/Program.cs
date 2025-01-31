using System.Diagnostics;

namespace Dntc.Cli;

public static class Program
{
    public static async Task<int> Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("No manifest file specified.");
            return 1;
        }

        var performMethodQuery = args.Length >= 2 && args[1].ToLower() == "query";
        var manifestFileName = Path.GetFullPath(args[0]);
        var manifest = await Manifest.ParseManifestAsync(manifestFileName);
        
        ShowManifestInfo(manifestFileName, manifest);

        if (!IsManifestValid(manifest, performMethodQuery))
        {
            Console.WriteLine("Invalid manifest specified");
            return 1;
        }

        var transpiler = new Transpiler(manifest);

        if (performMethodQuery)
        {
            transpiler.Query();
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(manifest.DotNetProjectDirectory))
            {
                await BuildDotNetProjectAsync(manifest);
            }
          
            // Set the working directory to the assembly directory in case Mono.cecil tries to
            // load assembly references at runtime. Otherwise, it won't find them.
            Directory.SetCurrentDirectory(manifest.AssemblyDirectory!);
            await transpiler.RunAsync();
        }

        return 0;
    }
    
    private static void ShowManifestInfo(string manifestFileName, Manifest manifest)
    {
        Console.WriteLine($"Manifest file: {manifestFileName}");
        Console.WriteLine($".Net project dir: {manifest.DotNetProjectDirectory}");
        Console.WriteLine($"Build in debug mode: {manifest.BuildInDebugMode}");
        Console.WriteLine($"Assembly dir: {manifest.AssemblyDirectory}");
        Console.WriteLine($"Output dir: {manifest.OutputDirectory}");
        
        Console.WriteLine();
        Console.WriteLine("Assemblies to load:");
        foreach (var assembly in manifest.AssembliesToLoad)
        {
            Console.WriteLine($"\t{assembly}");
        }
        
        Console.WriteLine();
        Console.WriteLine("Methods to transpile:");
        foreach (var method in manifest.MethodsToTranspile)
        {
            Console.WriteLine($"\t{method}");
        }

        Console.WriteLine();
        Console.WriteLine("Globals to transpile:");
        foreach (var global in manifest.GlobalsToTranspile)
        {
            Console.WriteLine($"\t{global}");
        }
        
        Console.WriteLine();
    }

    private static bool IsManifestValid(Manifest manifest, bool allowNoMethods)
    {
        if (string.IsNullOrWhiteSpace(manifest.AssemblyDirectory))
        {
            Console.WriteLine("Error: No assembly directory provided or it does not exist");
            return false;
        }

        if (string.IsNullOrWhiteSpace(manifest.OutputDirectory))
        {
            Console.WriteLine("Error: No output directory specified, but one is required");
            return false;
        }

        if (manifest.AssembliesToLoad.Count == 0)
        {
            Console.WriteLine("Error: No assemblies specified to load");
            return false;
        }
        
        if (!allowNoMethods && manifest.MethodsToTranspile.Count == 0)
        {
            Console.WriteLine("Error: No methods specified to transpile");
            return false;
        }
        
        return true;
    }

    private static async Task BuildDotNetProjectAsync(Manifest manifest)
    {
        var dotnetRoot = Environment.GetEnvironmentVariable("DOTNET_ROOT");
        if (dotnetRoot == null)
        {
            dotnetRoot = GetDotNetExecutableFromPath();
            if (dotnetRoot == null)
            {
                var message = "No DOTNET_ROOT environment variable found and dotnet cli executable not found in PATH";
                throw new InvalidOperationException(message);
            }
        }
        
        var dotnetExecutable = Path.Combine(dotnetRoot, "dotnet");
        if (!File.Exists(dotnetExecutable))
        {
            var message = $"dotnet executable not found in {dotnetExecutable}";
            throw new InvalidOperationException(message);
        }
        
        Console.WriteLine($"Building project located in ${manifest.DotNetProjectDirectory}");
        var processInfo = new ProcessStartInfo
        {
            WorkingDirectory = manifest.DotNetProjectDirectory,
            FileName = dotnetExecutable,
            Arguments = manifest.BuildInDebugMode
                ? "build -c Debug"
                : "build -c Release",
        };

        var process = Process.Start(processInfo);
        await process!.WaitForExitAsync();
        
        Console.WriteLine();
    }

    /// <summary>
    /// Iterates through all directories in the PATH environment variable, looking for the dotnet cli
    /// </summary>
    private static string? GetDotNetExecutableFromPath()
    {
        var pathEnv = Environment.GetEnvironmentVariable("PATH");
        if (pathEnv == null)
        {
            return null;
        }

        var paths = pathEnv.Split(Path.PathSeparator);
        foreach (var directory in paths)
        {
            var withExtensionExists = File.Exists(Path.Combine(directory, "dotnet.exe"));
            var withoutExtensionExists = File.Exists(Path.Combine(directory, "dotnet"));

            if (withExtensionExists || withoutExtensionExists)
            {
                return directory;
            }
        }

        // Not found
        return null;
    }
}
