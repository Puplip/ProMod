
using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.IO;
using IPA.Utilities;

namespace ProMod;

public class ProConfig
{

    [JsonProperty("HeightGuideEnabled")]
    public bool heightGuideEnabled = false;

    [JsonProperty("HeightGuideOffset")]
    public float heightGuideOffset = 0.75f;

    [JsonProperty("HeightGuideLength")]
    public float heightGuideLength = 1.0f;

    [JsonProperty("JumpSetting"), JsonConverter(typeof(StringEnumConverter))]
    public ProJumpSetting jumpSetting = ProJumpSetting.HouseSpecial;

    [JsonProperty("JumpDistance")]
    public float jumpDistance = 17.0f;
    [JsonProperty("MinJumpDistance")]
    public float minJumpDistance = 10.0f;
    [JsonProperty("MaxJumpDistance")]
    public float maxJumpDistance = 10.0f;

    [JsonProperty("ReactionTime")]
    public float reactionTime = 400f;
    [JsonProperty("MinReactionTime")]
    public float minReactionTime = 350f;
    [JsonProperty("MaxReactionTime")]
    public float maxReactionTime = 750f;

    [JsonProperty("BombColorEnabled")]
    public bool bombColorEnabled = false;

    [JsonProperty("BombColor")]
    public ProColorSerializable bombColor = Color.white;

    [JsonProperty("BombColorMultiplier")]
    public float bombColorMultiplier = 10f;

    //[JsonConverter(typeof(StringEnumConverter)),JsonProperty("JDRTOverrideMode")]
    //public ProJDRTOverrideMode jdrtOverrideMode = ProJDRTOverrideMode.None;



    [JsonProperty("ProHUD")]
    public ProHUDConfig proHUDConfig = new ProHUDConfig();

    [JsonProperty("CutScores")]
    public ProCutScoreConfig cutScores = new ProCutScoreConfig();

    [JsonProperty("GameplayEffects")]
    public ProGameplayEffectConfig gameplayEffects = new ProGameplayEffectConfig();

    [JsonProperty("HMDCameraMask")]
    public ProCameraMaskConfig hmdCameraMask = new ProCameraMaskConfig();

    [JsonIgnore]
    private static string filePath { get => Path.Combine(UnityGame.UserDataPath, "ProMod.json"); }

    public static void Load()
    {
        if (!File.Exists(filePath))
        {
            Plugin.Log.Info("Creating New ProMod Config...");
            Plugin.Config = new ProConfig();
        }
        else
        {
            Plugin.Log.Info("Loading ProMod Config...");
            try
            {
                Plugin.Config = JsonConvert.DeserializeObject<ProConfig>(File.ReadAllText(filePath));
            }catch (Exception ex)
            {
                Plugin.Log.Error("Failed to Load Config!");
                Plugin.Log.Error(ex);
                string errorConfigPath = Path.Combine(UnityGame.UserDataPath, "ProMod_ERROR.json");
                File.WriteAllText(errorConfigPath, File.ReadAllText(filePath));
                Plugin.Log.Error($"Broken Config Moved to: {errorConfigPath}");

                Plugin.Log.Info("Creating New ProMod Config...");
                Plugin.Config = new ProConfig();
            }
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
        if(bombColor == null)
        {
            bombColor = Color.white;
        }
        bombColorMultiplier = Mathf.Clamp(bombColorMultiplier, 0f, 50f);

        if (cutScores == null)
        {
            cutScores = new ProCutScoreConfig();
        }
        if (cutScores.cutScorePoints == null)
        {
            cutScores.cutScorePoints = ProDefaults.CutScores();
        }

        if (proHUDConfig == null)
        {
            proHUDConfig = new ProHUDConfig();
        }
        if (proHUDConfig.accColorPoints == null)
        {
            proHUDConfig.accColorPoints = ProDefaults.AccColors();
        }
        if (proHUDConfig.healthBarFullColor == null)
        {
            proHUDConfig.healthBarFullColor = Color.green;
        }
        if (proHUDConfig.healthBarFailColor == null)
        {
            proHUDConfig.healthBarFailColor = Color.red;
        }

        if (gameplayEffects == null)
        {
            gameplayEffects = new ProGameplayEffectConfig();
        }

        if (hmdCameraMask == null)
        {
            hmdCameraMask = new ProCameraMaskConfig();
        }

        cutScores.cutScorePoints.RemoveAll((x) => x == null || x.color == null);

        proHUDConfig.accColorPoints.RemoveAll((x) => x == null || x.color == null);

        HashSet<int> cutScoreSet = new HashSet<int>();

        for (int i = 0; i < cutScores.cutScorePoints.Count; i++)
        {
            if (cutScoreSet.Contains(cutScores.cutScorePoints[i].score))
            {
                Plugin.Log.Info("Removing Duplicate CutScore Config: " + cutScores.cutScorePoints[i].score);
                cutScores.cutScorePoints.RemoveAt(i);
                i--;
            }
            else if (cutScores.cutScorePoints[i].score < 0 || cutScores.cutScorePoints[i].score > 115)
            {
                Plugin.Log.Info("Removing Invalid CutScore Config: " + cutScores.cutScorePoints[i].score);
                cutScores.cutScorePoints.RemoveAt(i);
                i--;
            }
            else
            {
                cutScoreSet.Add(cutScores.cutScorePoints[i].score);
            }
        }

        if (!cutScoreSet.Contains(0))
        {
            cutScores.cutScorePoints.Add(new ProCutScorePointConfig { 
                displayStyle = ProCutScorePointConfig.DisplayStyle.CutScore,
                score = 0,
                size = 100,
                color = Color.white
            });
        }
        cutScores.cutScorePoints.Sort();

        HashSet<int> accSet = new HashSet<int>();

        for (int i = 0; i < proHUDConfig.accColorPoints.Count; i++)
        {
            if (accSet.Contains(proHUDConfig.accColorPoints[i].accuracy))
            {
                Plugin.Log.Info("Removing Duplicate AccColor Config: " + proHUDConfig.accColorPoints[i].accuracy);
                proHUDConfig.accColorPoints.RemoveAt(i);
                i--;
                continue;
            }
            
            if (proHUDConfig.accColorPoints[i].accuracy > 100 || proHUDConfig.accColorPoints[i].accuracy < 0)
            {
                Plugin.Log.Info("Removing Invalid AccColor Config: " + proHUDConfig.accColorPoints[i].accuracy);
                proHUDConfig.accColorPoints.RemoveAt(i);
                i--;
                continue;
            }

            accSet.Add(proHUDConfig.accColorPoints[i].accuracy);
        }

        if (!accSet.Contains(0))
        {
            proHUDConfig.accColorPoints.Add(new ProAccColorPointConfig(0, Color.white));
        }

        proHUDConfig.accColorPoints.Sort();

    }
}
