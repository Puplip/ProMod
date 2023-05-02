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


        public static void RefreshScore_Postfix(int score, int maxPossibleCutScore, ref TextMeshPro ____text, ref Color ____color, ref float ____colorAMultiplier, ref SpriteRenderer ____maxCutDistanceScoreIndicator)
        {

            if (!Plugin.Config.CutScoresEnabled) { return; }

            int maxCutScoreOffset = 115 - maxPossibleCutScore;

            foreach (Config.ProCutScoreConfig cutScoreConfig in Plugin.Config.CutScores)
            {
                if(score >= cutScoreConfig.score - maxCutScoreOffset)
                {
                    ____text.richText = true;
                    ____text.text = "<size=" + (maxPossibleCutScore == 115 ? cutScoreConfig.size : 100) + "%><color=#" + ColorUtility.ToHtmlStringRGB(cutScoreConfig.color) + ">" + score;
                    ____color = cutScoreConfig.color;
                    ____colorAMultiplier = 1.0f;
                    ____maxCutDistanceScoreIndicator.enabled = false;
                    return;
                }
            }
        }
        internal static void Init()
        {
            Plugin.harmony.Patch(
                original: typeof(FlyingScoreEffect).GetMethod(nameof(FlyingScoreEffect.RefreshScore)),
                postfix: new HarmonyMethod(typeof(ProCutScorePatch).GetMethod(nameof(ProCutScorePatch.RefreshScore_Postfix)))
            );
        }
    }
}
