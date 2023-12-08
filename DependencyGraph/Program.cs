using GraphGenerator;
using Mono.Options;
using System.Text.Json;


var verbose = false;
var input = string.Empty;
var json = string.Empty;
var showHelp = false;

var options = new OptionSet {
    { "v|verbose", "Enable verbose output", v => verbose = v != null },
    { "i|input=", "The input string", i => input = i },
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

var dependencyRetriever = new DependencyRetriever();

HashSet<AssemblyDetails> processedAssemblies;
try
{
    processedAssemblies = dependencyRetriever.GetDependencyByAssembly(input, new List<string>(), verbose);
}
catch (FileNotFoundException ex)
{
    Console.WriteLine(ex.Message);
    return 1;
}

var jsonOptions = new JsonSerializerOptions
{
    WriteIndented = true
};

if (json != string.Empty)
{
    var outputJson = JsonSerializer.Serialize(processedAssemblies, jsonOptions);
    File.WriteAllText(json, outputJson);
}

foreach (var assembly in processedAssemblies)
{
    Console.WriteLine($"Assembly: {assembly}");
    foreach (var dependency in assembly.Dependencies)
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

void ShowHelp(OptionSet options)
{
    Console.WriteLine("Usage: DependencyGraph [OPTIONS]+");
    Console.WriteLine("Iterates through the assemblies and gets their dependencies");
    Console.WriteLine();
    Console.WriteLine("Options:");
    options.WriteOptionDescriptions(Console.Out);
}
