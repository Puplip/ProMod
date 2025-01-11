using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ProMod.HUD
{
    public class ProHUDUtil
    {
        public static string Ratio(float ratio)
        {
            int n = Mathf.RoundToInt(ratio * 10000f);
            string sign = n < 0 ? "-" : "";
            n = Math.Abs(n);
            return $"<size=100%>{sign}{n / 100}<size=75%>{n % 100:D2}";
        }
        public static string SignedRatio(float ratio)
        {
            int n = Mathf.RoundToInt(ratio * 10000f);
            string sign = n < 0 ? "-" : "+";
            n = Math.Abs(n);
            return $"<size=100%>{sign}{n / 100}<size=75%>{n % 100:D2}";
        }

        public static string Percent(float ratio)
        {
            int n = Mathf.RoundToInt(ratio * 1000f);
            string sign = n < 0 ? "-" : "";
            n = Math.Abs(n);
            return $"<size=75%>{sign}{n / 10}.{n % 10:D1}%";
        }
        public static string Degrees(float degrees)
        {
            int n = Mathf.RoundToInt(degrees * 10f);
            string sign = n < 0 ? "-" : "";
            n = Math.Abs(n);
            return $"<size=100%>{sign}{n / 10}.{n % 10:D1}°";
        }

        public static string MinuteSecond(float timeSeconds)
        {

            int n = Mathf.RoundToInt(timeSeconds);
            string sign = n < 0 ? "-" : "";
            n = Math.Abs(n);
            return $"<size=100%>{sign}{n / 60}:{n % 60:D2}";
        }

        public static string Float2(float value)
        {
            int n = Mathf.RoundToInt(value * 100f);
            string sign = n < 0 ? "-" : "";
            n = Mathf.Abs(n);
            return $"<size=100%>{sign}{n / 100}<size=75%>{(n % 100):D2}";
        }

        public static string Millimeters(float meters)
        {
            int n = Mathf.RoundToInt(meters * 1000f);
            string sign = n < 0 ? "-" : "";
            n = Mathf.Abs(n);
            return $"<size=100%>{sign}{n}<size=67%>mm";
        }

        public static string Centimeters(float meters)
        {
            int n = Mathf.RoundToInt(meters * 1000f);
            string sign = n < 0 ? "-" : "";
            n = Mathf.Abs(n);
            return $"<size=100%>{sign}{n / 10}.{n % 10:D1}<size=67%>cm";
        }

        public static Color AccColor(int accPercentInteger)
        {
            if (Plugin.Config.proHUDConfig.accColorsEnabled)
            {
                int roundedAcc = accPercentInteger;

                foreach (ProAccColorPointConfig accColor in Plugin.Config.proHUDConfig.accColorPoints)
                {
                    if (roundedAcc >= accColor.accuracy)
                    {
                        return accColor.color;
                    }
                }
            }
            return Color.white;
        }
        public static Color AccColor(float ratio)
        {
            return AccColor(Mathf.RoundToInt(ratio * 10000f) / 100);
        }

        public static string AccColorString(float ratio)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGB(AccColor(ratio))}>";
        }

        public static string AccColorRatio(float ratio)
        {
            if (Plugin.Config.proHUDConfig.accColorsEnabled)
            {
                return AccColorString(ratio) + Ratio(ratio);
            }

            return Ratio(ratio);
        }

    }
}
