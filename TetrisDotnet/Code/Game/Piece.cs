using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SFML.System;
using TetrisDotnet.Code.Utils.Enums;

namespace TetrisDotnet.Code.Game
{
	public class Piece
	{
		public List<Vector2i> Blocks { get; private set; }
		public List<Vector2i> GetGlobalBlocks => Blocks.Select(block => block + Position).ToList();
		public PieceType Type { get; }
		public Vector2i Position { get; set; }
		public int RotationIndex { get; private set; }

		public Piece(PieceType type)
		{
			Type = type;
			Blocks = PieceTypeUtils.GetPieceTypeBlocks(type);
			Position = PieceTypeUtils.GetDefaultPosition(type);
		}
		
		public Piece(Piece piece)
		{
			Type = piece.Type;
			Blocks = piece.Blocks;
			Position = piece.Position;
			RotationIndex = piece.RotationIndex;
		}

		private readonly List<Vector2i> cornerBlocks = new List<Vector2i>
		{
			new Vector2i(0, 0),
			new Vector2i(2, 0),
			new Vector2i(2, 2),
			new Vector2i(0, 2)
		};

		private readonly List<Vector2i> outerBlocks = new List<Vector2i>
		{
			new Vector2i(1, 0),
			new Vector2i(2, 1),
			new Vector2i(1, 2),
			new Vector2i(0, 1)
		};

		private Vector2i RotateBlock(Vector2i block, Rotation rotation)
		{
			int index = cornerBlocks.FindIndex(b => b == block);
			if (index >= 0)
			{
				return cornerBlocks[(index + (int) rotation) % 4];
			}

			index = outerBlocks.FindIndex(b => b == block);
			if (index >= 0)
			{
				return outerBlocks[(index + (int) rotation) % 4];
			}

			return block;
		}

		private int currentLinePieceRotationIndex = 0;

		private readonly List<List<Vector2i>> linePieceRotations = new List<List<Vector2i>>
		{
			new List<Vector2i> {new Vector2i(0, 1), new Vector2i(1, 1), new Vector2i(2, 1), new Vector2i(3, 1)},
			new List<Vector2i> {new Vector2i(2, 0), new Vector2i(2, 1), new Vector2i(2, 2), new Vector2i(2, 3)},
			new List<Vector2i> {new Vector2i(0, 2), new Vector2i(1, 2), new Vector2i(2, 2), new Vector2i(3, 2)},
			new List<Vector2i> {new Vector2i(1, 0), new Vector2i(1, 1), new Vector2i(1, 2), new Vector2i(1, 3)}
		};

		public void Rotate(Rotation rotation)
		{
			switch (Type)
			{
				case PieceType.O:
					return;
				case PieceType.I:
					currentLinePieceRotationIndex += (int) rotation;
					Blocks = linePieceRotations[currentLinePieceRotationIndex % 4];
					break;
				default:
					Blocks = Blocks.Select(block => RotateBlock(block, rotation)).ToList();
					break;
			}

			RotationIndex = (RotationIndex + (int) rotation) % 4;
		}

		public List<Vector2i> SimulateRotation(Rotation rotation)
		{
			return Type switch
			{
				PieceType.O => Blocks,
				PieceType.I => linePieceRotations[(int) (currentLinePieceRotationIndex + rotation) % 4],
				_ => Blocks.Select(block => RotateBlock(block, rotation)).ToList()
			};
		}
	}
}