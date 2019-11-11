using System.Collections.Generic;
using SFML.System;
using TetrisDotnet.Code.Utils;

namespace TetrisDotnet.Code.Game.World
{
	class Grid
	{
		public const int GridHeight = 22;
		public const int GridWidth = 10;

		private readonly PieceType[,] grid;

		public Grid()
		{
			grid = new PieceType[GridWidth, GridHeight];
			InitializeGrid();
		}

		// TODO: Fuck this function.
		public PieceType[,] GetDrawable()
		{
			PieceType[,] visibleArray = new PieceType[GridWidth - 2, GridHeight];

			for (int x = 0; x < GridWidth - 2; x++)
			{
				for (int y = 0; y < GridHeight; y++)
				{
					//i + 2 since we only want the grid where we actually see the pieces
					visibleArray[x, y] = grid[x + 2, y];
				}
			}

			return visibleArray;
		}

		public List<int> GetFullRows()
		{
			List<int> fullRows = new List<int>();

			for (int y = 0; y < GridHeight; y++)
			{
				bool isFull = true;

				for (int x = 0; x < GridWidth; x++)
				{
					if (grid[x, y] == PieceType.Dead) continue;
					isFull = false;
					break;
				}

				if (isFull)
				{
					fullRows.Add(y);
				}
			}

			return fullRows;
		}

		public bool CheckLose()
		{
			for (int y = 0; y < 2; y++)
			{
				for (int x = 0; x < GridWidth; x++)
				{
					if (grid[x, y] == PieceType.Dead)
					{
						return true;
					}
				}
			}

			return false;
		}

		private void InitializeGrid()
		{
			for (int x = 0; x < GridWidth; x++)
			{
				for (int y = 0; y < GridHeight; y++)
				{
					grid[x, y] = PieceType.Empty;
				}
			}
		}

		public void KillPiece(Piece piece)
		{
			for (int x = 0; x < 4; x++)
			{
				for (int y = 0; y < 4; y++)
				{
					if (!OutOfRange(new Vector2i(piece.position.X + x, piece.position.Y + y)) &&
					    grid[piece.position.X + x, piece.position.Y + y] == piece.type)
					{
						grid[piece.position.X + x, piece.position.Y + y] = PieceType.Dead;
					}
				}
			}
		}

		public void RemovePiece(Piece piece)
		{
			for (int x = 0; x < 4; x++)
			{
				for (int y = 0; y < 4; y++)
				{
					if (!OutOfRange(new Vector2i(piece.position.X + x, piece.position.Y + y)) &&
					    grid[piece.position.X + x, piece.position.Y + y] == piece.type)
					{
						grid[piece.position.X + x, piece.position.Y + y] = PieceType.Empty;
					}
				}
			}
		}

		private static bool OutOfRange(Vector2i position)
		{
			return position.Y >= GridHeight || position.Y < 0 || 
			       position.X >= GridWidth || position.X < 0;
		}

		public void RemoveRow(int rowIdx)
		{
			for (int y = rowIdx; y > 0; y--)
			{
				for (int x = 0; x < grid.GetLength(1); x++)
				{
					if (grid[x, y] == PieceType.Dead || grid[x, y] == PieceType.Empty)
					{
						grid[x, y] = grid[x, y - 1];
					}
				}
			}
		}
		
		public bool CanPlacePiece(Piece piece, Vector2i position)
		{
			foreach (Vector2i block in piece.blocks)
			{
				Vector2i newPosition = block + position;

				if (OutOfRange(newPosition) || grid[newPosition.X, newPosition.Y] == PieceType.Dead)
				{
					return false;
				}
			}

			return true;
		}
		
		public void AddPiece(Piece piece, Vector2i position)
		{
			piece.position += position;
			foreach (Vector2i block in piece.getGlobalBlocks)
			{
				grid[block.X, block.Y] = piece.type;
			}
		}
		
		public void MovePiece(Piece piece, Vector2i newPosition)
		{
			if (CanPlacePiece(piece, newPosition))
			{
				RemoveGhostPiece(piece);
				RemovePiece(piece);

				AddPiece(piece, newPosition);
				PlaceGhostPiece(piece);
			}
		}
		
		// All of the possible kickback positions. Used in the function "CanRotatePiece".
		private static readonly int[] NoKickBack = {0, 0};
		private static readonly int[] BackToRight = {1, 0};
		private static readonly int[] BackToLeft = {-1, 0};
		private static readonly int[] BackDown = {0, 1};
		private static readonly int[] DiagLeft = {-1, 1};
		private static readonly int[] DiagRight = {1, 1};
		private static readonly int[] BackUp = {0, -1};
		private static readonly int[] LineToRight = {2, 0};
		private static readonly int[] LineToLeft = {-2, 0};
		private static readonly int[] LineDown = {0, 2};
		private static readonly int[] LineUp = {0, -2};

