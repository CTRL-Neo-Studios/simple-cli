using SimpleCLI.Exceptions;

namespace SimpleCLI.Commands.Builtin;

public class HelpCommand : SimpleCLICommand
{
    private readonly SimpleCLIParser _parser;
    
    public HelpCommand(SimpleCLIParser parser)
    {
        _parser = parser;
    }
    
    public override string Name => "help";
    public override string Description => "Displays help information";
    public override string Usage => "help [command]";
    
    public override void Execute(SimpleCLIArgs args, SimpleCLIParser context)
    {
        if (args.Arguments.Count == 0)
        {
            // List all commands
            context.Output("Available commands:");
            foreach (SimpleCLICommand cmd in _parser.Commands.Values.Distinct().OrderBy(c => c.Name))
            {
                context.Output($"  {cmd.Name.PadRight(15)} - {cmd.Description}");
            }
            context.Output("Type 'help <command>' for more information");
        }
        else
        {
            // Show help for specific command
            string commandName = args[0].ToLower();
            if (_parser.Commands.TryGetValue(commandName, out SimpleCLICommand command))
            {
                context.Output($"{command.Name} - {command.Description}");
                context.Output($"Usage: {command.Usage}");
                
                if (command.Aliases.Length > 0)
                    context.Output($"Aliases: {string.Join(", ", command.Aliases)}");
            }
            else
            {
                throw new SimpleCLIException($"Command not found: {commandName}");
            }
        }
    }
}