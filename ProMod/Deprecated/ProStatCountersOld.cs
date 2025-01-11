//using BeatSaberMarkupLanguage;
//using IPA.Utilities;
//using System.Collections.Generic;
//using TMPro;
//using UnityEngine;
//using UnityEngine.PlayerLoop;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

//namespace ProMod.Stats
//{
//    /*    public class ProStat_InstantAccBar : ProStatRatioBar
//        {
//            public override float UpdateRatio(ProStatData proStatData)
//            {
//                return proStatData.acc;
//            }
//        }*/

//    //public class ProStat_MaxAcc : ProStatTitleValue
//    //{
//    //    protected override string Title { get; set; } = "Max ACC";
//    //    protected override string Value(ProStatData proStatData)
//    //    {
//    //        float maxAcc = proStatData.maxBeatmapScore > 0 ? (float)(proStatData.maxBeatmapScore - proStatData.maxCurrentScore + proStatData.score) / (float)proStatData.maxBeatmapScore : 1.0f;
//    //        return RatioColor(maxAcc) + RatioValue(maxAcc);
//    //    }
//    //}

//    public class ProStat_ProAcc : ProStatCanvas
//    {

//        TextMeshProUGUI title;
//        TextMeshProUGUI bigAcc;
//        TextMeshProUGUI smallAcc;
//        TextMeshProUGUI fcAcc;

//        protected override void InitCanvas()
//        {
//            title = CreateText(new Rect(0f, 0f, 1f, 0.2f));
//            bigAcc = CreateText(new Rect(0f, 0.2f, 1f, 0.8f));
//            smallAcc = CreateText(new Rect(0f, 0.2f, 1f, 0.6f));
//            fcAcc = CreateText(new Rect(0f, 0.8f, 1f, 0.2f));
//        }
//        public override void OnData(ProStatData proStatData)
//        {

//            if (proStatData.nonAccDamage > 0)
//            {
//                title.text = "EST ACC";
//                float estimatedFinalAcc = ((float)(proStatData.maxBeatmapScore - proStatData.maxCurrentScore) * proStatData.fullComboAcc + (float)proStatData.score) / (float)proStatData.maxBeatmapScore;
//                float fullComboAcc = proStatData.fullComboAcc;
//                bigAcc.text = "";
//                smallAcc.text = RatioColor(estimatedFinalAcc) + RatioValue(estimatedFinalAcc);
//                fcAcc.text = "FC " + RatioColor(fullComboAcc) + RatioValue(fullComboAcc);
//            }
//            else
//            {
//                float acc = proStatData.acc;
//                title.text = "ACC";
//                bigAcc.text = RatioColor(acc) + RatioValue(acc);
//                smallAcc.text = "";
//                fcAcc.text = "";
//            }

//        }
//    }

//    public class ProStat_Combo : ProStatCanvas
//    {
//        private List<Rect> textRects = new List<Rect>() {
//            new Rect(0f,0f,1f,0.2f),
//            new Rect(0f,0.2f,1f,0.6f),
//            new Rect(0f,0.2f,1f,0.8f),
//            new Rect(0f,0.8f,1f,0.2f)
//        };

//        TextMeshProUGUI title;
//        TextMeshProUGUI bigCombo;
//        TextMeshProUGUI smallCombo;
//        TextMeshProUGUI errors;

//        protected override void InitCanvas()
//        {
//            title = CreateText(new Rect(0f, 0f, 1f, 0.2f));
//            bigCombo = CreateText(new Rect(0f, 0.2f, 1f, 0.8f));
//            smallCombo = CreateText(new Rect(0f, 0.2f, 1f, 0.6f));
//            errors = CreateText(new Rect(0f, 0.8f, 1f, 0.2f));
//        }
//        public override void OnData(ProStatData proStatData)
//        {

//            if (proStatData.fullCombo)
//            {
//                title.text = "Full Combo";
//                bigCombo.text = IntValue(proStatData.combo);
//                smallCombo.text = "";
//                errors.text = "";
//            }
//            else
//            {
//                float acc = proStatData.acc;
//                title.text = "Combo";
//                bigCombo.text = "";
//                smallCombo.text = IntValue(proStatData.combo);
//                errors.text = "Errors: " + IntValue(proStatData.comboBreaks);
//            }

//        }
//    }

//    public class ProStat_Remaining : ProStatCanvas
//    {
//        private List<Rect> textRects = new List<Rect>() {
//            new Rect(0f,0f,0.3f,0.5f),
//            new Rect(0f,0.5f,0.3f,0.5f),
//            new Rect(0.3f,0f,0.70f,0.5f),
//            new Rect(0.3f,0.5f,0.70f,0.5f)
//        };

//        TextMeshProUGUI timeTitle;
//        TextMeshProUGUI notesTitle;
//        TextMeshProUGUI time;
//        TextMeshProUGUI notes;

//        protected override void InitCanvas()
//        {
//            timeTitle = CreateText(new Rect(0f, 0f, 0.3f, 0.5f));
//            notesTitle = CreateText( new Rect(0f, 0.5f, 0.3f, 0.5f));
//            time = CreateText( new Rect(0.3f, 0f, 0.70f, 0.5f));
//            notes = CreateText( new Rect(0.3f, 0.5f, 0.70f, 0.5f));
//        }

