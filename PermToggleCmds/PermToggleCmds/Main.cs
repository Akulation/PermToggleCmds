using HarmonyLib;
using PermToggleCmds.Models;
using PermToggleCmds.Utils;
using Rocket.API;
using Rocket.API.Collections;
using Rocket.Core;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System.Linq;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace PermToggleCmds
{
    public class Main : RocketPlugin<Configuration>
    {
        public string PluginName = "PermToggleCommands";
        public string PluginVersion = "v1.0.0";
        public string PluginSupportURL = "discord.gg/SdnkVzJAWF";

        public static Main Instance { get; set; }
        public Harmony Harmony { get; set; }

        protected override void Load()
        {
            Instance = this;

            Harmony = new("Akulation.PermToggleCmds");
            Harmony.PatchAll();

            U.Events.OnPlayerConnected += OnPlayerConnected;

            DatabaseUtil.CreateDatabaseIfNotExists();

            Logger.Log($"{PluginName} {PluginVersion} loaded successfully!");
        }

        private void OnPlayerConnected(UnturnedPlayer player)
        {
            if (DatabaseUtil.GetPlayerActiveCommands(player.CSteamID.m_SteamID).Any())
            {
                var commands = DatabaseUtil.GetPlayerActiveCommands(player.CSteamID.m_SteamID);

                foreach (var command in commands )
                {
                    R.Commands.Execute(player, command.Command);

                    // toggle the command again as the plugin executing it will toggle it off for the player
                    DatabaseUtil.ToggleCommand(command);
                }
            }
        }

        public void OnExecute(IRocketCommand command, IRocketPlayer player, string[] args)
        {
            if (Configuration.Instance.Commands.Any(x => x.Name.ToLower() == command.Name.ToLower()))
            {
                string _args = string.Join(" ", args);
                ToggleCommand toggleCommand = new() { Command = command.Name.ToLower() + " " + _args, PlayerId = (player as UnturnedPlayer).CSteamID.m_SteamID };

                if (!Configuration.Instance.SaveArguments)
                {
                    toggleCommand.Command = command.Name.ToLower();
                }

                DatabaseUtil.ToggleCommand(toggleCommand);
            }
        }

        protected override void Unload()
        {
            Instance = null;

            Harmony.UnpatchAll();
            Harmony = null;

            U.Events.OnPlayerConnected -= OnPlayerConnected;

            Logger.Log($"{PluginName} {PluginVersion} has been unloaded successfully!");
        }

        public override TranslationList DefaultTranslations => new()
        {
            { "Error", "<color=red>An error has occured.</color>" }
        };
    }
}