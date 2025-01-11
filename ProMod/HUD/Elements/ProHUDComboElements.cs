using ProMod.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ProMod.HUD.Elements;

[ProHUDElement("ComboMain")]
public class ProComboMain : ProHUDCompoundElement
{
    public override IEnumerable<string> ChildElements
    {
        get
        {
            return new string[]
            {
                "Combo","NewLine",
                "Errors",
                "VerticalSpacer16",
                "SongProgress"
            };
        }
    }
}

[ProHUDElement("Combo")]
public class ProCombo : ProHUDCompoundElement
{

    [ProHUDElement("Combo.Title", 180, 24)]
    public class Title : ProHUDTextElement
    {

        public override string UpdateText(ProStats proStats)
        {
            return proStats.currentCombo == proStats.maxPossibleCurrentCombo ? "Full Combo" : "Combo";
        }
    }
    [ProHUDElement("Combo.Value", 180, 60)]
    public class Value : ProHUDTextElement
    {

        public override string UpdateText(ProStats proStats)
        {
            return $"{proStats.currentCombo}";
        }
    }

    [ProHUDElement("Combo.FullComboTitle", 180, 36)]
    public class FullComboTitle : ProHUDTextElement
    {
        public override bool UpdateEnabled(ProStats proStats)
        {
            return proStats.maxPossibleCurrentCombo == proStats.currentCombo;
        }
        public override string UpdateText(ProStats proStats)
        {
            return "Full Combo";
        }
    }
    public override IEnumerable<string> ChildElements
    {
        get
        {
            switch (Plugin.Config.proHUDConfig.comboStyle)
            {
                case ProHUDConfig.ComboStyle.FullCombo:
                    return new string[]
                {
                    "Combo.FullComboTitle","NewLine"
                };
                case ProHUDConfig.ComboStyle.Combo:
                    return new string[]
                {
                    "Combo.Title","NewLine","Combo.Value"
                };
                default: return new string[] { };
            }
        }
    }
}

[ProHUDElement("Errors")]
public class ProErrors : ProHUDCompoundElement
{

    [ProHUDElement("Errors.Errors", 180, 36)]
    public class Errors : ProHUDTextElement
    {
        public override bool UpdateEnabled(ProStats proStats)
        {
            return proStats.comboBreakCount > 0;
        }
        public override string UpdateText(ProStats proStats)
        {
            string errorString = proStats.comboBreakCount > 1 ? "Errors" : "Error";
            return $"{proStats.comboBreakCount} <size=67%>{errorString}";
        }
    }

    [ProHUDElement("Errors.MissTitle", 90, 24)]
    public class MissTitle : ProHUDTextElement
    {
        public override void Initialize(RectTransform rectTransform)
        {
            base.Initialize(rectTransform);
            textMeshProUGUI.alignment = TMPro.TextAlignmentOptions.MidlineLeft;
        }
        public override bool UpdateEnabled(ProStats proStats)
        {
            return proStats.missCount > 0;
        }
        public override string UpdateText(ProStats proStats)
        {
            return "Miss";
        }
    }

    [ProHUDElement("Errors.MissValue", 90, 36)]
    public class MissValue : ProHUDTextElement
    {
        public override void Initialize(RectTransform rectTransform)
        {
            base.Initialize(rectTransform);
            //textMeshProUGUI.alignment = TMPro.TextAlignmentOptions.MidlineLeft;
        }
        public override bool UpdateEnabled(ProStats proStats)
        {
            return proStats.missCount > 0;
        }
        public override string UpdateText(ProStats proStats)
        {
            return $"{proStats.missCount}";
        }
    }

    [ProHUDElement("Errors.BadCutTitle", 90, 24)]
    public class BadCutTitle : ProHUDTextElement
    {
        public override void Initialize(RectTransform rectTransform)
        {
            base.Initialize(rectTransform);
            textMeshProUGUI.alignment = TMPro.TextAlignmentOptions.MidlineLeft;
        }
        public override bool UpdateEnabled(ProStats proStats)
        {
            return proStats.badCutCount > 0;
        }
        public override string UpdateText(ProStats proStats)
        {
            return "Bad Cut";
        }
    }

    [ProHUDElement("Errors.BadCutValue", 90, 36)]
    public class BadCutValue : ProHUDTextElement
    {
        public override void Initialize(RectTransform rectTransform)
        {
            base.Initialize(rectTransform);
            //textMeshProUGUI.alignment = TMPro.TextAlignmentOptions.MidlineLeft;
        }
        public override bool UpdateEnabled(ProStats proStats)
        {
            return proStats.badCutCount > 0;
        }
        public override string UpdateText(ProStats proStats)
        {
            return $"{proStats.badCutCount}";
        }
    }

    [ProHUDElement("Errors.BombCutTitle", 90, 24)]
    public class BombCutTitle : ProHUDTextElement
    {
        public override void Initialize(RectTransform rectTransform)
        {
            base.Initialize(rectTransform);
            textMeshProUGUI.alignment = TMPro.TextAlignmentOptions.MidlineLeft;
        }
        public override bool UpdateEnabled(ProStats proStats)
        {
            return proStats.bombCutCount > 0;
        }
        public override string UpdateText(ProStats proStats)
        {
            return "Bomb";
        }
    }

    [ProHUDElement("Errors.BombCutValue", 90, 36)]
    public class BombCutValue : ProHUDTextElement
    {
        public override void Initialize(RectTransform rectTransform)
        {
            base.Initialize(rectTransform);
            //textMeshProUGUI.alignment = TMPro.TextAlignmentOptions.MidlineLeft;
        }
        public override bool UpdateEnabled(ProStats proStats)
        {
            return proStats.bombCutCount > 0;
        }
        public override string UpdateText(ProStats proStats)
        {
            return $"{proStats.bombCutCount}";
        }
    }

    [ProHUDElement("Errors.WallTouchTitle", 90, 24)]
    public class WallTouchTitle : ProHUDTextElement
    {
        public override void Initialize(RectTransform rectTransform)
        {
            base.Initialize(rectTransform);
            textMeshProUGUI.alignment = TMPro.TextAlignmentOptions.MidlineLeft;
        }
        public override bool UpdateEnabled(ProStats proStats)
        {
            return proStats.wallTouchCount > 0;
        }
        public override string UpdateText(ProStats proStats)
        {
            return "Wall";
        }
    }

    [ProHUDElement("Errors.WallTouchValue", 90, 36)]
    public class WallTouchValue : ProHUDTextElement
    {
        public override void Initialize(RectTransform rectTransform)
        {
            base.Initialize(rectTransform);
            //textMeshProUGUI.alignment = TMPro.TextAlignmentOptions.MidlineLeft;
        }
        public override bool UpdateEnabled(ProStats proStats)
        {
            return proStats.wallTouchCount > 0;
        }
        public override string UpdateText(ProStats proStats)
        {
            return $"{proStats.wallTouchCount}";
        }
    }
    public override IEnumerable<string> ChildElements
    {
        get
        {
            switch (Plugin.Config.proHUDConfig.errorStyle)
            {
                case ProHUDConfig.ErrorStyle.Errors: return new string[]
                {
                    "Errors.Errors"
                };
                case ProHUDConfig.ErrorStyle.Category: return new string[]
                {
                    "Errors.MissValue","Errors.MissTitle","NewLine",
                    "Errors.BadCutValue","Errors.BadCutTitle","NewLine",
                    "Errors.BombCutValue","Errors.BombCutTitle","NewLine",
                    "Errors.WallTouchValue","Errors.WallTouchTitle"
                };
                default: return new string[] { };
            }
        }
    }
}
