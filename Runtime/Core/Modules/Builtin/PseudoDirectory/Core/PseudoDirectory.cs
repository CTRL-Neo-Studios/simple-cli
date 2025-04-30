using System.Collections.Generic;
using System.Linq;

namespace SimpleCLI.Runtime.Core.Modules.Builtin.PseudoDirectory.Core
{
    public class PseudoDirectory : PseudoFileSystemItem
    {
        private readonly List<PseudoFileSystemItem> _contents = new();

        public IEnumerable<PseudoFileSystemItem> Contents => _contents;

        public PseudoDirectory(PseudoDirectory parent, string name) 
            : base(parent, name)
        {
        }

        public void AddChild(PseudoFileSystemItem item)
        {
            if (_contents.Any(x => x.Name == item.Name))
                throw new System.IO.IOException($"Item already exists: {item.Name}");
                
            _contents.Add(item);
        }

        public PseudoFileSystemItem? GetChild(string name)
        {
            return _contents.Find(x => x.Name == name);
        }
    }
}