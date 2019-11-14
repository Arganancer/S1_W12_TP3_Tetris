using System;
using System.Collections.Generic;
using System.Linq;
using SFML.System;
using TetrisDotnet.Code.AI.Pathfinding;
using TetrisDotnet.Code.Game;
using TetrisDotnet.Code.Game.World;
using TetrisDotnet.Code.Utils.Enums;
using TetrisDotnet.Code.Utils.Extensions;

namespace TetrisDotnet.Code.AI
{
	class Evaluator
	{
		private class FinalPiece
		{
			public Piece Piece;
			public int NbOfHoles;
			public int LinesCleared;
			public float bumpiness;
			public float AggregateHeight;
			public int TopHeight;

			public FinalPiece()
			{
				NbOfHoles = 0;
				LinesCleared = 0;
				bumpiness = 0;
				AggregateHeight = 0;
			}

			public float GetWeight()
			{
				return bumpiness * 0.1f +
				       NbOfHoles * 2.0f +
				       TopHeight * -0.2f +
				       //AggregateHeight * 0.01f +
				       GetLinesClearedWeight();
			}

			private float GetLinesClearedWeight()
			{
				switch (Piece.type)
				{
					case PieceType.I:
						if (LinesCleared == 4)
						{
							return -100;
						}
						else if (LinesCleared == 3)
						{
							return -40;
						}
						else if (LinesCleared == 2)
						{
							return -10;
						}
						else if (LinesCleared == 1)
						{
							return -3;
						}

						break;
					case PieceType.O:
					case PieceType.T:
					case PieceType.S:
					case PieceType.Z:
						if (LinesCleared == 2)
						{
							return -50;
						}
						else if (LinesCleared == 1)
						{
							return -15;
						}

						break;
					case PieceType.J:
					case PieceType.L:
						if (LinesCleared == 3)
						{
							return -100;
						}
						else if (LinesCleared == 2)
						{
							return -20;
						}
						else if (LinesCleared == 1)
						{
							return -5;
						}

						break;
				}

				return 0;
			}
		}

		public Action GetBestPlacement(State state)
		{
			Queue<FinalPiece> finalPieces = new Queue<FinalPiece>(
				GenerateAllFinalPossibilities(state, state.currentPiece).OrderBy(finalPiece => finalPiece.GetWeight()));

			if (state.currentHeldPiece != PieceType.Empty)
			{
				FinalPiece heldPiece =
					GetBestPiece(GenerateAllFinalPossibilities(state, new Piece(state.currentHeldPiece)));
				if (heldPiece.GetWeight() < finalPieces.Peek().GetWeight())
				{
					return new Action(ActionType.Hold, null);
				}
			}
			else
			{
				return new Action(ActionType.Hold, null);
			}

			Stack<PathNode> finalPath = null;
			while (finalPath == null)
			{
				finalPath = Pathfinder.pathfinder.FindPath(state, state.currentPiece, finalPieces.Dequeue().Piece);
			}

			return new Action(ActionType.Place, finalPath);
		}

		private List<FinalPiece> GenerateAllFinalPossibilities(State state, Piece originalPiece)
		{
			List<FinalPiece> finalPieces = new List<FinalPiece>();

			for (int x = 0; x < Grid.GridWidth; ++x)
			{
				for (int y = 0; y < Grid.GridHeight; ++y)
				{
					if ((y == Grid.GridHeight - 1 || state.GetBlock(x, y + 1)) &&
					    !state.GetBlock(x, y) || originalPiece.ContainsPoint(new Vector2i(x, y)))
					{
						for (int i = 0; i < 4; i++)
						{
							Piece piece = new Piece(originalPiece);
							for (int j = 0; j < i; j++)
							{
								piece.Rotate(Rotation.Clockwise);
							}

							for (int anchor = 0; anchor < piece.blocks.Count; anchor++)
							{
								if (PositionIsValid(state, new Piece(piece), anchor, new Vector2i(x, y),
									out FinalPiece newFinalPiece))
								{
									finalPieces.Add(newFinalPiece);
								}
							}
						}
					}
				}
			}

			return finalPieces;
		}

		private FinalPiece GetBestPiece(List<FinalPiece> pieces)
		{
			FinalPiece finalPiece = pieces.First();

			foreach (FinalPiece piece in pieces)
			{
				if (piece.GetWeight() < finalPiece.GetWeight())
				{
					finalPiece = piece;
				}
			}

			return finalPiece;
		}

