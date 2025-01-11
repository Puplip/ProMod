using Zenject;
using Newtonsoft.Json;
using UnityEngine;

namespace ProMod
{
    public class ProGameplayCamera : MonoBehaviour
    {
        [Inject]
        private MainCamera _mainCamera;

        private int _cameraMask;
        private int _originalCameraMask;

        private void Start()
        {
            _cameraMask = Plugin.Config.hmdCameraMask;
            _originalCameraMask = _mainCamera.camera.cullingMask;
            _mainCamera.camera.cullingMask = _cameraMask;
        }
        //float nextLogTime = 0;
        //private void Update()
        //{
        //    if (Time.time >= nextLogTime)
        //    {
        //        nextLogTime = Time.time + 10.0f;
        //        Plugin.Log.Info($"Default HMD Camera Mask: 0x{_mainCamera.camera.cullingMask:X8}");
        //    }
        //}

        private void OnDisable()
        {
            _mainCamera.camera.cullingMask = _originalCameraMask;
        }

    }
}
