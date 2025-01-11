using System;
using System.Collections.Generic;
using UnityEngine;
using BeatSaberMarkupLanguage.Attributes;
using Zenject;
using System.ComponentModel;
using HMUI;

namespace ProMod.UI;

internal class ProHUDTabUI : ProUI, IInitializable, IDisposable
{
    public void Initialize()
    {

    }

    public void Dispose()
    {

    }

    #region Main
    [UIValue("UIValue_ProHUDEnabled")]
    private bool UIValue_ProHUDEnabled
    {
        get => Plugin.Config.proHUDConfig.proHUDEnabled;
        set
        {
            Plugin.Config.proHUDConfig.proHUDEnabled = value;
            InvokePropertyChanged();
            Plugin.Config.Save();
        }
    }

    [UIValue("UIValue_ShowCutStats")]
    private bool UIValue_ShowCutStats
    {
        get => Plugin.Config.proHUDConfig.showCutStats;
        set
        {
            Plugin.Config.proHUDConfig.showCutStats = value;
            InvokePropertyChanged();
            Plugin.Config.Save();
        }
    }

    [UIValue("UIValue_ShowScoreDensity")]
    private bool UIValue_ShowScoreDensity
    {
        get => Plugin.Config.proHUDConfig.showScoreDensity;
        set
        {
            Plugin.Config.proHUDConfig.showScoreDensity = value;
            InvokePropertyChanged();
            Plugin.Config.Save();
        }
    }

    #endregion

    #region Positions

    [UIValue("UIValue_LeftRightElement")]
    private ProHUDConfig.LeftRightElementPositions UIValue_LeftRightElement
    {
        get => Plugin.Config.proHUDConfig.leftRightElementPositions;
        set
        {
            Plugin.Config.proHUDConfig.leftRightElementPositions = value;
            InvokePropertyChanged();
            Plugin.Config.Save();
        }
    }

    [UIValue("UIValue_LeftRightElementChoices")]
    private List<object> UIValue_LeftRightElementChoices = new List<object> {
        ProHUDConfig.LeftRightElementPositions.AccuracyLeftComboRight,
        ProHUDConfig.LeftRightElementPositions.ComboLeftAccuracyRight,
    };

    [UIAction("UIValue_LeftRightElementFormatter")]
    private string UIValue_LeftRightElementFormatter(ProHUDConfig.LeftRightElementPositions leftRightPosition)
    {
        switch (leftRightPosition)
        {
            case ProHUDConfig.LeftRightElementPositions.AccuracyLeftComboRight:
                return "Accuracy Left";
            case ProHUDConfig.LeftRightElementPositions.ComboLeftAccuracyRight:
                return "Combo Left";
        }
        return "";
    }

    [UIValue("UIValue_TopBottomElement")]
    private ProHUDConfig.TopBottomElementPositions UIValue_TopBottomElement
    {
        get => Plugin.Config.proHUDConfig.topBottomElementPositions;
        set
        {
            Plugin.Config.proHUDConfig.topBottomElementPositions = value;
            InvokePropertyChanged();
            Plugin.Config.Save();
        }
    }

    [UIValue("UIValue_TopBottomElementChoices")]
    private List<object> UIValue_TopBottomElementChoices = new List<object> {
        ProHUDConfig.TopBottomElementPositions.CutStatsTop,
        ProHUDConfig.TopBottomElementPositions.ScoreDensityTop,
        ProHUDConfig.TopBottomElementPositions.BothTopCutStatsLeft,
        ProHUDConfig.TopBottomElementPositions.BothTopScoreDensityLeft,
        ProHUDConfig.TopBottomElementPositions.BothBottomCutStatsLeft,
        ProHUDConfig.TopBottomElementPositions.BothBottomScoreDensityLeft,
    };

