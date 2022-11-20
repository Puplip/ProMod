using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Zenject;

namespace ProMod
{
    /// <summary>
    /// Monobehaviours (scripts) are added to GameObjects.
    /// For a full list of Messages a Monobehaviour can receive from the game, see https://docs.unity3d.com/ScriptReference/MonoBehaviour.html.
    /// </summary>
	public class ProHUD : MonoBehaviour
    {

        [Inject]
        private PlayerDataModel _playerDataModel;

        private float _playerHeight = -1.0f;

        private void Update() {

            if (!Plugin.Config.ShowHeightGuide) {
                gameObject.SetActive(false);
            }

            if (_playerHeight != _playerDataModel.playerData.playerSpecificSettings.playerHeight) {

                _playerHeight = _playerDataModel.playerData.playerSpecificSettings.playerHeight;

                float jumpOffsetY = PlayerHeightToJumpOffsetYProvider.JumpOffsetYForPlayerHeight(_playerHeight);
                foreach (Transform child in gameObject.transform.GetComponentsInChildren<Transform>(true))
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

                transform.position = new Vector3(0.0f, 0.0f, 1.5f);

            }

        }

    }
}
