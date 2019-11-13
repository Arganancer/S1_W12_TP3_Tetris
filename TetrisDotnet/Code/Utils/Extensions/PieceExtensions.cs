using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using SFML.System;
using TetrisDotnet.Code.Game;

namespace TetrisDotnet.Code.Utils.Extensions
{
	static class PieceExtensions
	{
		public static bool ContainsPoint(this Piece piece, Vector2i point)
		{
			return piece.blocks.Contains(point);
		}

		public static List<Vector2i> GetLowestPointsOfPiece(this Piece piece)
		{
			Dictionary<int, Vector2i> highest = new Dictionary<int, Vector2i>();

			foreach (var block in piece.blocks)
			{
				if (highest.ContainsKey(block.X))
				{
					if (highest[block.X].Y < block.Y)
					{
						highest[block.X] = block;
					}
				}
				else
				{
					highest[block.X] = block;
				}
			}

			return highest.Values.ToList();
		}
		
		public static List<Vector2i> GetHighestPointsOfPiece(this Piece piece)
		{
			Dictionary<int, Vector2i> highest = new Dictionary<int, Vector2i>();

			foreach (var block in piece.blocks)
			{
				if (highest.ContainsKey(block.X))
				{
					if (highest[block.X].Y > block.Y)
					{
						highest[block.X] = block;
					}
				}
				else
				{
					highest[block.X] = block;
				}
			}

			return highest.Values.ToList();
		}
		
		public static List<Vector2i> GetLeftMostPointsOfPiece(this Piece piece)
		{
			Dictionary<int, Vector2i> highest = new Dictionary<int, Vector2i>();

			foreach (var block in piece.blocks)
			{
				if (highest.ContainsKey(block.Y))
				{
					if (highest[block.Y].X > block.X)
					{
						highest[block.Y] = block;
					}
				}
				else
				{
					highest[block.Y] = block;
				}
			}

			return highest.Values.ToList();
		}
		
		public static List<Vector2i> GetRightMostPointsOfPiece(this Piece piece)
		{
			Dictionary<int, Vector2i> highest = new Dictionary<int, Vector2i>();

			foreach (var block in piece.blocks)
			{
				if (highest.ContainsKey(block.Y))
				{
					if (highest[block.Y].X < block.X)
					{
						highest[block.Y] = block;
					}
				}
				else
				{
					highest[block.Y] = block;
				}
			}

			return highest.Values.ToList();
		}
	}
}