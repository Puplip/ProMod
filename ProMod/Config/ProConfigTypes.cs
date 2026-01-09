using System;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using CameraUtils.Core;
using System.Collections.Generic;
using System.ComponentModel;

namespace ProMod
{
    public enum ProJumpSetting
    {
        BaseGame,
        JumpDistance,
        ReactionTime,
        HouseSpecial
    }

    public class ProHUDConfig
    {
        public enum AccStyle
        {
            Instant,
            InstantBar,
            Smart,
            SmartBar,
            None
        }
        public enum FullComboAccStyle
        {
            AccOnly,
            DiffOnly,
            AccAndDiff,
            None
        }

        public enum TimeDependenceStyle
        {
            Percent,
            Angle,
            None
        }
        public enum SwingStyle
        {
            SwingRating,
            SeperateScore,
            CombindedScore,
            SeperateUnderswingDamage,
            CombinedUnderswingDamage,
            None
        }
        public enum CenterDistanceStyle
        {
            Score,
            Millimeters,
            None
        }
        public enum CutStatsCenterStyle
        {
            HistogramTop,
            HistogramBottom,
            HistogramOnly,
            CutOnly,
            None
        }


        public enum ComboStyle
        {
            FullCombo,
            Combo,
            None
        }
        public enum ErrorStyle
        {
            Category,
            Errors,
            None
        }
        public enum ProgressStyle
        {
            TimeLeft,
            NotesLeft,
            TimeFraction,
            NotesFraction,
            None
        }

        public enum TopBottomElementPositions
        {
            CutStatsTop,
            ScoreDensityTop,
            BothTopCutStatsLeft,
            BothTopScoreDensityLeft,
            BothBottomCutStatsLeft,
            BothBottomScoreDensityLeft
        }
        public enum LeftRightElementPositions
        {
            AccuracyLeftComboRight,
            ComboLeftAccuracyRight
        }
        //main
        [JsonProperty("ProHUDEnabled")]
        public bool proHUDEnabled = false;

        [JsonConverter(typeof(StringEnumConverter)), JsonProperty("LeftRightElementPositions")]
        public LeftRightElementPositions leftRightElementPositions = LeftRightElementPositions.ComboLeftAccuracyRight;

        [JsonConverter(typeof(StringEnumConverter)), JsonProperty("TopBottomElementPositions")]
        public TopBottomElementPositions topBottomElementPositions = TopBottomElementPositions.CutStatsTop;

        [JsonProperty("ShowScoreDensity")]
        public bool showScoreDensity = false;

        [JsonProperty("ShowCutStats")]
        public bool showCutStats = false;

        //acc
        [JsonConverter(typeof(StringEnumConverter)), JsonProperty("AccStyle")]
        public AccStyle accStyle = AccStyle.SmartBar;

        [JsonConverter(typeof(StringEnumConverter)), JsonProperty("FullComboAccStyle")]
        public FullComboAccStyle fullComboAccStyle = FullComboAccStyle.AccAndDiff;

        [JsonProperty("ShowLeftRightAcc")]
        public bool showLeftRightAcc = true;

        [JsonProperty("AccColorsEnabled")]
        public bool accColorsEnabled = true;

        [JsonProperty("AccColors")]
        public List<ProAccColorPointConfig> accColorPoints;

        //combo
        [JsonConverter(typeof(StringEnumConverter)), JsonProperty("ComboStyle")]
        public ComboStyle comboStyle = ComboStyle.Combo;

        //Song Progress
        [JsonConverter(typeof(StringEnumConverter)), JsonProperty("ProgressStyle")]
        public ProgressStyle songProgressStyle = ProgressStyle.TimeFraction;

        //health bar
        [JsonConverter(typeof(StringEnumConverter)), JsonProperty("ErrorStyle")]
        public ErrorStyle errorStyle = ErrorStyle.Category;

        [JsonProperty("HealthBarFailColor")]
        public ProColorSerializable healthBarFailColor = Color.red;

        [JsonProperty("HealthBarFullColor")]
        public ProColorSerializable healthBarFullColor = Color.green;

        //cut stats
        [JsonConverter(typeof(StringEnumConverter)), JsonProperty("CutStatsCenterStyle")]
        public CutStatsCenterStyle cutStatsCenterStyle = CutStatsCenterStyle.HistogramTop;

        [JsonConverter(typeof(StringEnumConverter)), JsonProperty("TimeDependenceStyle")]
        public TimeDependenceStyle timeDependenceStyle = TimeDependenceStyle.Angle;

        [JsonConverter(typeof(StringEnumConverter)), JsonProperty("SwingStyle")]
        public SwingStyle swingStyle = SwingStyle.SeperateScore;

        [JsonConverter(typeof(StringEnumConverter)), JsonProperty("CenterDistanceStyle")]
        public CenterDistanceStyle centerDistanceStyle = CenterDistanceStyle.Score;


    }

    public class ProGameplayEffectConfig
    {
        [JsonProperty("Debris")]
        public bool debris = true;

