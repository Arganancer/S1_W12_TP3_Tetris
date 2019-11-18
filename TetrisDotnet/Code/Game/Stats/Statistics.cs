using System.Diagnostics;
using TetrisDotnet.Code.Events;
using TetrisDotnet.Code.Events.EventData;

namespace TetrisDotnet.Code.Game.Stats
{
	public class Statistics
	{
		public float DropSpeed { get; private set; }
		public int Level { get; private set; }
		private int linesUntilNextLevel;

		private int score;

		public int Score
		{
			get => score;
			private set
			{
				if (score == value) return;
				score = value;
				Application.EventSystem.ProcessEvent(EventType.ScoreUpdated, new IntEventData(score));
			}
		}

		private int combo;

		public int Combo
		{
			get => combo;
			private set
			{
				if (combo == value) return;
				combo = value;
				Application.EventSystem.ProcessEvent(EventType.ComboUpdated, new IntEventData(combo));
			}
		}

		public int BackToBackChain { get; private set; }

		public Statistics()
		{
			Application.EventSystem.Subscribe(EventType.PiecePlaced, OnPiecePlaced);
			Application.EventSystem.Subscribe(EventType.PieceDropped, OnPieceDropped);
			IncreaseLevel();
		}

		~Statistics()
		{
			Application.EventSystem.Unsubscribe(EventType.PiecePlaced, OnPiecePlaced);
			Application.EventSystem.Unsubscribe(EventType.PieceDropped, OnPieceDropped);
		}

		private void OnPiecePlaced(EventData eventData)
		{
			PiecePlacedEventData piecePlaced = eventData as PiecePlacedEventData;

			Debug.Assert(piecePlaced != null, nameof(piecePlaced) + " != null");

			Move[] moves = piecePlaced.Moves.ToArray();

			if (moves.LinesCleared() != Move.None)
			{
				++Combo;
				AddScore(moves);

				if (moves.IsDifficult())
				{
					++BackToBackChain;
				}
				else
				{
					BackToBackChain = 0;
				}

				UpdateLinesUntilNextLevel(moves.LinesCleared());
			}
			else
			{
				Combo = 0;
			}
		}

		private void UpdateLinesUntilNextLevel(Move lines)
		{
			int linesValue = 0;
			switch (lines)
			{
				case Move.Single:
					linesValue = 1;
					break;
				case Move.Double:
					linesValue = 3;
					break;
				case Move.Triple:
					linesValue = 5;
					break;
				case Move.Tetris:
					linesValue = 8;
					break;
			}

			linesUntilNextLevel -= linesValue;
			if (linesUntilNextLevel <= 0)
			{
				IncreaseLevel();
				CalculateLinesUntilNextLevel();
			}
		}

		private void IncreaseLevel()
		{
			++Level;
			DropSpeed = 0.8f - (Level - 1) * 0.007f;
			Application.EventSystem.ProcessEvent(EventType.LevelUp, new IntEventData(Level));
		}

		private void CalculateLinesUntilNextLevel()
		{
			linesUntilNextLevel = 5 * Level;
		}

		private void OnPieceDropped(EventData eventData)
		{
			PieceDroppedEventData pieceDropped = eventData as PieceDroppedEventData;

			Debug.Assert(pieceDropped != null, nameof(pieceDropped) + " != null");

			int multiplier = pieceDropped.DropType == Drop.HardDrop ? 2 : 1;

			Score += pieceDropped.DistanceDropped * multiplier;
		}

		public void AddScore(params Move[] moves)
		{
			Score += moves.CountPoints(Level, Combo, BackToBackChain);
		}
	}
}