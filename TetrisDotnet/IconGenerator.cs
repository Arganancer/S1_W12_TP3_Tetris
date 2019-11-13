using SFML.Graphics;

namespace TetrisDotnet
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