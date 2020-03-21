﻿using UnityEngine;

namespace EmuLib.Hooks
{
	public class Loader
	{
		public static GameObject HookObject
		{
			get
			{
				GameObject result = GameObject.Find("Application (Main Client)");

				if (result == null)
				{
					result = new GameObject("EmuInstance");
					Object.DontDestroyOnLoad(result);
				}

				return result;
			}
		}

		public static void Load()
		{
			HookObject.GetOrAddComponent<EmuInstance>();
		}
	}
}
