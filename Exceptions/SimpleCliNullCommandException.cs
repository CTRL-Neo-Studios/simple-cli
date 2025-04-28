namespace SimpleCLI.Exceptions;

public class SimpleCliNullCommandException : SimpleCliException
{
    public SimpleCliNullCommandException(string message) : base(message) { }
}