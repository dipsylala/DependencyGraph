using System.Data;
using GraphGenerator;
using Mono.Options;
using System.Text.Json;


var verbose = false;
var input = string.Empty;
var json = string.Empty;
var showHelp = false;
var norecurse = false;
var showDependencies = false;
var showTopLevel = false;

var options = new OptionSet {
    { "v|verbose", "Enable verbose output", v => verbose = v != null },
    { "i|input=", "Path to the input file (can include file wildcards)", i => input = i },
    { "n|norecurse", "Don't recurse - focus on the input file", n => norecurse = n != null},
    { "d|dependencies", "Show only the dependencies (assemblies that are referenced by others)", d => showDependencies = d != null},
    { "t|toplevel", "Show only the top-level modules (assemblies not referenced by others)", t => showTopLevel = t != null},
    { "j|json=", "The output json file", i => json = i },
    { "h|help", "Show this message and exit", h => showHelp = h != null },
};

try
{
    options.Parse(args);
}
catch (OptionException e)
{
    Console.WriteLine(e.Message);
    Console.WriteLine("Try `--help' for more information.");
    return 1;
}

if (showHelp)
{
    ShowHelp(options);
    return 1;
}

if (verbose)
{
    Console.WriteLine("Verbose mode is ON.");
}

if (ValidateArgs() == false)
{
    return 1;
}

HashSet<AssemblyDetails> processedAssemblies;
try
{
    processedAssemblies = DependencyRetriever.GetDependencyByAssembly(input, new List<string>(), verbose);
}
catch (FileNotFoundException ex)
{
    Console.WriteLine(ex.Message);
    return 1;
}

DependencyRetriever.SetTopLevelAssemblies(processedAssemblies);

var jsonOptions = new JsonSerializerOptions
{
    WriteIndented = true
};

if (json != string.Empty)
{
    var outputJson = JsonSerializer.Serialize(processedAssemblies, jsonOptions);
    File.WriteAllText(json, outputJson);
}

if (showDependencies || showTopLevel)
{
    IEnumerable<AssemblyDetails> assembliesToShow;

    if (showDependencies && showTopLevel)
    {
        // Show all assemblies if both flags are specified
        assembliesToShow = processedAssemblies.OrderBy(a => a.Name);
    }
    else if (showDependencies)
    {
        // Show only dependencies (assemblies that are NOT top-level)
        assembliesToShow = processedAssemblies.Where(a => !a.IsTopLevel).OrderBy(a => a.Name);
    }
    else // showTopLevel
    {
        // Show only top-level assemblies
        assembliesToShow = processedAssemblies.Where(a => a.IsTopLevel).OrderBy(a => a.Name);
    }

    foreach (var assembly in assembliesToShow)
    {
        Console.WriteLine(assembly);
    }

    return 0;
}

foreach (var assembly in processedAssemblies)
{
    Console.WriteLine($"Assembly: {assembly}");
    foreach (var dependency in assembly.Dependencies.OrderBy(x => x.Name))
    {
        Console.WriteLine($"  Depends on: {dependency}");
    }
}

return 0;

bool ValidateArgs()
{
    if (string.IsNullOrEmpty(input))
    {
        Console.WriteLine("Error: No input specified.");
        Console.WriteLine("Try `--help' for more information.");
        return false;
    }

    return true;
}

void ShowHelp(OptionSet optionset)
{
    Console.WriteLine("Usage: DependencyGraph [OPTIONS]+");
    Console.WriteLine("Iterates through the assemblies and gets their dependencies");
    Console.WriteLine();
    Console.WriteLine("Options:");
    optionset.WriteOptionDescriptions(Console.Out);
}
