using System.Runtime.Versioning;

namespace GraphGenerator
{
    public class AssemblyDetails
    {
        public string Name { get; set; }
        public string? Version { get; set; }

        public string FullPath { get; set; }

        public string? TargetFramework { get; set; }

        public bool Found { get; set; }

        public List<AssemblyDetails> Dependencies { get; set; }

        public AssemblyDetails(string name, string fullPath, string? version, string? targetFramework, bool found)
        {
            Name = name;
            FullPath = fullPath;
            Version = version;
            TargetFramework = targetFramework;
            Found = found;

            Dependencies = new List<AssemblyDetails>();
        }

        public override bool Equals(object? obj)
        {
            return obj is AssemblyDetails details && FullPath == details.FullPath;
        }

        public override int GetHashCode()
        {
            return FullPath.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Name}, Version={Version}, Path={FullPath}, Target Framework='{TargetFramework ?? "unavailable"}', Found = {Found}";
        }
    }
}
