using System;
using UnityEngine;
using HarmonyLib;
using System.Reflection;

namespace ProMod
{
    public static class ProHUDPatch
    {

        public static float hudWidth = 0;
        public static float hudDistance = 0;
        public static Vector3 energyPanelPos = Vector3.zero;

        public static event Action HUDPositionReady;

        public static void CoreGameHUDController_Initialize_Postfix(CoreGameHUDController __instance)
        {
            //ProUtil.SetActiveRecursive(__instance.gameObject, !Plugin.Config.proHUDConfig.proHUDEnabled);

            if (!Plugin.Config.proHUDConfig.proHUDEnabled) { return; }


            __instance.gameObject.SetActive(false);

            energyPanelPos = __instance.energyPanelGo.transform.position;

            hudWidth = Mathf.Abs(__instance.immediateRankGo.transform.position.x);
            if(hudWidth < 2f)
            {
                hudWidth = 2f;
            }


            hudDistance = __instance.immediateRankGo.transform.position.z;

            HUDPositionReady?.Invoke();
        }

        internal static void Init()
        {
            Plugin.harmony.Patch(
                original: typeof(CoreGameHUDController).GetMethod("Initialize",BindingFlags.Instance | BindingFlags.NonPublic),
                postfix: new HarmonyMethod(typeof(ProHUDPatch).GetMethod(nameof(ProHUDPatch.CoreGameHUDController_Initialize_Postfix)))
            );
        }

    }
}