    [UIAction("UIValue_TopBottomElementFormatter")]
    private string UIValue_TopBottomElementFormatter(ProHUDConfig.TopBottomElementPositions leftRightPosition)
    {
        switch (leftRightPosition)
        {
            case ProHUDConfig.TopBottomElementPositions.CutStatsTop:
                return "Cut Stats Top";
            case ProHUDConfig.TopBottomElementPositions.ScoreDensityTop:
                return "Score Density Top";
            case ProHUDConfig.TopBottomElementPositions.BothTopCutStatsLeft:
                return "Both Top Cut Stats Left";
            case ProHUDConfig.TopBottomElementPositions.BothTopScoreDensityLeft:
                return "Both Top Score Density Left";
            case ProHUDConfig.TopBottomElementPositions.BothBottomCutStatsLeft:
                return "Both Bottom Cut Stats Left";
            case ProHUDConfig.TopBottomElementPositions.BothBottomScoreDensityLeft:
                return "Both Bottom Score Density Left";
        }
        return "";
    }

    #endregion

    #region AccColors

    [UIValue("UIValue_AccColorsEnabled")]
    private bool UIValue_AccColorsEnabled
    {
        get => Plugin.Config.proHUDConfig.accColorsEnabled;
        set
        {
            Plugin.Config.proHUDConfig.accColorsEnabled = value;
            InvokePropertyChanged();
            Plugin.Config.Save();
        }
    }

    private ProAccColorPointConfig accColorPointConfig;
    
    [UIValue("UIValue_AccColorPointExists")]
    private bool UIValue_AccColorPointExists => accColorPointConfig != null;

    [UIValue("UIValue_AccColorPointNotExists")]
    private bool UIValue_AccColorPointNotExists => accColorPointConfig == null;

    [UIValue("UIValue_PrevAccColorPointExists")]
    private bool UIValue_PrevAccColorPointExists
    {
        get
        {
            foreach(ProAccColorPointConfig accPoint in Plugin.Config.proHUDConfig.accColorPoints)
            {
                if(accPoint.accuracy < UIValue_AccColorPointAcc)
                {
                    return true;
                }
            }
            return false;
        }
    }

    [UIValue("UIValue_NextAccColorPointExists")]
    private bool UIValue_NextAccColorPointExists
    {
        get
        {
            foreach (ProAccColorPointConfig accPoint in Plugin.Config.proHUDConfig.accColorPoints)
            {
                if (accPoint.accuracy > UIValue_AccColorPointAcc)
                {
                    return true;
                }
            }
            return false;
        }
    }

    [UIAction("UIAction_PrevAccColorPoint")]
    private void UIAction_PrevAccColorPoint()
    {
        foreach (ProAccColorPointConfig accPoint in Plugin.Config.proHUDConfig.accColorPoints)
        {
            if (accPoint.accuracy < UIValue_AccColorPointAcc)
            {
                UIValue_AccColorPointAcc = accPoint.accuracy;
                return;
            }
        }
    }

    [UIAction("UIAction_NextAccColorPoint")]
    private void UIAction_NextAccColorPoint()
    {
        ProAccColorPointConfig nextAccPoint = null;
        foreach (ProAccColorPointConfig accPoint in Plugin.Config.proHUDConfig.accColorPoints)
        {
            if (accPoint.accuracy > UIValue_AccColorPointAcc)
            {
                nextAccPoint = accPoint;
            }else
            {
                break;
            }
        }
        if (nextAccPoint != null)
        {
            UIValue_AccColorPointAcc = nextAccPoint.accuracy;
        }
    }

    [UIAction("UIAction_RemoveAccColorPoint")]
    private void UIAction_RemoveAccColorPoint()
    {
        if (accColorPointConfig != null)
        {
            Plugin.Config.proHUDConfig.accColorPoints.Remove(accColorPointConfig);
            Plugin.Config.Validate();
            Plugin.Config.Save();
            UIValue_AccColorPointAcc = UIValue_AccColorPointAcc;
        }
    }

