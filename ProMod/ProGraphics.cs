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
    public class ProDustSweeper : MonoBehaviour
    {

        private float lastParticleSweep = float.MinValue;

        private void Update()
        {
            if (lastParticleSweep + 2.0f < Time.time)
            {
                foreach (ParticleSystem ps in Resources.FindObjectsOfTypeAll<ParticleSystem>())
                {
                    if (ps.name == "DustPS" && ps.gameObject.activeSelf)
                    {
                        ps.gameObject.SetActive(false);
                        Plugin.Log.Info("Cleaned up some dust...");
                    }
                }
                lastParticleSweep = Time.time + 2.0f;
            }
        }
    }

}
