using System.Diagnostics;
using TetrisDotnet.Code.Events;
using TetrisDotnet.Code.Events.EventData;

namespace TetrisDotnet.Code.Game.Stats
{
	public class Statistics
	{
		public float DropSpeed { get; private set; }
		private int level;
		private int linesUntilNextLevel;
		
		private int nbOfPiecesPlayed;
		private int NbOfPiecesPlayed
		{
			get => nbOfPiecesPlayed;
			set
			{
				nbOfPiecesPlayed = value;
				Application.EventSystem.ProcessEvent(EventType.PiecesPlacedUpdated, new IntEventData(nbOfPiecesPlayed));
			}
		}

		private int score;
		private int Score
		{
			get => score;
			set
			{
				if (score == value) return;
				score = value;
				Application.EventSystem.ProcessEvent(EventType.ScoreUpdated, new IntEventData(score));
			}
		}

		private int combo;
		private int Combo
		{
			get => combo;
			set
			{
				if (combo == value) return;
				combo = value;
				Application.EventSystem.ProcessEvent(EventType.ComboUpdated, new IntEventData(combo));
			}
		}

		private int BackToBackChain { get; set; }
		
		public Statistics()
		{
			Application.EventSystem.Subscribe(EventType.PiecePlaced, OnPiecePlaced);
			Application.EventSystem.Subscribe(EventType.PieceDropped, OnPieceDropped);
			IncreaseLevel();
			CalculateLinesUntilNextLevel();
		}

		~Statistics()
		{
			Application.EventSystem.Unsubscribe(EventType.PiecePlaced, OnPiecePlaced);
			Application.EventSystem.Unsubscribe(EventType.PieceDropped, OnPieceDropped);
		}

		private void OnPiecePlaced(EventData eventData)
		{
			++NbOfPiecesPlayed;
			
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
			++level;
			DropSpeed = 0.8f - (level - 1) * 0.007f;
			Application.EventSystem.ProcessEvent(EventType.LevelUp, new IntEventData(level));
		}

		private void CalculateLinesUntilNextLevel()
		{
			linesUntilNextLevel = 5 * level;
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
			Score += moves.CountPoints(level, Combo, BackToBackChain);
		}
	}
}