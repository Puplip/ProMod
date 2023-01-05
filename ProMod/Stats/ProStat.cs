using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;
using UnityEngine;
using TMPro;
using BeatSaberMarkupLanguage;
using System.Collections.Generic;
using System;

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

            RegisterStat<ProStat_LeftTimeDependence>();
            RegisterStat<ProStat_RightTimeDependence>();

            RegisterStat<ProStat_LeftAcc>();
            RegisterStat<ProStat_RightAcc>();

            RegisterStat<ProStat_LeftAimScore>();
            RegisterStat<ProStat_RightAimScore>();
            RegisterStat<ProStat_LeftSwingScore>();
            RegisterStat<ProStat_RightSwingScore>();
            RegisterStat<ProStat_LeftPreSwingScore>();
            RegisterStat<ProStat_RightPreSwingScore>();
            RegisterStat<ProStat_LeftPostSwingScore>();
            RegisterStat<ProStat_RightPostSwingScore>();


            RegisterStat<ProStat_LeftAimDistance>();
            RegisterStat<ProStat_RightAimDistance>();

            RegisterStat<ProStat_LeftAimDamage>();
            RegisterStat<ProStat_RightAimDamage>();

            RegisterStat<ProStat_LeftSwingDamage>();
            RegisterStat<ProStat_RightSwingDamage>();
            RegisterStat<ProStat_LeftPreSwingDamage>();
            RegisterStat<ProStat_RightPreSwingDamage>();
            RegisterStat<ProStat_LeftPostSwingDamage>();
            RegisterStat<ProStat_RightPostSwingDamage>();
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
            gameObject.layer = (int)Camera2Layer.UI;
            transform.localPosition = statLocationData.Pos;
            transform.localEulerAngles = statLocationData.Angle;
            transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

            Canvas canvas = gameObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            RectTransform rectTransform = transform as RectTransform;
            rectTransform.sizeDelta = statLocationData.Size * 100.0f;
            Plugin.Log.Info("rectTransform.sizeDelta: [" + rectTransform.sizeDelta.x + ", " + rectTransform.sizeDelta.y + "]");

            text = BeatSaberUI.CreateText((RectTransform)canvas.transform, "", Vector2.zero);
            text.rectTransform.sizeDelta = statLocationData.Size * 100.0f;
            text.fontSize = statLocationData.Size.y * 70.0f;
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
        protected string RatioValue(float top,float bottom)
        {
            return RatioValue(bottom != 0.0f ? top / bottom : 1.0f);
        }
        protected string RatioValue(float v)
        {
            int i = Mathf.RoundToInt(v * 10000.0f);
            return string.Format("<size=100%>{0}<size=60%>{1:00}", Mathf.FloorToInt(i / 100), Mathf.RoundToInt(i % 100));
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
            if (Plugin.Config.StatColorsEnabled)
            {
                foreach (Config.ProAccColorConfig accColor in Plugin.Config.AccColors)
                {
                    if (Mathf.FloorToInt(acc * 100.0f) >= accColor.score)
                    {
                        return "<color=#" + ColorUtility.ToHtmlStringRGB(accColor.color) + ">";
                    }
                }
            }
            return "";
        }
        protected override string UpdateText(ProStatData proStatData)
        {
            return "<line-height=55%><size=30%>" + Title + "\n" + Value(proStatData);
        }
    }
}