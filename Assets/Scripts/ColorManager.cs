using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Colors
{
	public static class ColorManager {

		public enum ColorName
		{
			Transparent,
			Red,
			Orange,
			Yellow,
			Green,
			Blue,
			Purple,
			Black,
			White
		};

		public static Color RED = Color.red;
		public static Color ORANGE = new Color(1,0.5f,0,1);
		public static Color YELLOW = Color.yellow;
		public static Color GREEN = Color.green;
		public static Color BLUE = Color.blue;
		public static Color PURPLE = Color.magenta;
		public static Color BLACK = Color.black;
		public static Color WHITE = Color.white;


		public static Color findColor(ColorName colorName) {
			switch (colorName)
			{
				case ColorName.Red:
					return RED;
				case ColorName.Orange:
					return ORANGE;
				case ColorName.Yellow:	
					return YELLOW;
				case ColorName.Green:
					return GREEN;
				case ColorName.Blue:
					return BLUE;
				case ColorName.Purple:
					return PURPLE;
				case ColorName.White:
					return WHITE;
				case ColorName.Black:
					return BLACK;
				case ColorName.Transparent:	
				default:
					return new Color(0f,0f,0f,0f);
			}
		}
		public static bool isMixed(ColorName colorName) {
			switch (colorName)
			{
				case ColorName.Red:
				case ColorName.Yellow:	
				case ColorName.Blue:
				case ColorName.Black:
					return false;
				case ColorName.Orange:
				case ColorName.Green:
				case ColorName.Purple:
				case ColorName.White:
					return true;
				case ColorName.Transparent:
				default:
					return false;
			}
		}
		public static ColorName[] mixture(ColorName colorName) {
			if (!isMixed(colorName)) {
				return new ColorName[] {};
			}			
			switch (colorName)
			{
				case ColorName.Orange:
					return new ColorName[] {ColorName.Yellow, ColorName.Red};
				case ColorName.Green:
					return new ColorName[] {ColorName.Yellow, ColorName.Blue};
				case ColorName.Purple:
					return new ColorName[] {ColorName.Blue, ColorName.Red};
				case ColorName.White:
					return new ColorName[] {ColorName.Blue, ColorName.Yellow, ColorName.Red};
				default:
					return new ColorName[] {};
			}
		}
	}
}
	
