using System;
using BeatSaberMarkupLanguage.Attributes;
using Zenject;
using HMUI;

namespace ProMod.UI;

internal class ProTabUI : ProUI, IInitializable, IDisposable
{

    public void Initialize()
    {
        BeatSaberMarkupLanguage.GameplaySetup.GameplaySetup.Instance.AddTab("ProMod", "ProMod.Resources.TabUI.bsml",this,BeatSaberMarkupLanguage.GameplaySetup.MenuType.All);
    }

    public void Dispose()
    {
        BeatSaberMarkupLanguage.GameplaySetup.GameplaySetup.Instance.RemoveTab("ProMod");
    }

    [UIValue(nameof(UIValue_ProHeightTabUI)), Inject]
    private ProHeightTabUI UIValue_ProHeightTabUI;

    [UIValue(nameof(UIValue_ProJumpTabUI)), Inject]
    private ProJumpTabUI UIValue_ProJumpTabUI;

    [UIValue(nameof(UIValue_ProHUDTabUI)), Inject]
    private ProHUDTabUI UIValue_ProHUDTabUI;

    [UIValue(nameof(UIValue_ProDisplayTabUI)), Inject]
    private ProDisplayTabUI UIValue_ProDisplayTabUI;

    [UIValue(nameof(UIValue_ProCutScoresTabUI)), Inject]
    private ProCutScoresTabUI UIValue_ProCutScoresTabUI;

    [UIValue(nameof(UIValue_ProGameplayTabUI)), Inject]
    private ProGameplayTabUI UIValue_ProGameplayTabUI;

    [UIAction("UIAction_SelectTab")]
    private void UIAction_SelectTab(SegmentedControl segmentedControl, int tabIndex)
    {
    }

}
