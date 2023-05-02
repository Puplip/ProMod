using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;
using System.Reflection;
using HarmonyLib;
using System.IO;

/*using BeatSaberMarkupLanguage;*/
using CameraUtils.Core;

namespace ProMod
{
    public class ProGameplayCamera : IInitializable
    {
        [Inject]
        private MainCamera _mainCamera;
        public void Initialize()
        {
            _mainCamera.camera.cullingMask &= ~(1 << ((int)VisibilityLayer.Environment));
            _mainCamera.camera.cullingMask &= ~(1 << ((int)VisibilityLayer.NeonLight));
            _mainCamera.camera.cullingMask &= ~(1 << ((int)VisibilityLayer.Default));
            _mainCamera.camera.cullingMask &= ~(1 << ((int)VisibilityLayer.Skybox));
            _mainCamera.camera.cullingMask &= ~(1 << ((int)VisibilityLayer.NonReflectedParticles));
            _mainCamera.camera.cullingMask &= ~(1 << ((int)VisibilityLayer.Water));
        }
    }
}