//        public override void OnData(ProStatData proStatData)
//        {

//            timeTitle.text = "<size=35%>Time\nLeft";
//            notesTitle.text = "<size=35%>Notes\nLeft";
//            time.text = MinuteSecondValue(proStatData.songLength - proStatData.songTime);
//            notes.text = IntValue(proStatData.beatmapNoteCount - proStatData.scoredNoteCount);

//        }
//    }


//    public class ProStat_LeftRightCut : ProStatCanvas
//    {
//        private List<Rect> textRects = new List<Rect>() {
//            new Rect(0f,0f,0.4f,0.25f),
//            new Rect(0.4f,0f,0.2f,0.25f),
//            new Rect(0.6f,0f,0.4f,0.25f),

//            new Rect(0f,0.25f,0.4f,0.25f),
//            new Rect(0.4f,0.25f,0.2f,0.25f),
//            new Rect(0.6f,0.25f,0.4f,0.25f),

//            new Rect(0f,0.5f,0.4f,0.25f),
//            new Rect(0.4f,0.5f,0.2f,0.25f),
//            new Rect(0.6f,0.5f,0.4f,0.25f),

//            new Rect(0f,0.75f,0.4f,0.25f),
//            new Rect(0.4f,0.75f,0.2f,0.25f),
//            new Rect(0.6f,0.75f,0.4f,0.25f)
//        };


//        TextMeshProUGUI accLeft, accTitle, accRight;
//        TextMeshProUGUI tdLeft, tdTitle, tdRight;
//        TextMeshProUGUI bsLeft, bsTitle, bsRight;
//        TextMeshProUGUI asLeft, asTitle, asRight;


//        protected override void InitCanvas()
//        {
//            accLeft = CreateText(new Rect(0f, 0f, 0.4f, 0.25f));
//            accTitle = CreateText(new Rect(0.4f, 0f, 0.2f, 0.25f));
//            accRight = CreateText(new Rect(0.6f, 0f, 0.4f, 0.25f));

//            tdLeft = CreateText(new Rect(0f, 0.25f, 0.4f, 0.25f));
//            tdTitle = CreateText(new Rect(0.4f, 0.25f, 0.2f, 0.25f));
//            tdRight = CreateText(new Rect(0.6f, 0.25f, 0.4f, 0.25f));

//            bsLeft = CreateText(new Rect(0f, 0.5f, 0.4f, 0.25f));
//            bsTitle = CreateText(new Rect(0.4f, 0.5f, 0.2f, 0.25f));
//            bsRight = CreateText(new Rect(0.6f, 0.5f, 0.4f, 0.25f));

//            asLeft = CreateText(new Rect(0f, 0.75f, 0.4f, 0.25f));
//            asTitle = CreateText(new Rect(0.4f, 0.75f, 0.2f, 0.25f));
//            asRight = CreateText(new Rect(0.6f, 0.75f, 0.4f, 0.25f));
//        }

//        public override void OnData(ProStatData proStatData)
//        {
//            float leftAcc = proStatData.SaberAcc(SaberType.SaberA);
//            float rightAcc = proStatData.SaberAcc(SaberType.SaberB);

//            accLeft.text = RatioColor(leftAcc) + RatioValue(leftAcc);
//            accTitle.text = "ACC";
//            accRight.text = RatioColor(rightAcc) + RatioValue(rightAcc);


//            tdLeft.text = AngleValue(proStatData.CutStatsBySaber[SaberType.SaberA].TimeDependenceAngle.Average());
//            tdTitle.text = "TD";
//            tdRight.text = AngleValue(proStatData.CutStatsBySaber[SaberType.SaberB].TimeDependenceAngle.Average());


//            bsLeft.text = RatioValue(proStatData.CutStatsBySaber[SaberType.SaberA].PreSwingRating.Average());
//            bsTitle.text = "BS";
//            bsRight.text = RatioValue(proStatData.CutStatsBySaber[SaberType.SaberB].PreSwingRating.Average());


//            asLeft.text = RatioValue(proStatData.CutStatsBySaber[SaberType.SaberA].PostSwingRating.Average());
//            asTitle.text = "AS";
//            asRight.text = RatioValue(proStatData.CutStatsBySaber[SaberType.SaberB].PostSwingRating.Average());
            
//        }
//    }

//    public class ProStat_HealthBar : ProStatRatioBar
//    {

//        protected TextMeshProUGUI text;
//        public override void Init(ProStatLocationData statLocationData)
//        {
//            base.Init(statLocationData);

//            text = BeatSaberUI.CreateText((RectTransform)gameObject.GetComponent<Canvas>().transform, "", Vector2.zero);
//            text.rectTransform.sizeDelta = statLocationData.Size * 100.0f;
//            text.fontSize = statLocationData.Size.y * 62.0f;
//            text.alignment = TextAlignmentOptions.Center;
//        }

//        public override float UpdateRatio(ProStatData proStatData)
//        {
//            Canvas canvas = gameObject.GetComponent<Canvas>();
//            showFill = proStatData.energy < 1.0f && !proStatData.failed;
//            showBorder = proStatData.energy < 1.0f || proStatData.failed;
//            if (proStatData.failed)
//            {
//                text.text = "Failed";
//            }
//            else
//            {
//                text.text = "";
//            }

//            return proStatData.energy;
//        }
//    }

//}