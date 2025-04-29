using System;

namespace SimpleCLI.Runtime.Commands
{
    public abstract class SimpleCliCommand
    {
        public abstract string Name { get; }
        public virtual string Description { get; } = "No description available";
        public virtual string Usage { get; } = "";
        public virtual string[] Aliases { get; } = Array.Empty<string>();
    
        public abstract void Execute(SimpleCliArgs args, SimpleCliParser context);
    
        public virtual string[] GetAutoCompleteSuggestions(int argIndex, string input)
        {
            return Array.Empty<string>();
        }
    }
}