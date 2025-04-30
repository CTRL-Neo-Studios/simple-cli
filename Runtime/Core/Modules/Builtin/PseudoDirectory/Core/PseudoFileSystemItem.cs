namespace SimpleCLI.Runtime.Core.Modules.Builtin.PseudoDirectory.Core
{
    public abstract class PseudoFileSystemItem
    {
        public string Name { get; }
        public PseudoDirectory Parent { get; }
        public string FullPath => Parent == null ? Name : $"{Parent.FullPath}/{Name}";

        protected PseudoFileSystemItem(PseudoDirectory parent, string name)
        {
            Parent = parent;
            Name = name;
            
            parent?.AddChild(this);
        }
    }
}