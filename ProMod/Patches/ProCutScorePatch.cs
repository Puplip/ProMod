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

            if (!Plugin.Config.cutScores.cutScoresEnabled) { return; }

            int rescaledScore = score;

            if(maxPossibleCutScore != 115)
            {
                rescaledScore = Mathf.RoundToInt((float)score / (float)maxPossibleCutScore * 115f);
            }

            foreach (ProCutScorePointConfig cutScorePoint in Plugin.Config.cutScores.cutScorePoints)
            {
                if(rescaledScore >= cutScorePoint.score)
                {
                    ____text.richText = true;

                    ____text.text = $"<size={(maxPossibleCutScore == 115 ? cutScorePoint.size : Plugin.Config.cutScores.abnormalNoteSize)}%>{score}";

                    ____color = cutScorePoint.color;
                    ____colorAMultiplier = 1.0f;
                    ____maxCutDistanceScoreIndicator.enabled = false;

                    return;
                    //switch (cutScorePoint.displayStyle)
                    //{
                    //    case ProCutScorePointConfig.DisplayStyle.CutScore:
                    //        ____text.text = $"<size={(maxPossibleCutScore == 115 ? cutScorePoint.size : Plugin.Config.cutScores.abnormalNoteSize)}%>{score}";
                    //        return;
                    //    //case ProCutScorePointConfig.DisplayStyle.CustomString:
                    //    //    ____text.text = $"<size={(maxPossibleCutScore == 115 ? cutScorePoint.size : Plugin.Config.cutScores.abnormalNoteSize)}%>{cutScorePoint.customString}";
                    //    //    return;
                    //    //case ProCutScorePointConfig.DisplayStyle.LetterRank:

                    //    //    float acc = (float)score / (float)maxPossibleCutScore;
                    //    //    if(acc > 0.9f)
                    //    //    {
                    //    //        ____text.text = $"<size={(maxPossibleCutScore == 115 ? cutScorePoint.size : Plugin.Config.cutScores.abnormalNoteSize)}%>SS";
                    //    //    } else if (acc > 0.8f)
                    //    //    {
                    //    //        ____text.text = $"<size={(maxPossibleCutScore == 115 ? cutScorePoint.size : Plugin.Config.cutScores.abnormalNoteSize)}%>S";
                    //    //    } else if (acc > 0.65f)
                    //    //    {
                    //    //        ____text.text = $"<size={(maxPossibleCutScore == 115 ? cutScorePoint.size : Plugin.Config.cutScores.abnormalNoteSize)}%>A";
                    //    //    } else if (acc > 0.5f)
                    //    //    {
                    //    //        ____text.text = $"<size={(maxPossibleCutScore == 115 ? cutScorePoint.size : Plugin.Config.cutScores.abnormalNoteSize)}%>B";
                    //    //    } else if (acc > 0.35f)
                    //    //    {
                    //    //        ____text.text = $"<size={(maxPossibleCutScore == 115 ? cutScorePoint.size : Plugin.Config.cutScores.abnormalNoteSize)}%>C";
                    //    //    } else if (acc > 0.2f)
                    //    //    {
                    //    //        ____text.text = $"<size={(maxPossibleCutScore == 115 ? cutScorePoint.size : Plugin.Config.cutScores.abnormalNoteSize)}%>D";
                    //    //    } else
                    //    //    {
                    //    //        ____text.text = $"<size={(maxPossibleCutScore == 115 ? cutScorePoint.size : Plugin.Config.cutScores.abnormalNoteSize)}%>E";
                    //    //    }
                            
                    //    //    return;
                    //}
                }
            }
        }
        internal static void Init()
        {
            Plugin.harmony.Patch(
                original: typeof(FlyingScoreEffect).GetMethod("RefreshScore",BindingFlags.NonPublic | BindingFlags.Instance),
                postfix: new HarmonyMethod(typeof(ProCutScorePatch).GetMethod(nameof(ProCutScorePatch.RefreshScore_Postfix)))
            );
        }
    }
}
