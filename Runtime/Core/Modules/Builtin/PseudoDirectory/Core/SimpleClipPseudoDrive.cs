using System;
using System.Collections.Generic;
using System.IO;

namespace SimpleCLI.Runtime.Core.Modules.Builtin.PseudoDirectory.Core
{
    public class SimpleCliPseudoDrive
    {
        private readonly PseudoDirectory _root;
        private PseudoDirectory _current;

        public string CurrentPath => _current.FullPath;

        public SimpleCliPseudoDrive()
        {
            _root = new PseudoDirectory(null, "");
            _current = _root;
        }

        public string ChangeDirectory(string path)
        {
            var target = ResolvePath(path);
            _current = target ?? throw new DirectoryNotFoundException($"Directory not found: {path}");
            return _current.FullPath;
        }

        public IEnumerable<PseudoFileSystemItem?> ListDirectory(string path = "")
        {
            var target = string.IsNullOrEmpty(path) ? _current : ResolvePath(path);
            if (target == null || target is not { } dir)
                throw new DirectoryNotFoundException($"Directory not found: {path}");
            
            return dir.Contents;
        }

        public bool CreateDirectory(string path)
        {
            var parts = path.Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
            var current = _current;

            foreach (var part in parts)
            {
                var existing = current.GetChild(part);
                if (existing == null)
                {
                    current = new PseudoDirectory(current, part);
                    continue;
                }
            
                if (existing is PseudoDirectory dir)
                {
                    current = dir;
                }
                else
                {
                    return false; // File exists with same name
                }
            }
        
            return true;
        }

        private PseudoDirectory ResolvePath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return _current;
            
            if (path == "/" || path == "\\")
                return _root;

            var parts = path.Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
            PseudoDirectory current = path.StartsWith("/") ? _root : _current;

            foreach (var part in parts)
            {
                if (part == ".")
                    continue;
                
                if (part == "..")
                {
                    current = current.Parent ?? current;
                    continue;
                }

                var child = current.GetChild(part);
                if (child is PseudoDirectory dir)
                {
                    current = dir;
                }
                else if (child != null)
                {
                    return null; // Not a directory
                }
                else
                {
                    return null; // Not found
                }
            }
        
            return current;
        }
    }
}