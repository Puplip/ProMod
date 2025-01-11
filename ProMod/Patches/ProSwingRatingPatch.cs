using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Zenject;
using System.Reflection;
using HarmonyLib;
using System.Reflection.Emit;
using IPA.Utilities;
using System.CodeDom;
using System.IO;

namespace ProMod.Patches
{
    public class ProSwingRating
    {
        public float beforeCutSwingRating;
        public float afterCutSwingRating;
    }

    public static class ProSwingRatingPatch
    {
        public static Dictionary<SaberSwingRatingCounter, ProSwingRating> swingRatingCache = new Dictionary<SaberSwingRatingCounter, ProSwingRating>();
        private static Dictionary<SaberMovementData, float> beforeCutRatingCache = new Dictionary<SaberMovementData, float>();
        public static void SaberSwingRatingCounter_Init_Hook(SaberSwingRatingCounter saberSwingRatingCounter, ISaberMovementData saberMovementData)
        {

            //Plugin.Log.Info("SaberSwingRatingCounter_Init_Hook");

            if (!swingRatingCache.ContainsKey(saberSwingRatingCounter))
            {
                swingRatingCache[saberSwingRatingCounter] = new ProSwingRating();
            }

            if (beforeCutRatingCache.ContainsKey((SaberMovementData)saberMovementData))
            {
                swingRatingCache[saberSwingRatingCounter].beforeCutSwingRating = beforeCutRatingCache[(SaberMovementData)saberMovementData];
            }
            //else
            //{
            //    Plugin.Log.Error("SaberMovementData not found in beforeCutRatingCache");
            //}

        }

        public static void SaberSwingRatingCounter_ProcessNewData_BeforeCut_Hook(SaberSwingRatingCounter saberSwingRatingCounter)
        {

            //Plugin.Log.Info("SaberSwingRatingCounter_ProcessNewData_BeforeCut_Hook");

            ISaberMovementData saberMovementData = saberSwingRatingCounter.GetField<ISaberMovementData, SaberSwingRatingCounter>("_saberMovementData");

            if (!swingRatingCache.ContainsKey(saberSwingRatingCounter))
            {
                swingRatingCache[saberSwingRatingCounter] = new ProSwingRating();
            }

            if (beforeCutRatingCache.ContainsKey((SaberMovementData)saberMovementData))
            {
                swingRatingCache[saberSwingRatingCounter].beforeCutSwingRating = beforeCutRatingCache[(SaberMovementData)saberMovementData];
            }
            //else
            //{
            //    Plugin.Log.Error("SaberMovementData not found in beforeCutRatingCache");
            //}

        }

        public static void SaberSwingRatingCounter_ProcessNewData_AfterCut_Hook(SaberSwingRatingCounter saberSwingRatingCounter, float afterCutRating)
        {
            //Plugin.Log.Info("SaberSwingRatingCounter_ProcessNewData_AfterCut_Hook");
            if (!swingRatingCache.ContainsKey(saberSwingRatingCounter))
            {
                swingRatingCache[saberSwingRatingCounter] = new ProSwingRating();
            }

            swingRatingCache[saberSwingRatingCounter].afterCutSwingRating = afterCutRating;
        }

        public static void SaberSwingRatingCounter_ProcessNewData_IncrementAfterCut_Hook(float afterCutRatingIncrement, SaberSwingRatingCounter saberSwingRatingCounter)
        {
            //Plugin.Log.Info("SaberSwingRatingCounter_ProcessNewData_IncrementAfterCut_Hook");
            if (!swingRatingCache.ContainsKey(saberSwingRatingCounter))
            {
                swingRatingCache[saberSwingRatingCounter] = new ProSwingRating();
            }

            swingRatingCache[saberSwingRatingCounter].afterCutSwingRating += afterCutRatingIncrement;
        }

        public static void SaberMovementData_ComputeSwingRating_Hook(SaberMovementData saberMovementData, float beforeCutRating)
        {
            //Plugin.Log.Info("SaberMovementData_ComputeSwingRating_Hook");
            beforeCutRatingCache[saberMovementData] = beforeCutRating;
        }



