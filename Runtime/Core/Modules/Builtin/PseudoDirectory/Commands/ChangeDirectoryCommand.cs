using System.IO;
using SimpleCLI.Runtime.Core.Commands;
using SimpleCLI.Runtime.Core.Modules.Builtin.PseudoDirectory.Core;

namespace SimpleCLI.Runtime.Core.Modules.Builtin.PseudoDirectory.Commands
{
    public class ChangeDirectoryCommand : SimpleCliCommand
    {
        private readonly SimpleCliPseudoDrive _drive;

        public ChangeDirectoryCommand(SimpleCliPseudoDrive drive)
        {
            _drive = drive;
        }

        public override string Name => "chdir";
        public override string Description => "Change the current working directory";
        public override string[] Aliases => new[] { "cd", "changedir" };

        public override string Usage => "chdir Command" +
                                        "- chdir [path]\n" +
                                        "   Changes to the specified directory.\n" +
                                        "   Use '..' for parent directory or '/' for root.\n";

        public override void Execute(SimpleCliArgs args, SimpleCliParser context)
        {
            if (args.Arguments.Count == 0)
            {
                context.Output("Current directory: " + _drive.CurrentPath);
                return;
            }

            var path = args[0];
            try
            {
                var newPath = _drive.ChangeDirectory(path);
                context.Output($"Changed to {newPath}");
            }
            catch (DirectoryNotFoundException ex)
            {
                context.Error(ex.Message);
            }
        }
    }
}