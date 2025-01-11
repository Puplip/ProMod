using System;
using System.Collections.Generic;
using UnityEngine;
using BeatSaberMarkupLanguage.Attributes;
using Zenject;

namespace ProMod.UI;


internal class ProCutScoresTabUI : ProUI, IInitializable, IDisposable
{
    public void Initialize()
    {
        UIValue_PointScore = 0;
    }

    public void Dispose()
    {

    }

    [UIValue("UIValue_CutScoresEnabled")]
    private bool UIValue_CutScoresEnabled
    {
        get => Plugin.Config.cutScores.cutScoresEnabled;
        set
        {
            Plugin.Config.cutScores.cutScoresEnabled = value;
            InvokePropertyChanged();
            Plugin.Config.Save();
        }
    }

    [UIValue("UIValue_AbnormalScoreSize")]
    private int UIValue_AbnormalScoreSize
    {
        get => Plugin.Config.cutScores.abnormalNoteSize;
        set
        {
            Plugin.Config.cutScores.abnormalNoteSize = value;
            InvokePropertyChanged();
            Plugin.Config.Save();
        }
    }

    private ProCutScorePointConfig cutScorePointConfig;

    [UIValue("UIValue_CutScorePointExists")]
    private bool UIValue_CutScorePointExists => cutScorePointConfig != null;

    [UIValue("UIValue_CutScorePointNotExists")]
    private bool UIValue_CutScorePointNotExists => cutScorePointConfig == null;


    [UIValue("UIValue_NextCutScorePointExists")]
    private bool UIValue_NextCutScorePointExists
    {
        get
        {
            foreach(ProCutScorePointConfig configPoint in Plugin.Config.cutScores.cutScorePoints)
            {
                if (configPoint.score > _UIValue_PointScore)
                {
                    return true;
                }
            }
            return false;
        }
    }

    [UIValue("UIValue_PrevCutScorePointExists")]
    private bool UIValue_PrevCutScorePointExists
    {
        get
        {
            foreach (ProCutScorePointConfig configPoint in Plugin.Config.cutScores.cutScorePoints)
            {
                if (configPoint.score < _UIValue_PointScore)
                {
                    return true;
                }
            }
            return false;
        }
    }

    [UIAction("UIAction_PrevCutScorePoint")]
    private void UIAction_PrevCutScorePoint()
    {
        foreach (ProCutScorePointConfig configPoint in Plugin.Config.cutScores.cutScorePoints)
        {
            if (configPoint.score < _UIValue_PointScore)
            {
                UIValue_PointScore = configPoint.score;
                break;
            }
        }
    }

    [UIAction("UIAction_NextCutScorePoint")]
    private void UIAction_NextCutScorePoint()
    {
        ProCutScorePointConfig nextConfigPoint = null;
        foreach (ProCutScorePointConfig configPoint in Plugin.Config.cutScores.cutScorePoints)
        {
            if (configPoint.score > _UIValue_PointScore)
            {
                nextConfigPoint = configPoint;
            }
            else
            {
                break;
            }
        }
        if (nextConfigPoint != null)
        {
            UIValue_PointScore = nextConfigPoint.score;
        }
    }

    [UIAction("UIAction_RemoveCutScorePoint")]
    private void UIAction_RemoveCutScorePoint()
    {
        if(UIValue_CutScorePointExists)
        {
            Plugin.Config.cutScores.cutScorePoints.Remove(cutScorePointConfig);
            Plugin.Config.Validate();
            Plugin.Config.Save();
            UIValue_PointScore = UIValue_PointScore;
        }
    }

    [UIAction("UIAction_NewCutScorePoint")]
    private void UIAction_NewCutScorePoint()
    {
        if(UIValue_CutScorePointExists)
        {
            Plugin.Config.cutScores.cutScorePoints.Add(new ProCutScorePointConfig { score = UIValue_PointScore });
            Plugin.Config.Validate();
            Plugin.Config.Save();
            UIValue_PointScore = UIValue_PointScore;
        }
    }


