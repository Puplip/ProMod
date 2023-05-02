using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using IPA.Config.Stores;
using IPA.Config.Stores.Attributes;
using IPA.Config.Stores.Converters;
using UnityEngine;
using ProMod.Stats;


namespace ProMod.Config
{
    internal static class ProDefaults
    {
        internal static List<ProReactionTimePoint> RTCurve()
        {
            return new List<ProReactionTimePoint>()
            {
                new ProReactionTimePoint(0.0f,500.0f),
                new ProReactionTimePoint(18.0f,450.0f),
                new ProReactionTimePoint(22.0f,400.0f)
            };
        }

        internal static List<ProCutScoreConfig> CutScores()
        {
            return new List<ProCutScoreConfig>() {
                new ProCutScoreConfig(114,new Color(1.0f,0.76f,0.0f,1.0f),200),
                new ProCutScoreConfig(112,new Color(0.0f,0.76f,1.0f,1.0f),175),
                new ProCutScoreConfig(110,new Color(1.0f,1.0f,1.0f,1.0f),150),
                new ProCutScoreConfig(108,new Color(1.0f,1.0f,1.0f,1.0f),125),
                new ProCutScoreConfig(0,new Color(0.76f,0.0f,0.0f,1.0f),100)
            };
        }

        internal static List<ProAccColorConfig> AccColors()
        {
            return new List<ProAccColorConfig>() {
                new ProAccColorConfig(100,new Color(1.0f,1.0f,0.0f,1.0f)),
                new ProAccColorConfig(99,new Color(1.0f,0.38f,0.0f,1.0f)),
                new ProAccColorConfig(98,new Color(1.0f,0.0f,0.38f,1.0f)),
                new ProAccColorConfig(97,new Color(0.76f,0.0f,1.0f,1.0f)),
                new ProAccColorConfig(96,new Color(0.0f,0.38f,1.0f,1.0f)),
                new ProAccColorConfig(95,new Color(1.0f,0.76f,0.0f,1.0f)),
                new ProAccColorConfig(93,new Color(0.76f,1.0f,0.0f,1.0f)),
                new ProAccColorConfig(92,new Color(0.38f,1.0f,0.0f,1.0f)),
                new ProAccColorConfig(91,new Color(0.0f,1.0f,0.38f,1.0f)),
                new ProAccColorConfig(90,new Color(0.0f,1.0f,0.76f,1.0f)),
                new ProAccColorConfig(80,new Color(0.76f,0.0f,0.0f,1.0f)),
                new ProAccColorConfig(0,new Color(0.38f,0.0f,0.0f,1.0f))
            };
        }
        internal static List<ProStatConfig> ProStats()
        {
            return new List<ProStatConfig>() {
                new ProStatConfig("ProStat_EstimateAcc",ProStatLocation.MainHUD_TopRight),
                new ProStatConfig("ProStat_Combo",ProStatLocation.MainHUD_TopLeft),
                new ProStatConfig("ProStat_TimeLeft",ProStatLocation.MainHUD_BottomLeft),
                new ProStatConfig("ProStat_LeftRightAcc",ProStatLocation.MainHUD_BottomRight),/*
                new ProStatConfig("ProStat_InstantAccBar",ProStatLocation.BottomBar)*/

                /*new ProStatConfig("ProStat_LeftAcc",ProStatLocation.RearLeftPanel_TopLeft),
                new ProStatConfig("ProStat_LeftTimeDependence",ProStatLocation.RearLeftPanel_TopRight),
                new ProStatConfig("ProStat_LeftAimDamage",ProStatLocation.RearLeftPanel_BottomLeft),
                new ProStatConfig("ProStat_LeftSwingDamage",ProStatLocation.RearLeftPanel_BottomRight),

                new ProStatConfig("ProStat_RightAcc",ProStatLocation.RearRightPanel_TopLeft),
                new ProStatConfig("ProStat_RightTimeDependence",ProStatLocation.RearRightPanel_TopRight),
                new ProStatConfig("ProStat_RightAimDamage",ProStatLocation.RearRightPanel_BottomLeft),
                new ProStatConfig("ProStat_RightSwingDamage",ProStatLocation.RearRightPanel_BottomRight)*/
            };
        }

        
    }
}