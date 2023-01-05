using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Zenject;

namespace ProMod
{

    public abstract class ProHeight : MonoBehaviour
    {
        internal void UpdateJumpOffsetY(float jumpOffsetY)
        {
            foreach (Transform child in transform.GetComponentsInChildren<Transform>(true))
            {
                switch (child.name)
                {
                    case "BottomRow":
                        child.position = new Vector3(child.position.x, jumpOffsetY + 0.85f, child.position.z);
                        break;
                    case "MiddleRow":
                        child.position = new Vector3(child.position.x, jumpOffsetY + 1.4f, child.position.z);
                        break;
                    case "TopRow":
                        child.position = new Vector3(child.position.x, jumpOffsetY + 1.9f, child.position.z);
                        break;
                }
            }
        }
    }

    public class ProHeightMenu : ProHeight
    {

        [Inject]
        private PlayerDataModel _playerDataModel;

        private float _playerHeight = -1.0f;

        private bool _childrenActive;

        private void Awake()
        {
            ProUtil.SetLayerRecursive(gameObject, Camera2Layer.UI);
        }

        private void Update() {

            if (!Plugin.Config.HeightGuideMenuEnabled) {
                for(var i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
                _childrenActive = false;
            } else if(!_childrenActive)
            {
                for (var i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(true);
                }
                _childrenActive = true;
            }

            if (_playerHeight != _playerDataModel.playerData.playerSpecificSettings.playerHeight) {

                _playerHeight = _playerDataModel.playerData.playerSpecificSettings.playerHeight;

                UpdateJumpOffsetY(PlayerHeightToJumpOffsetYProvider.JumpOffsetYForPlayerHeight(_playerHeight));

                transform.localScale = new Vector3(1.0f, 1.0f, Plugin.Config.HeightGuideLength);
                transform.localPosition = new Vector3(0.0f, 0.0f, Plugin.Config.HeightGuideOffset);

            }

        }

    }

    public class ProHeightGameplay : ProHeight
    {

        [Inject]
        private IJumpOffsetYProvider _jumpOffsetYProvider;

        private void Awake()
        {
            if (!Plugin.Config.HeightGuideGameplayEnabled)
            {
                gameObject.SetActive(false);
                return;
            }

            ProUtil.SetLayerRecursive(gameObject,Camera2Layer.FirstPerson);

            UpdateJumpOffsetY(_jumpOffsetYProvider.jumpOffsetY);

            transform.localScale = new Vector3(1.0f, 1.0f, Plugin.Config.HeightGuideLength);
            transform.localPosition = new Vector3(0.0f, 0.0f, Plugin.Config.HeightGuideOffset);

        }

    }
}
