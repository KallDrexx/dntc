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
* [How It Works](#how-it-works)
  * [Notable Features](#notable-features)
  * [Limitations](#limitations)
  * [Pipeline](#pipeline)
    * [1 - Read All Assemblies](#1---read-all-assemblies)
    * [2 - Form Dependency Graphs](#2---form-dependency-graphs)
    * [3 - Determine Conversion Requirements For Each Dependency](#3---determine-conversion-requirements-for-each-dependency)
    * [4 - Generate an Implementation Plan](#4---generate-an-implementation-plan)
    * [5 - Abstract Syntax Tree Creation](#5---abstract-syntax-tree-creation)
    * [6 - Final Output Generation](#6---final-output-generation)
  * [Customization Via Attributes](#customization-via-attributes)
    * [NativeFunctionCallAttribute](#nativefunctioncallattribute)
    * [NativeGlobalAttribute](#nativeglobalattribute)
    * [CustomFunctionNameAttribute](#customfunctionnameattribute)
    * [CustomDeclarationAttribute](#customdeclarationattribute)
    * [CustomFileNameAttribute](#customfilenameattribute)
    * [IgnoreInHeaderAttribute](#ignoreinheaderattribute)
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

# How It Works

## Notable Features

* Customization of the transpilation process via attributes
* Easy support for dotnet transpiled code to reference native functions and globals
* Generic method support (as long as concrete type can be determined via static analysis)
* Function pointers passed in from C99 into transpiled methods
  * Represented in C# via `delegate*<T>`
* Arrays being passed in from C99 into transpiled methods.
* Static constructor transpilation with helper functions to allow native code to determine
  when the cost of static constructors are paid.

## Limitations

While the transpiler is still in development, it has some note-worthy limitations to keep in mind.
The following functionality is not supported:

* Most reference types. 
* Interfaces (outside of generic type parameters)
* Casting between non-primitive types (including struct inheritance)
* Most string operations
* Generic functions as a root method for transpilation
  * Any function that takes a generic must have a concrete type for its arguments that can be
    statically guaranteed.
* mscorlib and native types/methods in the .net framework.

Most of these limitations have plans for solving in the future.

## Pipeline

The transpilation process runs as a series of steps run one at a time.

### 1 - Read All Assemblies

All assemblies from the manifest are read. We create a catalog of custom definitions for each 
type we find, their methods, and fields.

### 2 - Form Dependency Graphs

For each method to transpile specified in the manifest look at all types and fields the transpiled 
method references and add it to a dependency graph. The process is then repeated for each method
the transpiled method invokes.

### 3 - Determine Conversion Requirements For Each Dependency

For each dependency found in the dependency graph, we create a catalog of information of how 
each type, method, and field should be transpiled. This includes the name of the header and 
source file it will be compiled in, what its name will be in C, etc...

### 4 - Generate an Implementation Plan

Based on the dependency graphs we calculated, we form an implementation plan for what header
files and source files will need to be generated. For each header and source file that is planned
out it figures out which other headers are required to be referenced, what globals are declared 
or has implementations, and which methods are declared or implemented.

### 5 - Abstract Syntax Tree Creation

Once we know which types, globals, and methods will go in each header and source file, the transpiler
goes through each file and generates an abstract syntax tree with a non-textual representation of
the transpiled output. Any .net methods that are implemented will have their IL instructions analyzed
and converted into C99 abstract expressions and statements.

The user of ASTs allows the process to be flexible and easily testable.

### 6 - Final Output Generation

Then the ASTs for each output file are taken, and they are converted into actual C99 compilable text.

## Customization Via Attributes

Several attributes can be used to customize how specific fields, types, and methods are transpiled.

### NativeFunctionCallAttribute

Annotating a method with the `[NativeFunctionCall]` tells the transpiler that any calls to
the method should be replaced with a function with the provided name. This allows using the .net 
method as a stub, and replace it with a function defined in C. 

This can be used for transpiled code to call functionality that needs to be specially optimized, 
or invoking capabilities that can't be invoked from transpiled code (e.g. `printf()`);

### NativeGlobalAttribute

Static fields can be annotated with `[NativeGlobal]` to denote that the field itself shouldn't be
transpiled, but instead references to it are replaced with calls to a natively defined global.

This is useful when integrating into a system that has globals defined and they need to be referenced
by the .net code.

### CustomFunctionNameAttribute

Methods annotated with `[CustomFunctionName]` allow the method to be named something specific when
transpiled instead of relying on its auto-generated name. 

This allows providing a nicer name for functions that need to be called from the integrated C99 code.

### CustomDeclarationAttribute

Methods annotated with `[CustomDeclaration]` allows customizing the complete declaration of a function,
including the return type and parameters. 

This is useful when a method needs to be transpiled but declared using a macro.

### CustomFileNameAttribute

Methods, fields and types annotated with `[CustomFileName]` allows changing the name of the header
and source file that the type will be declared and implemented in. 

### IgnoreInHeaderAttribute

Methods, fields, and types annotated with `[IgnoreInHeader]` will only be declared in a source file
and not exist in a header file.

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

The [Octahedron SDL sample](Samples/Octahedron/Octahedron.Sdl) utilizes the transpiler to compile
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

[![ESP32S3 gif](img/octahedron-esp32s3.png)](https://www.youtube.com/watch?v=W-Fh5kGQaDg)
