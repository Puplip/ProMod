using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;
using UnityEngine;
using TMPro;

namespace ProMod.Stats
{

    public abstract class ProStat
    {
        public virtual string GetText(ProStatData _proStatData)
        {
            return "<size=60%>ProMod\n<size=100%>69<size=60>420";
        }

        public virtual string UIPosition => "";

        public static string Title(string v)
        {
            return "<size=30%>" + v + "\n";
        }
        public static string Int(string v)
        {
            return "<size=100%>" + v + "\n";
        }
        public static string Ratio(float v)
        {
            return string.Format("<size=100%>{0}<size=60%>{1:00}", Mathf.RoundToInt(v * 100.0f), Mathf.RoundToInt(v * 10000.0f) % 100);
        }

    }

    public class ProStat_Acc : ProStat
    {
        public override string UIPosition => "TopMiddleStat";
        public override string GetText(ProStatData _proStatData)
        {
            return Title("ACC") + (_proStatData.maxScore > 0 ? Ratio((float)_proStatData.score / (float)_proStatData.maxScore) : Ratio(0));
        }
    }

    public class ProStat_Combo : ProStat
    {
        public override string UIPosition => "TopLeftStat";
        public override string GetText(ProStatData _proStatData)
        {
            return Title("Combo") + (_proStatData.maxScore > 0 ? Ratio((float)_proStatData.score / (float)_proStatData.maxScore) : Ratio(0));
        }
    }

    public class ProStat_ComboDamage : ProStat
    {
        public override string UIPosition => "TopRightStat";
        public override string GetText(ProStatData _proStatData)
        {
            return Title("Combo DMG") + "<size=100%>-" + (_proStatData.maxScore > 0 ? Ratio((float)_proStatData.comboPenalty / (float)_proStatData.maxScore) : Ratio(0));
        }
    }

    public class ProStat_LeftAcc : ProStat
    {
        public override string UIPosition => "LeftTopStat";
        public override string GetText(ProStatData _proStatData)
        {
            ProCutStats cutStats = _proStatData.CutStatsBySaber[SaberType.SaberB];
            return Title("L Acc") + Ratio(cutStats.CutScore.Average(cutStats.Count)/115.0f);
        }
    }

    public class ProStat_RightAcc : ProStat
    {
        public override string UIPosition => "RightTopStat";
        public override string GetText(ProStatData _proStatData)
        {
            ProCutStats cutStats = _proStatData.CutStatsBySaber[SaberType.SaberA];
            return Title("R Acc") + Ratio(cutStats.CutScore.Average(cutStats.Count) / 115.0f);
        }
    }

    public class ProStat_LeftSwing : ProStat
    {
        public override string UIPosition => "LeftBottomStat";
        public override string GetText(ProStatData _proStatData)
        {
            ProCutStats cutStats = _proStatData.CutStatsBySaber[SaberType.SaberB];
            return Title("L Swing") + Ratio(cutStats.CutScoreSwing.Average(cutStats.Count) / 100.0f);
        }
    }

    public class ProStat_RightSwing : ProStat
    {
        public override string UIPosition => "RightBottomStat";
        public override string GetText(ProStatData _proStatData)
        {
            ProCutStats cutStats = _proStatData.CutStatsBySaber[SaberType.SaberA];
            return Title("R Swing") + Ratio(cutStats.CutScoreSwing.Average(cutStats.Count) / 100.0f);
        }
    }

    public class ProStat_LeftAim : ProStat
    {
        public override string GetText(ProStatData _proStatData)
        {
            ProCutStats cutStats = _proStatData.CutStatsBySaber[SaberType.SaberB];
            return Title("L Aim") + Ratio(cutStats.CutScoreAim.Average(cutStats.Count) / 100.0f);
        }
    }

    public class ProStat_RightAim : ProStat
    {
        public override string GetText(ProStatData _proStatData)
        {
            ProCutStats cutStats = _proStatData.CutStatsBySaber[SaberType.SaberA];
            return Title("R Aim") + Ratio(cutStats.CutScoreAim.Average(cutStats.Count) / 100.0f);
        }
    }

    public class ProStatUIController : MonoBehaviour
    {
        [Inject]
        private ProStatData _statData;

        private List<ProStat> _statList;
        private void Awake()
        {
            _statData.onChangeEvent += ProStatData_onChangeEvent;
            _statList = new List<ProStat>() {
                new ProStat_Acc(),
                new ProStat_Combo(),
                new ProStat_ComboDamage(),
                new ProStat_LeftAcc(),
                new ProStat_RightAcc(),
                new ProStat_LeftSwing(),
                new ProStat_RightSwing()
            };
        }
        private void ProStatData_onChangeEvent()
        {
            RefreshUI();
        }

        private void RefreshUI()
        {
            foreach(TextMeshPro x in GetComponentsInChildren<TextMeshPro>())
            {
                foreach(ProStat proStat in _statList)
                {
                    if(x.gameObject.name == proStat.UIPosition)
                    {
                        x.text = proStat.GetText(_statData);
                    }
                }
            }
        }
    }

}