
using System.Text;
using System.Threading.Tasks;
using Zenject;
using UnityEngine;
using TMPro;
using Newtonsoft.Json;
using ProMod.Config;

namespace ProMod.Stats
{
    public enum ProStatLocation
    {
        Custom,

        MainHUD_TopLeft,
        MainHUD_TopRight,

        MainHUD_BottomLeft,
        MainHUD_BottomRight,

        TopBar,
        BottomBar,

        TopBar_Left,
        TopBar_Center,
        TopBar_Right,

        BottomBar_Left,
        BottomBar_Center,
        BottomBar_Right,

        RearLeftPanel_Top,
        RearLeftPanel_Bottom,
        RearLeftPanel_Left,
        RearLeftPanel_Right,

        RearRightPanel_Top,
        RearRightPanel_Bottom,
        RearRightPanel_Left,
        RearRightPanel_Right,

        RearLeftPanel_TopLeft,
        RearLeftPanel_TopRight,
        RearLeftPanel_BottomLeft,
        RearLeftPanel_BottomRight,

        RearRightPanel_TopLeft,
        RearRightPanel_TopRight,
        RearRightPanel_BottomLeft,
        RearRightPanel_BottomRight,

        RearBottom_Left,
        RearBottom_Right
    }
    public class ProStatLocationData
    {
        public ProVector3Serializable Pos = Vector3.zero;

        public ProVector3Serializable Angle = Vector3.zero;

        public float Width = 0.0f;

        public float Height = 0.0f;

        [JsonIgnore]
        public Vector2 Size
        {
            get => new Vector2(Width, Height);
            set { Width = value.x; Height = value.y; }
        }


