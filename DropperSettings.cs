using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using VoxelTycoon;
using Logger = VoxelTycoon.Logger;

// This doesn't work yet

namespace DefaultNamespace
{
    public class DropperSettings
    {
        [HotkeySetting("base/dropper_hotkey", null, null, null, false)]
        public Setting<Hotkey> DropperKey { get; } = (Setting<Hotkey>) new Hotkey(KeyCode.F2);
    }
    
    
    [HarmonyPatch(typeof(Settings))]
    [HarmonyPatch("GetProperties")]
    public class DropperSettingsPatcher
    {

        // Patch the game's method that loads all the settings to show in the configuration panel.
        private static void Postfix(IEnumerable<PropertyInfo> __result)
        {
            Logger logger = new Logger<DropperSettingsPatcher>();
            logger.Log("In postfix for GetProperties");
            __result = __result.Concat(typeof(DropperSettings).GetProperties());
        }
    }
}