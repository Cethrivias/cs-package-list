using System.Text.RegularExpressions;

namespace Core.Models;

public record PackagesFinderOptions
{
    public Regex? Exclude { get; set; }
    public bool Licence { get; set; } = false;
}