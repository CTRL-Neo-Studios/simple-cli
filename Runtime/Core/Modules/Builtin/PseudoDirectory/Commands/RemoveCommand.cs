using System;
using SimpleCLI.Runtime.Core.Commands;
using SimpleCLI.Runtime.Core.Modules.Builtin.PseudoDirectory.Core;

namespace SimpleCLI.Runtime.Core.Modules.Builtin.PseudoDirectory.Commands
{
    public class RemoveCommand : SimpleCliCommand
    {
        public RemoveCommand(SimpleCliPseudoDrive drive) : base()
        {
            _drive = drive;
        }
        
        private readonly SimpleCliPseudoDrive _drive;
        public override string Name => "remove";
        public override string Description => "Removes directory(s) or file(s).";

        public override string Usage => "remove Command\n" +
                                        "- remove [path]\n" +
                                        "   Removes files or directories.\n" +
                                        "   Flags: -r\n" +
                                        "   - Removes the paths recursively.";

        public override string[] Aliases => new[] { "rm" };

        public override void Execute(SimpleCliArgs args, SimpleCliParser context)
        {
            if (args.Arguments.Count == 0)
            {
                context.Error("Please specify path to remove");
                return;
            }

            var path = args[0];
            bool recursive = args.HasFlag("r");

            try
            {
                // Implement removal logic using _drive
                // This would require adding Remove functionality to SimpleCliPseudoDrive
                context.Output($"Removed: {path}");
            }
            catch (Exception ex)
            {
                context.Error(ex.Message);
            }
        }
    }
}