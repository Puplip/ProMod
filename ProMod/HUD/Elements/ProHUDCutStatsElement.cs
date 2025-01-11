using ProMod.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProMod.HUD.Elements;

[ProHUDElement("CutStats")]
public class ProHUDCutStats : ProHUDCompoundElement
{
    public class CenterDistance
    {
        [ProHUDElement("CutStats.CenterDistance.LeftScore", 90, 30)]
        public class LeftScore : ProHUDTextElement
        {
            public override string UpdateText(ProStats proStats)
            {
                return ProHUDUtil.Float2(proStats[SaberType.SaberA].centerDistanceCutScore.Average());
            }
        }

        [ProHUDElement("CutStats.CenterDistance.RightScore", 90, 30)]
        public class RightScore : ProHUDTextElement
        {
            public override string UpdateText(ProStats proStats)
            {
                return ProHUDUtil.Float2(proStats[SaberType.SaberB].centerDistanceCutScore.Average());
            }
        }
        [ProHUDElement("CutStats.CenterDistance.LeftMillimeters", 90, 30)]
        public class LeftMillimeters : ProHUDTextElement
        {
            public override string UpdateText(ProStats proStats)
            {
                return ProHUDUtil.Millimeters(proStats[SaberType.SaberA].centerDistance.Average());
            }
        }

        [ProHUDElement("CutStats.CenterDistance.RightMillimeters", 90, 30)]
        public class RightMillimeters : ProHUDTextElement
        {
            public override string UpdateText(ProStats proStats)
            {
                return ProHUDUtil.Millimeters(proStats[SaberType.SaberB].centerDistance.Average());
            }
        }
        
    }
    
    public class TimeDependence
    {
        [ProHUDElement("CutStats.TimeDependence.LeftPercent", 90, 30)]
        public class LeftPercent : ProHUDTextElement
        {
            public override string UpdateText(ProStats proStats)
            {
                return ProHUDUtil.Percent(proStats[SaberType.SaberA].timeDependence.Average());
            }
        }

        [ProHUDElement("CutStats.TimeDependence.RightPercent", 90, 30)]
        public class RightPercent : ProHUDTextElement
        {
            public override string UpdateText(ProStats proStats)
            {
                return ProHUDUtil.Percent(proStats[SaberType.SaberB].timeDependence.Average());
            }
        }

        [ProHUDElement("CutStats.TimeDependence.LeftAngle", 90, 30)]
        public class LeftAngle : ProHUDTextElement
        {
            public override string UpdateText(ProStats proStats)
            {
                return ProHUDUtil.Degrees(proStats[SaberType.SaberA].timeDependenceAngle.Average());
            }
        }

        [ProHUDElement("CutStats.TimeDependence.RightAngle", 90, 30)]
        public class RightAngle : ProHUDTextElement
        {
            public override string UpdateText(ProStats proStats)
            {
                return ProHUDUtil.Degrees(proStats[SaberType.SaberB].timeDependenceAngle.Average());
            }
        }
    }
    
    public class BeforeCut
    {
        [ProHUDElement("CutStats.BeforeCut.LeftRating", 90, 30)]
        public class LeftRating : ProHUDTextElement
        {
            public override string UpdateText(ProStats proStats)
            {
                return ProHUDUtil.Percent(proStats[SaberType.SaberA].beforeCutSwingRating.Average());
            }
        }

        [ProHUDElement("CutStats.BeforeCut.RightRating", 90, 30)]
        public class RightRating : ProHUDTextElement
        {
            public override string UpdateText(ProStats proStats)
            {
                return ProHUDUtil.Percent(proStats[SaberType.SaberB].beforeCutSwingRating.Average());
            }
        }

        [ProHUDElement("CutStats.BeforeCut.LeftScore", 90, 30)]
        public class LeftScore : ProHUDTextElement
        {
            public override string UpdateText(ProStats proStats)
            {
                return ProHUDUtil.Float2(proStats[SaberType.SaberA].beforeCutScore.Average());
            }
        }

