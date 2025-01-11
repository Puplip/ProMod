//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Zenject;
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;
//using BeatSaberMarkupLanguage;
//using System.Collections.Generic;
//using System;
//using CameraUtils.Core;
//using HMUI;

//namespace ProMod.Stats
//{
//    public abstract class ProStat : MonoBehaviour
//    {
//        public abstract void Init(ProStatLocationData statLocationData);
//        public abstract void OnData(ProStatData proStatData);

//        private static Dictionary<string, Type> registeredStats = new Dictionary<string, Type>();

//        public static void RegisterStat<T>() where T : ProStat
//        {
//            if (Exists(typeof(T).Name))
//            {
//                return;
//            }
//            registeredStats.Add(typeof(T).Name, typeof(T));
//            Plugin.Log.Info("Registered Stat: " + typeof(T).Name);
//        }
//        public static bool Exists(string name)
//        {
//            return registeredStats.ContainsKey(name);
//        }

//        public static Type GetStat(string name)
//        {
//            if (!Exists(name))
//            {
//                return null;
//            }
//            return registeredStats[name];
//        }
//        internal static void Init()
//        {

//            RegisterStat<ProStat_HealthBar>();

//            //RegisterStat<ProStat_Acc>();
//            //RegisterStat<ProStat_MaxAcc>();
//            RegisterStat<ProStat_ProAcc>();
//            RegisterStat<ProStat_Combo>();
//            //RegisterStat<ProStat_ComboDamage>();
//            RegisterStat<ProStat_Remaining>();
//            RegisterStat<ProStat_LeftRightCut>();

//            RegisterStat<ProStat_HealthBar>();
//        }
//    }

//    public abstract class ProStatCanvas : ProStat
//    {

//        protected List<TextMeshProUGUI> Texts = new List<TextMeshProUGUI>();



//        protected abstract void InitCanvas();
//        public sealed override void Init(ProStatLocationData statLocationData)
//        {
//            transform.localScale = new Vector3(0.01f, 0.01f, 0f);

//            //gameObject.AddComponent<CurvedCanvasSettings>();
//            Canvas canvas = gameObject.AddComponent<Canvas>();

//            canvas.renderMode = RenderMode.WorldSpace;
//            RectTransform rectTransform = transform as RectTransform;
//            rectTransform.sizeDelta = statLocationData.Size * 100f;
//            rectTransform.pivot = Vector2.one * 0.5f;
//            rectTransform.anchoredPosition3D = statLocationData.Pos;

//            InitCanvas();

//            ProUtil.SetLayerRecursive(gameObject, VisibilityLayer.UI);
//        }
//        protected Image CreateImage(Rect rect, Texture2D texture, Color color)
//        {
//            Image image = new GameObject("ProStatImage").AddComponent<Image>();
//            RectTransform imageTransform = image.transform as RectTransform;
//            imageTransform.SetParent(transform, false);
//            imageTransform.anchorMin = new Vector2(rect.xMin, 1f - rect.yMin);
//            imageTransform.anchorMax = new Vector2(rect.xMax, 1f - rect.yMax);
//            imageTransform.pivot = Vector2.one * 0.5f;
//            imageTransform.sizeDelta = Vector2.zero;
//            imageTransform.anchoredPosition3D = Vector2.zero;

//            image.material = ProUtil.NoGlowNoFogSpriteMaterial;
//            image.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), Vector2.one * 0.5f);
//            image.color = color;

//            return image;
//        }

//        protected TextMeshProUGUI CreateText(Rect rect, string text = "")
//        {

//            var textMeshProUGUI = BeatSaberUI.CreateText<TextMeshProUGUI>(transform as RectTransform, "", Vector2.zero, Vector2.zero);

//            textMeshProUGUI.rectTransform.anchorMin = new Vector2(rect.xMin, 1f - rect.yMin);
//            textMeshProUGUI.rectTransform.anchorMax = new Vector2(rect.xMax, 1f - rect.yMax);

