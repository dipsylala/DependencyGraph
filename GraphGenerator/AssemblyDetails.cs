namespace GraphGenerator
{
    public class AssemblyDetails
    {
        public string Name { get; set; }
        public string Version { get; set; }

        public string FullPath { get; set; }

        public bool Found { get; set; }

        public List<AssemblyDetails> Dependencies { get; set; }

        public AssemblyDetails(string fullPath, string name, string version, bool found)
        {
            Name = name;
            Version = version;
            FullPath = fullPath;
            Found = found;
            Dependencies = new List<AssemblyDetails>();
        }

        public override bool Equals(object obj)
        {
            return obj is AssemblyDetails details && FullPath == details.FullPath;
        }

        public override int GetHashCode()
        {
            return FullPath.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Name}, Version={Version}, Path={FullPath}, Found = {Found}";
        }
    }
}
