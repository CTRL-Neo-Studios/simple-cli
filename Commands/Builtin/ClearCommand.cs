namespace SimpleCLI.Commands.Builtin;

public class ClearCommand : SimpleCliCommand
{
    public override string Name => "clear";
    public override string Description => "Clears the console";
    
    public override void Execute(SimpleCliArgs args, SimpleCliParser context)
    {
        // The actual clearing should be handled by the UI
        context.Output("[clear]");
    }
}