﻿using System.Collections.Generic;
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

		public void Rotate(Rotation rotation)
		{
			switch (type)
			{
				case PieceType.O:
					return;
				case PieceType.I:
					break;
				default:
					List<Vector2i> positions = blocks.Select(block => RotateBlock(block, rotation)).ToList();
					blocks = positions;
					break;
			}
		}
	}
}