        [JsonProperty("Particles")]
        public bool particles = true;

        [JsonProperty("Vibration")]
        public bool vibration = true;

        [JsonProperty("Shockwaves")]
        public bool shockwaves = true;

        [JsonProperty("BombExplosions")]
        public bool bombExplosions = true;
    }

    public class ProCameraMaskConfig
    {
        [JsonIgnore]
        private int mask = 0x3F3BFF37;

        public bool Default                    { get => GetLayer(VisibilityLayer.Default                   ); set => SetLayer(VisibilityLayer.Default                   , value); }
        public bool TransparentFX              { get => GetLayer(VisibilityLayer.TransparentFX             ); set => SetLayer(VisibilityLayer.TransparentFX             , value); }
        public bool IgnoreRaycast              { get => GetLayer(VisibilityLayer.IgnoreRaycast             ); set => SetLayer(VisibilityLayer.IgnoreRaycast             , value); }
        public bool ThirdPerson                { get => GetLayer(VisibilityLayer.ThirdPerson               ); set => SetLayer(VisibilityLayer.ThirdPerson               , value); }
        public bool Water                      { get => GetLayer(VisibilityLayer.Water                     ); set => SetLayer(VisibilityLayer.Water                     , value); }
        public bool UI                         { get => GetLayer(VisibilityLayer.UI                        ); set => SetLayer(VisibilityLayer.UI                        , value); }
        public bool FirstPerson                { get => GetLayer(VisibilityLayer.FirstPerson               ); set => SetLayer(VisibilityLayer.FirstPerson               , value); }
        public bool HmdOnly { get => GetLayer(VisibilityLayer.HmdOnly); set => SetLayer(VisibilityLayer.HmdOnly, value); }
        public bool Note                       { get => GetLayer(VisibilityLayer.Note                      ); set => SetLayer(VisibilityLayer.Note                      , value); }
        public bool NoteDebris                 { get => GetLayer(VisibilityLayer.NoteDebris                ); set => SetLayer(VisibilityLayer.NoteDebris                , value); }
        public bool Avatar                     { get => GetLayer(VisibilityLayer.Avatar                    ); set => SetLayer(VisibilityLayer.Avatar                    , value); }
        public bool Obstacle                   { get => GetLayer(VisibilityLayer.Obstacle                  ); set => SetLayer(VisibilityLayer.Obstacle                  , value); }
        public bool Saber                      { get => GetLayer(VisibilityLayer.Saber                     ); set => SetLayer(VisibilityLayer.Saber                     , value); }
        public bool NeonLight                  { get => GetLayer(VisibilityLayer.NeonLight                 ); set => SetLayer(VisibilityLayer.NeonLight                 , value); }
        public bool Environment                { get => GetLayer(VisibilityLayer.Environment               ); set => SetLayer(VisibilityLayer.Environment               , value); }
        public bool GrabPassTexture1           { get => GetLayer(VisibilityLayer.GrabPassTexture1          ); set => SetLayer(VisibilityLayer.GrabPassTexture1          , value); }
        public bool CutEffectParticles         { get => GetLayer(VisibilityLayer.CutEffectParticles        ); set => SetLayer(VisibilityLayer.CutEffectParticles        , value); }
        public bool ScreenDisplacement         { get => GetLayer(VisibilityLayer.ScreenDisplacement        ); set => SetLayer(VisibilityLayer.ScreenDisplacement        , value); }
        public bool DesktopOnly                { get => GetLayer(VisibilityLayer.DesktopOnly               ); set => SetLayer(VisibilityLayer.DesktopOnly               , value); }
        public bool NonReflectedParticles      { get => GetLayer(VisibilityLayer.NonReflectedParticles     ); set => SetLayer(VisibilityLayer.NonReflectedParticles     , value); }
        public bool EnvironmentPhysics         { get => GetLayer(VisibilityLayer.EnvironmentPhysics        ); set => SetLayer(VisibilityLayer.EnvironmentPhysics        , value); }
        public bool AlwaysVisible              { get => GetLayer(VisibilityLayer.AlwaysVisible             ); set => SetLayer(VisibilityLayer.AlwaysVisible             , value); }
        public bool Event                      { get => GetLayer(VisibilityLayer.Event                     ); set => SetLayer(VisibilityLayer.Event                     , value); }
        public bool DesktopOnlyAndReflected    { get => GetLayer(VisibilityLayer.DesktopOnlyAndReflected   ); set => SetLayer(VisibilityLayer.DesktopOnlyAndReflected   , value); }
        public bool HmdOnlyAndReflected        { get => GetLayer(VisibilityLayer.HmdOnlyAndReflected       ); set => SetLayer(VisibilityLayer.HmdOnlyAndReflected       , value); }
        public bool FixMRAlpha                 { get => GetLayer(VisibilityLayer.FixMRAlpha                ); set => SetLayer(VisibilityLayer.FixMRAlpha                , value); }
        public bool AlwaysVisibleAndReflected  { get => GetLayer(VisibilityLayer.AlwaysVisibleAndReflected ); set => SetLayer(VisibilityLayer.AlwaysVisibleAndReflected , value); }
        public bool DontShowInExternalMRCamera { get => GetLayer(VisibilityLayer.DontShowInExternalMRCamera); set => SetLayer(VisibilityLayer.DontShowInExternalMRCamera, value); }
        public bool PlayersPlace               { get => GetLayer(VisibilityLayer.PlayersPlace              ); set => SetLayer(VisibilityLayer.PlayersPlace              , value); }
        public bool Skybox                     { get => GetLayer(VisibilityLayer.Skybox                    ); set => SetLayer(VisibilityLayer.Skybox                    , value); }
        public bool MRForegroundClipPlane      { get => GetLayer(VisibilityLayer.MRForegroundClipPlane     ); set => SetLayer(VisibilityLayer.MRForegroundClipPlane     , value); }
        public bool Reserved                   { get => GetLayer(VisibilityLayer.Reserved                  ); set => SetLayer(VisibilityLayer.Reserved                  , value); }

