# StartupCommands
**StartupCommands** is an [Oxide](https://umod.org/) plugin that allows Rust server admins to run configurable lists of commands after server wipes and server restarts.
- Each command can be individually enabled or disabled in the configuration.
- The plugin correctly supports multiple-parameter commands with spaces in the parameters.
### Table of Contents  
- [Installation](#installation)  
- [Permissions](#permissions)  
- [Commands](#commands)  
- [Configuration](#configuration)  
- [Credits](#credits)
## Installation
Download the plugin:
```bash
git clone https://github.com/rustireland/startupcommands.git
```
Copy it to your Oxide plugins directory:
```bash
cp startupcommands/StartupCommands.cs oxide/plugins
```
Oxide will compile and load the plugin automatically.
## Permissions
This plugin doesn't make use of the Oxide permissions system.
## Commands
This plugin doesn't provide any console or chat commands.
## Configuration
The settings and options can be configured in the `StartupCommands.json` file under the `oxide/config` directory. The use of an editor and validator is recommended to avoid formatting issues and syntax errors.

When run for the first time, the plugin will create a default configuration file with all commands *disabled*.
```json
{
  "Startup Commands": [
    {
      "Command": "env.time",
      "Parameters": [
        "9"
      ],
      "Enabled": false
    },
    {
      "Command": "env.progresstime",
      "Parameters": [
        "false"
      ],
      "Enabled": false
    }
  ],
  "Wipe Commands": [
    {
      "Command": "pasteback",
      "Parameters": [
        "arena",
        "stability",
        "false"
      ],
      "Enabled": false
    },
    {
      "Command": "pasteback",
      "Parameters": [
        "adminbase",
        "undestr",
        "true"
      ],
      "Enabled": false
    }
  ]
}
```
# Credits
- [Agamemnon](https://github.com/agamemnon23) - Code, testing.
- [Black_demon6](https://github.com/TheBlackdemon6) - Initial concept, feature requests, and testing.
