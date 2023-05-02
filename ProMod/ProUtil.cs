using UnityEngine;
using System;
using CameraUtils.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.IO;
using IPA.Utilities;

namespace ProMod
{

	public class ProUtil {

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
        }

		public static void PrintCameraMask(Camera camera)
        {
			Plugin.Log.Info("Camera: " + GetGameObjectPath(camera.gameObject));
			for(int i = 0; i < 32; i++)
            {
				if((camera.cullingMask & (1 << i)) != 0)
                {
					Plugin.Log.Info(((VisibilityLayer)i).ToString());
                }
			}
        }


		public static void DumpAllGameObjectsWithComponent<T>(string dumpName,int layerMask = 0x7fffffff) where T : Component
        {
			List<GameObjectInfo> data = new List<GameObjectInfo>();

			foreach(T component in Resources.FindObjectsOfTypeAll<T>())
            {
                if (component.gameObject.activeInHierarchy && (layerMask & (1<<component.gameObject.layer)) != 0)
                {
					GameObjectInfo gameObjectInfo = new GameObjectInfo();
					gameObjectInfo.layer = (VisibilityLayer)component.gameObject.layer;
					gameObjectInfo.path = GetGameObjectPath(component.gameObject);
					foreach(Component goComponent in component.GetComponents<Component>())
                    {
						gameObjectInfo.componentNames.Add(goComponent.GetType().FullName);
					}
					data.Add(gameObjectInfo);
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
	
}