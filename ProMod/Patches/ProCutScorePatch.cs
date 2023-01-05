using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;
using System.Reflection;
using HarmonyLib;
using TMPro;

namespace ProMod
{

    /// <summary>
    /// Remove Note Cut Effects
    /// </summary>
    public static class ProCutScorePatch
    {


        public static bool RefreshScore_Prefix(int score, int maxPossibleCutScore, ref TextMeshPro ____text, ref Color ____color, ref float ____colorAMultiplier)
        {

            if (!Plugin.Config.CutScoresEnabled) { return true; }

            foreach (Config.ProCutScoreConfig cutScoreConfig in Plugin.Config.CutScores)
            {
                if(score >= cutScoreConfig.score)
                {
                    ____text.richText = true;
                    ____text.text = "<size=" + cutScoreConfig.size + "%><color=#" + ColorUtility.ToHtmlStringRGB(cutScoreConfig.color) + ">" + score;
                    ____color = cutScoreConfig.color;
                    ____colorAMultiplier = 1.0f;
                    return false;
                }
            }

            ____text.text = "";
            return false;
        }
        internal static void Init()
        {
            Plugin.harmony.Patch(
                original: typeof(FlyingScoreEffect).GetMethod(nameof(FlyingScoreEffect.RefreshScore)),
                prefix: new HarmonyMethod(typeof(ProCutScorePatch).GetMethod(nameof(ProCutScorePatch.RefreshScore_Prefix)))
            );
        }
    }
}
