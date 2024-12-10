Transpiles compiled .net code to C99 source code and headers. This allows code
originally written in C# (or any language that can compile down to MSIL) to be integrated into
applications built in other languages, or run .net code on systems that do not have a supported
.net runtime.

Since integration into non .net code bases is a core goal, the transpilation process can be
customized on a method by method basis.

<!-- TOC -->
* [Execution](#execution)
  * [File Deletion Warning](#file-deletion-warning)
  * [Method Querying](#method-querying)
* [Samples](#samples)
  * [Octahedron](#octahedron)
    * [SDL Project](#sdl-project)
    * [ESP32S3](#esp32s3)
<!-- TOC -->

# Execution

In order to perform a transpilation, a manifest needs to be created that instructs the transpiler
how to operate. The manifest is a `json` file containing an object with the following fields:

* `DotNetProjectDirectory` - If provided, the .net project in that directory will be built with the
    `dotnet` CLI tool. 
* `BuildInDebugMode` - If true and `DotNetProjectDirectory` is specified, the .net project to be built
    is built in debug mode.
* `AssemblyDirectory` - The directory containing assemblies to read for transpiling
* `AssembliesToLoad` - List of all `dll` files to read
* `MethodsToTranspile` - List of all .net methods that should be transpiled. Any methods called by a
    method listed here will also be transpiled, as long as any types and globals used.
  * The methods are specified in their IL id format. See [Method Querying](#method-querying) for 
* `OutputDirectory` - Directory to place all generated source and header files.
* `SingleGeneratedSourceFileName` - If specified, no header files will be generated and all types, globals,
    and functions are in a single file with the name provided.

Once a manifest file has been created, the transpiler can be invoked by passing the manifest file as
its argument (e.g. `dntc manifest.json`).

After a successful execution the specified methods and any types, globals, and other methods they depend
on will be transpiled.

## File Deletion Warning

To ensure the output directory only contains the most current and accurate transpilation results, 
the whole output directory is deleted. This prevents a method that is no longer being transpiled
from lingering around.

If the transpiler detects any non `.c` or `.h` files in the directory, it will abort the
transpilation process, as it's guaranteed a non-transpilated file would be deleted.

For this reason, it's usually a good practice to have the `OutputDirectory` set to its own
directory, such as `generated/`.

The directory wide deletion does not occur when `SingleGeneratedSourceFileName` is set. Instead,
only that one file is deleted.

## Method Querying

The `MethodsToTranspile` manifest value takes a list of strings where each represents the MSIL 
method identifiers. The Ids must match exactly what is seen when the compiled code is inspected.

To get valid values for the methods you are interested in transpiling, the manifest can be created
with the `MethodsToTranspile` value empty. The transpiler can then be invoked with the manifest file
followed by the `query` command (e.g. `dntc manifest.json query`). 

This will load all assemblies that were listed in the manifest and print out all method ids it finds.
You can find any root methods you are interested in transpiling and copy/paste them into the manifest.

# Samples

## Octahedron

The [Octahedron samples](Samples/Octahedron) demonstrates a (very basic) software rasterizer written 
in C#.  The [render function](Samples/Octahedron/Dntc.Samples.Octahedron.Common/Renderer.cs#L5) 
takes in an array RGB565 pixels representing the frame buffer, the camera definition, and how 
many seconds have passed since the starting. A list of triangles is retrieved based on which
shape is determined to be drawn, and those are rendered to the frame buffer.

![monogame gif](img/octahedron-monogame.gif)

The [Octahedron Monogame project](Samples/Octahedron/Dntc.Samples.Octahedron.Monogame) is a standard
C# project which instantiates a Monogame window, a `Texture2d` surface to render to, and then
passes the frame buffer array into the `Render()` function for drawing. It then takes the array
and renders the pixels in the array to the Texture to display it in the window.

This project is mostly to verify the C# works as expected and does not utilize the transpiler at all.

### SDL Project

The [Octahedron SDL sammple](Samples/Octahedron/Octahedron.Sdl) utilizes the transpiler to compile
the sample common project into C code that can be utilized in a standard C SDL project.

The transpilation process uses [its own dntc manifiest](Samples/Octahedron/manifest-sdl.json) to
generate the C code.

![SDL gif](img/octahedron-sdl.gif)

### ESP32S3

The [Ocahedron ESP32S3 sample](Samples/Octahedron/Octahedron.Esp32s3) allows running the sample
on an ESP32S3 all-in one board, all without a .net runtime. It uses 
[its own dntc manifest](Samples/Octahedron/manifest-esp32.json) to transpile the common project into
standard C code, and then utilizes an standard ESP-IDF main function to initialize the LCD, 
initialize a frame buffer array, call the `Render()` function, and display the frame buffer to
the attached parallel LCD.

![ESP32S3 gif](img/octahedron-esp32s3.gif)
