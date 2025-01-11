//using ProMod.Stats;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ProMod.HUD.Elements;

//[ProHUDElement("CenterDistance")]
//public class ProCenterDistance : ProHUDCompoundElement
//{
//    [ProHUDElement("CenterDistance.Title", 180, 24)]
//    public class Title : ProHUDTextElement
//    {
//        public override string UpdateText(ProStats proStats)
//        {
//            return "Center Distance";
//        }
//    }
//    [ProHUDElement("CenterDistance.LeftScore", 90, 36)]
//    public class LeftScore : ProHUDTextElement
//    {
//        public override string UpdateText(ProStats proStats)
//        {
//            return ProHUDUtil.Float2(proStats[SaberType.SaberA].centerDistanceCutScore.Average());
//        }
//    }

//    [ProHUDElement("CenterDistance.RightScore", 90, 36)]
//    public class RightScore : ProHUDTextElement
//    {
//        public override string UpdateText(ProStats proStats)
//        {
//            return ProHUDUtil.Float2(proStats[SaberType.SaberB].centerDistanceCutScore.Average());
//        }
//    }
//    [ProHUDElement("CenterDistance.LeftMillimeters", 90, 36)]
//    public class LeftMillimeters : ProHUDTextElement
//    {
//        public override string UpdateText(ProStats proStats)
//        {
//            return ProHUDUtil.Millimeters(proStats[SaberType.SaberA].centerDistance.Average());
//        }
//    }

//    [ProHUDElement("CenterDistance.RightMillimeters", 90, 36)]
//    public class RightMillimeters : ProHUDTextElement
//    {
//        public override string UpdateText(ProStats proStats)
//        {
//            return ProHUDUtil.Millimeters(proStats[SaberType.SaberB].centerDistance.Average());
//        }
//    }
//    [ProHUDElement("CenterDistance.LeftCentimeters", 90, 36)]
//    public class LeftCentimeters : ProHUDTextElement
//    {
//        public override string UpdateText(ProStats proStats)
//        {
//            return ProHUDUtil.Centimeters(proStats[SaberType.SaberA].centerDistance.Average());
//        }
//    }

//    [ProHUDElement("CenterDistance.RightCentimeters", 90, 36)]
//    public class RightCentimeters : ProHUDTextElement
//    {
//        public override string UpdateText(ProStats proStats)
//        {
//            return ProHUDUtil.Centimeters(proStats[SaberType.SaberB].centerDistance.Average());
//        }
//    }

//    [ProHUDElement("CenterDistance.Histogram", 180, 60)]
//    public class Histogram : ProHUDSaberHistogram
//    {
//        public override int HistogramBucketCount => 16;

//        public override ProIntegerHistogram GetIntegerHistogram(ProStats proStats, SaberType saberType)
//        {
//            return proStats[saberType].centerDistanceCutScore;
//        }
//    }
//    public override IEnumerable<string> ChildElements
//    {
//        get
//        {
//            switch (Plugin.Config.proHUDConfig.centerDistanceStyle)
//            {
//                case ProHUDConfig.CenterDistanceStyle.Score:
//                    return new string[]
//                {
//                    "CenterDistance.Title","NewLine",
//                    "CenterDistance.LeftScore","CenterDistance.RightScore"
//                };
//                case ProHUDConfig.CenterDistanceStyle.Millimeters:
//                    return new string[]
//                {
//                    "CenterDistance.Title","NewLine",
//                    "CenterDistance.LeftMillimeters","CenterDistance.RightMillimeters"
//                };
//                case ProHUDConfig.CenterDistanceStyle.Histogram:
//                    return new string[]
//                {
//                    "CenterDistance.Title",
//                    "VerticalSpacer8",
//                    "CenterDistance.Histogram"
//                };
//                default: return new string[] { };
//            }
//        }
//    }
//}

//[ProHUDElement("TimeDependence")]
//public class ProTimeDependence : ProHUDCompoundElement
//{
//    [ProHUDElement("TimeDependence.Title", 90, 24)]
//    public class Title : ProHUDTextElement
//    {
//        public override string UpdateText(ProStats proStats)
//        {
//            return "Time Dependence";
//        }
//    }

//    [ProHUDElement("TimeDependence.LeftPercent", 90, 36)]
//    public class LeftPercent : ProHUDTextElement
//    {
//        public override string UpdateText(ProStats proStats)
//        {
//            return ProHUDUtil.Percent(proStats[SaberType.SaberA].timeDependence.Average());
//        }
//    }

//    [ProHUDElement("TimeDependence.RightPercent", 90, 36)]
//    public class RightPercent : ProHUDTextElement
//    {
//        public override string UpdateText(ProStats proStats)
//        {
//            return ProHUDUtil.Percent(proStats[SaberType.SaberB].timeDependence.Average());
//        }
//    }

//    [ProHUDElement("TimeDependence.LeftAngle", 90, 36)]
//    public class LeftAngle : ProHUDTextElement
//    {
//        public override string UpdateText(ProStats proStats)
//        {
//            return ProHUDUtil.Degrees(proStats[SaberType.SaberA].timeDependenceAngle.Average());
//        }
//    }

