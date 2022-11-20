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
    /// Remove Dust Particle Effect
    /// </summary>
    public class ProClean : MonoBehaviour
    {

        private float lastParticleSweep = 0.0f;

        private void Update()
        {
            if (lastParticleSweep + 2.0f < Time.time)
            {

                foreach (ParticleSystem ps in Resources.FindObjectsOfTypeAll<ParticleSystem>())
                {
                    if (ps.name == "DustPS" && ps.gameObject.activeSelf)
                    {
                        
                        Plugin.Log.Info("Cleaned up some dust... ("+ps.transform.parent.name+").("+ ps.transform.name + ")");
                        ps.gameObject.SetActive(false);
                    }
                }

                lastParticleSweep = Time.time + 2.0f;
            }
        }
    }

    /// <summary>
    /// Remove Note Cut Effects
    /// </summary>
    public static class ProEffects
    {

        public static bool NoteCutCoreEffectsSpawner_HandleNoteWasCut_Prefix() {
            return false;
        }
        internal static void Init()
        {
            
            Plugin.harmony.Patch(
                original: typeof(NoteCutCoreEffectsSpawner).GetMethod(nameof(NoteCutCoreEffectsSpawner.HandleNoteWasCut)),
                prefix: new HarmonyMethod(typeof(ProEffects).GetMethod(nameof(ProEffects.NoteCutCoreEffectsSpawner_HandleNoteWasCut_Prefix))));

        }
    }
}
