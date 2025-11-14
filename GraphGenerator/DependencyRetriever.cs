using Mono.Cecil;

namespace GraphGenerator
{
    public class DependencyRetriever
    {
        internal static Queue<AssemblyDetails> CreateInitialQueue(string initialAssemblyPath, ReaderParameters readerParameters)
        {
            var assembliesToProcess = new Queue<AssemblyDetails>();
            // If no path specified, default to the current directory
            var directoryName = Path.GetDirectoryName(initialAssemblyPath);
            if (string.IsNullOrEmpty(directoryName))
            {
                directoryName = Directory.GetCurrentDirectory();
            }

            var initialFileMatches = Directory.GetFiles(directoryName, Path.GetFileName(initialAssemblyPath));
            foreach (var file in initialFileMatches)
            {
                try
                {
                    AssemblyDefinition.ReadAssembly(file, readerParameters);
                    var initialAssembly = new AssemblyDetails(Path.GetFileName(file), file, null, null, true);
                    assembliesToProcess.Enqueue(initialAssembly);
                }
                catch
                {
                    Console.WriteLine($"Could not load {file}");
                }
            }

            return assembliesToProcess;
        }

        internal static IAssemblyResolver CreateDefaultSearchDirectories(string initialAssemblyPath, List<string> additionalDirectories)
        {
            var assemblyResolver = new DefaultAssemblyResolver();
            assemblyResolver.AddSearchDirectory(Path.GetDirectoryName(initialAssemblyPath));

            // Adding directories to the assembly resolver
            foreach (var directory in additionalDirectories)
            {
                assemblyResolver.AddSearchDirectory(directory);
            }

            return assemblyResolver;

        }

        internal static string? GetTargetFramework(AssemblyDefinition assembly)
        {
            var targetFrameworkAttribute = assembly.CustomAttributes.FirstOrDefault(x => x.AttributeType.FullName == "System.Runtime.Versioning.TargetFrameworkAttribute");
            return targetFrameworkAttribute?.ConstructorArguments[0].Value.ToString();
        }

        internal static string? ResolveAssemblyPath(AssemblyNameReference assemblyRef, IAssemblyResolver resolver)
        {
            try
            {
                var assembly = resolver.Resolve(assemblyRef);
                return assembly.MainModule.FileName;
            }
            catch
            {
                return null;
            }
        }

        public static HashSet<AssemblyDetails> GetDependencyByAssembly(string initialAssemblyPath, List<string> additionalDirectories, bool verbose = false)
        {
            var assemblyResolver = CreateDefaultSearchDirectories(initialAssemblyPath, additionalDirectories);
            var readerParameters = new ReaderParameters { AssemblyResolver = assemblyResolver };

            var processedAssemblies = new HashSet<AssemblyDetails>();
            var assembliesToProcess = CreateInitialQueue(initialAssemblyPath, readerParameters);

            if (assembliesToProcess.Count == 0)
            {
                throw new FileNotFoundException($"Could not find {initialAssemblyPath}");
            }

            while (assembliesToProcess.Count > 0)
            {
                var assemblyDetails = assembliesToProcess.Dequeue();

                if (!processedAssemblies.Add(assemblyDetails))
                {
                    continue; // Skip already processed assemblies
                }

                try
                {
                    var cecilAssemblyDefinition = AssemblyDefinition.ReadAssembly(assemblyDetails.FullPath, readerParameters);
                    PopulateAssemblyDetails(cecilAssemblyDefinition, assemblyDetails);

                    if (verbose)
                    {
                        Console.WriteLine($"Processing assembly: {assemblyDetails}");
                    }

                    foreach (var cecilModule in cecilAssemblyDefinition.Modules)
                    {
                        foreach (var cecilAssemblyReference in cecilModule.AssemblyReferences)
                        {
                            var resolvedAssemblyPath = ResolveAssemblyPath(cecilAssemblyReference, assemblyResolver);
                            AssemblyDetails refAssemblyDetails;

                            if (resolvedAssemblyPath == null)
                            {
                                refAssemblyDetails = new AssemblyDetails(cecilAssemblyReference.Name, "", cecilAssemblyReference.Version.ToString(), null, false);
                                assemblyDetails.Dependencies.Add(refAssemblyDetails);
                                continue;
                            }

                            refAssemblyDetails = new AssemblyDetails(cecilAssemblyReference.Name, resolvedAssemblyPath, cecilAssemblyReference.Version.ToString(), null, true);

                            if (!processedAssemblies.Contains(refAssemblyDetails))
                            {
                                assemblyDetails.Dependencies.Add(refAssemblyDetails);
                                assembliesToProcess.Enqueue(refAssemblyDetails);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (verbose)
                    {
                        Console.WriteLine($"Failed to process assembly: {ex.Message}");
                    }
                }
            }


            return processedAssemblies;
        }

        private static void PopulateAssemblyDetails(AssemblyDefinition cecilAssemblyDefinition, AssemblyDetails assemblyDetails)
        {
            assemblyDetails.Name = cecilAssemblyDefinition.Name.Name;
            assemblyDetails.Version = cecilAssemblyDefinition.Name.Version.ToString();
            assemblyDetails.TargetFramework = GetTargetFramework(cecilAssemblyDefinition);
        }
    }
}
