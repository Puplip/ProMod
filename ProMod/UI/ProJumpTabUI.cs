using BeatSaberMarkupLanguage.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace ProMod.UI;

internal class ProJumpTabUI : ProUI, IInitializable, IDisposable
{

    [Inject] private StandardLevelDetailViewController standardLevelDetailViewController;
    [Inject] private GameplaySetupViewController gameplaySetupViewController;

    private float _njs;
    private float njs
    {
        get => _njs;
        set
        {
            _njs = value;
            UpdateDisplay();
        }
    }

    private float _bpm;
    private float bpm
    {
        get => _bpm;
        set
        {
            _bpm = value;
            UpdateDisplay();
        }
    }

    public void Initialize()
    {
        standardLevelDetailViewController.didChangeContentEvent += StandardLevelDetailViewController_didChangeContentEvent;
        standardLevelDetailViewController.didChangeDifficultyBeatmapEvent += StandardLevelDetailViewController_didChangeDifficultyBeatmapEvent;
        if (standardLevelDetailViewController.beatmapKey.IsValid() && standardLevelDetailViewController.beatmapLevel.beatmapBasicData.TryGetValue((standardLevelDetailViewController.beatmapKey.beatmapCharacteristic, standardLevelDetailViewController.beatmapKey.difficulty), out BeatmapBasicData value))
        {
            njs = value.noteJumpMovementSpeed;
            bpm = standardLevelDetailViewController.beatmapLevel.beatsPerMinute;
        }
        UpdateDisplay();
    }

    private void StandardLevelDetailViewController_didChangeContentEvent(StandardLevelDetailViewController standardLevelDetailViewController, StandardLevelDetailViewController.ContentType arg2)
    {
        if(standardLevelDetailViewController?.beatmapLevel?.beatmapBasicData == null)
        {
            return;
        }

        if (standardLevelDetailViewController.beatmapLevel.beatmapBasicData.TryGetValue((standardLevelDetailViewController.beatmapKey.beatmapCharacteristic, standardLevelDetailViewController.beatmapKey.difficulty), out BeatmapBasicData value))
        {
            njs = value.noteJumpMovementSpeed;
            bpm = standardLevelDetailViewController.beatmapLevel.beatsPerMinute;
        }
    }


    private void StandardLevelDetailViewController_didChangeDifficultyBeatmapEvent(StandardLevelDetailViewController standardLevelDetailViewController)
    {
        if (standardLevelDetailViewController?.beatmapLevel?.beatmapBasicData == null)
        {
            return;
        }

        if (standardLevelDetailViewController.beatmapLevel.beatmapBasicData.TryGetValue((standardLevelDetailViewController.beatmapKey.beatmapCharacteristic, standardLevelDetailViewController.beatmapKey.difficulty), out BeatmapBasicData value))
        {
            njs = value.noteJumpMovementSpeed;
            bpm = standardLevelDetailViewController.beatmapLevel.beatsPerMinute;
        }
    }

    public void Dispose()
    {
        standardLevelDetailViewController.didChangeDifficultyBeatmapEvent -= StandardLevelDetailViewController_didChangeDifficultyBeatmapEvent;
    }

    [UIValue(nameof(UIValue_JumpSetting))]
    private ProJumpSetting UIValue_JumpSetting
    {
        get => Plugin.Config.jumpSetting;
        set
        {
            Plugin.Config.jumpSetting = value;
            Plugin.Config.Save();
            UpdateDisplay();
        }
    }

    [UIValue(nameof(UIValue_JumpSettingOptions))]
    private List<object> UIValue_JumpSettingOptions = new List<object>(Enum.GetValues(typeof(ProJumpSetting)).OfType<ProJumpSetting>().Cast<object>());

    [UIAction(nameof(UIAction_FormatJumpSettingOption))]
    private string UIAction_FormatJumpSettingOption(ProJumpSetting jumpSetting)
    {
        return jumpSetting.NameWithSpaces();
    }

    [UIValue(nameof(UIValue_ReactionTime))]
    private float UIValue_ReactionTime
    {
        get => Plugin.Config.reactionTime;
        set
        {
            Plugin.Config.reactionTime = value;
            Plugin.Config.Save();
            UpdateDisplay();
        }
    }

    [UIValue(nameof(UIValue_CalcReactionTime))]
    private string UIValue_CalcReactionTime  {
        get
        {
            if(!float.IsFinite(njs) || !float.IsFinite(bpm))
            {
                return "Map not found";
            }

            if (Plugin.Config.jumpSetting == ProJumpSetting.HouseSpecial)
            {
                return (ProJumpPatch.HouseSpecial(bpm, njs) * 1000f).ToString("f0") + "ms";
            }

            return ProUtil.CalculateReactionTimeMilliseconds(Plugin.Config.jumpDistance, njs).ToString("f0") + "ms";
        }
    }

    [UIValue(nameof(UIValue_JumpDistance))]
    private float UIValue_JumpDistance
    {
        get => Plugin.Config.jumpDistance;
        set
        {
            Plugin.Config.jumpDistance = value;
            Plugin.Config.Save();
            UpdateDisplay();
        }
    }

    [UIValue(nameof(UIValue_CalcJumpDistance))]
    private string UIValue_CalcJumpDistance
    {
        get
        {
            if (!float.IsFinite(njs) || !float.IsFinite(bpm))
            {
                return "Map not found";
            }

            if(Plugin.Config.jumpSetting == ProJumpSetting.HouseSpecial)
            {
                return ProUtil.CalculateJumpDistanceSeconds(ProJumpPatch.HouseSpecial(bpm, njs), njs).ToString("f2") + "m";
            }

            return ProUtil.CalculateJumpDistanceMilliseconds(Plugin.Config.reactionTime, njs).ToString("f2") + "m";
        }
    }

