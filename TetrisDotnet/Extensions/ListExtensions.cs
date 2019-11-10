using System.Collections.Generic;

namespace TetrisDotnet.Extensions
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

        public static T PopLeft<T>(this List<T> list)
        {
            T r = list[0];
            list.RemoveAt(0);
            return r;
        }
    }
}