		private List<PieceType> RotatePieceArray(Piece piece, int directionOfRotation)
		{
			
			PieceType[,] pieceArray = piece.pieceArray;

			int iLength = pieceArray.GetLength(0);

			int jLength = pieceArray.GetLength(1);

			PieceType[,] newPieceArray = new PieceType[iLength, jLength];

			for (int i = iLength - 1; i >= 0; i--)
			{
				for (int j = 0; j < jLength; j++)
				{
					if (directionOfRotation == 0)
					{
						newPieceArray[j, iLength - 1 - i] = pieceArray[i, j];
					}
					else
					{
						newPieceArray[i, j] = pieceArray[j, iLength - 1 - i];
					}
				}
			}

			return newPieceArray;
		}

		// (2)
		/// <summary>
		/// Fonctionnalité qui est appelée quand le joueur appuie une touche pour faire tourner 
		/// la pièce active. Cette méthode gère toutes les autres méthodes associées à la rotation 
		/// de la pièce active.
		/// </summary>
		/// 
		/// <param name="piece">
		/// La pièce active qui sera tournée.
		/// </param>
		/// 
		/// <param name="directionOfRotation">
		/// Détermine si la pièce sera tournée dans le sens horaire (0), ou antihoraire (1).
		/// </param>
		/// 
		/// <returns>
		/// Retourne « vrai » si la pièce peut tourner, ou « faux » si elle ne peut pas tourner.
		/// </returns>
		public bool RotatePiece(Piece piece, int directionOfRotation)
		{
			int[] kickBackPosition = {0, 0};
			if (CanRotatePiece(piece, out kickBackPosition, directionOfRotation))
			{
				RemoveGhostPiece(piece);
				RemovePiece(piece);
				AddRotatedPiece(piece, kickBackPosition, directionOfRotation);
				PlaceGhostPiece(piece);
				return true;
			}

			return false;
		}

		// (2)
		/// <summary>
		/// Ajoute la nouvelle pièce tournée dans le tableau de jeu et remplace le tableau 
		/// partagé qui stocke la pièce avec le nouveau tableau tourner.
		/// </summary>
		/// 
		/// <param name="piece">
		/// La pièce active qui sera tournée.
		/// </param>
		/// 
		/// <param name="kickBackPosition">
		/// Décalage de la pièce active sur le tableau de jeu après avoir tourné.
		/// </param>
		/// 
		/// <param name="directionOfRotation">
		/// Détermine si la pièce sera tournée dans le sens horaire (0), ou antihoraire (1).
		/// </param>
		public void AddRotatedPiece(Piece piece, int[] kickBackPosition, int directionOfRotation)
		{
			PieceType[,] pieceArray = RotatePieceArray(piece, directionOfRotation);
			for (int i = 0; i < pieceArray.GetLength(0); i++)
			{
				for (int j = 0; j < pieceArray.GetLength(1); j++)
				{
					if (OutOfRange(piece.position.Y + i + kickBackPosition[0],
						    piece.position.X + j + kickBackPosition[1]) && pieceArray[i, j] == piece.type)
					{
						grid[piece.position.Y + i + kickBackPosition[0], piece.position.X + j + kickBackPosition[1]] =
							pieceArray[i, j];
					}
				}
			}

			Vector2i position = new Vector2i(kickBackPosition[1], kickBackPosition[0]);
			piece.pieceArray = pieceArray;
			piece.position += position;
		}

		// (2)
		/// <summary>
		/// Vérifie si la pièce peut être tournée et placée à un emplacement valide sur le tableau de jeu.
		/// </summary>
		/// 
		/// <param name="piece">
		/// La pièce active qui sera tournée.
		/// </param>
		/// 
		/// <param name="kickBackPosition">
		/// Décalage de la pièce active sur le tableau de jeu après avoir tourné.
		/// </param>
		/// 
		/// <param name="directionOfRotation">
		/// Détermine si la pièce sera tournée dans le sens horaire (0), ou antihoraire (1).
		/// </param>
		/// 
		/// <returns>
		/// Retourne « vrai » si la pièce peut tourner, ou « faux » si elle ne peut pas tourner.
		/// </returns>
		private bool CanRotatePiece(Piece piece, out int[] kickBackPosition, int directionOfRotation)
		{
			PieceType[,] newPieceArray = RotatePieceArray(piece, directionOfRotation);
			if (CheckPiecePositionValid(piece, newPieceArray, NoKickBack))
			{
				kickBackPosition = NoKickBack;
			}
			else if (CheckPiecePositionValid(piece, newPieceArray, BackToRight))
			{
				kickBackPosition = BackToRight;
			}
			else if (CheckPiecePositionValid(piece, newPieceArray, BackToLeft))
			{
				kickBackPosition = BackToLeft;
			}
			else if (CheckPiecePositionValid(piece, newPieceArray, BackDown))
			{
				kickBackPosition = BackDown;
			}
			else if (piece.type == PieceType.T && CheckPiecePositionValid(piece, newPieceArray, DiagLeft))
			{
				kickBackPosition = DiagLeft;
			}
			else if (piece.type == PieceType.T && CheckPiecePositionValid(piece, newPieceArray, DiagRight))
			{
				kickBackPosition = DiagRight;
			}
			else if (CheckPiecePositionValid(piece, newPieceArray, BackUp))
			{
				kickBackPosition = BackUp;
			}
			else if (piece.type == PieceType.I && CheckPiecePositionValid(piece, newPieceArray, LineToRight))
			{
				kickBackPosition = LineToRight;
			}
			else if (piece.type == PieceType.I && CheckPiecePositionValid(piece, newPieceArray, LineToLeft))
			{
				kickBackPosition = LineToLeft;
			}
			else if (piece.type == PieceType.I && CheckPiecePositionValid(piece, newPieceArray, LineDown))
			{
				kickBackPosition = LineDown;
			}
			else if (piece.type == PieceType.I && CheckPiecePositionValid(piece, newPieceArray, LineUp))
			{
				kickBackPosition = LineUp;
			}
			else
			{
				kickBackPosition = NoKickBack;
				return false;
			}

			return true;
		}

