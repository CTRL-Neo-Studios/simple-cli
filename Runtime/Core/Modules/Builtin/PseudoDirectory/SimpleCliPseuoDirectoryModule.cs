using SimpleCLI.Runtime.Core.Modules.Builtin.PseudoDirectory.Commands;
using SimpleCLI.Runtime.Core.Modules.Builtin.PseudoDirectory.Core;

namespace SimpleCLI.Runtime.Core.Modules.Builtin.PseudoDirectory
{
    public class SimpleCliPseuoDirectoryModule
    {
        private SimpleCliParser _parser;
        private SimpleCliPseudoDrive _drive;

        public string Name => "PseudoDirectory";
        public string Description => "Provides pseudo file system commands (cd, ls, mkdir, etc.)";

        public void Initialize(SimpleCliParser parser)
        {
            _parser = parser;
            _drive = new SimpleCliPseudoDrive();
            
            // Register directory-related commands
            parser.RegisterCommand(new MakeDirectoryCommand(_drive));
            parser.RegisterCommand(new RemoveCommand(_drive));
            parser.RegisterCommand(new ChangeDirectoryCommand(_drive));
            parser.RegisterCommand(new ListDirectoryCommand(_drive));
        }

        public void Shutdown()
        {
            // Clean up if needed
        }
    }
}