//    [ProHUDElement("TimeDependence.RightAngle", 90, 36)]
//    public class RightAngle : ProHUDTextElement
//    {
//        public override string UpdateText(ProStats proStats)
//        {
//            return ProHUDUtil.Degrees(proStats[SaberType.SaberB].timeDependenceAngle.Average());
//        }
//    }

//    public override IEnumerable<string> ChildElements
//    {
//        get
//        {
//            switch (Plugin.Config.proHUDConfig.timeDependenceStyle)
//            {
//                case ProHUDConfig.TimeDependenceStyle.Percent:
//                    return new string[]
//                {
//                    "TimeDependence.Title","NewLine",
//                    "TimeDependence.LeftPercent","TimeDependence.RightPercent"
//                };
//                case ProHUDConfig.TimeDependenceStyle.Angle:
//                    return new string[]
//                {
//                    "TimeDependence.Title","NewLine",
//                    "TimeDependence.LeftAngle","TimeDependence.RightAngle"
//                };
//                default: return new string[] { };
//            }
//        }
//    }
//}

//[ProHUDElement("Swing")]
//public class ProSwing : ProHUDCompoundElement
//{

//    [ProHUDElement("Swing.SwingTitle", 180, 24)]
//    public class SwingTitle : ProHUDTextElement
//    {
//        public override string UpdateText(ProStats proStats)
//        {
//            return "Swing";
//        }
//    }

//    [ProHUDElement("Swing.BeforeLeftRating", 90, 36)]
//    public class BeforeLeftRating : ProHUDTextElement
//    {
//        public override string UpdateText(ProStats proStats)
//        {
//            return ProHUDUtil.Percent(proStats[SaberType.SaberA].beforeCutSwingRating.Average());
//        }
//    }

//    [ProHUDElement("Swing.BeforeRightRating", 90, 36)]
//    public class BeforeRightRating : ProHUDTextElement
//    {
//        public override string UpdateText(ProStats proStats)
//        {
//            return ProHUDUtil.Percent(proStats[SaberType.SaberB].beforeCutSwingRating.Average());
//        }
//    }

//    [ProHUDElement("Swing.BeforeLeftScore", 90, 36)]
//    public class BeforeLeftScore : ProHUDTextElement
//    {
//        public override string UpdateText(ProStats proStats)
//        {
//            return ProHUDUtil.Float2(proStats[SaberType.SaberA].beforeCutScore.Average());
//        }
//    }

//    [ProHUDElement("Swing.BeforeRightScore", 90, 36)]
//    public class BeforeRightScore : ProHUDTextElement
//    {
//        public override string UpdateText(ProStats proStats)
//        {
//            return ProHUDUtil.Float2(proStats[SaberType.SaberB].beforeCutScore.Average());
//        }
//    }

//    [ProHUDElement("Swing.AfterLeftRating", 90, 36)]
//    public class AfterLeftRating : ProHUDTextElement
//    {
//        public override string UpdateText(ProStats proStats)
//        {
//            return ProHUDUtil.Percent(proStats[SaberType.SaberA].afterCutSwingRating.Average());
//        }
//    }

//    [ProHUDElement("Swing.AfterRightRating", 90, 36)]
//    public class AfterRightRating : ProHUDTextElement
//    {
//        public override string UpdateText(ProStats proStats)
//        {
//            return ProHUDUtil.Percent(proStats[SaberType.SaberB].afterCutSwingRating.Average());
//        }
//    }

//    [ProHUDElement("Swing.AfterLeftScore", 90, 36)]
//    public class AfterLeftScore : ProHUDTextElement
//    {
//        public override string UpdateText(ProStats proStats)
//        {
//            return ProHUDUtil.Float2(proStats[SaberType.SaberA].afterCutScore.Average());
//        }
//    }

//    [ProHUDElement("Swing.AfterRightScore", 90, 36)]
//    public class AfterRightScore : ProHUDTextElement
//    {
//        public override string UpdateText(ProStats proStats)
//        {
//            return ProHUDUtil.Float2(proStats[SaberType.SaberB].afterCutScore.Average());
//        }
//    }
//    public override IEnumerable<string> ChildElements
//    {
//        get
//        {

//            switch (Plugin.Config.proHUDConfig.swingStyle)
//            {
//                case ProHUDConfig.SwingStyle.Score: return new string[] {
//                    "Swing.SwingTitle","NewLine",
//                    "Swing.BeforeLeftScore","Swing.BeforeRightScore","NewLine",
//                    "Swing.AfterLeftScore","Swing.AfterRightScore"
//                };
//                case ProHUDConfig.SwingStyle.SwingRating: return new string[]
//                {
//                    "Swing.SwingTitle","NewLine",
//                    "Swing.BeforeLeftRating","Swing.BeforeRightRating","NewLine",
//                    "Swing.AfterLeftRating","Swing.AfterRightRating"
//                };
//            }

//            return new string[] { };
//        }
//    }
//}
