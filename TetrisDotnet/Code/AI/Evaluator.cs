using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SFML.System;
using TetrisDotnet.Code.AI.Pathfinding;
using TetrisDotnet.Code.Events;
using TetrisDotnet.Code.Events.EventData;
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
			public float Bumpiness;
			public float AggregateHeight;
			public int TopHeight;

			public FinalPiece()
			{
				NbOfHoles = 0;
				LinesCleared = 0;
				Bumpiness = 0;
				AggregateHeight = 0;
			}

			public float GetWeight()
			{
				return Bumpiness * BumpinessFactor +
				       NbOfHoles * NbOfHolesFactor +
				       TopHeight * TopHeightFactor +
				       AggregateHeight * AggregateHeightFactor +
				       GetLinesClearedWeight();
			}

			private float GetLinesClearedWeight()
			{
				switch (Piece.Type)
				{
					case PieceType.I:
						switch (LinesCleared)
						{
							case 4:
								return -100;
							case 3:
								return -40;
							case 2:
								return -10;
							case 1:
								return -3;
						}

						break;
					case PieceType.O:
						switch (LinesCleared)
						{
							case 2:
								return -100;
							case 1:
								return -10;
						}

						break;
					case PieceType.T:
					case PieceType.S:
					case PieceType.Z:
						switch (LinesCleared)
						{
							case 2:
								return -45;
							case 1:
								return -8;
						}

						break;
					case PieceType.J:
					case PieceType.L:
						switch (LinesCleared)
						{
							case 3:
								return -100;
							case 2:
								return -20;
							case 1:
								return -5;
						}

						break;
				}

				return 0;
			}
		}

		public static float BumpinessFactor { get; private set; } = 0.3f;
		public static float NbOfHolesFactor { get; private set; } = 1.8f;
		public static float TopHeightFactor { get; private set; } = -0.18f;
		public static float AggregateHeightFactor { get; private set; } = 0.4f;

		public Evaluator()
		{
			Application.EventSystem.Subscribe(EventType.AiBumpinessUpdated, OnBumpinessUpdated);
			Application.EventSystem.Subscribe(EventType.AiNbOfHolesUpdated, OnNbOfHolesUpdated);
			Application.EventSystem.Subscribe(EventType.AiTopHeightUpdated, OnTopHeightUpdated);
			Application.EventSystem.Subscribe(EventType.AiAggregateHeightUpdated, OnAggregateHeightUpdated);
		}

		private void OnAggregateHeightUpdated(EventData eventData)
		{
			FloatEventData floatEventData = eventData as FloatEventData;
			Debug.Assert(floatEventData != null, nameof(floatEventData) + " != null");
			AggregateHeightFactor = floatEventData.Value;
		}

		private void OnTopHeightUpdated(EventData eventData)
		{
			FloatEventData floatEventData = eventData as FloatEventData;
			Debug.Assert(floatEventData != null, nameof(floatEventData) + " != null");
			TopHeightFactor = floatEventData.Value;
		}

		private void OnNbOfHolesUpdated(EventData eventData)
		{
			FloatEventData floatEventData = eventData as FloatEventData;
			Debug.Assert(floatEventData != null, nameof(floatEventData) + " != null");
			NbOfHolesFactor = floatEventData.Value;
		}

		private void OnBumpinessUpdated(EventData eventData)
		{
			FloatEventData floatEventData = eventData as FloatEventData;
			Debug.Assert(floatEventData != null, nameof(floatEventData) + " != null");
			BumpinessFactor = floatEventData.Value;
		}

		public Action GetBestPlacement(State state)
		{
			Queue<FinalPiece> finalPieces = new Queue<FinalPiece>(
				GenerateAllFinalPossibilities(state, state.CurrentPiece).OrderBy(finalPiece => finalPiece.GetWeight()));

			if (state.CurrentHeldPiece != PieceType.Empty && state.CanSwap)
			{
				FinalPiece heldPiece =
					GetBestPiece(GenerateAllFinalPossibilities(state, new Piece(state.CurrentHeldPiece)));
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
				if (finalPieces.Count <= 0)
				{
					return null;
				}

				finalPath = Pathfinder.pathfinder.FindPath(state, state.CurrentPiece, finalPieces.Dequeue().Piece);
			}

			return new Action(ActionType.Place, finalPath);
		}

		private List<FinalPiece> GenerateAllFinalPossibilities(State state, Piece originalPiece)
		{
			List<FinalPiece> finalPieces = new List<FinalPiece>();

			for (int x = 0; x < Grid.GridWidth; ++x)
			{
				for (int y = 2; y < Grid.GridHeight; ++y)
				{
					if ((y == Grid.GridHeight - 1 || state.GetBlock(x, y + 1)) &&
					    !state.GetBlock(x, y) || originalPiece.ContainsPoint(new Vector2i(x, y)))
					{
						for (int i = 0; i < originalPiece.Type.PossibleRotations(); i++)
						{
							Piece piece = new Piece(originalPiece);
							for (int j = 0; j < i; j++)
							{
								piece.Rotate(Rotation.Clockwise);
							}

							for (int anchor = 0; anchor < piece.Blocks.Count; anchor++)
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
			if (pieces.Count <= 0) return null;

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
					Bumpiness = CalculateBumpiness(state, adjustedPiece),
					AggregateHeight = GetAggregateHeight(state, adjustedPiece),
					TopHeight = adjustedPiece.GetGlobalBlocks.Max(pos => pos.Y)
				};
				return true;
			}

			return false;
		}

		private int GetLinesCleared(State state, Piece piece)
		{
			List<Vector2i> blocks = piece.GetGlobalBlocks;
			int topRow = blocks.Max(pos => pos.Y);
			int bottomRow = blocks.Min(pos => pos.Y);

			int linesCleared = 0;

			for (int y = bottomRow; y <= topRow; y++)
			{
				bool isFull = true;
				for (int x = 0; x < Grid.GridWidth; x++)
				{
					if (!state.GetBlock(x, y) && !blocks.Contains(new Vector2i(x, y)))
					{
						isFull = false;
						break;
					}
				}

				if (isFull)
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
			piece.Position = positionModifier;
			return piece;
		}

		private Vector2i GetBlockOffset(Piece piece, Vector2i position, int anchor)
		{
			return position - piece.Blocks[anchor];
		}

		private bool PieceIsValid(State state, Piece piece)
		{
			foreach (var point in piece.GetGlobalBlocks)
			{
				if (point.X < 0 || point.X > Grid.GridWidth - 1 || point.Y < 2 || point.Y > Grid.GridHeight - 1)
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