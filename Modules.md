# Simple CLI Modules

Now you can add your own modules that runs within the CLI context!

## Design
The idea of modules is to modify the functions of Simple CLI without modifying them directly. Of course you can do that but sometimes it's easier to do so with a module.

Another idea of module is to make the features or your modifications non-destructive, meaning that you can plug or unplug
more functionalities of Simple CLI without having to delete or change the existing codebase that might make it hard to maintain later.

## Usage

**There's already a working module in place! You can go check out the Pseudo Directory Module in this repository, under `/Runtime/Core/Modules/Builtin/PseudoDirectory`.**

For advanced stuff, check out the [Module System Document](ModuleSystem.md).
