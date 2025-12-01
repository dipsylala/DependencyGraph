using System.Runtime.Versioning;
using System.IO;

namespace GraphGenerator
{
    public class AssemblyDetails
    {
        public string Name { get; set; }
        public string? Version { get; set; }

        public string FullPath { get; set; }

        public string? TargetFramework { get; set; }

        public bool Found { get; set; }

        public bool IsTopLevel { get; set; }

        public List<AssemblyDetails> Dependencies { get; set; }

        public AssemblyDetails(string name, string fullPath, string? version, string? targetFramework, bool found)
        {
            Name = name;
            FullPath = fullPath;
            Version = version;
            TargetFramework = targetFramework;
            Found = found;
            IsTopLevel = false;

            Dependencies = new List<AssemblyDetails>();
        }

        public override bool Equals(object? obj)
        {
            if (obj is not AssemblyDetails details)
            {
                return false;
            }

            // Normalize paths for comparison
            var thisPath = string.IsNullOrEmpty(FullPath) ? string.Empty : Path.GetFullPath(FullPath);
            var otherPath = string.IsNullOrEmpty(details.FullPath) ? string.Empty : Path.GetFullPath(details.FullPath);

            return string.Equals(thisPath, otherPath, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            // Normalize path for hash code
            try
            {
                var normalizedPath = string.IsNullOrEmpty(FullPath) ? string.Empty : Path.GetFullPath(FullPath);
                return StringComparer.OrdinalIgnoreCase.GetHashCode(normalizedPath);
            }
            catch
            {
                // If path normalization fails, use the original path
                return StringComparer.OrdinalIgnoreCase.GetHashCode(FullPath ?? string.Empty);
            }
        }

        public override string ToString()
        {
            return $"{Name}, Version={Version}, Path={FullPath}, Target Framework='{TargetFramework ?? "unavailable"}', Found = {Found}, IsTopLevel = {IsTopLevel}";
        }
    }
}
