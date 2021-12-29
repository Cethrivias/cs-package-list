using System.Text.RegularExpressions;
using System.Xml.Linq;
using Core.Models;

namespace Core;

public class PackagesFinder
{
    private readonly SolutionParser _solutionParser;
    private readonly ProjectParser _projectParser;

    public PackagesFinder(SolutionParser solutionParser, ProjectParser projectParser)
    {
        _solutionParser = solutionParser;
        _projectParser = projectParser;
    }

    public async Task<PackagesContainer> Find(PackagesFinderOptions options)
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
            Constants.SolutionExtension => await _solutionParser.Parse(file, options),
            Constants.ProjectExtension => await _projectParser.Parse(file, options),
            _ => throw new ArgumentOutOfRangeException()
        };
        packagesContainer.Add(file, node);
        
        return packagesContainer;
    }
}