//            textMeshProUGUI.alignment = TextAlignmentOptions.Midline;

//            textMeshProUGUI.fontSize = rect.height * 125f;
//            textMeshProUGUI.lineSpacing = textMeshProUGUI.fontSize * -0.25f;
//            textMeshProUGUI.color = Color.white;

//            return textMeshProUGUI;
//        }

//        #region Helper Functions
//        protected static string IntValue(int v)
//        {
//            return "<size=100%>" + v + "\n";
//        }
//        protected static string MinuteSecondValue(float v)
//        {
//            int seconds = Mathf.RoundToInt(v);
//            return string.Format("<size=100%>{0}:<size=75%>{1:00}", seconds / 60, seconds % 60);
//        }
//        protected static string MillisecondValue(float v)
//        {
//            int ms = Mathf.RoundToInt(v);
//            return string.Format("<size=100%>{0}<size=75%>ms", ms);
//        }
//        protected static string RatioValue(float top, float bottom)
//        {
//            return RatioValue(bottom != 0.0f ? top / bottom : 1.0f);
//        }
//        protected static string SmallRatioValue(float top, float bottom)
//        {
//            return SmallRatioValue(bottom != 0.0f ? top / bottom : 1.0f);
//        }
//        protected static string RatioValue(float v)
//        {
//            int i = Mathf.RoundToInt(v * 10000.0f);
//            return string.Format("<size=100%>{0}<size=75%>{1:00}", i / 100, i % 100);
//        }
//        protected static string SmallRatioValue(float v)
//        {
//            int i = Mathf.RoundToInt(v * 10000.0f);
//            return string.Format("<size=60%>{0}<size=45%>{1:00}", i / 100, i % 100);
//        }

//        protected static string AngleValue(float v)
//        {
//            int i = Mathf.RoundToInt(v * 100.0f);
//            return string.Format("<size=100%>{0}<size=75%>{1:00}<size=80%>°", i / 100, i % 100);
//        }
//        protected static string MillimeterValue(float v)
//        {
//            return string.Format("<size=100%>{0}<size=75%>mm", Mathf.RoundToInt(v));
//        }
//        protected static string RatioColor(float acc)
//        {
//            int roundedAcc = Mathf.RoundToInt(acc * 10000f) / 100;

//            if (Plugin.Config.StatColorsEnabled)
//            {
//                foreach (ProAccColorConfig accColor in Plugin.Config.AccColors)
//                {
//                    if (roundedAcc >= accColor.score)
//                    {
//                        return "<color=#" + ColorUtility.ToHtmlStringRGB(accColor.color) + ">";
//                    }
//                }
//            }
//            return "";
//        }

//        protected static string DoubleRatioColor(float acc1, float acc2)
//        {
//            return "<size=100%> " + RatioColor(acc1) + SmallRatioValue(acc1) + "<size=67%><color=#FFFFFF>|" + RatioColor(acc2) + SmallRatioValue(acc2) + "<size=100%> ";
//        }

//        #endregion

//    }

//    public abstract class ProStatRatioBar : ProStat {

//        protected Image ratioBarFillImage;
//        protected Image ratioBarBorderImage;

//        protected bool showFill = true;
//        protected bool showBorder = true;
//        protected float fillFadeInTime = 0.25f;
//        protected float fillFadeOutTime = 0.5f;
//        protected float borderFadeInTime = 0.25f;
//        protected float borderFadeOutTime = 0.5f;

//        private void Update()
//        {
//            if (showFill)
//            {
//                ratioBarFillImage.CrossFadeAlpha(1f, fillFadeInTime, false);
//            }
//            else
//            {
//                ratioBarFillImage.CrossFadeAlpha(0f, fillFadeOutTime, false);
//            }

