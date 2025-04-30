# SimpleCLI Pseudo Directory Module Documentation

## Overview

The Pseudo Directory module provides a virtual file system interface for the SimpleCLI, allowing users to navigate and manage a simulated directory structure through CLI commands.

## Features

- **Directory Navigation**: Change and view current working directory
- **Directory Management**: Create and remove directories
- **Content Listing**: View contents of directories
- **Path Resolution**: Supports both relative and absolute paths

## Installation

To use the Pseudo Directory module, register it with your SimpleCLI parser:

```csharp
var parser = new SimpleCliParser();
parser.RegisterModule(new SimpleCliPseudoDirectoryModule());
```

## Commands

### `cd` - Change Directory
**Alias:** `chdir`  
Changes the current working directory.

| Parameter | Required | Description                         |
|-----------|----------|-------------------------------------|
| path      | No       | Path to change to (default: current)|

**Examples:**
```bash
> cd myfolder         # Change to subdirectory
> cd ..               # Move to parent directory
> cd /                # Move to root directory
> cd                  # Show current directory
Current directory: /projects
```

---

### `ls` - List Directory
**Aliases:** `dir`, `list`  
Lists contents of the current or specified directory.

| Parameter | Required | Description                        |
|-----------|----------|------------------------------------|
| path      | No       | Directory to list (default: current)|

**Examples:**
```bash
> ls               # List current directory
> ls /projects     # List specific directory
> ls ..            # List parent directory
```

---

### `mkdir` - Make Directory
**Alias:** `makedir`  
Creates new directory(ies).

| Parameter | Required | Description              |
|-----------|----------|--------------------------|
| path      | Yes      | Directory path to create |

**Examples:**
```bash
> mkdir newfolder          # Create single directory
> mkdir path/to/newfolder  # Create nested directories
```

---

### `rm` - Remove
**Alias:** `remove`  
Removes directories or files.

| Parameter | Required | Description              |
|-----------|----------|--------------------------|
| path      | Yes      | Path to remove           |

**Flags:**
- `-r` Recursively remove directories

**Examples:**
```bash
> rm file.txt        # Remove file
> rm -r oldfolder    # Recursively remove directory
```

## Path Resolution Rules

1. **Absolute Paths**: Start with `/`
    - `/folder/subfolder`

2. **Relative Paths**: Use current directory as base
    - `subfolder`
    - `../parentfolder`

3. **Special Paths**:
    - `.` - Current directory
    - `..` - Parent directory
    - `/` - Root directory

## Usage Example

```bash
> mkdir projects
Created directory: projects
> cd projects
Changed to /projects
> mkdir cli-app
Created directory: cli-app
> ls
cli-app
> cd cli-app
Changed to /projects/cli-app
> cd ..
Changed to /projects
> cd /
Changed to /
```

## Extending the Module

Developers can extend the module by:

1. **Adding new commands**:
    - Create a new `SimpleCliCommand` implementation
    - Register command in the module's `Initialize` method

2. **Extending the file system**:
    - Modify `SimpleCliPseudoDrive` to support new functionality
    - Add new file system item types by extending `PseudoFileSystemItem`

## Limitations

1. Currently supports only basic directory operations
2. No file content manipulation in this version
3. No permission system implemented

## Version History

- **1.0.0**: Initial release with basic directory operations

---

*For support or feature requests, please open an issue in the project repository.*