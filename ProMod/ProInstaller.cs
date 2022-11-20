using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;
using UnityEngine;

namespace ProMod.ProInstaller
{
    public class ProPCAppInit : Installer
    {
        public override void InstallBindings()
        {
            Container.Bind<ProHUD>().FromNewComponentOnNewPrefab(ProAssets.HeightGuide).AsSingle().NonLazy();
            Container.Bind<ProClean>().FromNewComponentOnRoot().AsSingle().NonLazy();
        }
    }


    public class ProMainSettingsMenuViewControllersInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.Bind<ProTab>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
        }
    }

    public class ProGameplayCoreInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.Bind<Stats.ProStatData>().FromInstance(new Stats.ProStatData()).AsSingle().NonLazy();
            Container.Bind<Stats.ProStatUIController>().FromNewComponentOnNewPrefab(ProAssets.StandardStatView).AsSingle().NonLazy();
            Container.Bind<Stats.ProStatCollector>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
        }
    }

}
