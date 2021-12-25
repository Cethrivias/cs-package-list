using CliFx.Infrastructure;

namespace Cli.Views;

public abstract class AbstractView
{
    protected readonly IConsole Console;

    protected AbstractView(IConsole console)
    {
        Console = console;
    }
}