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
    public static class ProRTPatch
    {

        public static bool BeatmapObjectSpawnMovementData_Init_Prefix(float startNoteJumpMovementSpeed, ref BeatmapObjectSpawnMovementData.NoteJumpValueType noteJumpValueType, ref float noteJumpValue)
        {
            if (Plugin.Config.FixedRTEnabled)
            {
                noteJumpValueType = BeatmapObjectSpawnMovementData.NoteJumpValueType.JumpDuration;
                noteJumpValue = Plugin.Config.FixedRTValue / 1000.0f;
                Plugin.Log.Info("Fixed RT: " + noteJumpValue + "ms");
            } else if (Plugin.Config.RTCurveEnabled)
            {
                Plugin.Log.Info("Map NJS: "+ startNoteJumpMovementSpeed);
                Config.ProReactionTimePoint prevRTPoint = null;
                Config.ProReactionTimePoint nextRTPoint = null;
                for(int i = 0; i < Plugin.Config.RTCurve.Count; i++)
                {
                    Config.ProReactionTimePoint currentRTPoint = Plugin.Config.RTCurve[i];
                    if (startNoteJumpMovementSpeed > currentRTPoint.njs)
                    {
                        prevRTPoint = currentRTPoint;
                    } else
                    {
                        nextRTPoint = currentRTPoint;
                        break;
                    }
                }
                if(prevRTPoint == null)
                {
                    return true;
                }
                if(nextRTPoint != null)
                {
                    noteJumpValueType = BeatmapObjectSpawnMovementData.NoteJumpValueType.JumpDuration;
                    float rtDiff = nextRTPoint.njs - prevRTPoint.njs;
                    noteJumpValue = (startNoteJumpMovementSpeed - prevRTPoint.njs) / (rtDiff) * (nextRTPoint.rt - prevRTPoint.rt) + prevRTPoint.rt;
                    Plugin.Log.Info("Selected RT: " + noteJumpValue + "ms");
                    noteJumpValue /= 1000.0f;
                } else
                {
                    noteJumpValueType = BeatmapObjectSpawnMovementData.NoteJumpValueType.JumpDuration;
                    noteJumpValue = prevRTPoint.rt / 1000.0f;
                    Plugin.Log.Info("Selected RT: " + prevRTPoint.rt + "ms");
                }
            }
            return true;
        }

        public static void BeatmapObjectSpawnMovementData_Init_Postfix(float ____jumpDuration)
        {
            Plugin.Log.Info("Final Reaction Time: " + (____jumpDuration * 500.0f) + "ms");
        }


        public static void Init()
        {
            Plugin.harmony.Patch(
                original: typeof(BeatmapObjectSpawnMovementData).GetMethod(nameof(BeatmapObjectSpawnMovementData.Init)),
                prefix: new HarmonyMethod(typeof(ProRTPatch).GetMethod(nameof(ProRTPatch.BeatmapObjectSpawnMovementData_Init_Prefix)))
            );

            Plugin.harmony.Patch(
                original: typeof(BeatmapObjectSpawnMovementData).GetMethod(nameof(BeatmapObjectSpawnMovementData.Init)),
                postfix: new HarmonyMethod(typeof(ProRTPatch).GetMethod(nameof(ProRTPatch.BeatmapObjectSpawnMovementData_Init_Postfix)))
            );
        }
    }
}