		// (2)
		/// <summary>
		/// Utilisé en union avec la fonction « CanRotatePiece », vérifie si la pièce active peut être 
		/// placée à la position passer en paramètre. 
		/// </summary>
		/// 
		/// <param name="piece">
		/// La pièce active qui sera tournée.
		/// </param>
		/// 
		/// <param name="kickBackPosition">
		/// Décalage de la pièce active sur le tableau de jeu après avoir tourné.
		/// </param>
		/// 
		/// <param name="newPieceArray">
		/// Tableau 2D contenant la pièce à vérifier.
		/// </param>
		///
		/// <returns>
		/// Retourne « vrai » si la pièce peut être décalée à la position passée en paramètre, ou « faux » si elle ne peut pas.
		/// </returns>
		private bool CheckPiecePositionValid(Piece piece, PieceType[,] newPieceArray, int[] kickBackPosition)
		{
			for (int i = 0; i < newPieceArray.GetLength(0); i++)
			{
				for (int j = 0; j < newPieceArray.GetLength(1); j++)
				{
					int newYPos = piece.position.Y + i + kickBackPosition[0];
					int newXPos = piece.position.X + j + kickBackPosition[1];
					if (newPieceArray[i, j] == piece.type)
					{
						if (!OutOfRange(newYPos, newXPos) || grid[newYPos, newXPos] == PieceType.Dead)
						{
							return false;
						}
					}
				}
			}

			return true;
		}

		#endregion

		#region Ghost Piece and hard drop

		// (Bonus)
		/// <summary>
		/// Vérifie la plus basse position i valide que la pièce active peut atteindre en restant dans sa colonne courante. 
		/// </summary>
		/// 
		/// <param name="piece">
		/// La pièce active dont les indices seront utilisés.
		/// </param>
		/// 
		/// <returns>
		/// Retourne la distance entre la position de la pièce active et la plus basse position que la pièce active peut atteindre.
		/// </returns>
		public int CheckLowestPossiblePosition(Piece piece)
		{
			int lowestDownPosition = 0;

			for (; CanPlacePiece(piece, StaticVars.down * lowestDownPosition); lowestDownPosition++)
			{
			}

			return lowestDownPosition - 1;
		}

		// (Bonus)
		/// <summary>
		/// Place la pièce fantôme dans la grille de jeu.
		/// </summary>
		/// 
		/// <param name="piece">
		/// La pièce active dont les indices seront utilisés.
		/// </param>
		public void PlaceGhostPiece(Piece piece)
		{
			int ghostPiecePos = CheckLowestPossiblePosition(piece);

			PieceType[,] pieceArray;

			pieceArray = piece.pieceArray;

			for (int i = 0; i < pieceArray.GetLength(0); i++)
			{
				for (int j = 0; j < pieceArray.GetLength(1); j++)
				{
					if (OutOfRange(piece.position.Y + ghostPiecePos + i, piece.position.X + j) &&
					    pieceArray[i, j] == piece.type)
					{
						if (grid[piece.position.Y + ghostPiecePos + i, piece.position.X + j] != piece.type)
						{
							grid[piece.position.Y + ghostPiecePos + i, piece.position.X + j] = PieceType.Ghost;
						}
					}
				}
			}
		}

		// (Bonus)
		/// <summary>
		/// Détermine la pièce active à la plus basse position qu'elle peut être.
		/// </summary>
		/// 
		/// <param name="piece">
		/// La pièce active.
		/// </param>
		/// 
		/// <returns>
		/// Retourne la distance entre la pièce active et la plus basse position.
		/// </returns>
		public int DetermineDropdownPosition(Piece piece)
		{
			RemoveGhostPiece(piece);
			RemovePiece(piece);
			int dropDownPos = CheckLowestPossiblePosition(piece);
			return dropDownPos;
		}

		// (Bonus)
		/// <summary>
		/// Enlève la pièce fantôme du tableau de jeu.
		/// </summary>
		/// 
		/// <param name="piece">
		/// La pièce active.
		/// </param>
		public void RemoveGhostPiece(Piece piece)
		{
			for (int i = 0; i < grid.GetLength(0); i++)
			{
				for (int j = 0; j < grid.GetLength(1); j++)
				{
					if (OutOfRange(i, j) && grid[i, j] == PieceType.Ghost)
					{
						grid[i, j] = PieceType.Empty;
					}
				}
			}
		}
	}
}