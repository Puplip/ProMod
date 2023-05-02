using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using BeatSaberMarkupLanguage.Components.Settings;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Parser;
using Zenject;
using TMPro;

namespace ProMod
{
    public class ProTabHooks : IInitializable
    {

        [Inject]
        private PlayerDataModel _playerDataModel;

        [Inject]
        private GameplaySetupViewController _gameplaySetupViewController;

        public void Initialize()
        {
            ProTab.Instance._playerDataModel = _playerDataModel;
            ProTab.Instance._gameplaySetupViewController = _gameplaySetupViewController;
        }
    }
    internal class ProTab
    {
        internal static ProTab Instance;

        internal PlayerDataModel _playerDataModel;

        internal GameplaySetupViewController _gameplaySetupViewController;

        [UIParams]
        private BSMLParserParams _parserParams;

        internal static void Init()
        {
            if(Instance != null) { return; }
            Instance = new ProTab();

            BeatSaberMarkupLanguage.GameplaySetup.GameplaySetup.instance.AddTab("ProMod", "ProMod.Resources.TabUI.bsml", Instance);
            ProHeightPatch.heightValueChange += Instance.ProHeightPatch_heightValueChange;
        }

        private float _playerHeight;
        private void ProHeightPatch_heightValueChange(float height)
        {
            Plugin.Log.Info("ProHeightPatch_heightValueChange");
            _playerHeight = height;
            if(_parserParams != null)
            {
                _parserParams.EmitEvent("Event_PlayerHeight_Get");
            } else
            {
                Plugin.Log.Info("_parserParams is null");
            }
        }

        [UIValue("UIValue_HeightGuideEnabled")]
        private bool UIValue_HeightGuideEnabled
        {
            get => Plugin.Config.HeightGuideEnabled;
            set
            {
                Plugin.Config.HeightGuideEnabled = value;
                Plugin.Config.Save();
            }
        }

        [UIValue("UIValue_PlayerHeight")]
        private float UIValue_PlayerHeight
        {
            get => _playerHeight * 100.0f;
            set
            {
                _playerDataModel.playerData.SetPlayerSpecificSettings(_playerDataModel.playerData.playerSpecificSettings.CopyWith(playerHeight: value / 100.0f));
                
                if(_gameplaySetupViewController != null)
                {
                    _gameplaySetupViewController.Init();
                }
            }
        }

        [UIValue("UIValue_RTCurveEnabled")]
        private bool UIValue_RTCurveEnabled
        {
            get => Plugin.Config.RTCurveEnabled;
            set
            {
                Plugin.Config.RTCurveEnabled = value;
                Plugin.Config.Save();
            }
        }

        [UIValue("UIValue_FixedRTEnabled")]
        private bool UIValue_FixedRTEnabled
        {
            get => Plugin.Config.FixedRTEnabled;
            set
            {
                Plugin.Config.FixedRTEnabled = value;
                Plugin.Config.Save();
            }
        }

        [UIValue("UIValue_FixedRTValue")]
        private float UIValue_FixedRTValue
        {
            get => Plugin.Config.FixedRTValue;
            set
            {
                Plugin.Config.FixedRTValue = value;
                Plugin.Config.Save();
            }
        }

        [UIValue("UIValue_ProStatsEnabled")]
        private bool UIValue_ProStatsEnabled
        {
            get => Plugin.Config.ProStatsEnabled;
            set
            {
                Plugin.Config.ProStatsEnabled = value;
                Plugin.Config.Save();
            }
        }

        [UIValue("UIValue_StatColorsEnabled")]
        private bool UIValue_StatColorsEnabled
        {
            get => Plugin.Config.StatColorsEnabled;
            set
            {
                Plugin.Config.StatColorsEnabled = value;
                Plugin.Config.Save();
            }
        }

        [UIValue("UIValue_CutScoresEnabled")]
        private bool UIValue_CutScoresEnabled
        {
            get => Plugin.Config.CutScoresEnabled;
            set
            {
                Plugin.Config.CutScoresEnabled = value;
                Plugin.Config.Save();
            }
        }

        [UIValue("UIValue_GameplayEffectsDisabled")]
        private bool UIValue_GameplayEffectsDisabled
        {
            get => Plugin.Config.GameplayEffectsDisabled;
            set
            {
                Plugin.Config.GameplayEffectsDisabled = value;
                Plugin.Config.Save();
            }
        }

        [UIValue("UIValue_DisableEnvironmentInHMD")]
        private bool UIValue_HeightGuideGameplayEnabled
        {
            get => Plugin.Config.DisableEnvironmentInHMD;
            set
            {
                Plugin.Config.DisableEnvironmentInHMD = value;
                Plugin.Config.Save();
            }
        }
    }
}
