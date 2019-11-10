using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
	class Useful
	{

		//Pi in float instead of default double
		static readonly float PI = Convert.ToSingle(Math.PI);

		/// <summary>
		/// Calcul de la distance entre deux points
		/// </summary>
		/// <param name="point1">Le point 1</param>
		/// <param name="point2">Le point 2</param>
		/// <returns>
		/// Retourne la distance entre les deux points
		/// </returns>
		public static float DistanceBetweenTwoPoints(Vector2f point1, Vector2f point2)
		{

			return Convert.ToSingle(Math.Sqrt(Math.Pow(point2.X - point1.X, 2) + Math.Pow(point2.Y - point1.Y, 2)));

		}

		/// <summary>
		/// Convertit un angle en radian vers un angle en degré
		/// </summary>
		/// <param name="radian">L'angle en radian</param>
		/// <returns>Retourne l'angle convertit</returns>
		public static float RadianToDegree(float radian)
		{

			return 180.0f * radian / PI;

		}

		/// <summary>
		/// Convertit un angle en degré vers un angle en randian
		/// </summary>
		/// <param name="degree">L'angle en degré</param>
		/// <returns>Retourne l'angle convertit</returns>
		public static float DegreeToRadian(float degree)
		{

			return PI * degree / 180.0f;

		}

	}
}
