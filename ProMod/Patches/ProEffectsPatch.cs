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
        public static bool Debris_Prefix()
        {
            return Plugin.Config.gameplayEffects.debris;
        }
        public static bool Particles_Prefix()
        {
            return Plugin.Config.gameplayEffects.particles;
        }
        public static bool Vibration_Prefix()
        {
            return Plugin.Config.gameplayEffects.vibration;
        }
        public static bool Shockwaves_Prefix()
        {
            return Plugin.Config.gameplayEffects.shockwaves;
        }
        public static bool Explosions_Prefix()
        {
            return Plugin.Config.gameplayEffects.bombExplosions;
        }
        internal static void Init()
        {
            Plugin.harmony.Patch(
                original: typeof(HapticFeedbackManager).GetMethod(nameof(HapticFeedbackManager.PlayHapticFeedback)),
                prefix: new HarmonyMethod(typeof(ProEffectsPatch).GetMethod(nameof(ProEffectsPatch.Vibration_Prefix)))
            );

            Plugin.harmony.Patch(
                original: typeof(NoteCutParticlesEffect).GetMethod(nameof(NoteCutParticlesEffect.SpawnParticles)),
                prefix: new HarmonyMethod(typeof(ProEffectsPatch).GetMethod(nameof(ProEffectsPatch.Particles_Prefix)))
            );

            Plugin.harmony.Patch(
                original: typeof(ShockwaveEffect).GetMethod(nameof(ShockwaveEffect.SpawnShockwave)),
                prefix: new HarmonyMethod(typeof(ProEffectsPatch).GetMethod(nameof(ProEffectsPatch.Shockwaves_Prefix)))
            );

            Plugin.harmony.Patch(
                original: typeof(NoteDebrisSpawner).GetMethod(nameof(NoteDebrisSpawner.SpawnDebris)),
                prefix: new HarmonyMethod(typeof(ProEffectsPatch).GetMethod(nameof(ProEffectsPatch.Debris_Prefix)))
            );

            Plugin.harmony.Patch(
                original: typeof(BombExplosionEffect).GetMethod(nameof(BombExplosionEffect.SpawnExplosion)),
                prefix: new HarmonyMethod(typeof(ProEffectsPatch).GetMethod(nameof(ProEffectsPatch.Explosions_Prefix)))
            );
        }
    }
}
