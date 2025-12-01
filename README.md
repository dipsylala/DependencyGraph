Iterates through the provided assembly, retrieving its dependency information, and recursing through the dependencies.

# Examples

## Default Output:

```
❯  dotnet .\DependencyGraph.dll -i .\DependencyGraph.dll
Assembly: DependencyGraph, Version=1.0.0.0, Path=.\DependencyGraph.dll, Target Framework='.NETCoreApp,Version=v8.0', Found = True, IsTopLevel = True
  Depends on: GraphGenerator, Version=1.0.0.0, Path=.\GraphGenerator.dll, Target Framework='.NETCoreApp,Version=v8.0', Found = True, IsTopLevel = False
  Depends on: Mono.Options, Version=6.0.0.0, Path=.\Mono.Options.dll, Target Framework='.NETStandard,Version=v2.0', Found = True, IsTopLevel = False
  Depends on: System.Collections, Version=8.0.0.0, Path=C:\Program Files\dotnet\shared\Microsoft.NETCore.App\8.0.22\System.Collections.dll, Target Framework='.NETCoreApp,Version=v8.0', Found = True, IsTopLevel = False
  Depends on: System.Console, Version=8.0.0.0, Path=C:\Program Files\dotnet\shared\Microsoft.NETCore.App\8.0.22\System.Console.dll, Target Framework='.NETCoreApp,Version=v8.0', Found = True, IsTopLevel = False
  Depends on: System.Linq, Version=8.0.0.0, Path=C:\Program Files\dotnet\shared\Microsoft.NETCore.App\8.0.22\System.Linq.dll, Target Framework='.NETCoreApp,Version=v8.0', Found = True, IsTopLevel = False
  Depends on: System.Runtime, Version=8.0.0.0, Path=C:\Program Files\dotnet\shared\Microsoft.NETCore.App\8.0.22\System.Runtime.dll, Target Framework='.NETCoreApp,Version=v8.0', Found = True, IsTopLevel = False
  Depends on: System.Text.Json, Version=8.0.0.0, Path=C:\Program Files\dotnet\shared\Microsoft.NETCore.App\8.0.22\System.Text.Json.dll, Target Framework='.NETCoreApp,Version=v8.0', Found = True, IsTopLevel = False
Assembly: System.Runtime, Version=8.0.0.0, Path=C:\Program Files\dotnet\shared\Microsoft.NETCore.App\8.0.22\System.Runtime.dll, Target Framework='.NETCoreApp,Version=v8.0', Found = True, IsTopLevel = False
  Depends on: System.Private.CoreLib, Version=8.0.0.0, Path=C:\Program Files\dotnet\shared\Microsoft.NETCore.App\8.0.22\System.Private.CoreLib.dll, Target Framework='.NETCoreApp,Version=v8.0', Found = True, IsTopLevel = False
  Depends on: System.Private.Uri, Version=8.0.0.0, Path=C:\Program Files\dotnet\shared\Microsoft.NETCore.App\8.0.22\System.Private.Uri.dll, Target Framework='.NETCoreApp,Version=v8.0', Found = True, IsTopLevel = False

.... and more
```

## Just show top level modules, with a wildcard

```
❯ .\DependencyGraph.exe -t -i .\*.dll
DependencyGraph, Version=1.0.0.0, Path=.\DependencyGraph.dll, Target Framework='.NETCoreApp,Version=v8.0', Found = True, IsTopLevel = True
Mono.Cecil.Mdb, Version=0.11.5.0, Path=.\Mono.Cecil.Mdb.dll, Target Framework='.NETStandard,Version=v2.0', Found = True, IsTopLevel = True
Mono.Cecil.Pdb, Version=0.11.5.0, Path=.\Mono.Cecil.Pdb.dll, Target Framework='.NETStandard,Version=v2.0', Found = True, IsTopLevel = True
Mono.Cecil.Rocks, Version=0.11.5.0, Path=.\Mono.Cecil.Rocks.dll, Target Framework='.NETStandard,Version=v2.0', Found = True, IsTopLevel = True
```

## Just show dependencies

```
❯ dotnet .\DependencyGraph.dll -d -i .\DependencyGraph.dll
GraphGenerator, Version=1.0.0.0, Path=.\GraphGenerator.dll, Target Framework='.NETCoreApp,Version=v8.0', Found = True, IsTopLevel = False
Microsoft.Win32.Primitives, Version=8.0.0.0, Path=C:\Program Files\dotnet\shared\Microsoft.NETCore.App\8.0.22\Microsoft.Win32.Primitives.dll, Target Framework='.NETCoreApp,Version=v8.0', Found = True, IsTopLevel = False
Microsoft.Win32.Registry, Version=8.0.0.0, Path=C:\Program Files\dotnet\shared\Microsoft.NETCore.App\8.0.22\Microsoft.Win32.Registry.dll, Target Framework='.NETCoreApp,Version=v8.0', Found = True, IsTopLevel = False
Mono.Cecil, Version=0.11.5.0, Path=.\Mono.Cecil.dll, Target Framework='.NETStandard,Version=v2.0', Found = True, IsTopLevel = False
Mono.Options, Version=6.0.0.0, Path=.\Mono.Options.dll, Target Framework='.NETStandard,Version=v2.0', Found = True, IsTopLevel = False
netstandard, Version=2.1.0.0, Path=C:\Program Files\dotnet\shared\Microsoft.NETCore.App\8.0.22\netstandard.dll, Target Framework='.NETCoreApp,Version=v8.0', Found = True, IsTopLevel = False
System.Collections, Version=8.0.0.0, Path=C:\Program Files\dotnet\shared\Microsoft.NETCore.App\8.0.22\System.Collections.dll, Target Framework='.NETCoreApp,Version=v8.0', Found = True, IsTopLevel = False
System.Collections.Concurrent, Version=8.0.0.0, Path=C:\Program Files\dotnet\shared\Microsoft.NETCore.App\8.0.22\System.Collections.Concurrent.dll, Target Framework='.NETCoreApp,Version=v8.0', Found = True, IsTopLevel = False
System.Collections.Immutable, Version=8.0.0.0, Path=C:\Program Files\dotnet\shared\Microsoft.NETCore.App\8.0.22\System.Collections.Immutable.dll, Target Framework='.NETCoreApp,Version=v8.0', Found = True, IsTopLevel = False
System.Collections.NonGeneric, Version=8.0.0.0, Path=C:\Program Files\dotnet\shared\Microsoft.NETCore.App\8.0.22\System.Collections.NonGeneric.dll, Target Framework='.NETCoreApp,Version=v8.0', Found = True, IsTopLevel = False
System.Collections.Specialized, Version=8.0.0.0, Path=C:\Program Files\dotnet\shared\Microsoft.NETCore.App\8.0.22\System.Collections.Specialized.dll, Target Framework='.NETCoreApp,Version=v8.0', Found = True, IsTopLevel = False

.... and more
```
