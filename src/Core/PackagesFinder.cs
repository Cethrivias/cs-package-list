using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.XPath;
using Core.Models;

namespace Core;

public class PackagesFinder
{
    public PackagesContainer Find(Regex? exclude)
    {
        var packagesContainer = new PackagesContainer();

        var file = Directory
            .GetFiles(".")
            .Single(
                it => new[] { Constants.SolutionExtension, Constants.ProjectExtension }
                    .Contains(Path.GetExtension((string?) it))
            );


        AbstractNode node = Path.GetExtension(file) switch
        {
            Constants.SolutionExtension => ProcessSolution(file, exclude),
            Constants.ProjectExtension => ProcessProject(file, exclude),
            _ => throw new ArgumentOutOfRangeException()
        };
        packagesContainer.Add(file, node);


        return packagesContainer;
    }

    private SolutionNode ProcessSolution(string solutionPath, Regex? exclude)
    {
        var solutionNode = new SolutionNode(solutionPath);

        var lines = File.ReadAllLines(solutionPath);

        foreach (var line in lines)
        {
            if (!line.StartsWith("Project")) continue;

            var path = line.Split(",").ElementAtOrDefault(1)?.Trim(' ', '"');

            if (path is null || Path.GetExtension(path) != Constants.ProjectExtension) continue;

            solutionNode.Projects.Add(ProcessProject(path, exclude));
        }

        return solutionNode;
    }

    private ProjectNode ProcessProject(string projectPath, Regex? exclude)
    {
        var node = new ProjectNode(projectPath);

        var doc = XDocument.Load(File.OpenRead(node.Filepath));
        var elements = doc.XPathSelectElements("//Project/ItemGroup/PackageReference");

        foreach (var element in elements)
        {
            var name = element.Attributes().Single(it => it.Name == "Include").Value;
            var version = element.Attributes().Single(it => it.Name == "Version").Value;

            if (exclude is not null && exclude.IsMatch(name)) continue;

            node.Packages.Add(new Package(name, version));
        }

        return node;
    }
}