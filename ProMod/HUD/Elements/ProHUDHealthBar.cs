using ProMod.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnityEngine;

namespace ProMod.HUD.Elements;

[ProHUDElement("HealthBar")]
public class ProHealthBar : ProHUDCompoundElement
{
    public override IEnumerable<string> ChildElements => new string[] { "HealthBar.Bar", "HealthBar.Failed" };

    [ProHUDElement("HealthBar.Bar", 250, 20)]
    public class Bar : ProHUDOutlineBar
    {

        ProUtil.HSV failColor = (Color)Plugin.Config.proHUDConfig.healthBarFailColor;
        ProUtil.HSV fullColor = (Color)Plugin.Config.proHUDConfig.healthBarFullColor;

        public override void Initialize(RectTransform rectTransform)
        {
            base.Initialize(rectTransform);
        }


        public override bool UpdateEnabled(ProStats proStats)
        {
            return !proStats.failed;
        }
        public override Color UpdateColor(ProStats proStats)
        {
            return ProUtil.HSV.Lerp(failColor, fullColor, proStats.currentEnergy);
        }
        public override float UpdateRatio(ProStats proStats)
        {
            return proStats.currentEnergy;
        }
    }

    [ProHUDElement("HealthBar.Failed", 250, 36)]
    public class Failed : ProHUDTextElement
    {
        private string displayString = $"<color=#{ColorUtility.ToHtmlStringRGB(Plugin.Config.proHUDConfig.healthBarFailColor)}>Failed";
        public override bool UpdateEnabled(ProStats proStats)
        {
            return proStats.failed;
        }
        public override string UpdateText(ProStats proStats)
        {

            return displayString;
        }
    }


}



