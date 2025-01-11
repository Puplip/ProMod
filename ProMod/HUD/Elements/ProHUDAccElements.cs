using ProMod.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ProMod.HUD.Elements;

[ProHUDElement("Accuracy")]
public class ProAccuracy : ProHUDCompoundElement
{
    [ProHUDElement("Accuracy.InstantTitle", 180,24)]
    public class InstantTitle : ProHUDTextElement
    {
        public override string UpdateText(ProStats proStats)
        {
            return "ACC";
        }
    }
    [ProHUDElement("Accuracy.InstantValue", 180, 60)]
    public class InstantValue : ProHUDTextElement
    {
        public override string UpdateText(ProStats proStats)
        {
            return ProHUDUtil.AccColorRatio(proStats.currentAccuracy);
        }
    }
    [ProHUDElement("Accuracy.SmartTitle", 180, 24)]
    public class SmartTitle : ProHUDTextElement
    {
        public override string UpdateText(ProStats proStats)
        {
            return proStats.currentScore != proStats.currentMaxMultiplierScore ? "Est. ACC" : "ACC";
        }
    }
    [ProHUDElement("Accuracy.SmartValue", 180, 60)]
    public class SmartValue : ProHUDTextElement
    {
        public override string UpdateText(ProStats proStats)
        {
            return ProHUDUtil.AccColorRatio(proStats.estimatedFinalAccuracy);
        }
    }

    [ProHUDElement("Accuracy.InstantBar", 160, 24)]
    public class InstantBar : ProHUDTwoColorBar
    {
        int accIntegerPart = 0;
        int accDecimalPart = 0;

        public override Color UpdateFirstColor(ProStats proStats)
        {
            return ProHUDUtil.AccColor(accIntegerPart + 1);
        }

        public override float UpdateRatio(ProStats proStats)
        {
            int roundedAcc = Mathf.RoundToInt(proStats.currentAccuracy * 10000f);
            accIntegerPart = roundedAcc / 100;
            accDecimalPart = roundedAcc % 100;
            return (float)accDecimalPart / 100f;
        }
    }

    [ProHUDElement("Accuracy.SmartBar", 160, 24)]
    public class SmartBar : ProHUDTwoColorBar
    {
        int accIntegerPart = 0;
        int accDecimalPart = 0;

        public override Color UpdateFirstColor(ProStats proStats)
        {
            return ProHUDUtil.AccColor(accIntegerPart + 1);
        }

        public override float UpdateRatio(ProStats proStats)
        {
            int roundedAcc = Mathf.RoundToInt(proStats.estimatedFinalAccuracy * 10000f);
            accIntegerPart = roundedAcc / 100;
            accDecimalPart = roundedAcc % 100;
            return (float)accDecimalPart / 100f;
        }
    }

    [ProHUDElement("Accuracy.FullCombo")]
    public class FullCombo : ProHUDCompoundElement
    {
        [ProHUDElement("Accuracy.FullCombo.Title", 20, 24)]
        public class Title : ProHUDTextElement
        {
            public override bool UpdateEnabled(ProStats proStats)
            {
                return proStats.currentScore != proStats.currentMaxMultiplierScore;
            }
            public override string UpdateText(ProStats proStats)
            {
                return "FC";
            }
        }

        [ProHUDElement("Accuracy.FullCombo.AccValue", 80, 24)]
        public class AccValue : ProHUDTextElement
        {
            public override bool UpdateEnabled(ProStats proStats)
            {
                return proStats.currentScore != proStats.currentMaxMultiplierScore;
            }
            public override string UpdateText(ProStats proStats)
            {
                return ProHUDUtil.AccColorRatio(proStats.currentFullComboAccuracy);
            }
        }

