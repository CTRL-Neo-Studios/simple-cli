namespace SimpleCLI.Runtime.Core.Modules.Builtin.PseudoDirectory.Core
{
    public class PseudoFile : PseudoFileSystemItem
    {
        public string Content { get; set; }

        public PseudoFile(PseudoDirectory parent, string name, string content = "") 
            : base(parent, name)
        {
            Content = content;
        }
    }
}