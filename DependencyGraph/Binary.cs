namespace DependencyGraph
{
    public struct Binary
    {
        string Name { get; set; }
        string Version { get; set; }

        List<Binary> Dependencies { get; set; }
    }
}
