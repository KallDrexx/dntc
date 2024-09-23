namespace Dntc;

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
        var manifestFileName = args[0];
        var manifest = await Manifest.ParseManifestAsync(args[0]);
        
        ShowManifestInfo(manifestFileName, manifest);

        if (!IsManifestValid(manifest, performMethodQuery))
        {
            Console.WriteLine("Invalid manifest specified");
            return 1;
        }

        if (performMethodQuery)
        {
            AssemblyQuery.Run(manifest);
            return 0;
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
        
        Console.WriteLine("Assemblies to load:");
        foreach (var assembly in manifest.AssembliesToLoad)
        {
            Console.WriteLine($"\t{assembly}");
        }
        
        Console.WriteLine("Methods to transpile:");
        foreach (var method in manifest.MethodsToTranspile)
        {
            Console.WriteLine($"\t{method}");
        }
        
        Console.WriteLine();
    }

    private static bool IsManifestValid(Manifest manifest, bool allowNoMethods)
    {
        if (string.IsNullOrWhiteSpace(manifest.AssemblyDirectory) || !Directory.Exists(manifest.AssemblyDirectory))
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

        foreach (var assembly in manifest.AssembliesToLoad)
        {
            var path = Path.Combine(manifest.AssemblyDirectory, assembly);
            if (!File.Exists(path))
            {
                Console.WriteLine($"Error: the assembly '{path}` does not exist");
                return false;
            }
        }

        if (!allowNoMethods && manifest.MethodsToTranspile.Count == 0)
        {
            Console.WriteLine("Error: No methods specified to transpile");
            return false;
        }
        
        return true;
    }
}
