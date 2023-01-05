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
    internal class ProTab : MonoBehaviour
    {

        [Inject]
        private PlayerDataModel _playerDataModel;
        [Inject]
        private GameplaySetupViewController _gameplaySetupViewController;

        [UIParams]
        private BSMLParserParams _parserParams;

        private void Awake() {
            BeatSaberMarkupLanguage.GameplaySetup.GameplaySetup.instance.AddTab("ProMod", "ProMod.Resources.TabUI.bsml", this);
            gameObject.SetActive(false);
            Plugin.Log.Info("Bound ProTab to PlayerHeight");
            ProHeightPatch.heightValueChange += ProHeightPatch_heightValueChange;


        }

        private float _playerHeight;
        private void ProHeightPatch_heightValueChange(float height)
        {
            Plugin.Log.Info("Called ProTab ProHeight_heightValueChange");
            _playerHeight = height;
            _parserParams.EmitEvent("Event_PlayerHeight_Get");
        }

        [UIValue("UIValue_HeightGuideMenuEnabled")]
        private bool UIValue_HeightGuideMenuEnabled
        {
            get => Plugin.Config.HeightGuideMenuEnabled;
            set
            {
                Plugin.Config.HeightGuideMenuEnabled = value;
                Plugin.Config.Save();
            }
        }

        [UIValue("UIValue_HeightGuideGameplayEnabled")]
        private bool UIValue_HeightGuideGameplayEnabled
        {
            get => Plugin.Config.HeightGuideGameplayEnabled;
            set
            {
                Plugin.Config.HeightGuideGameplayEnabled = value;
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
                _gameplaySetupViewController.Init();
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
    }
}
