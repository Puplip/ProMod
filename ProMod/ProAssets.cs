using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProMod
{
    public static class ProAssets
    {

        public static GameObject HeightGuide;

        public static GameObject BackStat1x1;
        public static GameObject BackStat1x2;
        public static GameObject BackStat1x3;
        public static GameObject BackStat2x1;
        public static GameObject BackStat2x2;
        public static GameObject BackStat3x1;

        public static GameObject StandardStatView;
        public static void Init() {
            if (_init) return;

            AssetBundle assetBundle = AssetBundle.LoadFromStream(typeof(ProAssets).Assembly.GetManifestResourceStream("ProMod.Resources.promod_assets"));

            HeightGuide = assetBundle.LoadAsset<GameObject>("HeightGuide");

            BackStat1x1 = assetBundle.LoadAsset<GameObject>("BackStat1x1");
            BackStat1x2 = assetBundle.LoadAsset<GameObject>("BackStat1x2");
            BackStat1x3 = assetBundle.LoadAsset<GameObject>("BackStat1x3");
            BackStat2x1 = assetBundle.LoadAsset<GameObject>("BackStat2x1");
            BackStat2x2 = assetBundle.LoadAsset<GameObject>("BackStat2x2");
            BackStat3x1 = assetBundle.LoadAsset<GameObject>("BackStat3x1");

            StandardStatView = assetBundle.LoadAsset<GameObject>("StandardStatView");

            assetBundle.Unload(false);

            _init = true;
        }

        private static bool _init = false;
    }
}
