using SFML.Graphics;
//using System.Drawing;

namespace Tetris
{
	static class IconGenerator
	{

		public static byte[] IconToBytes(string filePath)
		{

			Image icon = new Image(filePath);

			return icon.Pixels;
		}

	}
}
