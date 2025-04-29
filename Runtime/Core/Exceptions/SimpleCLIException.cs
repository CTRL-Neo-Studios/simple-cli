using System;

namespace SimpleCLI.Runtime.Core.Exceptions
{
    public class SimpleCliException : Exception
    {
        public SimpleCliException(string message) : base(message) { }
    }
}