using System;

namespace SimpleCLI.Core.Exceptions
{
    public class SimpleCliException : Exception
    {
        public SimpleCliException(string message) : base(message) { }
    }
}