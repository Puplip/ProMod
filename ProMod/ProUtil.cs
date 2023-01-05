using UnityEngine;
using System;

namespace ProMod
{

    public enum Camera2Layer : int
    {
		ThirdPerson = 3,
		Water = 4,
		FirstPerson = 6,
		UI = 5,
		Notes = 8,
		Debris = 9,
		Avatar = 10,
		Walls = 11,
		Sabers = 12,
		CutParticles = 16,
		CustomNotes = 24,
		WallTextures = 25,
		Plattform = 28
	};

	public class ProUtil {

		public static void SetLayerRecursive(GameObject gameObject, Camera2Layer layer)
		{
			Transform transform = gameObject.transform;
			for (int i = 0; i < transform.childCount; i++)
			{
				GameObject child = transform.GetChild(i).gameObject;
				child.layer = (int)layer;
				SetLayerRecursive(child, layer);
			}
		}

	}
	
}