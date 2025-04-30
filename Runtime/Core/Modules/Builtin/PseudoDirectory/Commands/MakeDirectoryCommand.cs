using System;
using SimpleCLI.Runtime.Core.Commands;
using SimpleCLI.Runtime.Core.Modules.Builtin.PseudoDirectory.Core;

namespace SimpleCLI.Runtime.Core.Modules.Builtin.PseudoDirectory.Commands
{
    public class MakeDirectoryCommand : SimpleCliCommand
    {
        public MakeDirectoryCommand(SimpleCliPseudoDrive drive) : base()
        {
            _drive = drive;
        }
        
        private readonly SimpleCliPseudoDrive _drive;
        public override string Name => "makedir";
        public override string Description => "Makes directory(s).";
        public override string Usage => "makedir Command\n" +
                                        "- makedir [directory path]\n" +
                                        "   Makes the directory at the given path recursively.";
        public override string[] Aliases => new[] { "mkdir" };

        public override void Execute(SimpleCliArgs args, SimpleCliParser context)
        {
            if (args.Arguments.Count == 0)
            {
                context.Error("Please specify directory path");
                return;
            }

            try
            {
                var path = args[0];
                if (_drive.CreateDirectory(path))
                {
                    context.Output($"Created directory: {path}");
                }
                else
                {
                    context.Error($"Failed to create directory: {path}");
                }
            }
            catch (Exception ex)
            {
                context.Error(ex.Message);
            }
        }
    }
}