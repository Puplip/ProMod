using System;
using System.Collections.Generic;
using UnityEngine;
using BeatSaberMarkupLanguage.Attributes;
using Zenject;
using System.ComponentModel;
using HMUI;

namespace ProMod.UI;

internal class ProGameplayTabUI : ProUI, IInitializable, IDisposable
{
    public void Initialize()
    {

    }

    public void Dispose()
    {

    }

    [UIValue("UIValue_BombColorEnabled")]
    public bool UIValue_BombColorEnabled
    {
        get => Plugin.Config.bombColorEnabled;
        set
        {
            Plugin.Config.bombColorEnabled = value;
            InvokePropertyChanged();
            Plugin.Config.Save();
        }
    }

    [UIValue("UIValue_BombColor")]
    public Color UIValue_BombColor
    {
        get => Plugin.Config.bombColor;
        set
        {
            Plugin.Config.bombColor = value;
            InvokePropertyChanged();
            Plugin.Config.Save();
        }
    }

    [UIValue("UIValue_BombColorMultiplier")]
    public float UIValue_BombColorMultiplier
    {
        get => Plugin.Config.bombColorMultiplier;
        set
        {
            Plugin.Config.bombColorMultiplier = value;
            InvokePropertyChanged();
            Plugin.Config.Save();
        }
    }
}