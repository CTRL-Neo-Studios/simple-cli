# SimpleCLI Module System Documentation

## Overview

The SimpleCLI module system allows developers to extend the CLI's functionality through pluggable components. Modules can register commands, manage state, and integrate with other system components.

## Table of Contents
1. [Module Basics](#module-basics)
2. [Creating a Module](#creating-a-module)
3. [Module Lifecycle](#module-lifecycle)
4. [Best Practices](#best-practices)
5. [Example Modules](#example-modules)
6. [Advanced Topics](#advanced-topics)

---

## Module Basics

### Key Concepts
- **Modular Architecture**: Each feature set is self-contained
- **Loose Coupling**: Modules interact through the parser interface
- **Lifecycle Management**: Controlled initialization and shutdown

### Benefits
- Extend CLI without modifying core
- Isolate feature development
- Enable/disable features at runtime

---

## Creating a Module

### 1. Implement `ISimpleCliModule`

```csharp
public class MyModule : ISimpleCliModule
{
    public string Name => "MyModule";
    public string Description => "Adds awesome functionality";
    
    public void Initialize(SimpleCliParser parser)
    {
        // Register commands and setup
        parser.RegisterCommand(new MyCommand());
    }
    
    public void Shutdown()
    {
        // Clean up resources
    }
}
```

### 2. Register Commands
Add commands in the `Initialize` method:

```csharp
public void Initialize(SimpleCliParser parser)
{
    parser.RegisterCommand(new FeatureOneCommand());
    parser.RegisterCommand(new FeatureTwoCommand());
    
    // Optional: Store parser reference if needed
    _parser = parser;
}
```

### 3. Handle Dependencies
For module dependencies:

```csharp
public class DependentModule : ISimpleCliModule
{
    private readonly IMyDependency _dependency;
    
    public DependentModule(IMyDependency dependency)
    {
        _dependency = dependency;
    }
    
    // ... rest of implementation
}
```

---

## Module Lifecycle

### Initialization Sequence
1. Module instantiated
2. `Initialize()` called with parser reference
3. Commands registered
4. Ready for use

### Shutdown Sequence
1. `Shutdown()` called
2. Module should:
    - Unregister any event handlers
    - Dispose resources
    - Prepare for unloading

### Runtime Management
```csharp
// Register module
parser.RegisterModule(new MyModule());

// Unregister module
parser.UnregisterModule("MyModule");
```

---

## Best Practices

### Do:
✅ Keep modules focused on single responsibility  
✅ Handle your own resource cleanup  
✅ Use clear, descriptive command names  
✅ Document module requirements

### Don't:
❌ Modify parser internals directly  
❌ Make assumptions about other modules  
❌ Use global state unnecessarily

### Performance Considerations
- Initialize heavy resources on demand
- Cache frequent operations
- Avoid blocking in command execution

---

## Example Modules

### Basic Module Template
```csharp
public class TemplateModule : ISimpleCliModule
{
    public string Name => "Template";
    public string Description => "Example module template";
    
    private SimpleCliParser _parser;
    
    public void Initialize(SimpleCliParser parser)
    {
        _parser = parser;
        parser.RegisterCommand(new TemplateCommand());
    }
    
    public void Shutdown()
    {
        // Cleanup if needed
    }
    
    private class TemplateCommand : SimpleCliCommand
    {
        public override string Name => "template";
        public override void Execute(SimpleCliArgs args, SimpleCliParser context)
        {
            context.Output("Template command executed!");
        }
    }
}
```

### Module with Configuration
```csharp
public class ConfigurableModule : ISimpleCliModule
{
    public ModuleConfig Config { get; }
    
    public ConfigurableModule(ModuleConfig config)
    {
        Config = config;
    }
    
    // ... implementation
}
```

---

## Advanced Topics

### Cross-Module Communication
1. **Via Parser KV Store**:
```csharp
// Module A
parser.AddKeyValueData("sharedData", value);

// Module B
var data = parser.KvData["sharedData"];
```

2. **Custom Interfaces**:
```csharp
public interface IDataProvider
{
    object GetData();
}

public class ConsumerModule : ISimpleCliModule
{
    private readonly IDataProvider _provider;
    
    public ConsumerModule(IDataProvider provider)
    {
        _provider = provider;
    }
}
```

### Dynamic Module Loading
```csharp
// Load from assembly
var assembly = Assembly.LoadFrom("module.dll");
var moduleType = assembly.GetTypes()
                       .First(t => typeof(ISimpleCliModule).IsAssignableFrom(t));
var module = (ISimpleCliModule)Activator.CreateInstance(moduleType);
parser.RegisterModule(module);
```

### Error Handling
- Module should handle its own command errors
- Use `SimpleCliException` for user-facing errors
- Log internal errors appropriately

---

## Versioning Considerations
1. Module manifest should declare:
    - SimpleCLI version compatibility
    - Dependency versions
    - Module version

2. Recommended version format:  
   `Major.Feature.Patch`
    - Major: Breaking changes
    - Feature: New functionality
    - Patch: Bug fixes

---

## FAQ

**Q: Can modules be unloaded and reloaded?**  
A: Yes, via `UnregisterModule()` and re-registration, but may require app restart for complete cleanup.

**Q: How to handle module conflicts?**  
A: Use unique command names and consider namespace prefixes for common operations.

**Q: Are async commands supported?**  
A: The base system is synchronous, but modules can implement async patterns internally.

---

*For more examples and advanced techniques, see the SimpleCLI Module Development Guide.*