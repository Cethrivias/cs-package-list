namespace Core.Models;

public abstract class AbstractNode
{
    protected AbstractNode(string filepath)
    {
        Filepath = ParseFilepath(filepath);
    }

    private static string ParseFilepath(string filepath)
    {
        if (filepath.StartsWith("./"))
        {
            filepath = filepath[2..];
        }

        return Path.DirectorySeparatorChar switch
        {
            '\\' => filepath.Replace('/', '\\'),
            '/' => filepath.Replace('\\', '/'),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public string Filepath { get; }
}