namespace SimpleCLI.Commands;

public class SimpleCliArgs
{
    public List<string> Arguments { get; } = new List<string>();
    public Dictionary<string, string> Flags { get; } = new Dictionary<string, string>();
    
    public string GetFlag(string name, string defaultValue = null)
    {
        return Flags.TryGetValue(name.ToLower(), out string value) ? value : defaultValue;
    }
    
    public bool HasFlag(string name)
    {
        return Flags.ContainsKey(name.ToLower());
    }
    
    public string this[int index] => index < Arguments.Count ? Arguments[index] : null;
}