		private bool PositionIsValid(State state, Piece piece, int anchor, Vector2i position,
			out FinalPiece modifiedPiece)
		{
			modifiedPiece = null;

			Piece adjustedPiece = AdjustPieceToPosition(piece, position, anchor);
			if (PieceIsValid(state, adjustedPiece))
			{
				GetHoles(state, adjustedPiece, out int closedHoles, out int holes);
				modifiedPiece = new FinalPiece
				{
					Piece = adjustedPiece,
					NbOfHoles = holes,
					LinesCleared = GetLinesCleared(state, adjustedPiece),
					bumpiness = CalculateBumpiness(state, adjustedPiece),
					AggregateHeight = GetAggregateHeight(state, adjustedPiece),
					TopHeight = adjustedPiece.getGlobalBlocks.Max(pos => pos.Y)
				};
				return true;
			}

			return false;
		}

		private int GetLinesCleared(State state, Piece piece)
		{
			List<Vector2i> blocks = piece.getGlobalBlocks;
			int topRow = blocks.Max(pos => pos.Y);
			int bottomRow = blocks.Min(pos => pos.Y);

			int linesCleared = 0;

			for (int y = bottomRow; y <= topRow; y++)
			{
				int nbOfBlocks = 0;

				for (int x = 0; x < Grid.GridWidth; x++)
				{
					if (state.GetBlock(x, y))
					{
						++nbOfBlocks;
					}
				}

				nbOfBlocks += blocks.Count(pos => pos.Y == y);

				if (nbOfBlocks == Grid.GridWidth)
				{
					++linesCleared;
				}
			}

			return linesCleared;
		}

		private float CalculateBumpiness(State state, Piece piece)
		{
			float bumpiness = 0;
			int[] columnHeights = new int[Grid.GridWidth];

			for (int x = 0; x < Grid.GridWidth; x++)
			{
				for (int y = 0; y < Grid.GridHeight; y++)
				{
					if (state.GetBlock(x, y) || piece.ContainsPoint(new Vector2i(x, y)))
					{
						columnHeights[x] = y;
						if (x > 0)
						{
							bumpiness += Math.Abs(columnHeights[x] - columnHeights[x - 1]);
						}

						break;
					}
				}
			}

			return bumpiness;
		}

		private void GetHoles(State state, Piece piece, out int closedHoles, out int holes)
		{
			holes = 0;
			closedHoles = 0;

			List<Vector2i> lowestPointsOfPiece = piece.GetLowestPointsOfPiece();

			foreach (Vector2i point in lowestPointsOfPiece)
			{
				for (int y = point.Y + 1; y < Grid.GridHeight; y++)
				{
					if (state.GetBlock(point.X, y))
					{
						break;
					}

					++holes;
				}
			}
		}

		private Piece AdjustPieceToPosition(Piece piece, Vector2i position, int anchor)
		{
			Vector2i positionModifier = GetBlockOffset(piece, position, anchor);
			piece.position = positionModifier;
			return piece;
		}

		private Vector2i GetBlockOffset(Piece piece, Vector2i position, int anchor)
		{
//			Vector2i desiredPoint = points.First();
//
//			foreach (var point in points)
//			{
//				if (point.Y > desiredPoint.Y)
//				{
//					desiredPoint = point;
//				}
//				else if (point.Y == desiredPoint.Y)
//				{
//					if (point.X < desiredPoint.X)
//					{
//						desiredPoint = point;
//					}
//				}
//			}

			return position - piece.blocks[anchor];
		}

		private bool PieceIsValid(State state, Piece piece)
		{
			foreach (var point in piece.getGlobalBlocks)
			{
				if (point.X < 0 || point.X > Grid.GridWidth - 1 || point.Y < 0 || point.Y > Grid.GridHeight - 1)
				{
					return false;
				}

				if (state.GetBlock(point.X, point.Y))
				{
					return false;
				}
			}

			return true;
		}

		private float GetAggregateHeight(State state, Piece piece)
		{
			float aggregateHeight = 0;

			for (int x = 0; x < Grid.GridWidth; x++)
			{
				for (int y = 0; y < Grid.GridHeight; y++)
				{
					if (state.GetBlock(x, y) || piece.ContainsPoint(new Vector2i(x, y)))
					{
						aggregateHeight += Grid.GridHeight - y;
						break;
					}
				}
			}

			return aggregateHeight;
		}
	}
}