        public static IEnumerable<CodeInstruction> SaberSwingRatingCounter_Init_Transpiler(IEnumerable<CodeInstruction> originalInstructions)
        {
            //SaberSwingRatingCounter.Init

            List<CodeInstruction> modifiedMethod = originalInstructions.ToList();

            List<List<OpCode>> getBeforeCutPattern = new List<List<OpCode>>
            {
                new List<OpCode>{ OpCodes.Ldarg_0 },
                new List<OpCode>{ OpCodes.Ldarg_1 },
                new List<OpCode>{ OpCodes.Callvirt },
                new List<OpCode>{ OpCodes.Stfld }
            };

            List<CodeInstruction> cacheBeforeCut = new List<CodeInstruction>()
            {
                //load SaberSwingRatingCounter instance
                new CodeInstruction(OpCodes.Ldarg_0),

                //load ISaberMovementData instance
                new CodeInstruction(OpCodes.Ldarg_1),

                //cache
                CodeInstruction.Call(typeof(ProSwingRatingPatch),nameof(SaberSwingRatingCounter_Init_Hook))
            };

            //patch
            int cacheBeforeCutPosition = ProUtil.FindOpCodePattern(modifiedMethod, getBeforeCutPattern);
            if(cacheBeforeCutPosition == -1) {
                Plugin.Log.Error("Failed to patch SaberSwingRatingCounter_Init_Transpiler");
                return originalInstructions;
            }
            modifiedMethod.InsertRange(cacheBeforeCutPosition+getBeforeCutPattern.Count(),cacheBeforeCut);


            Plugin.Log.Info($"Patched SaberSwingRatingCounter.Init from length [{originalInstructions.Count()}] to [{modifiedMethod.Count()}]");

            return modifiedMethod;
        }
        public static IEnumerable<CodeInstruction> SaberSwingRatingCounter_ProcessNewData_Transpiler(IEnumerable<CodeInstruction> originalInstructions)
        {
            List<CodeInstruction> modifiedMethod = originalInstructions.ToList();

            //SaberSwingRatingCounter.ProcessNewData

            List<List<OpCode>> getBeforeCutPattern = new List<List<OpCode>>
            {
                new List<OpCode>{ OpCodes.Ldarg_0 },
                new List<OpCode>{ OpCodes.Ldarg_0 },
                new List<OpCode>{ OpCodes.Ldfld },
                new List<OpCode>{ OpCodes.Ldloc_3 },
                new List<OpCode>{ OpCodes.Callvirt },
                new List<OpCode>{ OpCodes.Stfld }
            };

            List<CodeInstruction> getBeforeCutHook = new List<CodeInstruction>()
            {
                //load SaberSwingRatingCounter instance
                new CodeInstruction(OpCodes.Ldarg_0),

                //cache
                CodeInstruction.Call(typeof(ProSwingRatingPatch),nameof(SaberSwingRatingCounter_ProcessNewData_BeforeCut_Hook))
            };

            List<List<OpCode>> clampAfterCutPattern = new List<List<OpCode>>
            {
                new List<OpCode>{ OpCodes.Ldarg_0 },
                new List<OpCode>{ OpCodes.Ldfld },
                new List<OpCode>{ OpCodes.Ldc_R4 },
                new List<OpCode>{ OpCodes.Ble_Un_S, OpCodes.Ble_Un }, 
                new List<OpCode>{ OpCodes.Ldarg_0 },
                new List<OpCode>{ OpCodes.Ldc_R4 },
                new List<OpCode>{ OpCodes.Stfld }
            };

            List<CodeInstruction> clampAfterCutHook = new List<CodeInstruction>()
            {
                //load SaberSwingRatingCounter instance
                new CodeInstruction(OpCodes.Ldarg_0),

                //load SaberSwingRatingCounter._afterCutRating
                new CodeInstruction(OpCodes.Ldarg_0),
                CodeInstruction.LoadField(typeof(SaberSwingRatingCounter),"_afterCutRating"),

                //cache values
                CodeInstruction.Call(typeof(ProSwingRatingPatch),nameof(SaberSwingRatingCounter_ProcessNewData_AfterCut_Hook))
            };

            List<List<OpCode>> incrementAfterCutPattern = new List<List<OpCode>>
            {
                new List<OpCode>{ OpCodes.Add },
                new List<OpCode>{ OpCodes.Stfld },

                new List<OpCode>{ OpCodes.Ldarg_0 },
                new List<OpCode>{ OpCodes.Ldfld },
                new List<OpCode>{ OpCodes.Ldc_R4 },
                new List<OpCode>{ OpCodes.Ble_Un_S, OpCodes.Ble_Un },
                new List<OpCode>{ OpCodes.Ldarg_0 },
                new List<OpCode>{ OpCodes.Ldc_R4 },
                new List<OpCode>{ OpCodes.Stfld }
            };

            List<CodeInstruction> incrementAfterCutHook = new List<CodeInstruction>()
            {
                //copy after cut step rating return value
                new CodeInstruction(OpCodes.Dup),

                //load SaberSwingRatingCounter instance
                new CodeInstruction(OpCodes.Ldarg_0),

                //cache values
                CodeInstruction.Call(typeof(ProSwingRatingPatch),nameof(SaberSwingRatingCounter_ProcessNewData_IncrementAfterCut_Hook))
            };


            //patch before cut
            int getBeforeCutPosition = ProUtil.FindOpCodePattern(modifiedMethod, getBeforeCutPattern);
            if (getBeforeCutPosition == -1)
            {
                Plugin.Log.Error("Failed to patch SaberSwingRatingCounter.ProcessNewData with Transpiler");
                return originalInstructions;
            }
            modifiedMethod.InsertRange(getBeforeCutPosition + getBeforeCutPattern.Count(), getBeforeCutHook);

            //patch first after cut
            int clampAfterCutPosition = ProUtil.FindOpCodePattern(modifiedMethod, clampAfterCutPattern);
            if (clampAfterCutPosition == -1)
            {
                Plugin.Log.Error("Failed to patch SaberSwingRatingCounter.ProcessNewData with Transpiler");
                return originalInstructions;
            }
            modifiedMethod.InsertRange(clampAfterCutPosition, clampAfterCutHook);

            //patch second after cut
            int incrementAfterCutPosition = ProUtil.FindOpCodePattern(modifiedMethod, incrementAfterCutPattern, clampAfterCutPosition + clampAfterCutHook.Count());
            if (incrementAfterCutPosition == -1) {
                Plugin.Log.Error("Failed to patch SaberSwingRatingCounter.ProcessNewData with Transpiler");
                return originalInstructions;
            }
            modifiedMethod.InsertRange(incrementAfterCutPosition, incrementAfterCutHook);

            Plugin.Log.Info($"Patched SaberSwingRatingCounter.ProcessNewData from length [{originalInstructions.Count()}] to [{modifiedMethod.Count()}]");
            return modifiedMethod;
        }
        public static IEnumerable<CodeInstruction> SaberMovementData_ComputeSwingRating_Transpiler(IEnumerable<CodeInstruction> originalInstructions)
        {
            List<CodeInstruction> modifiedMethod = originalInstructions.ToList();

            List<List<OpCode>> clampSwingRatingPattern = new List<List<OpCode>> {
                //new List<OpCode>{ OpCodes.Ldloc_S },
                new List<OpCode>{ OpCodes.Ldc_R4 },
                new List<OpCode>{ OpCodes.Ble_Un_S, OpCodes.Ble_Un },
                new List<OpCode>{ OpCodes.Ldc_R4 },
                new List<OpCode>{ OpCodes.Stloc_S },
                new List<OpCode>{ OpCodes.Ldloc_S },
                new List<OpCode>{ OpCodes.Ret }
            };

            List<CodeInstruction> clampSwingRatingHook = new List<CodeInstruction>()
            {
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldloc_S,4),
                CodeInstruction.Call(typeof(ProSwingRatingPatch),nameof(SaberMovementData_ComputeSwingRating_Hook))
            };

