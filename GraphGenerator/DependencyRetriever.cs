using Mono.Cecil;

namespace GraphGenerator
{
    public class DependencyRetriever
    {
        internal string? ResolveAssemblyPath(AssemblyNameReference assemblyRef, BaseAssemblyResolver resolver)
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

        public HashSet<AssemblyDetails> GetDependencyByAssembly(string initialAssemblyPath, List<string> additionalDirectories, bool verbose = false)
        {
            var assemblyResolver = new DefaultAssemblyResolver();

            assemblyResolver.AddSearchDirectory(Path.GetDirectoryName(initialAssemblyPath));

            // Adding directories to the assembly resolver
            foreach (var directory in additionalDirectories)
            {
                assemblyResolver.AddSearchDirectory(directory);
            }

            var readerParameters = new ReaderParameters { AssemblyResolver = assemblyResolver };
            var processedAssemblies = new HashSet<AssemblyDetails>();
            var assembliesToProcess = new Queue<AssemblyDetails>();

            // Let's see if it's worth continuing
            try
            {
                AssemblyDefinition.ReadAssembly(initialAssemblyPath, readerParameters);
            }
            catch
            {
                throw new FileNotFoundException($"Could not load {initialAssemblyPath}");
            }

            var initialAssembly = new AssemblyDetails(initialAssemblyPath, Path.GetFileName(initialAssemblyPath), "", true);

            assembliesToProcess.Enqueue(initialAssembly);

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
                    assemblyDetails.Name = cecilAssemblyDefinition.Name.Name;
                    assemblyDetails.Version = cecilAssemblyDefinition.Name.Version.ToString();

                    if (verbose)
                    {
                        Console.WriteLine($"Processing assembly: {assemblyDetails}");
                    }
                        
                    foreach (var cecilModule in cecilAssemblyDefinition.Modules)
                    {
                        foreach (var cecilAssemblyReference in cecilModule.AssemblyReferences)
                        {
                            var resolvedAssemblyPath = ResolveAssemblyPath(cecilAssemblyReference, assemblyResolver);

                            if (resolvedAssemblyPath == null)
                            {
                                var refAssemblyDetails = new AssemblyDetails("", cecilAssemblyReference.Name, cecilAssemblyReference.Version.ToString(), false);
                                assemblyDetails.Dependencies.Add(refAssemblyDetails);
                            } 
                            else
                            {
                                var refAssemblyDetails = new AssemblyDetails(resolvedAssemblyPath, cecilAssemblyReference.Name, cecilAssemblyReference.Version.ToString(), true);

                                if (!processedAssemblies.Contains(refAssemblyDetails))
                                {
                                    assemblyDetails.Dependencies.Add(refAssemblyDetails);
                                    assembliesToProcess.Enqueue(refAssemblyDetails);
                                }
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
    }
}