    private int _UIValue_PointScore;
    [UIValue("UIValue_PointScore")]
    private int UIValue_PointScore
    {
        get => _UIValue_PointScore;
        set
        {
            _UIValue_PointScore = value;

            cutScorePointConfig = null;
            foreach (ProCutScorePointConfig pointConfig in Plugin.Config.cutScores.cutScorePoints)
            {
                if(pointConfig.score == _UIValue_PointScore)
                {
                    cutScorePointConfig = pointConfig;
                    break;
                }
            }
            InvokePropertyChanged("UIValue_CutScorePointExists");
            InvokePropertyChanged("UIValue_CutScorePointNotExists");

            InvokePropertyChanged("UIValue_NextCutScorePointExists");
            InvokePropertyChanged("UIValue_PrevCutScorePointExists");

            InvokePropertyChanged("UIValue_PointScore");

            InvokePropertyChanged("UIValue_PointDisplayStyle");
            InvokePropertyChanged("UIValue_PointScoreSize");
            InvokePropertyChanged("UIValue_PointColor");

            InvokePropertyChanged("UIValue_PointCustomStringEnabled");
            InvokePropertyChanged("UIValue_PointCustomString");
        }
    }

    //[UIValue("UIValue_PointDisplayStyle")]
    //private ProCutScorePointConfig.DisplayStyle UIValue_PointDisplayStyle
    //{
    //    get => UIValue_CutScorePointExists ? cutScorePointConfig.displayStyle : ProCutScorePointConfig.DisplayStyle.CutScore;
    //    set
    //    {
    //        if (UIValue_CutScorePointExists)
    //        {
    //            cutScorePointConfig.displayStyle = value;
    //            InvokePropertyChanged();
    //            InvokePropertyChanged("UIValue_PointCustomStringEnabled");
    //            Plugin.Config.Save();
    //        }
    //    }
    //}

    //[UIValue("UIValue_PointDisplayStyleChoices")]
    //private List<object> UIValue_PointDisplayStyleChoices = new List<object> {
    //    ProCutScorePointConfig.DisplayStyle.CutScore,
    //    //ProCutScorePointConfig.DisplayStyle.LetterRank,
    //    //ProCutScorePointConfig.DisplayStyle.CustomString
    //};

    //[UIAction("UIValue_PointDisplayStyleFormatter")]
    //private string UIValue_PointDisplayStyleFormatter(ProCutScorePointConfig.DisplayStyle comboStyle)
    //{
    //    switch (comboStyle)
    //    {
    //        case ProCutScorePointConfig.DisplayStyle.CutScore:
    //            return "Cut Score";
    //        //case ProCutScorePointConfig.DisplayStyle.LetterRank:
    //        //    return "Letter Rank";
    //        //case ProCutScorePointConfig.DisplayStyle.CustomString:
    //        //    return "Custom String";
    //    }
    //    return "";
    //}

    [UIValue("UIValue_PointScoreSize")]
    private int UIValue_PointScoreSize
    {
        get => UIValue_CutScorePointExists ? cutScorePointConfig.size : 0;
        set
        {
            if (cutScorePointConfig != null)
            {
                cutScorePointConfig.size = value;
                InvokePropertyChanged();
                Plugin.Config.Save();
            }
        }
    }

    [UIValue("UIValue_PointColor")]
    private Color UIValue_PointColor
    {
        get => UIValue_CutScorePointExists ? cutScorePointConfig.color : Color.white;
        set
        {
            if (cutScorePointConfig != null)
            {
                cutScorePointConfig.color = value;
                InvokePropertyChanged();
                Plugin.Config.Save();
            }
        }
    }

    //[UIValue("UIValue_PointCustomStringEnabled")]
    //private bool UIValue_PointCustomStringEnabled => cutScorePointConfig != null && cutScorePointConfig.displayStyle == ProCutScorePointConfig.DisplayStyle.CustomString;


    //[UIValue("UIValue_PointCustomString")]
    //private string UIValue_PointCustomString
    //{
    //    get => UIValue_CutScorePointExists ? cutScorePointConfig.customString : "";
    //    set
    //    {
    //        if (cutScorePointConfig != null)
    //        {
    //            cutScorePointConfig.customString = value;
    //            InvokePropertyChanged();
    //            Plugin.Config.Save();
    //        }
    //    }
    //}

    private void UpdateDisplay()
    {
        
    }
}