        [ProHUDElement("CutStats.BeforeCut.RightScore", 90, 30)]
        public class RightScore : ProHUDTextElement
        {
            public override string UpdateText(ProStats proStats)
            {
                return ProHUDUtil.Float2(proStats[SaberType.SaberB].beforeCutScore.Average());
            }
        }

        [ProHUDElement("CutStats.BeforeCut.LeftDamage", 90, 30)]
        public class LeftDamage : ProHUDTextElement
        {
            public override string UpdateText(ProStats proStats)
            {
                return ProHUDUtil.Ratio((float)((double)proStats[SaberType.SaberA].beforeCutScoreDamage / (double)-proStats.maxPossibleScore));
            }
        }

        [ProHUDElement("CutStats.BeforeCut.RightDamage", 90, 30)]
        public class RightDamage : ProHUDTextElement
        {
            public override string UpdateText(ProStats proStats)
            {
                return ProHUDUtil.Ratio((float)((double)proStats[SaberType.SaberB].beforeCutScoreDamage / (double)-proStats.maxPossibleScore));
            }
        }
    }

    public class AfterCut
    {
        [ProHUDElement("CutStats.AfterCut.LeftRating", 90, 30)]
        public class LeftRating : ProHUDTextElement
        {
            public override string UpdateText(ProStats proStats)
            {
                return ProHUDUtil.Percent(proStats[SaberType.SaberA].afterCutSwingRating.Average());
            }
        }

        [ProHUDElement("CutStats.AfterCut.RightRating", 90, 30)]
        public class RightRating : ProHUDTextElement
        {
            public override string UpdateText(ProStats proStats)
            {
                return ProHUDUtil.Percent(proStats[SaberType.SaberB].afterCutSwingRating.Average());
            }
        }

        [ProHUDElement("CutStats.AfterCut.LeftScore", 90, 30)]
        public class LeftScore : ProHUDTextElement
        {
            public override string UpdateText(ProStats proStats)
            {
                return ProHUDUtil.Float2(proStats[SaberType.SaberA].afterCutScore.Average());
            }
        }

        [ProHUDElement("CutStats.AfterCut.RightScore", 90, 30)]
        public class RightScore : ProHUDTextElement
        {
            public override string UpdateText(ProStats proStats)
            {
                return ProHUDUtil.Float2(proStats[SaberType.SaberB].afterCutScore.Average());
            }
        }

        [ProHUDElement("CutStats.AfterCut.LeftDamage", 90, 30)]
        public class LeftDamage : ProHUDTextElement
        {
            public override string UpdateText(ProStats proStats)
            {
                return ProHUDUtil.Ratio((float)((double)proStats[SaberType.SaberA].afterCutScoreDamage / (double)-proStats.maxPossibleScore));
            }
        }

        [ProHUDElement("CutStats.AfterCut.RightDamage", 90, 30)]
        public class RightDamage : ProHUDTextElement
        {
            public override string UpdateText(ProStats proStats)
            {
                return ProHUDUtil.Ratio((float)((double)proStats[SaberType.SaberB].afterCutScoreDamage / (double)-proStats.maxPossibleScore));
            }
        }
    }

    public class CombinedCut
    {

        [ProHUDElement("CutStats.CombinedCut.LeftScore", 90, 30)]
        public class LeftScore : ProHUDTextElement
        {
            public override string UpdateText(ProStats proStats)
            {
                var saberStats = proStats[SaberType.SaberA];
                return ProHUDUtil.Float2(saberStats.beforeCutScore.Average() + saberStats.afterCutScore.Average());
            }
        }

        [ProHUDElement("CutStats.CombinedCut.RightScore", 90, 30)]
        public class RightScore : ProHUDTextElement
        {
            public override string UpdateText(ProStats proStats)
            {
                var saberStats = proStats[SaberType.SaberB];
                return ProHUDUtil.Float2(saberStats.beforeCutScore.Average() + saberStats.afterCutScore.Average());
            }
        }

        [ProHUDElement("CutStats.CombinedCut.LeftDamage", 90, 30)]
        public class LeftDamage : ProHUDTextElement
        {
            public override string UpdateText(ProStats proStats)
            {
                var saberStats = proStats[SaberType.SaberA];

                return ProHUDUtil.Ratio((float)((double)(saberStats.beforeCutScoreDamage + saberStats.afterCutScoreDamage) / (double)-proStats.maxPossibleScore));
            }
        }

