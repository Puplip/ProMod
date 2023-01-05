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

    /// <summary>
    /// Remove Note Cut Effects
    /// </summary>
    public static class ProEffectsPatch
    {

        public static bool EffectDisabler_Prefix()
        {
            return !Plugin.Config.GameplayEffectsDisabled;
        }
        internal static void Init()
        {
            Plugin.harmony.Patch(
                original: typeof(HapticFeedbackController).GetMethod(nameof(HapticFeedbackController.PlayHapticFeedback)),
                prefix: new HarmonyMethod(typeof(ProEffectsPatch).GetMethod(nameof(ProEffectsPatch.EffectDisabler_Prefix)))
            );

            Plugin.harmony.Patch(
                original: typeof(NoteCutParticlesEffect).GetMethod(nameof(NoteCutParticlesEffect.SpawnParticles)),
                prefix: new HarmonyMethod(typeof(ProEffectsPatch).GetMethod(nameof(ProEffectsPatch.EffectDisabler_Prefix)))
            );

            Plugin.harmony.Patch(
                original: typeof(ShockwaveEffect).GetMethod(nameof(ShockwaveEffect.SpawnShockwave)),
                prefix: new HarmonyMethod(typeof(ProEffectsPatch).GetMethod(nameof(ProEffectsPatch.EffectDisabler_Prefix)))
            );

            Plugin.harmony.Patch(
                original: typeof(NoteDebrisSpawner).GetMethod(nameof(NoteDebrisSpawner.SpawnDebris)),
                prefix: new HarmonyMethod(typeof(ProEffectsPatch).GetMethod(nameof(ProEffectsPatch.EffectDisabler_Prefix)))
            );

            Plugin.harmony.Patch(
                original: typeof(BombExplosionEffect).GetMethod(nameof(BombExplosionEffect.SpawnExplosion)),
                prefix: new HarmonyMethod(typeof(ProEffectsPatch).GetMethod(nameof(ProEffectsPatch.EffectDisabler_Prefix)))
            );
        }
    }
}
