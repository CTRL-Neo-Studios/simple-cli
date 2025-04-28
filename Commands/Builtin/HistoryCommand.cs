namespace SimpleCLI.Commands.Builtin;

public class HistoryCommand : SimpleCLICommand
{
    private readonly SimpleCLIParser _parser;
    
    public HistoryCommand(SimpleCLIParser parser)
    {
        _parser = parser;
    }
    
    public override string Name => "history";
    public override string Description => "Displays command history";
    
    public override void Execute(SimpleCLIArgs args, SimpleCLIParser context)
    {
        if (_parser.CommandHistory.Count == 0)
        {
            context.Output("No commands in history");
            return;
        }
        
        for (int i = 0; i < _parser.CommandHistory.Count; i++)
        {
            context.Output($"{i}: {_parser.CommandHistory[i]}");
        }
    }
}