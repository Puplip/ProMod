using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;
using UnityEngine;
using TMPro;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.FloatingScreen;
using BeatSaberMarkupLanguage.ViewControllers;
using BeatSaberMarkupLanguage.Attributes;
using HMUI;

namespace ProMod.Stats
{
    public class ProStatViewController : BSMLResourceViewController
    {

        public override string ResourceName => "ProMod.Resources.ProStat.bsml";

        public string ProStatElement = "ProStat_Acc";

        [UIComponent("UIComponent_text")]
        public TextMeshProUGUI text;

    }

    public class ProStatController : MonoBehaviour
    {

        [Inject]
        private IJumpOffsetYProvider _jumpOffsetYProvider;

        [Inject]
        private ProStatData _statData;

        [Inject]
        private PlayerDataModel _playerDataModel;

        private List<ProStat> _proStats = new List<ProStat>();

        private void Awake()
        {


            _statData.onChangeEvent += ProStatData_onChangeEvent;

            transform.position = Vector3.up * (_jumpOffsetYProvider.jumpOffsetY + 1.4f);
            transform.rotation = Quaternion.identity;

            

            for (int i = 0; i < Plugin.Config.ProStats.Count; i++)
            {

                Config.ProStatConfig statConfig = Plugin.Config.ProStats[i];
                Plugin.Log.Info("Creating ProStat: " + statConfig.name);
                if (!ProStat.Exists(statConfig.name)) continue;


                GameObject statObject = new GameObject(statConfig.name);
                statObject.transform.SetParent(transform);
                ProStat proStat = (ProStat)statObject.AddComponent(ProStat.GetStat(statConfig.name));

                Plugin.Log.Info("Initializing ProStat: " + statConfig.name);
                if(statConfig.location == ProStatLocation.Custom)
                {
                    proStat.Init(statConfig.customLocation);
                }
                else
                {
                    proStat.Init(ProStatLocationData.GetLocationData(statConfig.location));
                }

                _proStats.Add(proStat);

            }

        }

        private void ProStatData_onChangeEvent()
        {
            foreach(ProStat proStat in _proStats)
            {
                proStat.OnData(_statData);
            }
        }

    }
}