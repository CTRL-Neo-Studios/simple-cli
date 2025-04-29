namespace SimpleCLI.Runtime.Core.Exceptions
{
    public class SimpleCliNullCommandException : SimpleCliException
    {
        public SimpleCliNullCommandException(string message) : base(message) { }
    }
}