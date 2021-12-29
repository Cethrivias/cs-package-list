namespace Core.Models;

public record Package(string Name, string Version, License? License = null)
{
    public License? License { get; set; } = License;
}