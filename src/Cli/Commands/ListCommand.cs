using System.Text.RegularExpressions;
using Cli.Views;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using Core;
using Core.Models;

namespace Cli.Commands;

[Command(Description = "Lists installed packages")]
public class ListCommand : ICommand
{
    private readonly PackagesFinder _packagesFinder;

    public ListCommand(PackagesFinder packagesFinder)
    {
        _packagesFinder = packagesFinder;
    }

    [CommandOption("exclude", 'e', Description = "Excludes packages that match a regular expression")]
    public Regex? Exclude { get; set; }

    [CommandOption(
        "license",
        'l',
        Description = "Includes license information in the output. Makes requests to NuGet API"
    )]
    public bool License { get; set; } = false;

    public async ValueTask ExecuteAsync(IConsole console)
    {
        var options = new PackagesFinderOptions
        {
            Exclude = Exclude,
            Licence = License,
        };
        var packages = await _packagesFinder.Find(options);
        new ListCommandView(console, packages).Print();
    }
}