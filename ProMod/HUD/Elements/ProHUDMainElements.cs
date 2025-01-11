using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProMod.HUD.Elements;



[ProHUDElement("BothTopBottomElements")]
public class ProBothTopBottomElements : ProHUDCompoundElement
{
    public override IEnumerable<string> ChildElements
    {
        get
        {
            switch (Plugin.Config.proHUDConfig.topBottomElementPositions)
            {
                case ProHUDConfig.TopBottomElementPositions.BothTopCutStatsLeft:
                case ProHUDConfig.TopBottomElementPositions.BothBottomCutStatsLeft:
                    return new string[]
                    {
                        Plugin.Config.proHUDConfig.showCutStats ? "CutStats" : "Empty",
                        Plugin.Config.proHUDConfig.showScoreDensity ? "ScoreDensity" : "Empty",
                    };
                case ProHUDConfig.TopBottomElementPositions.BothTopScoreDensityLeft:
                case ProHUDConfig.TopBottomElementPositions.BothBottomScoreDensityLeft:
                    return new string[]
                    {
                        Plugin.Config.proHUDConfig.showScoreDensity ? "ScoreDensity" : "Empty",
                        Plugin.Config.proHUDConfig.showCutStats ? "CutStats" : "Empty",
                    };
            }
            return new string[] { };
            
        }
    }
}



//[ProHUDElement("TopMain")]
//public class ProHUDTopMain : ProHUDCompoundElement
//{
//    public override IEnumerable<string> ChildElements
//    {
//        get
//        {
//            return new string[]
//            {
//                "Swing",
//                "HorizontalSpacer16",
//                "CenterDistance",
//                "HorizontalSpacer16",
//                "TimeDependence"
//            };
//        }
//    }
//}