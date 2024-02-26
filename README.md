Iterates through the provided assembly, retrieving its dependency information, and recursing through the dependencies.

EG:

`dotnet DependencyGraph.dll -i DependencyGraph.dll`

Example Output - getting DependencyGraph to iterate itself:

```
❯ dotnet .\DependencyGraph.dll -i .\DependencyGraph.dll
Assembly: DependencyGraph, Version=1.0.0.0, Path=.\DependencyGraph.dll, Target Framework=.NETCoreApp,Version=v7.0, Found = True
  Depends on: System.Runtime, Version=7.0.0.0, Path=C:\Program Files\dotnet\shared\Microsoft.NETCore.App\7.0.16\System.Runtime.dll, Target Framework=.NETCoreApp,Version=v7.0, Found = True
  Depends on: Mono.Options, Version=6.0.0.0, Path=.\Mono.Options.dll, Target Framework=.NETStandard,Version=v2.0, Found = True
  Depends on: GraphGenerator, Version=1.0.0.0, Path=.\GraphGenerator.dll, Target Framework=.NETCoreApp,Version=v7.0, Found = True
  Depends on: System.Collections, Version=7.0.0.0, Path=C:\Program Files\dotnet\shared\Microsoft.NETCore.App\7.0.16\System.Collections.dll, Target Framework=.NETCoreApp,Version=v7.0, Found = True
  Depends on: System.Text.Json, Version=7.0.0.0, Path=C:\Program Files\dotnet\shared\Microsoft.NETCore.App\7.0.16\System.Text.Json.dll, Target Framework=.NETCoreApp,Version=v7.0, Found = True
  Depends on: System.Console, Version=7.0.0.0, Path=C:\Program Files\dotnet\shared\Microsoft.NETCore.App\7.0.16\System.Console.dll, Target Framework=.NETCoreApp,Version=v7.0, Found = True
Assembly: System.Runtime, Version=7.0.0.0, Path=C:\Program Files\dotnet\shared\Microsoft.NETCore.App\7.0.16\System.Runtime.dll, Target Framework=.NETCoreApp,Version=v7.0, Found = True
  Depends on: System.Private.CoreLib, Version=7.0.0.0, Path=C:\Program Files\dotnet\shared\Microsoft.NETCore.App\7.0.16\System.Private.CoreLib.dll, Target Framework=.NETCoreApp,Version=v7.0, Found = True
```
