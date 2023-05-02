using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;
using System.Reflection;
using HarmonyLib;

namespace ProMod
{
    public static class ProHUDPatch
    {
        public static bool CoreGameHUDController_Initialize_Prefix(CoreGameHUDController __instance)
        {
            __instance.gameObject.SetActive(!Plugin.Config.ProStatsEnabled);
            return !Plugin.Config.ProStatsEnabled;
        }

        internal static void Init()
        {
            Plugin.harmony.Patch(
                original: typeof(CoreGameHUDController).GetMethod(nameof(CoreGameHUDController.Initialize)),
                prefix: new HarmonyMethod(typeof(ProHUDPatch).GetMethod(nameof(ProHUDPatch.CoreGameHUDController_Initialize_Prefix)))
            );
        }

    }
}
