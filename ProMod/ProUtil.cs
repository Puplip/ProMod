using UnityEngine;
using CameraUtils.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.IO;
using IPA.Utilities;
using System.Linq;
using HarmonyLib;
using System.Reflection.Emit;
using JetBrains.Annotations;
using System;

namespace ProMod;

public class ProUtil
{




    public class HSV
    {
        public float h;
        public float s;
        public float v;

        public static implicit operator Color(HSV hsv)
        {
            return Color.HSVToRGB(hsv.h, hsv.s, hsv.v);
        }
        public static implicit operator HSV(Color color)
        {
            HSV hsv = new HSV();
            Color.RGBToHSV(color, out hsv.h, out hsv.s, out hsv.v);

            return hsv;
        }
        public static HSV Lerp(HSV hsv1, HSV hsv2, float ratio)
        {
            float hDelta = hsv2.h - hsv1.h;
            if (hDelta > 0.5f)
            {
                hDelta -= 1f;
            }
            if (hDelta < -0.5f)
            {
                hDelta += 1f;
            }

            HSV hsv = new HSV();
            hsv.h = hsv1.h + hDelta * ratio;
            while (hsv.h > 1f)
            {
                hsv.h -= 1f;
            }

            while (hsv.h < 0f)
            {
                hsv.h += 1f;
            }

            hsv.s = hsv1.s + (hsv2.s - hsv1.s) * ratio;
            hsv.v = hsv1.v + (hsv2.v - hsv1.v) * ratio;

            return hsv;
        }

    }


    public const float MIN_RT_MILLISECONDS = 100.0f;
    public const float MIN_NJS = 1.0f;

    private static Material _noGlowNoFogSpriteMaterial = null;
    public static Material NoGlowNoFogSpriteMaterial => _noGlowNoFogSpriteMaterial != null ? _noGlowNoFogSpriteMaterial : _noGlowNoFogSpriteMaterial = (from m in Resources.FindObjectsOfTypeAll<Material>() where m.name == "NoGlowNoFogSprite" select m).First();

    public static int InsertInstructionsAtOpCodePattern(List<CodeInstruction> method, List<List<OpCode>> pattern, List<CodeInstruction> insertCode, int startIndex = 0)
    {
        int insertions = 0;
        int pos;
        while ((pos = FindOpCodePattern(method, pattern, startIndex)) != -1)
        {
            Plugin.Log.Info($"Found Pattern at [{pos}]");
            insertions++;
            method.InsertRange(pos, insertCode);
            startIndex = pos + insertCode.Count + pattern.Count;
            Plugin.Log.Info($"NextSearch start [{startIndex}]");
        }
        return insertions;
    }

    public static bool InsertInstructionsAtOpCodePatternOnce(List<CodeInstruction> method, List<List<OpCode>> pattern, List<CodeInstruction> insertCode, int startIndex = 0)
    {
        int pos = startIndex;
        if ((pos = FindOpCodePattern(method, pattern, pos)) != -1)
        {
            method.InsertRange(pos, insertCode);
            return true;
        }
        return false;
    }

    public static List<int> FindAllOpCodePattern(List<CodeInstruction> method, List<List<OpCode>> pattern, int startIndex = 0)
    {
        List<int> result = new List<int>();
        int pos = startIndex;
        while ((pos = FindOpCodePattern(method, pattern, pos)) != -1)
        {
            result.Add(pos);
            pos += pattern.Count;
        }
        return result;
    }

    public static int FindOpCodePattern(List<CodeInstruction> method, List<List<OpCode>> pattern, int startIndex = 0)
    {

        for (int i = startIndex; i <= method.Count - pattern.Count; i++)
        {
            bool foundPattern = true;
            for (int j = 0; j < pattern.Count; j++)
            {

                bool opCodeOk = false;

                for (int k = 0; k < pattern[j].Count; k++)
                {
                    if (method[i + j].opcode == pattern[j][k])
                    {
                        opCodeOk = true;
                        break;
                    }
                }

                if (!opCodeOk)
                {
                    foundPattern = false;
                    break;
                }
            }

            if (foundPattern)
            {
                return i;
            }
        }

        return -1;

    }



