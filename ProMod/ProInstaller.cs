using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;
using UnityEngine;

namespace ProMod.ProInstaller
{
/*    public class ProPCAppInit : Installer
    {
        public override void InstallBindings()
        {
        }
    }*/


    public class ProMainSettingsMenuViewControllersInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.Bind<ProHeightMenu>().FromNewComponentOnNewPrefab(ProAssets.HeightGuide).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<ProTabHooks>().AsSingle().NonLazy();
        }
    }

    public class ProGameplayCoreInstaller : Installer
    {
        public override void InstallBindings()
        {
            if (Plugin.Config.ProStatsEnabled)
            {
                Container.Bind<Stats.ProStatData>().AsSingle().NonLazy();
                Container.Bind<Stats.ProStatController>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
                Container.Bind<Stats.ProStatCollector>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
            } else
            {
                Container.Unbind<Stats.ProStatData>();
                Container.Unbind<Stats.ProStatController>();
                Container.Unbind<Stats.ProStatCollector>();
            }

            if (Plugin.Config.DisableEnvironmentInHMD)
            {
                Container.BindInterfacesAndSelfTo<ProGameplayCamera>().AsSingle().NonLazy();
            } else
            {
                Container.UnbindInterfacesTo<ProGameplayCamera>();
            }
            if (Plugin.Config.HeightGuideEnabled)
            {
                Container.Bind<ProHeightGameplay>().FromNewComponentOnNewPrefab(ProAssets.HeightGuide).AsSingle().NonLazy();
            } else
            {
                Container.Unbind<ProHeightGameplay>();
            }
        }
    }

}
