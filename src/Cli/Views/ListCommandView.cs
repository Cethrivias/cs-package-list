using CliFx.Infrastructure;
using Core.Models;

namespace Cli.Views;

public class ListCommandView : AbstractView
{
    private readonly PackagesContainer _packagesContainer;

    public ListCommandView(IConsole console, PackagesContainer packagesContainer) : base(console)
    {
        _packagesContainer = packagesContainer;
    }

    public void Print()
    {
        var cwd = Directory.GetCurrentDirectory();
        Console.Output.WriteLine($"Current working directory: {cwd}");

        foreach (var node in _packagesContainer.Values)
        {
            PrintNode(node);
        }
    }

    private void PrintNode(AbstractNode node)
    {
        switch (node)
        {
            case SolutionNode solutionNode:
                PrintSolutionNode(solutionNode);
                break;
            case ProjectNode projectNode:
                PrintProjectNode(projectNode);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(node));
        }

        Console.Output.WriteLine(new string('-', 10));
        PrintUnique(node);
    }

    private void PrintSolutionNode(SolutionNode solutionNode)
    {
        Console.Output.WriteLine($"Solution: {solutionNode.Filepath}");

        foreach (var project in solutionNode.Projects)
        {
            PrintProjectNode(project);
        }
    }

    private void PrintProjectNode(ProjectNode project)
    {
        Console.Output.WriteLine($"Project: {project.Filepath}. {project.Packages.Count} packages");

        foreach (var (name, version) in project.Packages)
        {
            Console.Output.WriteLine($" - Package: {name}, {version}");
        }
    }

    private void PrintUnique(AbstractNode node)
    {
        var packages = new HashSet<Package>();

        switch (node)
        {
            case SolutionNode solutionNode:
                solutionNode.Projects.ForEach(
                    projectNode => projectNode.Packages.ForEach(package => packages.Add(package))
                );
                break;
            case ProjectNode projectNode:
                projectNode.Packages.ForEach(package => packages.Add(package));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(node));
        }

        Console.Output.WriteLine($"Unique packages: {packages.Count}");
        foreach (var (name, version) in packages)
        {
            Console.Output.WriteLine($" - {name}, {version}");
        }
    }
}