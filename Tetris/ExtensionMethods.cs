using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
	static class ListExtension
	{
		public static T Pop<T>(this List<T> list)
		{
			int index = list.Count - 1;
			T r = list[index];
			list.RemoveAt(index);
			return r;
		}
	}

}
