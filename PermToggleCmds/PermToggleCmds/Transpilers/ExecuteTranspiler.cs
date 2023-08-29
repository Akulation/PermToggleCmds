using HarmonyLib;
using Rocket.API;
using Rocket.Core.Commands;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace PermToggleCmds.Transpilers
{
    // Feli.RocketMod.CommandLogger was used for reference as this was my first time doing a Harmony Transpiler

    [HarmonyPatch(typeof(RocketCommandManager)), HarmonyPatch(nameof(RocketCommandManager.Execute))]
    public class ExecuteTranspiler
    {
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Execute(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes = new(instructions);

            for (int i = 0; i < codes.Count; i++)
            {
                CodeInstruction instruction = codes[i];

                if (instruction.opcode == OpCodes.Callvirt && instruction.Calls(AccessTools.Method(typeof(IRocketCommand), nameof(IRocketCommand.Execute))))
                {
                    instruction.opcode = OpCodes.Call;
                    instruction.operand = AccessTools.Method(typeof(ExecuteTranspiler), nameof(OnCommandExecute));
                }
            }

            return codes;
        }

        public static void OnCommandExecute(IRocketCommand command, IRocketPlayer player, string[] args)
        {
            Main.Instance?.OnExecute(command, player, args);

            command.Execute(player, args);
        }
    }
}
