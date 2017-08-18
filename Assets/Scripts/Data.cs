using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace InterSceneData
{
	/**
		Static Class to keep data between scenes and scripts
	**/
	public static class Data {

		public static int currentLevel = 1;
		public static bool gameLoaded = false;
		public static float sensitivityX = 7.0F;
		public static float sensitivityY = 7.0F;
		public static float volume = 1.0F;
		public static float sfx = 2.0F;
	}
}

