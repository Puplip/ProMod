using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;
using System.Reflection;
using HarmonyLib;
using static BeatmapObjectSpawnMovementData;


namespace ProMod;

public static class ProJumpPatch
{
    private static List<float> fractions = Enumerable.Range(1, 8).Select(e => (float)e)
            .Concat(Enumerable.Range(1, 8).Select(i => (float)i - 0.5f)).ToList();

    private static List<float> worseFractions = Enumerable.Range(1, 8).Select(i => (float)i - 0.75f)
        .Concat(Enumerable.Range(1, 8).Select(i => (float)i - 0.25f))
        .Concat(Enumerable.Range(1, 8).Select(i => (float)i - 2f / 3f))
        .Concat(Enumerable.Range(1, 8).Select(i => (float)i - 1f / 3f))
        .Concat(Enumerable.Range(1, 8).Select(i => (float)i - 5f / 6f))
        .Concat(Enumerable.Range(1, 8).Select(i => (float)i - 1f / 6f))
        .Concat(Enumerable.Range(1, 8).Select(i => (float)i - 1f / 8f))
        .Concat(Enumerable.Range(1, 8).Select(i => (float)i - 3f / 8f))
        .Concat(Enumerable.Range(1, 8).Select(i => (float)i - 5f / 8f))
        .Concat(Enumerable.Range(1, 8).Select(i => (float)i - 7f / 8f))
        .ToList();

    public static bool VariableMovementDataProvider_Init_Prefix(float startHalfJumpDurationInBeats, float maxHalfJumpDistance, float noteJumpMovementSpeed, float minRelativeNoteJumpSpeed, float bpm, ref BeatmapObjectSpawnMovementData.NoteJumpValueType noteJumpValueType, ref float noteJumpValue, Vector3 centerPosition, Vector3 forwardVector){
    //public static bool BeatmapObjectSpawnMovementData_Init_Prefix(float startNoteJumpMovementSpeed, float startBpm, ref BeatmapObjectSpawnMovementData.NoteJumpValueType noteJumpValueType, ref float noteJumpValue)
    //{

        float beatDuration = 60f / bpm;

        Plugin.Log.Info($"VariableMovementDataProvider_Init NJS: {noteJumpMovementSpeed}");
        Plugin.Log.Info($"VariableMovementDataProvider_Init BPM: {bpm:f2} ({beatDuration*1000f:f0}ms per beat)");
        Plugin.Log.Info($"VariableMovementDataProvider_Init Jump Setting: {Plugin.Config.jumpSetting.NameWithSpaces():f2}");

        switch (Plugin.Config.jumpSetting)
        {
            case ProJumpSetting.BaseGame:
                return true;
            case ProJumpSetting.ReactionTime:
                noteJumpValueType = BeatmapObjectSpawnMovementData.NoteJumpValueType.JumpDuration;
                noteJumpValue = Plugin.Config.reactionTime / 1000.0f;
                return true;
            case ProJumpSetting.JumpDistance:
                noteJumpValueType = BeatmapObjectSpawnMovementData.NoteJumpValueType.JumpDuration;
                noteJumpValue = ProUtil.CalculateReactionTimeSeconds(Plugin.Config.jumpDistance, noteJumpMovementSpeed);
                return true;
            case ProJumpSetting.HouseSpecial:
                noteJumpValueType = BeatmapObjectSpawnMovementData.NoteJumpValueType.JumpDuration;
                noteJumpValue = HouseSpecial(bpm, noteJumpMovementSpeed);
                return true;
            default:
                return true;
        }
    }

    public static float HouseSpecial(float bpm, float njs)
    {
        float beatDuration = 60f / bpm;
        float minReactionTime = Plugin.Config.reactionTime / 1000f;
        float targetReactionTime = (ProUtil.CalculateReactionTimeSeconds(Plugin.Config.jumpDistance, njs) + (Plugin.Config.reactionTime / 1000f)) / 2f;

        float beatFraction = float.NaN;
        float error = 0.010f;

        while ((!float.IsFinite(beatFraction) || beatFraction <= 0) && error <= 0.045f)
        {
            beatFraction = fractions.FirstOrDefault(e => Mathf.Abs(e * beatDuration - targetReactionTime) <= error);
            error += 0.010f;
        }
        error = 0.010f;
        while ((!float.IsFinite(beatFraction) || beatFraction <= 0) && error <= 0.045f)
        {
            beatFraction = worseFractions.FirstOrDefault(e => Mathf.Abs(e * beatDuration - targetReactionTime) <= error);
            error += 0.010f;
        }

        if (!float.IsFinite(beatFraction) || beatFraction <= 0)
        {
            return targetReactionTime;
        }

        return beatFraction * beatDuration;
    }

    public static void VariableMovementDataProvider_Init_Postfix(VariableMovementDataProvider __instance, float ____jumpDuration,float ____jumpDistance)
    {
        Plugin.Log.Info("VariableMovementDataProvider_Init Reaction Time: " + (____jumpDuration * 500.0f) + "ms");
        Plugin.Log.Info("VariableMovementDataProvider_Init JumpDistance: " + ____jumpDistance + "m");
    }

    public static void Init()
    {
        Plugin.harmony.Patch(
            original: typeof(VariableMovementDataProvider).GetMethod(nameof(VariableMovementDataProvider.Init)),
            prefix: new HarmonyMethod(typeof(ProJumpPatch).GetMethod(nameof(ProJumpPatch.VariableMovementDataProvider_Init_Prefix)))
        );

        Plugin.harmony.Patch(
            original: typeof(VariableMovementDataProvider).GetMethod(nameof(VariableMovementDataProvider.Init)),
            postfix: new HarmonyMethod(typeof(ProJumpPatch).GetMethod(nameof(ProJumpPatch.VariableMovementDataProvider_Init_Postfix)))
        );
    }
}