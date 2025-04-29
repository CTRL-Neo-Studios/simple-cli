namespace SimpleCLI.Runtime.Exceptions
{
    public class SimpleCliNullCommandException : SimpleCliException
    {
        public SimpleCliNullCommandException(string message) : base(message) { }
    }
}