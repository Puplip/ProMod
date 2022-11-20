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

namespace ProMod
{
    internal class ProTab : MonoBehaviour
    {
        [Inject]
        private PlayerDataModel _playerDataModel;
        [Inject]
        private ProHUD _proHUD;
        [Inject]
        private GameplaySetupViewController _gameplaySetupViewController;
        [Inject]
        private StandardLevelDetailViewController _standardLevelDetailViewController;

        [UIParams]
        private BSMLParserParams _parserParams;
        private void Awake() {
            BeatSaberMarkupLanguage.GameplaySetup.GameplaySetup.instance.AddTab("ProMod", "ProMod.Resources.TabUI.bsml", this);
            gameObject.SetActive(false);
            Plugin.Log.Info("Bound ProTab to PlayerHeight");
            ProHeight.heightValueChange += ProHeight_heightValueChange;
            BSMLParserParams parserParams;
        }

        private float _playerHeight;
        private void ProHeight_heightValueChange(float height)
        {
            Plugin.Log.Info("Called ProTab ProHeight_heightValueChange");
            _playerHeight = height;
            _parserParams.EmitEvent("Event_PlayerHeight_Get");
            
        }

        [UIValue("UIValue_ShowHUD")]
        private bool UIValue_ShowHUD
        {
            get
            {
                return Plugin.Config.ShowHeightGuide;
            }
            set
            {
                _proHUD.gameObject.SetActive(value);
                Plugin.Config.ShowHeightGuide = value;
                Plugin.Config.Changed();
            }
        }


        [UIValue("UIValue_PlayerHeight")]
        private float UIValue_PlayerHeight
        {
            get
            {
                return _playerHeight * 100.0f;
            }
            set
            {
                _playerDataModel.playerData.SetPlayerSpecificSettings(_playerDataModel.playerData.playerSpecificSettings.CopyWith(playerHeight: value / 100.0f));
                if (_gameplaySetupViewController)
                {
                    _gameplaySetupViewController.Init();
                }
            }
        }

        [UIComponent("UIComponent_PlayerHeight")]
        private GenericSetting UIComponent_PlayerHeight;

/*        private void Update()
        {
            UIComponent_PlayerHeight.ReceiveValue();
        }*/

    }
}
