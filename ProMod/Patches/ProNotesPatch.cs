using System;
using System.Reflection;
using UnityEngine;
using HarmonyLib;
using IPA.Utilities;
using System.Reflection.Emit;


namespace ProMod.Patches
{
    //public class ProNoteRipper : MonoBehaviour
    //{
    //    float lastTime = 0;
    //    public void Update()
    //    {
    //        float currenTime = Time.time;
    //        if (currenTime - lastTime > 2.0) {
    //            lastTime = currenTime;
    //            ProUtil.DumpAllGameObjectsWithComponent<BombNoteController>("BombDump");
    //        }
    //    }
    //}
    public static class ProNotesPatch
    {
        private static FieldAccessor<ConditionalMaterialSwitcher, Material>.Accessor _material0Accessor = FieldAccessor<ConditionalMaterialSwitcher, Material>.GetAccessor("_material0");
        private static FieldAccessor<ConditionalMaterialSwitcher, Material>.Accessor _material1Accessor = FieldAccessor<ConditionalMaterialSwitcher, Material>.GetAccessor("_material1");
        public static void BombNoteController_Init_Postfix(ref BombNoteController __instance)
        {

            if (!Plugin.Config.bombColorEnabled) { return; }

            ConditionalMaterialSwitcher cms = __instance.gameObject.GetComponentInChildren<ConditionalMaterialSwitcher>();

            Color customBombColor = new Color(Plugin.Config.bombColor.r, Plugin.Config.bombColor.g, Plugin.Config.bombColor.b, 0.5f);
            _material0Accessor(ref cms).SetColor("_SimpleColor", customBombColor);
            _material1Accessor(ref cms).SetColor("_SimpleColor", customBombColor);

            _material0Accessor(ref cms).SetFloat("_FinalColorMul", Plugin.Config.bombColorMultiplier);
            _material1Accessor(ref cms).SetFloat("_FinalColorMul", Plugin.Config.bombColorMultiplier);

        }
        public static void Init()
        {
            Plugin.harmony.Patch(
                original: typeof(BombNoteController).GetMethod(nameof(BombNoteController.Init)),
                postfix: new HarmonyMethod(typeof(ProNotesPatch).GetMethod(nameof(BombNoteController_Init_Postfix)))
            );
        }
    }
}