        public static implicit operator int(ProCameraMaskConfig proCameraMask)
        {
            return proCameraMask.mask;
        }

        public static implicit operator ProCameraMaskConfig(int i)
        {
            ProCameraMaskConfig proCameraMask = new ProCameraMaskConfig();

            proCameraMask.mask = i;

            return proCameraMask;
        }
        public void SetLayer(VisibilityLayer layer, bool value)
        {
            int bitMask = (1 << (int)layer);
            mask = value ? bitMask | mask : ~bitMask & mask;
        }
        public bool GetLayer(VisibilityLayer layer)
        {
            return (mask & (1 << (int)layer)) != 0;
        }

    }
    public class ProColorSerializable
    {
        [JsonProperty("R")]
        public float r = 1.0f;

        [JsonProperty("G")]
        public float g = 1.0f;

        [JsonProperty("B")]
        public float b = 1.0f;

        public static implicit operator Color(ProColorSerializable proColor)
        {
            return new Color(proColor.r, proColor.g, proColor.b);
        }

        public static implicit operator ProColorSerializable(Color proColor)
        {
            return new ProColorSerializable() { r = proColor.r, g = proColor.g, b = proColor.b };
        }
    }
    public class ProVector2Serializable
    {
        [JsonProperty("X")]
        public float x = 1.0f;

        [JsonProperty("Y")]
        public float y = 1.0f;

        public static implicit operator Vector2(ProVector2Serializable proVector2)
        {
            return new Vector2(proVector2.x, proVector2.y);
        }

        public static implicit operator ProVector2Serializable(Vector2 vector2)
        {
            return new ProVector2Serializable() { x = vector2.x, y = vector2.y };
        }
    }
    public class ProVector3Serializable
    {
        [JsonProperty("X")]
        public float x = 1.0f;

        [JsonProperty("Y")]
        public float y = 1.0f;

        [JsonProperty("Z")]
        public float z = 1.0f;

        public static implicit operator Vector3(ProVector3Serializable proVector)
        {
            return new Vector3(proVector.x, proVector.y, proVector.z);
        }

        public static implicit operator ProVector3Serializable(Vector3 vector3)
        {
            return new ProVector3Serializable() { x = vector3.x, y = vector3.y, z = vector3.z };
        }
    }


    public class ProCutScoreConfig
    {

        [JsonProperty("CutScoresEnabled")]
        public bool cutScoresEnabled = false;

        [JsonProperty("AbnormalNoteSize")]
        public int abnormalNoteSize = 100;

        [JsonProperty("CutScorePoints")]
        public List<ProCutScorePointConfig> cutScorePoints;


    }



    public class ProCutScorePointConfig : IComparable<ProCutScorePointConfig>
    {
        public enum DisplayStyle
        {
            CutScore,
            //LetterRank,
            //CustomString
        }


        [JsonConverter(typeof(StringEnumConverter)), JsonProperty("DisplayStyle")]
        public DisplayStyle displayStyle = DisplayStyle.CutScore;

        [JsonProperty("CustomString")]
        public string customString = "";

        [JsonProperty("Score")]
        public int score = 0;

        [JsonProperty("Color")]
        public ProColorSerializable color = new Color(1.0f,1.0f,1.0f);

        [JsonProperty("Size")]
        public int size = 100;
        public ProCutScorePointConfig() { }

        public int CompareTo(ProCutScorePointConfig other)
        {
            return other.score.CompareTo(score);
        }
    }

    public class ProAccColorPointConfig : IComparable<ProAccColorPointConfig>
    {

        [JsonProperty("Score")]
        public int accuracy = 0;

        [JsonProperty("Color")]
        public ProColorSerializable color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        public ProAccColorPointConfig(int _score, Color _color)
        {
            accuracy = _score;
            color = _color;
        }

        public ProAccColorPointConfig() { }

        public int CompareTo(ProAccColorPointConfig other)
        {
            return other.accuracy.CompareTo(accuracy);
        }
    }


}