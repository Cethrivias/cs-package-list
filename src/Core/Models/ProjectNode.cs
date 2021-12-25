namespace Core.Models;

public class ProjectNode : AbstractNode
{
    public ProjectNode(string filepath) : base(filepath)
    {
    }

    public List<Package> Packages { get; } = new();
}