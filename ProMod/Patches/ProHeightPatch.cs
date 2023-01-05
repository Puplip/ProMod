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
    public static class ProHeightPatch
    {
        public static event Action<float> heightValueChange = new Action<float>(_heightValueChange);

        public static bool PlayerHeightSettingsController_RefreshUI_Prefix(PlayerHeightSettingsController __instance, ref TextMeshProUGUI ____text, ref float ____value)
        {
            heightValueChange(____value);
            ____text.text = string.Format("<size=85%>{0}cm", Mathf.Round(____value * 100));
            return false;
        }

        internal static void Init()
        {
            Plugin.harmony.Patch(
                original: typeof(PlayerHeightSettingsController).GetMethod(nameof(PlayerHeightSettingsController.RefreshUI)),
                prefix: new HarmonyMethod(typeof(ProHeightPatch).GetMethod(nameof(ProHeightPatch.PlayerHeightSettingsController_RefreshUI_Prefix)))
            );
        }

        private static void _heightValueChange(float penis)
        {

        }

    }
}
