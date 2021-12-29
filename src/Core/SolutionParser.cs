using Core.Models;

namespace Core;

public class SolutionParser
{
    private readonly ProjectParser _projectParser;

    public SolutionParser(ProjectParser projectParser)
    {
        _projectParser = projectParser;
    }

    public async ValueTask<SolutionNode> Parse(string solutionPath, PackagesFinderOptions options)
    {
        var solutionNode = new SolutionNode(solutionPath);

        var lines = await File.ReadAllLinesAsync(solutionPath);

        foreach (var line in lines)
        {
            if (!line.StartsWith("Project")) continue;

            var path = line.Split(",").ElementAtOrDefault(1)?.Trim(' ', '"');

            if (path is null || Path.GetExtension(path) != Constants.ProjectExtension) continue;

            var projectNode = await _projectParser.Parse(path, options);

            solutionNode.Projects.Add(projectNode);
        }

        return solutionNode;
    }
}