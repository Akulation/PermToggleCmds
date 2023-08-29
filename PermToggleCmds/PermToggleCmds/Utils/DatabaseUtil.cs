using Newtonsoft.Json;
using PermToggleCmds.Helpers;
using PermToggleCmds.Models;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PermToggleCmds.Utils
{
    internal class DatabaseUtil
    {
        private static readonly string path = Main.Instance.Directory + "/data.json";

        public static void CreateDatabaseIfNotExists()
        {
            if (File.Exists(path))
            {
                return;
            }
            else
            {
                List<ToggleCommand> commandInfo = new();
                var json = JsonConvert.SerializeObject(commandInfo);

                File.WriteAllText(path, json);
            }
        }

        public static List<ToggleCommand> Get()
        {
            return JsonConvert.DeserializeObject<List<ToggleCommand>>(File.ReadAllText(path));
        }

        public static void Remove(ToggleCommand command)
        {
            var allCommands = Get();
            allCommands.Remove(allCommands.First(x => x.Command == command.Command && x.PlayerId == command.PlayerId));

            var json = JsonConvert.SerializeObject(allCommands);
            File.WriteAllText(path, json);
        }

        public static void Add(ToggleCommand command)
        {
            var allCommands = Get();
            allCommands.Add(new() { Command = command.Command, PlayerId = command.PlayerId });

            var json = JsonConvert.SerializeObject(allCommands);
            File.WriteAllText(path, json);
        }

        public static List<ToggleCommand> GetPlayerActiveCommands(ulong playerId)
        {
            var data = Get();

            var playerData = data.Where(x => x.PlayerId == playerId).ToList();

            return playerData;
        }

        public static void ToggleCommand(ToggleCommand toggleCommand)
        {
            var commands = GetPlayerActiveCommands(toggleCommand.PlayerId);

            if (commands.Any(x => x.Command == toggleCommand.Command && x.PlayerId == toggleCommand.PlayerId))
            {
                MessageUtil.SendDebug("Removing togglecommand from data file");
                Remove(toggleCommand);
            }
            else
            {
                MessageUtil.SendDebug("Adding togglecommand to data file");
                Add(toggleCommand);
            }
        }
    }
}