    [UIAction(nameof(UIAction_FormatReactionTime))]
    private string UIAction_FormatReactionTime(float value)
    {
        return $"{value:f0}ms";
    }

    [UIAction(nameof(UIAction_FormatJumpDistance))]
    private string UIAction_FormatJumpDistance(float value)
    {
        return $"{value:F2}m";
    }


    [UIValue(nameof(UIValue_EditReactionTime))]
    private bool UIValue_EditReactionTime => Plugin.Config.jumpSetting switch
    {
        ProJumpSetting.ReactionTime => true,
        ProJumpSetting.HouseSpecial => true,
        _ => false
    };

    [UIValue(nameof(UIValue_EditJumpDistance))]
    private bool UIValue_EditJumpDistance => Plugin.Config.jumpSetting switch
    {
        ProJumpSetting.JumpDistance => true,
        ProJumpSetting.HouseSpecial => true,
        _ => false
    };

    [UIValue(nameof(UIValue_ViewReactionTime))] private bool UIValue_ViewReactionTime => Plugin.Config.jumpSetting == ProJumpSetting.JumpDistance;
    [UIValue(nameof(UIValue_ViewJumpDistance))] private bool UIValue_ViewJumpDistance => Plugin.Config.jumpSetting == ProJumpSetting.ReactionTime;

    [UIValue(nameof(UIValue_ShowFractions))] private bool UIValue_ShowFractions => Plugin.Config.jumpSetting == ProJumpSetting.JumpDistance || Plugin.Config.jumpSetting == ProJumpSetting.ReactionTime;
    [UIValue(nameof(UIValue_HideFractions))] private bool UIValue_HideFractions => !UIValue_ShowFractions;

    private void SetBeatFraction(float fraction)
    {

        if(!(float.IsFinite(njs) && float.IsFinite(bpm)))
        {
            return;
        }

        float reactionTime = 60f / bpm * fraction;

        switch (Plugin.Config.jumpSetting)
        {
            case ProJumpSetting.JumpDistance:
                Plugin.Config.jumpDistance = ProUtil.CalculateJumpDistanceSeconds(reactionTime, njs);
                break;
            case ProJumpSetting.ReactionTime:
                Plugin.Config.reactionTime = reactionTime * 1000f;
                break;
            case ProJumpSetting.HouseSpecial:
                Plugin.Config.jumpDistance = ProUtil.CalculateJumpDistanceSeconds(reactionTime, njs);
                Plugin.Config.reactionTime = reactionTime * 1000f;
                break;
            default:
                return;
        }

        Plugin.Config.Save();
        UpdateDisplay();
    }

    [UIAction(nameof(UIAction_SelectBeat_21))] private void UIAction_SelectBeat_21() => SetBeatFraction(2f / 1f);
    [UIAction(nameof(UIAction_SelectBeat_74))] private void UIAction_SelectBeat_74() => SetBeatFraction(7f / 4f);
    [UIAction(nameof(UIAction_SelectBeat_53))] private void UIAction_SelectBeat_53() => SetBeatFraction(5f / 3f);
    [UIAction(nameof(UIAction_SelectBeat_32))] private void UIAction_SelectBeat_32() => SetBeatFraction(3f / 2f);
    [UIAction(nameof(UIAction_SelectBeat_43))] private void UIAction_SelectBeat_43() => SetBeatFraction(4f / 3f);
    [UIAction(nameof(UIAction_SelectBeat_54))] private void UIAction_SelectBeat_54() => SetBeatFraction(5f / 4f);
    [UIAction(nameof(UIAction_SelectBeat_11))] private void UIAction_SelectBeat_11() => SetBeatFraction(1f / 1f);
    [UIAction(nameof(UIAction_SelectBeat_34))] private void UIAction_SelectBeat_34() => SetBeatFraction(3f / 4f);
    [UIAction(nameof(UIAction_SelectBeat_23))] private void UIAction_SelectBeat_23() => SetBeatFraction(2f / 3f);
    [UIAction(nameof(UIAction_SelectBeat_12))] private void UIAction_SelectBeat_12() => SetBeatFraction(1f / 2f);
    [UIAction(nameof(UIAction_SelectBeat_13))] private void UIAction_SelectBeat_13() => SetBeatFraction(1f / 3f);
    [UIAction(nameof(UIAction_SelectBeat_14))] private void UIAction_SelectBeat_14() => SetBeatFraction(1f / 4f);

    internal void UpdateDisplay()
    {
        InvokePropertyChanged(nameof(UIValue_ReactionTime));
        InvokePropertyChanged(nameof(UIValue_JumpDistance));

        InvokePropertyChanged(nameof(UIValue_CalcReactionTime));
        InvokePropertyChanged(nameof(UIValue_CalcJumpDistance));

        InvokePropertyChanged(nameof(UIValue_EditReactionTime));
        InvokePropertyChanged(nameof(UIValue_EditJumpDistance));

        InvokePropertyChanged(nameof(UIValue_ViewReactionTime));
        InvokePropertyChanged(nameof(UIValue_ViewJumpDistance));

        InvokePropertyChanged(nameof(UIValue_HideFractions));
        InvokePropertyChanged(nameof(UIValue_ShowFractions));
    }

}
