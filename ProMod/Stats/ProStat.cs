using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BeatSaberMarkupLanguage;
using System.Collections.Generic;
using System;
using CameraUtils.Core;

namespace ProMod.Stats
{
    public abstract class ProStat : MonoBehaviour
    {
        public abstract void Init(ProStatLocationData statLocationData);
        public abstract void OnData(ProStatData proStatData);

        private static Dictionary<string, Type> registeredStats = new Dictionary<string, Type>();

        public static void RegisterStat<T>() where T : ProStat
        {
            if (Exists(typeof(T).Name))
            {
                return;
            }
            registeredStats.Add(typeof(T).Name, typeof(T));
            Plugin.Log.Info("Registered Stat: " + typeof(T).Name);
        }
        public static bool Exists(string name)
        {
            return registeredStats.ContainsKey(name);
        }

        public static Type GetStat(string name)
        {
            if (!Exists(name))
            {
                return null;
            }
            return registeredStats[name];
        }
        internal static void Init()
        {
            RegisterStat<ProStat_InstantAcc>();
            RegisterStat<ProStat_MaxAcc>();
            RegisterStat<ProStat_EstimateAcc>();
            RegisterStat<ProStat_Combo>();
            RegisterStat<ProStat_ComboDamage>();
            RegisterStat<ProStat_TimeLeft>();
            RegisterStat<ProStat_LeftRightAcc>();/*
            RegisterStat<ProStat_InstantAccBar>();*/
        }
    }

    public abstract class ProStatEmpty : ProStat
    {
        public override void Init(ProStatLocationData statLocationData)
        {
            return;
        }
        public override void OnData(ProStatData proStatData)
        {
            return;
        }
    }

    public abstract class ProStatTextBox : ProStat
    {
        protected TextMeshProUGUI text;
        protected abstract string UpdateText(ProStatData proStatData);
        public override void Init(ProStatLocationData statLocationData)
        {
            gameObject.layer = (int)VisibilityLayer.UI;
            transform.localPosition = statLocationData.Pos;
            transform.localEulerAngles = statLocationData.Angle;
            transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

            Canvas canvas = gameObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            RectTransform rectTransform = transform as RectTransform;
            rectTransform.sizeDelta = statLocationData.Size * 100.0f;

            text = BeatSaberUI.CreateText((RectTransform)canvas.transform, "", Vector2.zero);
            text.rectTransform.sizeDelta = statLocationData.Size * 100.0f;
            text.fontSize = statLocationData.Size.y * 62.0f;
            text.alignment = TextAlignmentOptions.Center;

        }
        public override void OnData(ProStatData proStatData)
        {
            text.text = UpdateText(proStatData);
        }
    }

    public abstract class ProStatTitleValue : ProStatTextBox
    {
        protected virtual string Title { get; set; } = "Title";
        protected virtual string Value(ProStatData proStatData)
        {
            return RatioValue(1.0f);
        }
        protected static string IntValue(int v)
        {
            return "<size=100%>" + v + "\n";
        }
        protected static string MinuteSecondValue(float v)
        {
            int seconds = Mathf.RoundToInt(v);
            return string.Format("<size=100%>{0}<size=80%>:<size=60%>{1:00}", seconds / 60, seconds % 60);
        }
        protected static string MillisecondValue(float v)
        {
            int ms = Mathf.RoundToInt(v);
            return string.Format("<size=100%>{0}<size=60%>ms", ms);
        }
        protected static string RatioValue(float top,float bottom)
        {
            return RatioValue(bottom != 0.0f ? top / bottom : 1.0f);
        }
        protected static string SmallRatioValue(float top, float bottom)
        {
            return SmallRatioValue(bottom != 0.0f ? top / bottom : 1.0f);
        }
        protected static string RatioValue(float v)
        {
            int i = Mathf.RoundToInt(v * 10000.0f);
            return string.Format("<size=100%>{0}<size=60%>{1:00}", Mathf.FloorToInt(i / 100), Mathf.RoundToInt(i % 100));
        }
        protected static string SmallRatioValue(float v)
        {
            int i = Mathf.RoundToInt(v * 10000.0f);
            return string.Format("<size=60%>{0}<size=40%>{1:00}", Mathf.FloorToInt(i / 100), Mathf.RoundToInt(i % 100));
        }

        protected static string AngleValue(float v)
        {
            int i = Mathf.RoundToInt(v * 100.0f);
            return string.Format("<size=100%>{0}<size=60%>{1:00}<size=80%>°", Mathf.FloorToInt(i / 100), Mathf.RoundToInt(i % 100));
        }
        protected static string MillimeterValue(float v)
        {
            return string.Format("<size=100%>{0}<size=60%>mm", Mathf.RoundToInt(v));
        }
        protected static string RatioColor(float acc)
        {
            float roundedAcc = Mathf.Round(acc * 10000f) / 10000f;

            if (Plugin.Config.StatColorsEnabled)
            {
                foreach (Config.ProAccColorConfig accColor in Plugin.Config.AccColors)
                {
                    if (Mathf.FloorToInt(roundedAcc * 100.0f) >= accColor.score)
                    {
                        return "<color=#" + ColorUtility.ToHtmlStringRGB(accColor.color) + ">";
                    }
                }
            }
            return "";
        }

        protected static string DoubleRatioColor(float acc1,float acc2)
        {
            return RatioColor(acc1) + SmallRatioValue(acc1) + "<size=60%><color=#FFFFFF>|" + RatioColor(acc2) + SmallRatioValue(acc2);
        }

        protected override string UpdateText(ProStatData proStatData)
        {
            return "<line-height=55%><size=30%>" + Title + "\n" + Value(proStatData);
        }
    }

    public abstract class ProStatRatioBar : ProStat {

        protected Image ratioBarImage; 
        public override void Init(ProStatLocationData statLocationData)
        {
            gameObject.layer = (int)VisibilityLayer.UI;
            transform.localPosition = statLocationData.Pos;
            transform.localEulerAngles = statLocationData.Angle;
            transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

            Canvas canvas = gameObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            RectTransform rectTransform = transform as RectTransform;
            rectTransform.sizeDelta = statLocationData.Size * 100.0f;

            ratioBarImage = new GameObject("RatioBarImage").AddComponent<Image>();
            RectTransform imageRectTransform = ratioBarImage.transform as RectTransform;
            imageRectTransform.SetParent(canvas.transform,false);
            imageRectTransform.sizeDelta = statLocationData.Size * 100.0f;

            Texture2D whiteTex = Texture2D.whiteTexture;
            ratioBarImage.sprite = Sprite.Create(whiteTex, new Rect(0, 0, whiteTex.width, whiteTex.height), Vector2.one * 0.5f, 100, 1);
            ratioBarImage.type = Image.Type.Filled;
            ratioBarImage.fillMethod = Image.FillMethod.Horizontal;
            ratioBarImage.color = Color.white;
            ratioBarImage.fillAmount = 1.0f;
        }

        public override void OnData(ProStatData proStatData)
        {
            ratioBarImage.fillAmount = UpdateRatio(proStatData);
        }

        public abstract float UpdateRatio(ProStatData proStatData);
    }
}