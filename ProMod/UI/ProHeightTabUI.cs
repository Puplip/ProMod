using System;
using System.Collections.Generic;
using UnityEngine;
using BeatSaberMarkupLanguage.Attributes;
using Zenject;
using System.ComponentModel;
using HMUI;
using System.Reflection;

namespace ProMod.UI;

internal class ProHeightTabUI : ProUI, IInitializable, IDisposable 
{
    [Inject]
    private PlayerDataModel _playerDataModel;
    [Inject]
    private GameplaySetupViewController _gameplaySetupViewController;

    public void Initialize()
    {
        ProHeightPatch.heightValueChange += ProHeightPatch_heightValueChange;
    }

    public void Dispose()
    {
        ProHeightPatch.heightValueChange -= ProHeightPatch_heightValueChange;
    }

    private float _playerHeight = 1.4f;
    private void ProHeightPatch_heightValueChange(float height)
    {
        _playerHeight = height;
        InvokePropertyChanged("UIValue_PlayerHeight");
    }

    [UIValue("UIValue_HeightGuideEnabled")]
    private bool UIValue_HeightGuideEnabled
    {
        get => Plugin.Config.heightGuideEnabled;
        set
        {
            Plugin.Config.heightGuideEnabled = value;
            Plugin.Config.Save();
        }
    }

    private static readonly MethodInfo GameplaySetupViewController_Init = typeof(GameplaySetupViewController).GetMethod("Init", BindingFlags.Instance | BindingFlags.NonPublic);
    [UIValue("UIValue_PlayerHeight")]
    private float UIValue_PlayerHeight
    {
        get => _playerHeight * 100.0f;
        set
        {
            if (_playerDataModel != null)
            {
                _playerDataModel.playerData.SetPlayerSpecificSettings(_playerDataModel.playerData.playerSpecificSettings.CopyWith(playerHeight: value / 100.0f));
            }

            if (_gameplaySetupViewController != null)
            {
                GameplaySetupViewController_Init.Invoke(_gameplaySetupViewController, new object[] { });
            }
        }
    }

    [UIAction("UIAction_FormatPlayerHeight")]
    private string UIAction_FormatPlayerHeight(float value)
    {
        return value.ToString("F0") + "cm";
    }
}
