using System.Text.RegularExpressions;
using Cli.Views;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using Core;

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

    public ValueTask ExecuteAsync(IConsole console)
    {
        var packages = _packagesFinder.Find(Exclude);
        new ListCommandView(console, packages).Print();

        return default;
    }
}