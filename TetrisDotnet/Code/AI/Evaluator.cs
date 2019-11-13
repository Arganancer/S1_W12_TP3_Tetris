﻿using System.Collections.Generic;
using System.Linq;
using SFML.System;
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
			public int NbOfClosedHoles;
			public int TopHeight;

			public FinalPiece()
			{
				NbOfHoles = 0;
				NbOfClosedHoles = 0;
				TopHeight = 0;
			}

			public float GetWeight()
			{
				int topHeightValue = Grid.GridHeight - TopHeight;

				return NbOfClosedHoles + (NbOfHoles * 1.5f) + (topHeightValue * 1.0f);
			}
		}

		public Action GetBestPlacement(State state)
		{
			FinalPiece currentFinalPiece = GetBestPiece(GenerateAllFinalPossibilities(state, state.currentPiece));
			if (state.currentHeldPiece != PieceType.Empty)
			{
				FinalPiece heldPiece =
					GetBestPiece(GenerateAllFinalPossibilities(state, new Piece(state.currentHeldPiece)));
				if (heldPiece.GetWeight() < currentFinalPiece.GetWeight())
				{
					return new Action(ActionType.Hold, null);
				}
			}
			else
			{
				return new Action(ActionType.Hold, null);
			}

			return new Action(ActionType.Place, currentFinalPiece.Piece);
		}

		private List<FinalPiece> GenerateAllFinalPossibilities(State state, Piece originalPiece)
		{
			List<FinalPiece> finalPieces = new List<FinalPiece>();
			for (int x = 0; x < Grid.GridWidth; ++x)
			{
				for (int y = 0; y < Grid.GridHeight; ++y)
				{
					if (y != 0 && (y == Grid.GridHeight - 1 || state.GetBlock(x, y + 1)) && !originalPiece.ContainsPoint(new Vector2i(x, y)))
					{
						for (int i = 0; i < 4; i++)
						{
							Piece piece = new Piece(originalPiece);
							for (int j = 0; j < i; j++)
							{
								piece.Rotate(Rotation.Clockwise);
							}
							if (PositionIsValid(state, piece, new Vector2i(x, y), out FinalPiece newFinalPiece))
							{
								finalPieces.Add(newFinalPiece);
							}
						}
						break;
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

		private bool PositionIsValid(State state, Piece piece, Vector2i position, out FinalPiece modifiedPiece)
		{
			modifiedPiece = null;

			Piece adjustedPiece = AdjustPieceToPosition(piece, position);
			if (PieceIsValid(state, adjustedPiece))
			{
				GetHoles(state, adjustedPiece, out int closedHoles, out int holes);
				modifiedPiece = new FinalPiece
				{
					Piece = adjustedPiece,
					NbOfClosedHoles = closedHoles,
					NbOfHoles = holes,
					TopHeight = GetPieceTopHeight(adjustedPiece)
				};
				return true;
			}
			return false;
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

		private Piece AdjustPieceToPosition(Piece piece, Vector2i position)
		{
			Vector2i positionModifier = GetBlockOffset(piece.blocks, position);
			piece.position = positionModifier;
			return piece;
		}

		private Vector2i GetBlockOffset(IEnumerable<Vector2i> points, Vector2i position)
		{
			Vector2i desiredPoint = points.First();

			foreach (var point in points)
			{
				if(point.Y > desiredPoint.Y)
				{
					desiredPoint = point;
				}
				else if(point.Y == desiredPoint.Y)
				{
					if(point.X < desiredPoint.X)
					{
						desiredPoint = point;
					}
				}
			}

			return position - desiredPoint;
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

		private int GetPieceTopHeight(Piece piece)
		{
			return piece.getGlobalBlocks.Min(point => point.Y);
		}
	}
}
