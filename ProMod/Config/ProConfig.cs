
using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.IO;
using IPA.Utilities;

namespace ProMod.Config
{
    public class ProConfig
    {
        public bool HeightGuideEnabled  = true;
        public float HeightGuideOffset = 0.75f;
        public float HeightGuideLength  = 1.0f;
        public bool CutScoresEnabled = true;
        public bool RTCurveEnabled = true;
        public bool FixedRTEnabled = false;
        public float FixedRTValue = 420.0f;
        public bool ProStatsEnabled = true;
        public bool StatColorsEnabled = true;
        public bool GameplayEffectsDisabled = true;
        public bool DisableEnvironmentInHMD = true;

        public List<ProCutScoreConfig> CutScores;
        public List<ProReactionTimePoint> RTCurve;
        public List<ProAccColorConfig> AccColors;
        public List<ProStatConfig> ProStats;

        [JsonIgnore]
        private static string filePath { get => Path.Combine(UnityGame.UserDataPath, "ProMod.json"); }

        public static void Load()
        {
            if (!File.Exists(filePath) || true)
            {
                Plugin.Log.Info("Creating New ProMod Config...");
                Plugin.Config = new ProConfig();
            }
            else
            {
                Plugin.Log.Info("Loading ProMod Config...");
                Plugin.Config = JsonConvert.DeserializeObject<ProConfig>(File.ReadAllText(filePath));
            }
            Plugin.Config.Validate();
            Plugin.Config.Save();
        }

        public void Save()
        {
            File.WriteAllText(filePath, JsonConvert.SerializeObject(this, Formatting.Indented));
        }

        public void Validate()
        {
            if(CutScores == null)
            {
                CutScores = ProDefaults.CutScores();
            }
            if(RTCurve == null)
            {
                RTCurve = ProDefaults.RTCurve();
            }
            if (AccColors == null)
            {
                AccColors = ProDefaults.AccColors();
            }
            if (ProStats == null)
            {
                ProStats = ProDefaults.ProStats();
            }

            ProStats.RemoveAll((x) => x == null || x.name == null || x.customLocation == null);

            RTCurve.RemoveAll((x) => x == null);

            CutScores.RemoveAll((x) => x == null);

            AccColors.RemoveAll((x) => x == null);

            HashSet<int> cutScoreSet = new HashSet<int>();

            for (int i = 0; i < CutScores.Count; i++)
            {
                if (cutScoreSet.Contains(CutScores[i].score))
                {
                    Plugin.Log.Info("Removing Duplicate CutScore Config: " + CutScores[i].score);
                    CutScores.RemoveAt(i);
                    i--;
                }
                else if (CutScores[i].score < 0 || CutScores[i].score > 115)
                {
                    Plugin.Log.Info("Removing Invalid CutScore Config: " + CutScores[i].score);
                    CutScores.RemoveAt(i);
                    i--;
                }
                else
                {
                    cutScoreSet.Add(CutScores[i].score);
                }
            }

            CutScores.Sort();

            HashSet<int> accSet = new HashSet<int>();

            for (int i = 0; i < AccColors.Count; i++)
            {
                if (accSet.Contains(AccColors[i].score))
                {
                    Plugin.Log.Info("Removing Duplicate AccColor Config: " + AccColors[i].score);
                    AccColors.RemoveAt(i);
                    i--;
                }
                else if (AccColors[i].score > 100 || AccColors[i].score < 0)
                {
                    Plugin.Log.Info("Removing Invalid AccColor Config: " + AccColors[i].score);
                    AccColors.RemoveAt(i);
                    i--;
                }
                else
                {
                    accSet.Add(AccColors[i].score);
                }
            }

            AccColors.Sort();

            HashSet<float> njsSet = new HashSet<float>();

            for (int i = 0; i < RTCurve.Count; i++)
            {
                if (RTCurve[i].rt < 100.0f || RTCurve[i].njs < 0.0f)
                {
                    Plugin.Log.Info("Removing Invalid JumpDistance Config: " + RTCurve[i].njs + "njs");
                    RTCurve.RemoveAt(i);
                    i--;
                }
                else if (njsSet.Contains(RTCurve[i].njs))
                {
                    Plugin.Log.Info("Removing Duplicate JumpDistance Config: " + RTCurve[i].njs + "njs");
                    RTCurve.RemoveAt(i);
                    i--;
                }
                else
                {
                    njsSet.Add(AccColors[i].score);
                }
            }

            RTCurve.Sort();

            if(RTCurve.Count < 2)
            {
                RTCurve = ProDefaults.RTCurve();
            }
        }

    }
}