    [UIAction("UIAction_NewAccColorPoint")]
    private void UIAction_NewAccColorPoint()
    {
        if (accColorPointConfig == null)
        {
            Plugin.Config.proHUDConfig.accColorPoints.Add(new ProAccColorPointConfig { accuracy = UIValue_AccColorPointAcc });
            Plugin.Config.Validate();
            Plugin.Config.Save();
            UIValue_AccColorPointAcc = UIValue_AccColorPointAcc;
        }
    }

    private int _UIValue_AccColorPointAcc;
    [UIValue("UIValue_AccColorPointAcc")]
    private int UIValue_AccColorPointAcc
    {
        get => _UIValue_AccColorPointAcc;
        set
        {
            _UIValue_AccColorPointAcc = value;

            accColorPointConfig = null;
            foreach (ProAccColorPointConfig accPoint in Plugin.Config.proHUDConfig.accColorPoints)
            {
                if (accPoint.accuracy == UIValue_AccColorPointAcc)
                {
                    accColorPointConfig = accPoint;
                    break;
                }
            }
            InvokePropertyChanged("UIValue_AccColorPointExists");
            InvokePropertyChanged("UIValue_AccColorPointNotExists");

            InvokePropertyChanged("UIValue_PrevAccColorPointExists");
            InvokePropertyChanged("UIValue_NextAccColorPointExists");

            InvokePropertyChanged("UIValue_AccColorPointAcc");
            InvokePropertyChanged("UIValue_AccColorPointColor");
        }
    }

    [UIValue("UIValue_AccColorPointColor")]
    private Color UIValue_AccColorPointColor
    {
        get => UIValue_AccColorPointExists ? accColorPointConfig.color : Color.white;
        set
        {
            if (UIValue_AccColorPointExists)
            {
                accColorPointConfig.color = value;
                InvokePropertyChanged();
                Plugin.Config.Save();
            }
        }
    }

    #endregion

    #region Accuracy
    [UIValue("UIValue_ShowLeftRightAcc")]
    private bool UIValue_ShowLeftRightAcc
    {
        get => Plugin.Config.proHUDConfig.showLeftRightAcc;
        set
        {
            Plugin.Config.proHUDConfig.showLeftRightAcc = value;
            InvokePropertyChanged();
            Plugin.Config.Save();
        }
    }
    [UIValue("UIValue_AccStyle")]
    private ProHUDConfig.AccStyle UIValue_AccStyle
    {
        get => Plugin.Config.proHUDConfig.accStyle;
        set
        {
            Plugin.Config.proHUDConfig.accStyle = value;
            InvokePropertyChanged();
            Plugin.Config.Save();
        }
    }

    [UIValue("UIValue_AccStyleChoices")]
    private List<object> UIValue_AccStyleChoices = new List<object> {
        ProHUDConfig.AccStyle.None,
        ProHUDConfig.AccStyle.Instant,
        ProHUDConfig.AccStyle.Smart,
        ProHUDConfig.AccStyle.InstantBar,
        ProHUDConfig.AccStyle.SmartBar
    };


    [UIAction("UIAction_AccStyleFormatter")]
    private string UIAction_AccStyleFormatter(ProHUDConfig.AccStyle accStyle)
    {
        switch (accStyle)
        {
            case ProHUDConfig.AccStyle.None:
                return "None";
            case ProHUDConfig.AccStyle.Smart:
                return "Smart";
            case ProHUDConfig.AccStyle.Instant:
                return "Instant";
            case ProHUDConfig.AccStyle.SmartBar:
                return "Smart with Bar";
            case ProHUDConfig.AccStyle.InstantBar:
                return "Instant with Bar";
        }
        return "";
    }

    [UIValue("UIValue_FullComboAccStyle")]
    private ProHUDConfig.FullComboAccStyle UIValue_FullComboAccStyle
    {
        get => Plugin.Config.proHUDConfig.fullComboAccStyle;
        set
        {
            Plugin.Config.proHUDConfig.fullComboAccStyle = value;
            InvokePropertyChanged();
            Plugin.Config.Save();
        }
    }

