using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using IPA.Config.Stores;
using IPA.Config.Stores.Attributes;
using IPA.Config.Stores.Converters;
using UnityEngine;
using ProMod.Stats;


namespace ProMod;

internal static class ProDefaults
{

    internal static List<ProCutScorePointConfig> CutScores()
    {
        return new List<ProCutScorePointConfig>() {
            new ProCutScorePointConfig
            {
                displayStyle = ProCutScorePointConfig.DisplayStyle.CutScore,
                score = 114,
                size = 300,
                color = (Color)new Color32(255,191,0,255)
            },
            new ProCutScorePointConfig
            {
                displayStyle = ProCutScorePointConfig.DisplayStyle.CutScore,
                score = 112,
                size = 250,
                color = (Color)new Color32(0,128,255,255)
            },
            new ProCutScorePointConfig
            {
                displayStyle = ProCutScorePointConfig.DisplayStyle.CutScore,
                score = 110,
                size = 200,
                color = (Color)new Color32(255,255,255,255)
            },
            new ProCutScorePointConfig
            {
                displayStyle = ProCutScorePointConfig.DisplayStyle.CutScore,
                score = 104,
                size = 150,
                color = (Color)new Color32(255,0,128,255)
            },
            new ProCutScorePointConfig
            {
                displayStyle = ProCutScorePointConfig.DisplayStyle.CutScore,
                score = 0,
                size = 200,
                color = (Color)new Color32(255,0,0,0)
            }
        };
    }

    internal static List<ProAccColorPointConfig> AccColors()
    {
        return new List<ProAccColorPointConfig>() {
            new ProAccColorPointConfig(100,(Color)new Color32(255,255,255,255)),
            new ProAccColorPointConfig(99, (Color)new Color32(255,0,128,255)),
            new ProAccColorPointConfig(98, (Color)new Color32(225,85,0,255)),
            new ProAccColorPointConfig(97, (Color)new Color32(191,0,255,255)),
            new ProAccColorPointConfig(96, (Color)new Color32(0,128,255,255)),
            new ProAccColorPointConfig(95, (Color)new Color32(255,191,0,255)),
            new ProAccColorPointConfig(90, (Color)new Color32(128,255,0,255)),
            new ProAccColorPointConfig(80, (Color)new Color32(255,0,0,255)),
            new ProAccColorPointConfig(0,  (Color)new Color32(128,0,0,0))
        };
    }
}