        [ProHUDElement("CutStats.CombinedCut.RightDamage", 90, 30)]
        public class RightDamage : ProHUDTextElement
        {
            public override string UpdateText(ProStats proStats)
            {
                var saberStats = proStats[SaberType.SaberB];
                return ProHUDUtil.Ratio((float)((double)(saberStats.beforeCutScoreDamage + saberStats.afterCutScoreDamage) / (double)-proStats.maxPossibleScore));
            }
        }
    }


    [ProHUDElement("CutStats.Left")]
    public class Left : ProHUDCompoundElement
    {
        public override IEnumerable<string> ChildElements
        {
            get
            {

                List<string> children = new List<string>();

                switch (Plugin.Config.proHUDConfig.centerDistanceStyle)
                {
                    case ProHUDConfig.CenterDistanceStyle.Score:
                        children.Add("CutStats.CenterDistance.LeftScore");
                        break;
                    case ProHUDConfig.CenterDistanceStyle.Millimeters:
                        children.Add("CutStats.CenterDistance.LeftMillimeters");
                        break;
                }

                switch (Plugin.Config.proHUDConfig.swingStyle)
                {
                    case ProHUDConfig.SwingStyle.SeperateScore:
                        children.Add("CutStats.BeforeCut.LeftScore");
                        children.Add("VerticalSpacer4");
                        children.Add("CutStats.AfterCut.LeftScore");
                        children.Add("VerticalSpacer4");
                        break;
                    case ProHUDConfig.SwingStyle.CombindedScore:
                        children.Add("CutStats.CombinedCut.LeftScore");
                        children.Add("VerticalSpacer4");
                        break;
                    case ProHUDConfig.SwingStyle.SeperateUnderswingDamage:
                        children.Add("CutStats.BeforeCut.LeftDamage");
                        children.Add("VerticalSpacer4");
                        children.Add("CutStats.AfterCut.LeftDamage");
                        children.Add("VerticalSpacer4");
                        break;
                    case ProHUDConfig.SwingStyle.CombinedUnderswingDamage:
                        children.Add("CutStats.CombinedCut.LeftDamage");
                        children.Add("VerticalSpacer4");
                        break;
                    case ProHUDConfig.SwingStyle.SwingRating:
                        children.Add("CutStats.BeforeCut.LeftRating");
                        children.Add("VerticalSpacer4");
                        children.Add("CutStats.AfterCut.LeftRating");
                        children.Add("VerticalSpacer4");
                        break;
                }

                switch (Plugin.Config.proHUDConfig.timeDependenceStyle)
                {
                    case ProHUDConfig.TimeDependenceStyle.Percent:
                        children.Add("CutStats.TimeDependence.LeftPercent");
                        children.Add("VerticalSpacer4");
                        break;
                    case ProHUDConfig.TimeDependenceStyle.Angle:
                        children.Add("CutStats.TimeDependence.LeftAngle");
                        children.Add("VerticalSpacer4");
                        break;
                }

                return children;
            }
        }
    }

