using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

using IPA;
using IPALogger = IPA.Logging.Logger;
using IPAConfig = IPA.Config.Config;
using PluginMetadata = IPA.Loader.PluginMetadata;
using IPA.Config.Stores;
using SiraUtil.Zenject;
using HarmonyLib;
using ProMod.Patches;

namespace ProMod
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        internal static Plugin Instance { get; private set; }
        internal static PluginMetadata Metadata { get; private set; }
        internal static IPALogger Log { get; private set; }
        internal static ProConfig Config { get; set; }

        internal static Harmony harmony { get; private set; }

        [Init]
        public Plugin(IPALogger log, Zenjector zenjector)
        {

            harmony = new Harmony("com.puplip.promod");
            Instance = this;
            Log = log;

            ProConfig.Load();

            ProAssets.Init();
            ProEffectsPatch.Init();
            ProHeightPatch.Init();
            ProHUDPatch.Init();
            ProCutScorePatch.Init();
            ProJumpPatch.Init();
            ProNotesPatch.Init();
            ProSwingRatingPatch.Init();

            HUD.ProHUD.RegisterElements();

            zenjector.Install<ProInstaller.ProMainSettingsMenuViewControllersInstaller, MainSettingsMenuViewControllersInstaller>();
            zenjector.Install<ProInstaller.ProGameplayCoreInstaller, GameplayCoreInstaller>();


        }

    }
}
