using System.Collections.Generic;
using System.IO;
using SimpleCLI.Runtime.Core.Commands;
using SimpleCLI.Runtime.Core.Modules.Builtin.PseudoDirectory.Core;

namespace SimpleCLI.Runtime.Core.Modules.Builtin.PseudoDirectory.Commands
{
    public class ListDirectoryCommand : SimpleCliCommand
    {
        private readonly SimpleCliPseudoDrive _drive;

        public ListDirectoryCommand(SimpleCliPseudoDrive drive) : base()
        {
            _drive = drive;
        }

        public override string Name => "list";
        public override string[] Aliases => new[] { "dir", "ls" };
        public override string Description => "List directory contents";
        
        public override string Usage => "list Command\n" +
                                        "- list [path]\n" +
                                        "   Lists contents of the specified or current directory";

        public override void Execute(SimpleCliArgs args, SimpleCliParser context)
        {
            var path = args.Arguments.Count > 0 ? args[0] : null;
            try
            {
                var contents = _drive.ListDirectory(path);
                foreach (var item in contents)
                {
                    context.Output(item.Name);
                }
            }
            catch (DirectoryNotFoundException ex)
            {
                context.Error(ex.Message);
            }
        }
    }
}