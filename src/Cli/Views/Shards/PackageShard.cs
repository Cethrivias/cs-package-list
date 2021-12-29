using CliFx.Infrastructure;
using Core.Models;

namespace Cli.Views.Shards;

public class PackageShard : AbstractShard
{
    private readonly Package _package;

    public PackageShard(IConsole console, Package package) : base(console)
    {
        _package = package;
    }

    public void Print()
    {
        var output = $" - {_package.Name} {_package.Version}";
        if (_package.License is not null)
        {
            output += $", {_package.License.Name} {_package.License.Url}";
        }

        Console.Output.WriteLine(output);
    }
}