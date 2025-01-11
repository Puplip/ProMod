using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProMod
{
    public static class ProAssets
    {

        public static GameObject HeightGuide { get; private set; }
        public static GameObject NoteCore { get; private set; }
        public static GameObject StaticNoteLarge { get; private set; }
        public static GameObject StaticNoteSmall { get; private set; }
        public static GameObject ProSaber { get; private set; }

        private static AssetBundle assetBundle;

        public static void Init() {
            if (_init) return;

            var q = typeof(ProAssets).Assembly.GetManifestResourceStream("ProMod.Resources.promod_assets");

            if(q == null)
            {
                ProMod.Plugin.Log.Error("typeof(ProAssets).Assembly.GetManifestResourceStream(\"ProMod.Resources.promod_assets\") = null!");
            }

            assetBundle = AssetBundle.LoadFromStream(typeof(ProAssets).Assembly.GetManifestResourceStream("ProMod.Resources.promod_assets"));

            HeightGuide = assetBundle.LoadAsset<GameObject>("HeightGuide");
            NoteCore = assetBundle.LoadAsset<GameObject>("NoteCore");



            HeightGuide = assetBundle.LoadAsset<GameObject>("HeightGuide");
            NoteCore = assetBundle.LoadAsset<GameObject>("NoteCore");
            StaticNoteLarge = assetBundle.LoadAsset<GameObject>("StaticNoteLarge");
            StaticNoteSmall = assetBundle.LoadAsset<GameObject>("StaticNoteSmall");
            ProSaber = assetBundle.LoadAsset<GameObject>("ProSaber");

            assetBundle.Unload(false);

            _init = true;
        }

        private static bool _init = false;
    }
}