    public static float CalculateJumpDistanceSeconds(float reactionTimeSeconds, float noteJumpSpeed)
    {
        return reactionTimeSeconds * 2.0f * noteJumpSpeed;
    }
    public static float CalculateJumpDistanceMilliseconds(float reactionTimeMilliseconds, float noteJumpSpeed)
    {
        return reactionTimeMilliseconds / 500.0f * noteJumpSpeed;
    }
    public static float CalculateReactionTimeSeconds(float jumpDistance, float noteJumpSpeed)
    {
        return jumpDistance / noteJumpSpeed / 2.0f;
    }
    public static float CalculateReactionTimeMilliseconds(float jumpDistance, float noteJumpSpeed)
    {
        return jumpDistance / noteJumpSpeed * 500.0f;
    }
    public static void SetLayerRecursive(GameObject gameObject, VisibilityLayer layer)
    {
        gameObject.layer = (int)layer;
        foreach (Transform childTransform in gameObject.transform)
        {
            SetLayerRecursive(childTransform.gameObject, layer);
        }
    }

    public static void SetActiveRecursive(GameObject gameObject, bool active)
    {
        gameObject.SetActive(active);

        foreach (Transform childTransform in gameObject.transform)
        {
            GameObject childGameObject = childTransform.gameObject;
            childGameObject.SetActive(active);
            SetActiveRecursive(childGameObject, active);
        }
    }

    public static string GetGameObjectPath(GameObject gameObject)
    {
        string path = gameObject.name;
        while (gameObject.transform.parent != null)
        {
            gameObject = gameObject.transform.parent.gameObject;
            path = gameObject.name + "/" + path;
        }
        return path;
    }

    public class GameObjectInfo
    {
        [JsonConverter(typeof(StringEnumConverter)), JsonProperty("Layer")]
        public VisibilityLayer layer = VisibilityLayer.Default;

        [JsonProperty("Path")]
        public string path = "";

        [JsonProperty("Components")]
        public List<string> componentNames = new List<string>();


        [JsonProperty("Children")]
        public List<GameObjectInfo> childGameObjects = new List<GameObjectInfo>();

        public GameObjectInfo(GameObject gameObject)
        {
            path = GetGameObjectPath(gameObject);
            layer = (VisibilityLayer)gameObject.layer;

            foreach (Component component in gameObject.GetComponents<Component>())
            {
                componentNames.Add(component.GetType().FullName);
            }

            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                childGameObjects.Add(new GameObjectInfo(gameObject.transform.GetChild(i).gameObject));
            }
        }
    }

    public static void PrintCameraMask(Camera camera)
    {
        Plugin.Log.Info("Camera: " + GetGameObjectPath(camera.gameObject));
        for (int i = 0; i < 32; i++)
        {
            if ((camera.cullingMask & (1 << i)) != 0)
            {
                Plugin.Log.Info(((VisibilityLayer)i).ToString());
            }
        }
    }


    public static void DumpAllGameObjectsWithComponent<T>(string dumpName, int layerMask = 0x7fffffff) where T : Component
    {
        List<GameObjectInfo> data = new List<GameObjectInfo>();

        foreach (T component in Resources.FindObjectsOfTypeAll<T>())
        {
            if (component.gameObject.activeInHierarchy && (layerMask & (1 << component.gameObject.layer)) != 0)
            {
                data.Add(new GameObjectInfo(component.gameObject));
            }
        }

        string dumpFolderPath = Path.Combine(UnityGame.UserDataPath, "ProModDumps");

        if (!Directory.Exists(dumpFolderPath))
        {
            Directory.CreateDirectory(dumpFolderPath);
        }

        string outputPath = Path.Combine(dumpFolderPath, dumpName + ".json");
        int dumpIndex = 1;

        while (File.Exists(outputPath))
        {
            outputPath = Path.Combine(dumpFolderPath, dumpName + dumpIndex + ".json");
            dumpIndex++;
        }
        File.WriteAllText(outputPath, JsonConvert.SerializeObject(data, Formatting.Indented));
    }


}
