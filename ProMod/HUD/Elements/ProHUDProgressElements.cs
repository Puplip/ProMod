using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ProMod.Stats;

namespace ProMod.HUD.Elements;

[ProHUDElement("SongProgress")]
public class ProSongProgress : ProHUDCompoundElement
{
    [ProHUDElement("SongProgress.TimeLeftTitle", 180, 24)]
    public class TimeLeftTitle : ProHUDTextElement
    {
        public override string UpdateText(ProStats proStats)
        {
            return "Time Left";
        }
    }

    [ProHUDElement("SongProgress.TimeLeftValue", 180, 48)]
    public class TimeLeftValue : ProHUDTextElement
    {
        public override string UpdateText(ProStats proStats)
        {
            return ProHUDUtil.MinuteSecond(proStats.songLength - proStats.songProgress);
        }
    }

    [ProHUDElement("SongProgress.NotesLeftTitle", 180, 24)]
    public class NotesLeftTitle : ProHUDTextElement
    {
        public override string UpdateText(ProStats proStats)
        {
            return "Notes Left";
        }
    }

    [ProHUDElement("SongProgress.NotesLeftValue", 180, 48)]
    public class NotesLeftValue : ProHUDTextElement
    {
        public override string UpdateText(ProStats proStats)
        {
            return $"{proStats.maxPossibleCombo - proStats.maxPossibleCurrentCombo}";
        }
    }
    [ProHUDElement("SongProgress.TimeFraction")]
    public class TimeFraction : ProHUDCompoundElement
    {
        [ProHUDElement("SongProgress.TimeFraction.Top", 75, 24)]
        public class Top : ProHUDTextElement
        {
            public override void Initialize(RectTransform rectTransform)
            {
                base.Initialize(rectTransform);
                textMeshProUGUI.alignment = TMPro.TextAlignmentOptions.MidlineRight;
            }
            public override string UpdateText(ProStats proStats)
            {
                return $"{ProHUDUtil.MinuteSecond(proStats.songProgress)}";
            }
        }

        [ProHUDElement("SongProgress.TimeFraction.Slash", 30, 24)]
        public class Slash : ProHUDTextElement
        {
            public override string UpdateText(ProStats proStats)
            {
                return "/";
            }
        }

        [ProHUDElement("SongProgress.TimeFraction.Bottom", 75, 24)]
        public class Bottom : ProHUDTextElement
        {
            public override void Initialize(RectTransform rectTransform)
            {
                base.Initialize(rectTransform);
                textMeshProUGUI.alignment = TMPro.TextAlignmentOptions.MidlineLeft;
            }
            public override string UpdateText(ProStats proStats)
            {
                return $"{ProHUDUtil.MinuteSecond(proStats.songLength)}";
            }
        }
        public override IEnumerable<string> ChildElements => new string[] {
            "SongProgress.TimeFraction.Top",
            "SongProgress.TimeFraction.Slash",
            "SongProgress.TimeFraction.Bottom",
        };
    }

    [ProHUDElement("SongProgress.NotesFraction")]
    public class NotesFraction : ProHUDCompoundElement
    {
        [ProHUDElement("SongProgress.NotesFraction.Top", 75, 24)]
        public class Top : ProHUDTextElement
        {
            public override void Initialize(RectTransform rectTransform)
            {
                base.Initialize(rectTransform);
                textMeshProUGUI.alignment = TMPro.TextAlignmentOptions.MidlineRight;
            }
            public override string UpdateText(ProStats proStats)
            {
                return $"{proStats.maxPossibleCurrentCombo}";
            }
        }

        [ProHUDElement("SongProgress.NotesFraction.Slash", 30, 24)]
        public class Slash : ProHUDTextElement
        {
            public override string UpdateText(ProStats proStats)
            {
                return "/";
            }
        }

        [ProHUDElement("SongProgress.NotesFraction.Bottom", 75, 24)]
        public class Bottom : ProHUDTextElement
        {
            public override void Initialize(RectTransform rectTransform)
            {
                base.Initialize(rectTransform);
                textMeshProUGUI.alignment = TMPro.TextAlignmentOptions.MidlineLeft;
            }
            public override string UpdateText(ProStats proStats)
            {
                return $"{proStats.maxPossibleCombo}";
            }
        }
        public override IEnumerable<string> ChildElements => new string[] {
            "SongProgress.NotesFraction.Top",
            "SongProgress.NotesFraction.Slash",
            "SongProgress.NotesFraction.Bottom",
        };
    }

    [ProHUDElement("SongProgress.Bar", 160, 24)]
    public class Bar : ProHUDTwoColorBar
    {
        public override bool SeekLine => true;
        public override float UpdateRatio(ProStats proStats)
        {
            return proStats.songProgress / proStats.songLength;
        }
    }

    public override IEnumerable<string> ChildElements
    {
        get
        {
            switch (Plugin.Config.proHUDConfig.songProgressStyle)
            {
                case ProHUDConfig.ProgressStyle.NotesLeft: return new string[] {
                    "SongProgress.Bar","NewLine",
                    "SongProgress.NotesLeftTitle","NewLine",
                    "SongProgress.NotesLeftValue"
                };
                case ProHUDConfig.ProgressStyle.TimeLeft: return new string[] {
                    "SongProgress.Bar","NewLine",
                    "SongProgress.TimeLeftTitle","NewLine",
                    "SongProgress.TimeLeftValue"
                };
                case ProHUDConfig.ProgressStyle.NotesFraction: return new string[] {
                    "SongProgress.Bar","NewLine",
                    "SongProgress.NotesFraction"
                };
                case ProHUDConfig.ProgressStyle.TimeFraction: return new string[] {
                    "SongProgress.Bar","NewLine",
                    "SongProgress.TimeFraction"
                };
            }
            return new string[] { };
        }
    }

}

