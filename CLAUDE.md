# DNTC (.NET to C Transpiler) Development Instructions

## Context & Mission
You are working on DNTC, a transpiler that converts MSIL (Microsoft Intermediate Language) from compiled .NET 
applications into C code. The goal is enabling .NET applications to run on platforms without .NET runtime by 
leveraging native C compilers for final compilation.

## Architecture Overview

### Core Components
- **Dntc.Common** - Main transpiler engine containing all conversion logic
- **Dntc.Cli** - Command-line interface for executing transpilation
- **Dntc.Attributes** - Custom attributes for transpilation hints
- **Samples/Scratchpad** - Testing and validation framework

### Dntc.Common Structure
```
/Dependencies   - Dependency graph creation and management
/Definitions    - MSIL type/function interpretation rules
/Conversion     - Definition-to-C transpilation logic
/Planning       - C source/header file organization
/OpCodeHandling - MSIL opcode analysis and AST generation
/Syntax         - Custom AST framework for C code generation
```

### Testing Framework
```
/Samples/Scratchpad/
├── ScratchpadCSharp/     - C# test cases for MSIL scenarios
├── scratchpad_c/         - C project consuming generated code
└── scratchpad-release.json - Transpilation manifest
```

## Transpilation Pipeline

1. **Manifest Loading** - Parse JSON declaring assemblies, methods, globals, and plugins
2. **Dependency Analysis** - Build complete dependency graph from manifest entry points
3. **Definition Creation** - Generate DefinedType/Field/Method via `IDotNet*Definer` interfaces
4. **Definition Mutation** - Apply transformations through definition mutators
5. **Conversion Planning** - Create ConversionInfo types and apply conversion mutators
6. **Implementation Planning** - Organize C headers/sources and generate ASTs
7. **Code Generation** - Transform ASTs to final C code and write to disk

## Development Workflow

### Required Steps After Every Code Change
Execute these commands from project root in order:

```bash
# 1. Build entire solution
dotnet build Dntc.sln

# 2. Run transpiler on test project
dotnet run --project Dntc.Cli/Dntc.Cli.csproj -- Samples/Scratchpad/scratchpad-release.json

# 3. Build and test generated C code
(cd Samples/Scratchpad/scratchpad_c && cmake . && make && ./scratchpad_c) # The parenthesis are important!
```

### Validation Requirements
- Run `git status` after workflow completion
- Inspect all `.c` and `.h` file diffs to verify:
    - All intended changes are present
    - No unintentional modifications occurred

## Feature Development Process

### Adding New Features or Bug Fixes

1. **Create Test Case**
    - Add method(s) to `ScratchpadCSharp` project demonstrating the feature/fix
    - Update `scratchpad-release.json` manifest to include new methods

2. **Implement Changes**
    - Modify transpiler logic in appropriate `Dntc.Common` modules
    - Follow standard C# coding conventions

3. **Add Validation Code**
    - Update `Samples/Scratchpad/scratchpad_c/main.c` as needed
    - Add assertions to verify transpiled methods execute correctly

4. **Run Full Workflow**
    - Execute the complete development workflow
    - Verify generated C code compiles and runs correctly

## Key Interfaces & Extension Points

- **`IDotNetFieldDefiner`** - Custom field definition logic
- **`IDotNetTypeDefiner`** - Custom type definition logic
- **`IDotNetMethodDefiner`** - Custom method definition logic
- **Definition Mutators** - Transform definitions after creation
- **Conversion Mutators** - Transform conversion info before planning

## Code Quality Standards

- Follow standard C# coding conventions throughout
- Ensure all features have corresponding test cases in Scratchpad
- Validate generated C code compiles without warnings
- Verify runtime behavior matches original .NET semantics
- Document any complex transpilation logic or edge cases

## Debugging Tips

- Use Scratchpad project to isolate specific MSIL scenarios
- Examine generated C code diffs to understand transpilation changes
- Test edge cases with various .NET language features
- Validate dependency graph correctness for complex scenarios

## Success Criteria

A successful change should:
- Pass all build steps without errors
- Generate valid, compilable C code
- Maintain semantic equivalence with original .NET code
- Include comprehensive test coverage via Scratchpad