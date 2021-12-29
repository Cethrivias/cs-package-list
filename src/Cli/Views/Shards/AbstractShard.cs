using CliFx.Infrastructure;

namespace Cli.Views.Shards;

public class AbstractShard
{
    protected readonly IConsole Console;

    protected AbstractShard(IConsole console)
    {
        Console = console;
    }
}