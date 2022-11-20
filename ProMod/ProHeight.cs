using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using HarmonyLib;
using TMPro;

namespace ProMod
{
    static class ProHeight
    {
        public static event Action<float> heightValueChange;

        public static bool PlayerHeightSettingsController_RefreshUI_Prefix(PlayerHeightSettingsController __instance, ref TextMeshProUGUI ____text, ref float ____value)
        {
            heightValueChange(____value);
            ____text.text = string.Format("<size=85%>{0}cm", Mathf.Round(____value * 100));
            return false;
        }

        public static void Init()
        {
            heightValueChange += ProHeight_heightValueChange;
            Plugin.harmony.Patch(
                original: typeof(PlayerHeightSettingsController).GetMethod(nameof(PlayerHeightSettingsController.RefreshUI)),
                prefix: new HarmonyMethod(typeof(ProHeight).GetMethod(nameof(ProHeight.PlayerHeightSettingsController_RefreshUI_Prefix))));
        }

        private static void ProHeight_heightValueChange(float height)
        {
            Plugin.Log.Info("Called ProHeight ProHeight_heightValueChange");
        }
    }
}
