using System.Xml.Linq;
using Core.Models;

namespace Core;

public class NugetService
{
    private readonly HttpClient _nugetClient = new()
    {
        BaseAddress = new Uri("https://api.nuget.org")
    };
    
    public async ValueTask<License?> GetLicense(Package package)
    {
        try
        {
            var path = $"/v3-flatcontainer/{package.Name}/{package.Version}/{package.Name}.nuspec";
            var result = await _nugetClient.GetStringAsync(path);
            var doc = XDocument.Parse(result);
            var license = doc.Descendants().SingleOrDefault(it => it.Name.LocalName == "license")?.Value;
            var licenseUrl = doc.Descendants().SingleOrDefault(it => it.Name.LocalName == "licenseUrl")?.Value;

            return new License(license, licenseUrl);
        } catch (Exception e)
        {
            Console.Error.WriteLine(e);
            Console.Error.WriteLine($"Could not get license for {package}");
            return null;
        }
    }
}