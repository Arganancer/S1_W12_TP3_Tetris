using SFML.Graphics;
using System;
using System.Collections.Generic;
//using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