        [ProHUDElement("Accuracy.FullCombo.DiffValue", 80, 24)]
        public class DiffValue : ProHUDTextElement
        {
            public override bool UpdateEnabled(ProStats proStats)
            {
                return proStats.currentScore != proStats.currentMaxMultiplierScore;
            }
            public override string UpdateText(ProStats proStats)
            {
                return ProHUDUtil.SignedRatio(
                    (float)((double)(

                        //points lost to misses and bad cuts
                        (proStats.maxPossibleCurrentScore - proStats.maxPossibleCurrentGoodCutScore) +

                        //points lost to multiplier not being max
                        (proStats.currentMaxMultiplierScore - proStats.currentScore)

                    ) / (double)proStats.maxPossibleScore)
                );
            }
        }
        public override IEnumerable<string> ChildElements
        {
            get
            {
                switch (Plugin.Config.proHUDConfig.fullComboAccStyle)
                {
                    case ProHUDConfig.FullComboAccStyle.AccOnly: return new string[]
                    {
                        "Accuracy.FullCombo.Title","Accuracy.FullCombo.AccValue"
                    };
                    case ProHUDConfig.FullComboAccStyle.DiffOnly: return new string[]
                    {
                        "Accuracy.FullCombo.Title","Accuracy.FullCombo.DiffValue"
                    };
                    case ProHUDConfig.FullComboAccStyle.AccAndDiff: return new string[]
                    {
                        "Accuracy.FullCombo.AccValue","Accuracy.FullCombo.Title","Accuracy.FullCombo.DiffValue"
                    };
                    default: return new string[] { };
                }
            }
        }
    }

    [ProHUDElement("Accuracy.LeftRight")]
    public class LeftRight : ProHUDCompoundElement
    {

        [ProHUDElement("Accuracy.LeftRight.Left", 90, 36)]
        public class Left : ProHUDTextElement
        {
            public override string UpdateText(ProStats proStats)
            {
                return ProHUDUtil.AccColorRatio(proStats[SaberType.SaberA].currentFullComboAccuracy);
            }
        }
        [ProHUDElement("Accuracy.LeftRight.Right", 90, 36)]
        public class Right : ProHUDTextElement
        {
            public override string UpdateText(ProStats proStats)
            {
                return ProHUDUtil.AccColorRatio(proStats[SaberType.SaberB].currentFullComboAccuracy);
            }
        }
        public override IEnumerable<string> ChildElements => new string[] {
            "Accuracy.LeftRight.Left","Accuracy.LeftRight.Right"
        };
    }

    public override IEnumerable<string> ChildElements
    {
        get
        {
            List<string> result = new List<string>();

            switch (Plugin.Config.proHUDConfig.accStyle)
            {
                case ProHUDConfig.AccStyle.Instant:
                    result.AddRange(new string[]
                    {
                        "Accuracy.InstantTitle","NewLine",
                        "Accuracy.InstantValue"
                    });
                    break;
                case ProHUDConfig.AccStyle.Smart:
                    result.AddRange(new string[]
                    {
                        "Accuracy.SmartTitle","NewLine",
                        "Accuracy.SmartValue"
                    });
                    break;
                case ProHUDConfig.AccStyle.InstantBar:
                    result.AddRange(new string[]
                    {
                        "Accuracy.InstantTitle","NewLine",
                        "Accuracy.InstantValue","NewLine",
                        "Accuracy.InstantBar"
                    });
                    break;
                case ProHUDConfig.AccStyle.SmartBar:
                    result.AddRange(new string[]
                    {
                        "Accuracy.SmartTitle","NewLine",
                        "Accuracy.SmartValue","NewLine",
                        "Accuracy.SmartBar"
                    });
                    break;
            }

            if(Plugin.Config.proHUDConfig.fullComboAccStyle != ProHUDConfig.FullComboAccStyle.None)
            {
                result.Add("VerticalSpacer8");
                result.Add("Accuracy.FullCombo");
            }

            if (Plugin.Config.proHUDConfig.showLeftRightAcc)
            {
                result.Add("VerticalSpacer8");
                result.Add("Accuracy.LeftRight");
            }



            return result;
        }
    }
}