    [UIValue("UIValue_FullComboAccStyleChoices")]
    private List<object> UIValue_FullComboAccStyleChoices = new List<object> {
        ProHUDConfig.FullComboAccStyle.None,
        ProHUDConfig.FullComboAccStyle.AccOnly,
        ProHUDConfig.FullComboAccStyle.DiffOnly,
        ProHUDConfig.FullComboAccStyle.AccAndDiff
    };


    [UIAction("UIAction_FullComboAccStyleFormatter")]
    private string UIAction_FullComboAccStyleFormatter(ProHUDConfig.FullComboAccStyle fullComboAccStyle)
    {
        switch (fullComboAccStyle)
        {
            case ProHUDConfig.FullComboAccStyle.None:
                return "None";
            case ProHUDConfig.FullComboAccStyle.AccOnly:
                return "Acc Only";
            case ProHUDConfig.FullComboAccStyle.DiffOnly:
                return "Delta Only";
            case ProHUDConfig.FullComboAccStyle.AccAndDiff:
                return "Acc & Delta";
        }
        return "";
    }
    #endregion

    #region Misc.

    [UIValue("UIValue_ComboStyle")]
    private ProHUDConfig.ComboStyle UIValue_ComboStyle
    {
        get => Plugin.Config.proHUDConfig.comboStyle;
        set
        {
            Plugin.Config.proHUDConfig.comboStyle = value;
            InvokePropertyChanged();
            Plugin.Config.Save();
        }
    }

    [UIValue("UIValue_ComboStyleChoices")]
    private List<object> UIValue_ComboStyleChoices = new List<object> {
        ProHUDConfig.ComboStyle.None,
        ProHUDConfig.ComboStyle.Combo,
        ProHUDConfig.ComboStyle.FullCombo,
    };

    [UIAction("UIAction_ComboStyleFormatter")]
    private string UIAction_ComboStyleFormatter(ProHUDConfig.ComboStyle comboStyle)
    {
        switch (comboStyle)
        {
            case ProHUDConfig.ComboStyle.None:
                return "None";
            case ProHUDConfig.ComboStyle.Combo:
                return "Combo";
            case ProHUDConfig.ComboStyle.FullCombo:
                return "Full Combo";
        }
        return "";
    }

    [UIValue("UIValue_ErrorStyle")]
    private ProHUDConfig.ErrorStyle UIValue_ErrorStyle
    {
        get => Plugin.Config.proHUDConfig.errorStyle;
        set
        {
            Plugin.Config.proHUDConfig.errorStyle = value;
            InvokePropertyChanged();
            Plugin.Config.Save();
        }
    }

    [UIValue("UIValue_ErrorStyleChoices")]
    private List<object> UIValue_ErrorStyleChoices = new List<object> { ProHUDConfig.ErrorStyle.None, ProHUDConfig.ErrorStyle.Errors, ProHUDConfig.ErrorStyle.Category };

    
    [UIAction("UIAction_ErrorStyleFormatter")]
    private string UIAction_ErrorStyleFormatter(ProHUDConfig.ErrorStyle errorStyle)
    {
        switch (errorStyle)
        {
            case ProHUDConfig.ErrorStyle.None:
                return "None";
            case ProHUDConfig.ErrorStyle.Errors:
                return "Errors";
            case ProHUDConfig.ErrorStyle.Category:
                return "By Category";
        }
        return "";
    }

    [UIValue("UIValue_ProgressStyle")]
    private ProHUDConfig.ProgressStyle UIValue_ProgressStyle
    {
        get => Plugin.Config.proHUDConfig.songProgressStyle;
        set
        {
            Plugin.Config.proHUDConfig.songProgressStyle = value;
            InvokePropertyChanged();
            Plugin.Config.Save();
        }
    }

