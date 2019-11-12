using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SFML.System;
using TetrisDotnet.Code.Utils;

namespace TetrisDotnet.Code.Game
{
	class Piece
	{
		public List<Vector2i> blocks { get; private set; }
		public List<Vector2i> getGlobalBlocks => blocks.Select(block => block + position).ToList();
		public PieceType type { get; }
		public Vector2i position { get; set; }

		public Piece(PieceType type)
		{
			Debug.Assert(this.type != PieceType.Dead, "New piece was \"Dead\". This should never happen.");

			this.type = type;
			blocks = PieceTypeUtils.GetPieceTypeBlocks(type);
			position = PieceTypeUtils.GetDefaultPosition(type);
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

		// TODO: Garbage way of doing rotations. Temporary until I find a better solution (Or rather take the time to do better).
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
			switch (type)
			{
				case PieceType.O:
					return;
				case PieceType.I:
					currentLinePieceRotationIndex += (int) rotation;
					blocks = linePieceRotations[currentLinePieceRotationIndex % 4];
					break;
				default:
					blocks = blocks.Select(block => RotateBlock(block, rotation)).ToList();
					break;
			}
		}

		public List<Vector2i> SimulateRotation(Rotation rotation)
		{
			return type switch
			{
				PieceType.O => blocks,
				PieceType.I => linePieceRotations[(int) (currentLinePieceRotationIndex + rotation) % 4],
				_ => blocks.Select(block => RotateBlock(block, rotation)).ToList()
			};
		}
	}
}