//            if (showBorder)
//            {
//                ratioBarBorderImage.CrossFadeAlpha(1f, borderFadeInTime, false);
//            }
//            else
//            {
//                ratioBarBorderImage.CrossFadeAlpha(0f, borderFadeOutTime, false);
//            }
//        }

//        public override void Init(ProStatLocationData statLocationData)
//        {
//            transform.localScale = new Vector3(0.01f, 0.01f, 0f);

//            Canvas canvas = gameObject.AddComponent<Canvas>();
//            canvas.renderMode = RenderMode.WorldSpace;
//            RectTransform rectTransform = transform as RectTransform;

//            rectTransform.sizeDelta = statLocationData.Size * 100.0f;
//            rectTransform.pivot = Vector2.one * 0.5f;
//            rectTransform.anchoredPosition3D = statLocationData.Pos;

//            ratioBarFillImage = new GameObject("RatioBarFill").AddComponent<Image>();
//            ratioBarBorderImage = new GameObject("RatioBarFill").AddComponent<Image>();


//            RectTransform fillRectTransform = ratioBarFillImage.transform as RectTransform;
//            fillRectTransform.SetParent(canvas.transform, false);
//            fillRectTransform.anchorMin = Vector2.one * 0.5f;
//            fillRectTransform.anchorMax = Vector2.one * 0.5f;
//            fillRectTransform.pivot = Vector2.one * 0.5f;
//            fillRectTransform.sizeDelta = new Vector2(statLocationData.Width * 100f - 8f, statLocationData.Height * 100f - 8f);
//            fillRectTransform.anchoredPosition3D = Vector2.zero;

//            RectTransform borderRectTransform = ratioBarBorderImage.transform as RectTransform;
//            borderRectTransform.SetParent(canvas.transform, false);
//            borderRectTransform.anchorMin = Vector2.zero;
//            borderRectTransform.anchorMax = Vector2.one;
//            borderRectTransform.pivot = Vector2.one * 0.5f;
//            borderRectTransform.sizeDelta = Vector2.zero;
//            borderRectTransform.anchoredPosition3D = Vector2.zero;

//            ratioBarFillImage.material = ProUtil.NoGlowNoFogSpriteMaterial;
//            ratioBarBorderImage.material = ProUtil.NoGlowNoFogSpriteMaterial;

//            Texture2D whiteTexture = Texture2D.whiteTexture;
//            ratioBarFillImage.sprite = Sprite.Create(whiteTexture, new Rect(0, 0, whiteTexture.width, whiteTexture.height), Vector2.one * 0.5f);
//            ratioBarFillImage.type = Image.Type.Filled;
//            ratioBarFillImage.fillMethod = Image.FillMethod.Horizontal;
//            ratioBarFillImage.fillAmount = 1.0f;


//            int pixelWidth = Mathf.RoundToInt(statLocationData.Width * 400f);
//            int pixelHeight = Mathf.RoundToInt(statLocationData.Height * 400f);
//            Texture2D borderTexture = new Texture2D(pixelWidth, pixelHeight);
//            for(int x = 0; x < pixelWidth; x++)
//            {
//                for(int y = 0; y < pixelHeight; y++)
//                {
//                    if(x < 8 || y < 8 || x >= pixelWidth - 8 || y >= pixelHeight - 8)
//                    {
//                        borderTexture.SetPixel(x, y, Color.white);
//                    } else
//                    {
//                        borderTexture.SetPixel(x, y, Color.clear);
//                    }
//                }
//            }
//            borderTexture.Apply();

//            ratioBarBorderImage.sprite = Sprite.Create(borderTexture, new Rect(0, 0, borderTexture.width, borderTexture.height), Vector2.one * 0.5f);

//            ProUtil.SetLayerRecursive(gameObject, VisibilityLayer.UI);
//        }

//        public override void OnData(ProStatData proStatData)
//        {
//            ratioBarFillImage.fillAmount = UpdateRatio(proStatData);
//        }

//        public abstract float UpdateRatio(ProStatData proStatData);
//    }
//}