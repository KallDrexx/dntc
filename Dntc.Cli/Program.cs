﻿using System.Diagnostics;

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
            var message = "No DOTNET_ROOT environment variable found (needed to locate dotnet executable";
            throw new InvalidOperationException(message);
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
}
