using SFML.Graphics;

namespace TetrisDotnet.Code.Utils
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