        public static ProStatLocationData GetLocationData(ProStatLocation location)
        {
            ProStatLocationData ret = new ProStatLocationData();

            Vector3 angleBack = new Vector3(0.0f, 180.0f, 0.0f);
            Vector3 angleUp = new Vector3(14.0f, 0.0f, 0.0f);
            Vector3 angleDown = new Vector3(-14.0f, 0.0f, 0.0f);

            switch (location)
            {
                case ProStatLocation.MainHUD_TopLeft:
                    ret.Pos = new Vector3(-2.5f, 0.5f, 7.0f);
                    ret.Angle = Vector3.zero;
                    ret.Size = new Vector2(1.6f, 1.0f);
                    break;
                case ProStatLocation.MainHUD_TopRight:
                    ret.Pos = new Vector3(2.5f, 0.5f, 7.0f);
                    ret.Angle = Vector3.zero;
                    ret.Size = new Vector2(1.6f, 1.0f);
                    break;
                case ProStatLocation.MainHUD_BottomLeft:
                    ret.Pos = new Vector3(-2.5f, -0.5f, 7.0f);
                    ret.Angle = Vector3.zero;
                    ret.Size = new Vector2(1.6f, 1.0f);
                    break;
                case ProStatLocation.MainHUD_BottomRight:
                    ret.Pos = new Vector3(2.5f, -0.5f, 7.0f);
                    ret.Angle = Vector3.zero;
                    ret.Size = new Vector2(1.6f, 1.0f);
                    break;

                case ProStatLocation.TopBar:
                    ret.Pos = new Vector3(0.0f, 1.5f, 6.0f);
                    ret.Angle = angleDown;
                    ret.Size = new Vector2(2.0f, 1.2f);
                    break;
                case ProStatLocation.BottomBar:
                    ret.Pos = new Vector3(0.0f, -1.5f, 6.0f);
                    ret.Angle = angleUp;
                    ret.Size = new Vector2(2.0f, 1.2f);
                    break;

                case ProStatLocation.TopBar_Left:
                    ret.Pos = new Vector3(-0.6f, 1.5f, 6.0f);
                    ret.Angle = angleDown;
                    ret.Size = new Vector2(1.0f, 0.6f);
                    break;
                case ProStatLocation.TopBar_Center:
                    ret.Pos = new Vector3(0.0f, 1.5f, 6.0f);
                    ret.Angle = angleDown;
                    ret.Size = new Vector2(1.0f, 0.6f);
                    break;
                case ProStatLocation.TopBar_Right:
                    ret.Pos = new Vector3(0.6f, 1.5f, 6.0f);
                    ret.Angle = angleDown;
                    ret.Size = new Vector2(1.0f, 0.6f);
                    break;

                case ProStatLocation.BottomBar_Left:
                    ret.Pos = new Vector3(-0.6f, -1.5f, 6.0f);
                    ret.Angle = angleUp;
                    ret.Size = new Vector2(1.0f, 0.6f);
                    break;
                case ProStatLocation.BottomBar_Center:
                    ret.Pos = new Vector3(0.0f, -1.5f, 6.0f);
                    ret.Angle = angleUp;
                    ret.Size = new Vector2(1.0f, 0.6f);
                    break;
                case ProStatLocation.BottomBar_Right:
                    ret.Pos = new Vector3(0.6f, -1.5f, 6.0f);
                    ret.Angle = angleUp;
                    ret.Size = new Vector2(1.0f, 0.6f);
                    break;


                case ProStatLocation.RearLeftPanel_Top:
                    ret.Pos = new Vector3(1.0f, 0.9f, -4.0f);
                    ret.Angle = angleBack;
                    ret.Size = new Vector2(2.0f, 0.6f);
                    break;
                case ProStatLocation.RearLeftPanel_Bottom:
                    ret.Pos = new Vector3(1.0f, 0.3f, -4.0f);
                    ret.Angle = angleBack;
                    ret.Size = new Vector2(2.0f, 0.6f);
                    break;
                case ProStatLocation.RearLeftPanel_Left:
                    ret.Pos = new Vector3(1.5f, 0.6f, -4.0f);
                    ret.Angle = angleBack;
                    ret.Size = new Vector2(1.0f, 1.2f);
                    break;
                case ProStatLocation.RearLeftPanel_Right:
                    ret.Pos = new Vector3(0.5f, 0.6f, -4.0f);
                    ret.Angle = angleBack;
                    ret.Size = new Vector2(1.0f, 1.2f);
                    break;

                case ProStatLocation.RearRightPanel_Top:
                    ret.Pos = new Vector3(-1.0f, 0.9f, -4.0f);
                    ret.Angle = angleBack;
                    ret.Size = new Vector2(2.0f, 0.6f);
                    break;
                case ProStatLocation.RearRightPanel_Bottom:
                    ret.Pos = new Vector3(-1.0f, 0.3f, -4.0f);
                    ret.Angle = angleBack;
                    ret.Size = new Vector2(2.0f, 0.6f);
                    break;
                case ProStatLocation.RearRightPanel_Left:
                    ret.Pos = new Vector3(-0.5f, 0.6f, -4.0f);
                    ret.Angle = angleBack;
                    ret.Size = new Vector2(1.0f, 1.2f);
                    break;
                case ProStatLocation.RearRightPanel_Right:
                    ret.Pos = new Vector3(-1.5f, 0.6f, -4.0f);
                    ret.Angle = angleBack;
                    ret.Size = new Vector2(1.0f, 1.2f);
                    break;

                case ProStatLocation.RearLeftPanel_TopLeft:
                    ret.Pos = new Vector3(1.5f, 0.9f, -4.0f);
                    ret.Angle = angleBack;
                    ret.Size = new Vector2(1.0f, 0.6f);
                    break;
                case ProStatLocation.RearLeftPanel_TopRight:
                    ret.Pos = new Vector3(0.5f, 0.9f, -4.0f);
                    ret.Angle = angleBack;
                    ret.Size = new Vector2(1.0f, 0.6f);
                    break;
                case ProStatLocation.RearLeftPanel_BottomLeft:
                    ret.Pos = new Vector3(1.5f, 0.3f, -4.0f);
                    ret.Angle = angleBack;
                    ret.Size = new Vector2(1.0f, 0.6f);
                    break;
                case ProStatLocation.RearLeftPanel_BottomRight:
                    ret.Pos = new Vector3(0.5f, 0.3f, -4.0f);
                    ret.Angle = angleBack;
                    ret.Size = new Vector2(1.0f, 0.6f);
                    break;

                case ProStatLocation.RearRightPanel_TopLeft:
                    ret.Pos = new Vector3(-0.5f, 0.9f, -4.0f);
                    ret.Angle = angleBack;
                    ret.Size = new Vector2(1.0f, 0.6f);
                    break;
                case ProStatLocation.RearRightPanel_TopRight:
                    ret.Pos = new Vector3(-1.5f, 0.9f, -4.0f);
                    ret.Angle = angleBack;
                    ret.Size = new Vector2(1.0f, 0.6f);
                    break;
                case ProStatLocation.RearRightPanel_BottomLeft:
                    ret.Pos = new Vector3(-0.5f, 0.3f, -4.0f);
                    ret.Angle = angleBack;
                    ret.Size = new Vector2(1.0f, 0.6f);
                    break;
                case ProStatLocation.RearRightPanel_BottomRight:
                    ret.Pos = new Vector3(-1.5f, 0.3f, -4.0f);
                    ret.Angle = angleBack;
                    ret.Size = new Vector2(1.0f, 0.6f);
                    break;

                case ProStatLocation.RearBottom_Left:
                    ret.Pos = new Vector3(1.0f, 0.6f, -4.0f);
                    ret.Angle = angleBack;
                    ret.Size = new Vector2(2.0f, 1.2f);
                    break;
                case ProStatLocation.RearBottom_Right:
                    ret.Pos = new Vector3(-1.0f, 0.6f, -4.0f);
                    ret.Angle = angleBack;
                    ret.Size = new Vector2(2.0f, 1.2f);
                    break;
            }


            return ret;
        }

    }
}