            //patch
            if (!ProUtil.InsertInstructionsAtOpCodePatternOnce(modifiedMethod,clampSwingRatingPattern,clampSwingRatingHook))
            {
                Plugin.Log.Error("Failed to patch SaberMovementData.ComputeSwingRating with Transpiler");
                return originalInstructions;
            }


            Plugin.Log.Info($"Patched SaberMovementData.ComputeSwingRating from length [{originalInstructions.Count()}] to [{modifiedMethod.Count()}]");
            return modifiedMethod;
        }

        public static void ScoreController_Start_Postfix()
        {

            Plugin.Log.Info("Resetting Swing Rating Data...");

            swingRatingCache.Clear();
            beforeCutRatingCache.Clear();

        }
        public static void SaberMovementData_ComputeSwingRating_Postfix()
        {
            Plugin.Log.Info("SaberMovementData_ComputeSwingRating_Postfix");
        }

        public static void Init()
        {

            Plugin.Log.Info("Patching Swing Rating...");


            Plugin.harmony.Patch(
                original: typeof(SaberSwingRatingCounter).GetMethod(nameof(SaberSwingRatingCounter.ProcessNewData)),
                transpiler: new HarmonyMethod(typeof(ProSwingRatingPatch).GetMethod(nameof(ProSwingRatingPatch.SaberSwingRatingCounter_ProcessNewData_Transpiler)))
            );

            Plugin.harmony.Patch(
                original: typeof(SaberMovementData).GetMethod("ComputeSwingRating", BindingFlags.NonPublic | BindingFlags.Instance),
                transpiler: new HarmonyMethod(typeof(ProSwingRatingPatch).GetMethod(nameof(ProSwingRatingPatch.SaberMovementData_ComputeSwingRating_Transpiler)))
                //postfix: new HarmonyMethod(typeof(ProSwingRatingPatch).GetMethod(nameof(ProSwingRatingPatch.SaberMovementData_ComputeSwingRating_Postfix)))
            );

            Plugin.harmony.Patch(
                original: typeof(SaberSwingRatingCounter).GetMethod(nameof(SaberSwingRatingCounter.Init)),
                transpiler: new HarmonyMethod(typeof(ProSwingRatingPatch).GetMethod(nameof(ProSwingRatingPatch.SaberSwingRatingCounter_Init_Transpiler)))
            );

            Plugin.harmony.Patch(
                original: typeof(ScoreController).GetMethod("Start",BindingFlags.Instance | BindingFlags.NonPublic),
                postfix: new HarmonyMethod(typeof(ProSwingRatingPatch).GetMethod(nameof(ProSwingRatingPatch.ScoreController_Start_Postfix)))
            );
        }
    }
}
