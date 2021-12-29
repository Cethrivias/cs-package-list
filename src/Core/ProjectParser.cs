using System.Collections.Concurrent;
using System.Xml.Linq;
using System.Xml.XPath;
using Core.Models;

namespace Core;

public class ProjectParser
{
    private readonly NugetService _nugetService;

    public ProjectParser(NugetService nugetService)
    {
        _nugetService = nugetService;
    }

    public async ValueTask<ProjectNode> Parse(string projectPath, PackagesFinderOptions options)
    {
        try
        {
            var node = new ProjectNode(projectPath);

            var doc = XDocument.Load(File.OpenRead(node.Filepath));
            var elements = doc.XPathSelectElements("//Project/ItemGroup/PackageReference");

            var packages = new ConcurrentBag<Package>();
            await Parallel.ForEachAsync(
                elements,
                new ParallelOptions { MaxDegreeOfParallelism = 10 },
                ParsePackage(packages, options)
            );
            node.Packages.AddRange(packages);

            return node;
        } catch (Exception e)
        {
            Console.Error.WriteLine(e);
            throw new Exception($"Could not get packages for {projectPath}");
        }
    }

    private Func<XElement, CancellationToken, ValueTask> ParsePackage(ConcurrentBag<Package> packages, PackagesFinderOptions options)
    {
        return async (element, _) =>
        {
            var name = element.Attributes().Single(it => it.Name == "Include").Value;
            var version = ParseVersion(element);

            if (options.Exclude is not null && options.Exclude.IsMatch(name)) return;

            var package = new Package(name, version);
            if (options.Licence)
            {
                package.License = await _nugetService.GetLicense(package);
            }
            
            packages.Add(package);
        };
    }

    private static string ParseVersion(XElement element)
    {
        try
        {
            var version = element.Attributes().SingleOrDefault(it => it.Name == "Version");

            return version is not null
                ? version.Value
                : element.Elements().Single(it => it.Name == "Version").Value;
        } catch (Exception e)
        {
            Console.Error.WriteLine(e);
            throw new Exception("Could not get package version");
        }
    }
}