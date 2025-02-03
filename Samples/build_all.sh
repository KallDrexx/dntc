#!/bin/bash

cd "$(dirname "$0")"

dotnet build ../ -c Release
dotnet run --project ../Dntc.Cli/Dntc.Cli.csproj -- Scratchpad/scratchpad-release.json
dotnet run --project ../Dntc.Cli/Dntc.Cli.csproj -- Scratchpad/scratchpad-single-file.json
dotnet run --project ../Dntc.Cli/Dntc.Cli.csproj -- Octahedron/manifest-sdl.json
dotnet run --project ../Dntc.Cli/Dntc.Cli.csproj -- Octahedron/manifest-esp32.json
