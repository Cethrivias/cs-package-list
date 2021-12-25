namespace Core.Models;

public class SolutionNode : AbstractNode
{
    public SolutionNode(string filepath) : base(filepath)
    {
    }

    public List<ProjectNode> Projects { get; set; } = new();
}