    [ProHUDElement("CutStats.Right")]
    public class Right : ProHUDCompoundElement
    {
        public override IEnumerable<string> ChildElements
        {
            get
            {
                List<string> children = new List<string>();

                switch (Plugin.Config.proHUDConfig.centerDistanceStyle)
                {
                    case ProHUDConfig.CenterDistanceStyle.Score:
                        children.Add("CutStats.CenterDistance.RightScore");
                        break;
                    case ProHUDConfig.CenterDistanceStyle.Millimeters:
                        children.Add("CutStats.CenterDistance.RightMillimeters");
                        break;
                }

                switch (Plugin.Config.proHUDConfig.swingStyle)
                {
                    case ProHUDConfig.SwingStyle.SeperateScore:
                        children.Add("CutStats.BeforeCut.RightScore");
                        children.Add("VerticalSpacer4");
                        children.Add("CutStats.AfterCut.RightScore");
                        children.Add("VerticalSpacer4");
                        break;
                    case ProHUDConfig.SwingStyle.CombindedScore:
                        children.Add("CutStats.CombinedCut.RightScore");
                        children.Add("VerticalSpacer4");
                        break;
                    case ProHUDConfig.SwingStyle.SeperateUnderswingDamage:
                        children.Add("CutStats.BeforeCut.RightDamage");
                        children.Add("VerticalSpacer4");
                        children.Add("CutStats.AfterCut.RightDamage");
                        children.Add("VerticalSpacer4");
                        break;
                    case ProHUDConfig.SwingStyle.CombinedUnderswingDamage:
                        children.Add("CutStats.CombinedCut.RightDamage");
                        children.Add("VerticalSpacer4");
                        break;
                    case ProHUDConfig.SwingStyle.SwingRating:
                        children.Add("CutStats.BeforeCut.RightRating");
                        children.Add("VerticalSpacer4");
                        children.Add("CutStats.AfterCut.RightRating");
                        children.Add("VerticalSpacer4");
                        break;
                }

                switch (Plugin.Config.proHUDConfig.timeDependenceStyle)
                {
                    case ProHUDConfig.TimeDependenceStyle.Percent:
                        children.Add("CutStats.TimeDependence.RightPercent");
                        children.Add("VerticalSpacer4");
                        break;
                    case ProHUDConfig.TimeDependenceStyle.Angle:
                        children.Add("CutStats.TimeDependence.RightAngle");
                        children.Add("VerticalSpacer4");
                        break;
                }

                return children;
            }
        }
    }

    [ProHUDElement("CutStats.Center")]
    public class Center : ProHUDCompoundElement
    {
        [ProHUDElement("CutStats.Center.CutScore")]
        public class CutScore : ProHUDCompoundElement
        {
            [ProHUDElement("CutStats.Center.CutScore.Left", 90, 36)]
            public class Left : ProHUDTextElement
            {
                public override string UpdateText(ProStats proStats)
                {
                    return ProHUDUtil.Float2(proStats[SaberType.SaberA].cutScore.Average());
                }
            }

            [ProHUDElement("CutStats.Center.CutScore.Right", 90, 36)]
            public class Right : ProHUDTextElement
            {
                public override string UpdateText(ProStats proStats)
                {
                    return ProHUDUtil.Float2(proStats[SaberType.SaberB].cutScore.Average());
                }
            }
            public override IEnumerable<string> ChildElements => new string[]
            {
                "CutStats.Center.CutScore.Left",
                "CutStats.Center.CutScore.Right"
            };
        }
        [ProHUDElement("CutStats.Center.Histogram", 180, 72)]
        public class Histogram : ProHUDSaberHistogram
        {
            public override int HistogramBucketCount => 16;

            public override ProIntegerHistogram GetIntegerHistogram(ProStats proStats, SaberType saberType)
            {
                return proStats[saberType].centerDistanceCutScore;
            }
        }

        public override IEnumerable<string> ChildElements {
            get
            {
                switch (Plugin.Config.proHUDConfig.cutStatsCenterStyle)
                {
                    case ProHUDConfig.CutStatsCenterStyle.HistogramTop: return new string[]
                    {
                        "CutStats.Center.Histogram",
                        "VerticalSpacer16",
                        "CutStats.Center.CutScore",
                    };
                    case ProHUDConfig.CutStatsCenterStyle.HistogramBottom: return new string[]
                    {
                        "CutStats.Center.CutScore",
                        "VerticalSpacer16",
                        "CutStats.Center.Histogram",
                    };
                    case ProHUDConfig.CutStatsCenterStyle.HistogramOnly: return new string[]
                    {
                        "CutStats.Center.Histogram",
                    };
                    case ProHUDConfig.CutStatsCenterStyle.CutOnly: return new string[]
                    {
                        "CutStats.Center.CutScore",
                    };
                }
                return new string[] { };
            }
        } 
    }

    public override IEnumerable<string> ChildElements => new string[]
    {
        "CutStats.Left",
        "CutStats.Center",
        "CutStats.Right"
    };
}
