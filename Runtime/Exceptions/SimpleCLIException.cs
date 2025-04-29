using System;

namespace SimpleCLI.Runtime.Exceptions
{
    public class SimpleCliException : Exception
    {
        public SimpleCliException(string message) : base(message) { }
    }
}