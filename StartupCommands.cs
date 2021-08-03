using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Oxide.Core;
using Oxide.Core.Plugins;
using Oxide.Core.Libraries.Covalence;

namespace Oxide.Plugins
{
    [Info("StartupCommands", "Agamemnon", "1.0.0")]
    [Description("Runs a configurable lists of commands after server wipes and server restarts.")]
    class StartupCommands : RustPlugin
    {
        private ConfigData configData;
        private bool wiped = false;

        # region Oxide Hooks
        private void OnServerInitialized()
        {
            if (!LoadConfigVariables())
            {
                PrintError("ERROR: The config file is corrupt. Either fix or delete it and restart the plugin.");
                PrintError("ERROR: Unloading plugin.");
                Interface.Oxide.UnloadPlugin(this.Title);
                return;
            }

            if(wiped)
            {
                PrintWarning("Initializing post-wipe commands.");
                int wipeCount = 0;

                foreach (ServerCommand wipeCommand in configData.wipeCommands)
                {
                    if (wipeCommand.Enabled)
                    {
                        string parameters = "";
                        foreach (string parameter in wipeCommand.Parameters)
                        {
                            parameters = parameters + " \"" + parameter + "\"";
                        }

                        rust.RunServerCommand(wipeCommand.Command + parameters);
                        wipeCount++;
                    }
                }

                if (wipeCount == 0)
                    PrintWarning("All post-wipe commands are disabled.");
            }

            PrintWarning("Initializing startup commands.");
            int startupCount = 0;

            foreach (ServerCommand startupCommand in configData.startupCommands)
            {
                if (startupCommand.Enabled)
                {
                    string parameters = "";
                    foreach (string parameter in startupCommand.Parameters)
                    {
                        parameters = parameters + " \"" + parameter + "\"";
                    }

                    rust.RunServerCommand(startupCommand.Command + parameters);
                    startupCount++;
                }
            }

            if (startupCount == 0)
                PrintWarning("All startup commands are disabled.");
        }

        private void OnNewSave()
        {
            wiped = true;
        }
        #endregion

        #region Configuration
        private class ConfigData
        {
            [JsonProperty(PropertyName = "Startup Commands", ObjectCreationHandling = ObjectCreationHandling.Replace)]
            public List<ServerCommand> startupCommands = new List<ServerCommand>()
            {
                new ServerCommand("env.time", new List<string> { "9", }, false),
                new ServerCommand("env.progresstime", new List<string> { "false", }, false)
            };

            [JsonProperty(PropertyName = "Wipe Commands", ObjectCreationHandling = ObjectCreationHandling.Replace)]
            public List<ServerCommand> wipeCommands = new List<ServerCommand>()
            {
                new ServerCommand("pasteback", new List<string> { "arena", "stability", "false"}, false),
                new ServerCommand("pasteback", new List<string> { "adminbase", "undestr", "true"}, false)
            };
        }

        private bool LoadConfigVariables()
        {
            try
            {
                configData = Config.ReadObject<ConfigData>();
            }
            catch
            {
                return false;
            }

            SaveConfig(configData);
            return true;
        }

        protected override void LoadDefaultConfig()
        {
            PrintWarning("Creating new config file.");
            configData = new ConfigData();
            SaveConfig(configData);
        }

        private void SaveConfig(ConfigData config)
        {
            Config.WriteObject(config, true);
        }
        #endregion

        #region Helper Classes
        class ServerCommand
        {
            public string Command;
            public List<string> Parameters;
            public bool Enabled;

            public ServerCommand(string command, List<string> parameters, bool enabled)
            {
                Command = command;
                Parameters = parameters;
                Enabled = enabled;
            }
        }
        #endregion
    }
}