    [UIValue("UIValue_ProgressStyleChoices")]
    private List<object> UIValue_ProgressStyleChoices = new List<object> {
        ProHUDConfig.ProgressStyle.None,
        ProHUDConfig.ProgressStyle.TimeLeft,
        ProHUDConfig.ProgressStyle.NotesLeft,
        ProHUDConfig.ProgressStyle.TimeFraction,
        ProHUDConfig.ProgressStyle.NotesFraction
    };

    [UIAction("UIAction_ProgressStyleFormatter")]
    private string UIAction_ProgressStyleFormatter(ProHUDConfig.ProgressStyle errorStyle)
    {
        switch (errorStyle)
        {
            case ProHUDConfig.ProgressStyle.None:
                return "None";
            case ProHUDConfig.ProgressStyle.TimeLeft:
                return "Time Left";
            case ProHUDConfig.ProgressStyle.NotesLeft:
                return "Notes Left";
            case ProHUDConfig.ProgressStyle.TimeFraction:
                return "Time Fraction";
            case ProHUDConfig.ProgressStyle.NotesFraction:
                return "Notes Fraction";
        }
        return "";
    }

    [UIValue("UIValue_HealthBarFullColor")]
    private Color UIValue_HealthBarFullColor
    {
        get => Plugin.Config.proHUDConfig.healthBarFullColor;
        set
        {
            Plugin.Config.proHUDConfig.healthBarFullColor = value;
            InvokePropertyChanged();
            Plugin.Config.Save();
        }
    }

    [UIValue("UIValue_HealthBarFailColor")]
    private Color UIValue_HealthBarFailColor
    {
        get => Plugin.Config.proHUDConfig.healthBarFailColor;
        set
        {
            Plugin.Config.proHUDConfig.healthBarFailColor = value;
            InvokePropertyChanged();
            Plugin.Config.Save();
        }
    }

    #endregion

    #region CutStats

    [UIValue("UIValue_CenterStyle")]
    private ProHUDConfig.CutStatsCenterStyle UIValue_CenterStyle
    {
        get => Plugin.Config.proHUDConfig.cutStatsCenterStyle;
        set
        {
            Plugin.Config.proHUDConfig.cutStatsCenterStyle = value;
            InvokePropertyChanged();
            Plugin.Config.Save();
        }
    }

    [UIValue("UIValue_CenterStyleChoices")]
    private List<object> UIValue_CenterStyleChoices = new List<object>
    {
        ProHUDConfig.CutStatsCenterStyle.HistogramTop,
        ProHUDConfig.CutStatsCenterStyle.HistogramBottom,
        ProHUDConfig.CutStatsCenterStyle.HistogramOnly,
        ProHUDConfig.CutStatsCenterStyle.CutOnly,
        ProHUDConfig.CutStatsCenterStyle.None,
    };
    [UIAction("UIValue_CenterStyleFormatter")]
    private string UIValue_CenterStyleFormatter(ProHUDConfig.CutStatsCenterStyle cutStatsCenterStyle)
    {
        switch(cutStatsCenterStyle)
        {
            case ProHUDConfig.CutStatsCenterStyle.HistogramTop:
                return "Histogram On Top";
            case ProHUDConfig.CutStatsCenterStyle.HistogramBottom:
                return "Cut Score On Top";
            case ProHUDConfig.CutStatsCenterStyle.HistogramOnly:
                return "Histogram Only";
            case ProHUDConfig.CutStatsCenterStyle.CutOnly:
                return "Cut Score Only";
            case ProHUDConfig.CutStatsCenterStyle.None:
                return "None";
        }
        return "";
    }


    [UIValue("UIValue_TimeDependenceStyle")]
    private ProHUDConfig.TimeDependenceStyle UIValue_TimeDependenceStyle
    {
        get => Plugin.Config.proHUDConfig.timeDependenceStyle;
        set
        {
            Plugin.Config.proHUDConfig.timeDependenceStyle = value;
            InvokePropertyChanged();
            Plugin.Config.Save();
        }
    }

