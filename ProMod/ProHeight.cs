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
        public static event Action heightValueChange;

        public static bool PlayerHeightSettingsController_RefreshUI_Prefix(ref TextMeshProUGUI ____text, ref float ____value)
        {
            heightValueChange();
            ____text.text = string.Format("{0}cm", Mathf.Round(____value * 100));
            return false;
        }

        public static void Init()
        {
            heightValueChange += ProHeight_heightValueChange;
            Plugin.harmony.Patch(
                original: typeof(PlayerHeightSettingsController).GetMethod(nameof(PlayerHeightSettingsController.RefreshUI)),
                prefix: new HarmonyMethod(typeof(ProHeight).GetMethod(nameof(ProHeight.PlayerHeightSettingsController_RefreshUI_Prefix))));
        }

        private static void ProHeight_heightValueChange()
        {
            Plugin.Log.Info("Called ProHeight_heightValueChange");
        }
    }
}
