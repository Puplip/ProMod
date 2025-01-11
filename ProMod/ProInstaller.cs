using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;
using UnityEngine;

namespace ProMod.ProInstaller
{

    public class ProMainSettingsMenuViewControllersInstaller : Installer
    {
        public override void InstallBindings()
        {

            Container.Bind<ProHeightMenu>().FromNewComponentOnNewPrefab(ProAssets.HeightGuide).AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<UI.ProTabUI>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<UI.ProHeightTabUI>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<UI.ProHUDTabUI>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<UI.ProDisplayTabUI>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<UI.ProCutScoresTabUI>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<UI.ProGameplayTabUI>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<UI.ProJumpTabUI>().AsSingle().NonLazy();

        }
    }

    public class ProGameplayCoreInstaller : Installer
    {
        public override void InstallBindings()
        {
            if (Plugin.Config.proHUDConfig.proHUDEnabled)
            {
                Container.Bind<Stats.ProStats>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
                Container.Bind<HUD.ProHUDController>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
            }

            if (Plugin.Config.heightGuideEnabled)
            {
                Container.Bind<ProHeightGameplay>().FromNewComponentOnNewPrefab(ProAssets.HeightGuide).AsSingle().NonLazy();
            }


            //else
            //{
            //    Container.Unbind<Stats.ProStatData>();
            //    Container.Unbind<Stats.ProStatController>();
            //    Container.Unbind<Stats.ProStatCollector>();
            //}

            Container.Bind<ProGameplayCamera>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();


        }
    }

}
