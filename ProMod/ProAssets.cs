using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProMod
{
    public static class ProAssets
    {

        public static GameObject HeightGuide;

        public static void Init() {
            if (_init) return;

            AssetBundle assetBundle = AssetBundle.LoadFromStream(typeof(ProAssets).Assembly.GetManifestResourceStream("ProMod.Resources.promod_assets"));

            HeightGuide = assetBundle.LoadAsset<GameObject>("HeightGuide");

            assetBundle.Unload(false);

            _init = true;
        }

        private static bool _init = false;
    }
}