    [UIValue("UIValue_TimeDependenceStyleChoices")]
    private List<object> UIValue_TimeDependenceStyleChoices = new List<object> {
        ProHUDConfig.TimeDependenceStyle.None,
        ProHUDConfig.TimeDependenceStyle.Angle,
        ProHUDConfig.TimeDependenceStyle.Percent
    };


    [UIAction("UIAction_TimeDependenceStyleFormatter")]
    private string UIAction_TimeDependenceStyleFormatter(ProHUDConfig.TimeDependenceStyle timeDependenceStyle)
    {
        switch (timeDependenceStyle)
        {
            case ProHUDConfig.TimeDependenceStyle.None:
                return "None";
            case ProHUDConfig.TimeDependenceStyle.Angle:
                return "Angle";
            case ProHUDConfig.TimeDependenceStyle.Percent:
                return "Percent";
        }
        return "";
    }

    [UIValue("UIValue_CenterDistanceStyle")]
    private ProHUDConfig.CenterDistanceStyle UIValue_CenterDistanceStyle
    {
        get => Plugin.Config.proHUDConfig.centerDistanceStyle;
        set
        {
            Plugin.Config.proHUDConfig.centerDistanceStyle = value;
            InvokePropertyChanged();
            Plugin.Config.Save();
        }
    }

    [UIValue("UIValue_CenterDistanceStyleChoices")]
    private List<object> UIValue_CenterDistanceStyleChoices = new List<object> {
        ProHUDConfig.CenterDistanceStyle.None,
        ProHUDConfig.CenterDistanceStyle.Score,
        ProHUDConfig.CenterDistanceStyle.Millimeters
    };


    [UIAction("UIAction_CenterDistanceStyleFormatter")]
    private string UIAction_CenterDistanceStyleFormatter(ProHUDConfig.CenterDistanceStyle centerDistanceStyle)
    {
        switch (centerDistanceStyle)
        {
            case ProHUDConfig.CenterDistanceStyle.None:
                return "None";
            case ProHUDConfig.CenterDistanceStyle.Score:
                return "Score";
            case ProHUDConfig.CenterDistanceStyle.Millimeters:
                return "Millimeters";
        }
        return "";
    }

    [UIValue("UIValue_SwingStyle")]
    private ProHUDConfig.SwingStyle UIValue_SwingStyle
    {
        get => Plugin.Config.proHUDConfig.swingStyle;
        set
        {
            Plugin.Config.proHUDConfig.swingStyle = value;
            InvokePropertyChanged();
            Plugin.Config.Save();
        }
    }

    [UIValue("UIValue_SwingStyleChoices")]
    private List<object> UIValue_SwingStyleChoices = new List<object> {
        ProHUDConfig.SwingStyle.None,
        ProHUDConfig.SwingStyle.SeperateScore,
        ProHUDConfig.SwingStyle.CombindedScore,
        ProHUDConfig.SwingStyle.SwingRating,
        ProHUDConfig.SwingStyle.SeperateUnderswingDamage,
        ProHUDConfig.SwingStyle.CombinedUnderswingDamage,
    };

    [UIAction("UIAction_SwingStyleFormatter")]
    private string UIAction_SwingStyleFormatter(ProHUDConfig.SwingStyle swingStyle)
    {
        switch (swingStyle)
        {
            case ProHUDConfig.SwingStyle.None:
                return "None";
            case ProHUDConfig.SwingStyle.SeperateScore:
                return "Seperate Score";
            case ProHUDConfig.SwingStyle.CombindedScore:
                return "Combined Score";
            case ProHUDConfig.SwingStyle.SwingRating:
                return "Swing Rating";
            case ProHUDConfig.SwingStyle.SeperateUnderswingDamage:
                return "Seperate Dmg.";
            case ProHUDConfig.SwingStyle.CombinedUnderswingDamage:
                return "Combined Dmg.";
        }
        return "";
    }

    #endregion

}