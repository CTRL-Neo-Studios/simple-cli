namespace SimpleCLI.Commands.Builtin;

public class ClearCommand : SimpleCLICommand
{
    public override string Name => "clear";
    public override string Description => "Clears the console";
    
    public override void Execute(SimpleCLIArgs args, SimpleCLIParser context)
    {
        // The actual clearing should be handled by the UI
        context.Output("[clear]");
    }
}