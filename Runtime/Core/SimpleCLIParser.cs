using System;
using System.Collections.Generic;
using System.Linq;
using SimpleCLI.Runtime.Core.Commands;
using SimpleCLI.Runtime.Core.Commands.Builtin;
using SimpleCLI.Runtime.Core.Exceptions;
using SimpleCLI.Runtime.Core.Modules.Interfaces;
using SimpleCLI.Runtime.Core.Utility;

namespace SimpleCLI.Runtime.Core
{
    /// <summary>
    /// Main CLI command processor with parsing and execution capabilities
    /// </summary>
    public class SimpleCliParser
    {
        private Dictionary<string, SimpleCliCommand> _commands = new();
        private List<string> _commandHistory = new();
        private int _historyIndex = -1;
        private Dictionary<string, string> _kvData = new();
        private List<ISimpleCliModule> _modules = new List<ISimpleCliModule>();

        public IReadOnlyDictionary<string, SimpleCliCommand> Commands => _commands;
        public IReadOnlyList<string> CommandHistory => _commandHistory.AsReadOnly();
        public IReadOnlyDictionary<string, string> KvData => _kvData;
        public IReadOnlyList<ISimpleCliModule> Modules => _modules.AsReadOnly();

        // Events
        public event Action<string, SimpleCliColor>? OnOutput; // the float stores the argb value.
        public event Action<string>? OnError;
    
        // Settings
        public string Prompt { get; set; } = "> ";
        public int MaxHistorySize { get; set; } = 50;
        public bool EchoCommands { get; set; } = true;
        public SimpleCliColor DefaultColor { get; set; } = new("#00FF00");
    
        public SimpleCliParser()
        {
            // Register built-in commands
            RegisterCommand(new HelpCommand(this));
            RegisterCommand(new ClearCommand());
            RegisterCommand(new HistoryCommand(this));
        }

        public void AddKeyValueData(string key, string value) => _kvData.Add(key, value);
        public void RemoveKeyValueData(string key) => _kvData.Remove(key);
        public bool HasKeyValueDataKey(string key) => _kvData.ContainsKey(key);
        public bool HasKeyValueDataValue(string value) => _kvData.ContainsValue(value);
        
        public void RegisterModule(ISimpleCliModule module)
        {
            if (_modules.Any(m => m.Name == module.Name))
            {
                throw new SimpleCliCommandRegisterException($"Module already registered: {module.Name}");
            }
    
            _modules.Add(module);
            module.Initialize(this);
        }

        public void UnregisterModule(string moduleName)
        {
            var module = _modules.FirstOrDefault(m => m.Name == moduleName);
            if (module != null)
            {
                module.Shutdown();
                _modules.Remove(module);
            }
        }
    
        /// <summary>
        /// Register a new command
        /// </summary>
        public void RegisterCommand(SimpleCliCommand command)
        {
            if (_commands.ContainsKey(command.Name.ToLower()))
            {
                throw new SimpleCliCommandRegisterException($"Command already registered: {command.Name}");
                return;
            }
        
            _commands[command.Name.ToLower()] = command;
        
            // Register aliases
            foreach (var alias in command.Aliases)
            {
                if (!_commands.ContainsKey(alias.ToLower()))
                {
                    _commands[alias.ToLower()] = command;
                }
            }
        }
    
        /// <summary>
        /// Execute a command string
        /// </summary>
        public void Execute(string commandLine)
        {
            if (string.IsNullOrWhiteSpace(commandLine))
                return;
            
            // Add to history
            _commandHistory.Add(commandLine);
            if (_commandHistory.Count > MaxHistorySize)
                _commandHistory.RemoveAt(0);
            _historyIndex = -1;
        
            if (EchoCommands)
                Output($"{Prompt}{commandLine}");
        
            try
            {
                ParseAndExecute(commandLine);
            }
            catch (SimpleCliException ex)
            {
                Error(ex.Message);
            }
            catch (Exception ex)
            {
                Error($"Unhandled error: {ex.Message}");
            }
        }
    
        private void ParseAndExecute(string commandLine)
        {
            // Tokenize the input
            var tokens = Tokenize(commandLine);
            if (tokens.Count == 0)
                return;
            
            string commandName = tokens[0].ToLower();
            tokens.RemoveAt(0);
        
            if (!_commands.TryGetValue(commandName, out SimpleCliCommand command))
                throw new SimpleCliNullCommandException($"Command not found: {commandName}");
        
            // Parse arguments
            var args = new SimpleCliArgs();
            var flagMode = false;
            string currentFlag = null;
        
            foreach (var token in tokens)
            {
                if (token.StartsWith("--"))
                {
                    // Long flag (--help)
                    flagMode = true;
                    currentFlag = token.Substring(2).ToLower();
                    args.Flags[currentFlag] = "true"; // Default to boolean flag
                }
                else if (token.StartsWith("-"))
                {
                    // Short flag (-h)
                    flagMode = true;
                    currentFlag = token.Substring(1).ToLower();
                    args.Flags[currentFlag] = "true";
                }
                else if (flagMode)
                {
                    // Flag value
                    args.Flags[currentFlag] = token;
                    flagMode = false;
                }
                else
                {
                    // Positional argument
                    args.Arguments.Add(token);
                }
            }
        
            // Execute the command
            command.Execute(args, this);
        }
    
        private List<string> Tokenize(string input)
        {
            var tokens = new List<string>();
            bool inQuotes = false;
            int start = 0;
        
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '"')
                {
                    inQuotes = !inQuotes;
                }
                else if (char.IsWhiteSpace(input[i]) && !inQuotes)
                {
                    if (i > start)
                    {
                        string token = input.Substring(start, i - start);
                        tokens.Add(token.Trim('"'));
                    }
                    start = i + 1;
                }
            }
        
            // Add last token
            if (start < input.Length)
            {
                string token = input.Substring(start);
                tokens.Add(token.Trim('"'));
            }
        
            return tokens;
        }
    
        // Navigation methods
        public string GetPreviousCommand()
        {
            if (_commandHistory.Count == 0)
                return string.Empty;
            
            _historyIndex = Math.Min(_historyIndex + 1, _commandHistory.Count - 1);
            return _commandHistory[_commandHistory.Count - 1 - _historyIndex];
        }
    
        public string GetNextCommand()
        {
            if (_commandHistory.Count == 0 || _historyIndex <= 0)
            {
                _historyIndex = -1;
                return string.Empty;
            }
        
            _historyIndex--;
            return _commandHistory[_commandHistory.Count - 1 - _historyIndex];
        }
    
        // Autocomplete
        public string[] GetSuggestions(string partialCommand)
        {
            if (string.IsNullOrWhiteSpace(partialCommand))
                return _commands.Keys.ToArray();
        
            var parts = partialCommand.Split(' ');
            if (parts.Length == 1)
            {
                // Command completion
                return _commands.Keys
                    .Where(c => c.StartsWith(partialCommand.ToLower()))
                    .ToArray();
            }
            else
            {
                // Argument completion (delegate to command)
                string commandName = parts[0].ToLower();
                if (_commands.TryGetValue(commandName, out SimpleCliCommand command))
                {
                    return command.GetAutoCompleteSuggestions(parts.Length - 2, parts.Last());
                }
            }
        
            return Array.Empty<string>();
        }
    
        // Output methods
        public void Output(string message) => OnOutput?.Invoke(message, DefaultColor);
        public void Output(string message, SimpleCliColor color) => OnOutput?.Invoke(message, color);
        public void Error(string message) => OnError?.Invoke($"Error: {message}");
    }
}