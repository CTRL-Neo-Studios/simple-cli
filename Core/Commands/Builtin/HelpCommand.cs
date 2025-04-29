using System.Linq;
using SimpleCLI.Core.Exceptions;

namespace SimpleCLI.Core.Commands.Builtin
{
    public class HelpCommand : SimpleCliCommand
    {
        private readonly SimpleCliParser _parser;
    
        public HelpCommand(SimpleCliParser parser)
        {
            _parser = parser;
        }
    
        public override string Name => "help";
        public override string Description => "Displays help information";
        public override string Usage => "help [command]";
    
        public override void Execute(SimpleCliArgs args, SimpleCliParser context)
        {
            if (args.Arguments.Count == 0)
            {
                // List all commands
                context.Output("Available commands:");
                foreach (SimpleCliCommand cmd in _parser.Commands.Values.Distinct().OrderBy(c => c.Name))
                {
                    context.Output($"  {cmd.Name.PadRight(15)} - {cmd.Description}");
                }
                context.Output("Type 'help <command>' for more information");
            }
            else
            {
                // Show help for specific command
                string commandName = args[0].ToLower();
                if (_parser.Commands.TryGetValue(commandName, out SimpleCliCommand command))
                {
                    context.Output($"{command.Name} - {command.Description}");
                    context.Output($"Usage: {command.Usage}");
                
                    if (command.Aliases.Length > 0)
                        context.Output($"Aliases: {string.Join(", ", command.Aliases)}");
                }
                else
                {
                    throw new SimpleCliCommandException($"Command not found: {commandName}");
                }
            }
        }
    }
}