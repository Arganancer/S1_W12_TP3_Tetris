using System;
using System.Collections.Generic;
using SFML.System;
using Tetris;

namespace TetrisDotnet
{
    class Grid
    {
        private PieceType[,] grid;

        public Grid()
        {
            grid = new PieceType[22, 10];

            SetUpGrid();
        }

        public PieceType[,] GetDrawable()
        {
            PieceType[,] visibleArray = new PieceType[20, 10];

            for (int i = 0; i < visibleArray.GetLength(0); i++)
            {
                for (int j = 0; j < visibleArray.GetLength(1); j++)
                {
                    //i + 2 since we only want the grid where we actually see the pieces
                    visibleArray[i, j] = grid[i + 2, j];
                }
            }

            return visibleArray;
        }

        public bool[,] GetBoolGrid()
        {
            bool[,] visibleArray = new bool[20, 10];

            for (int i = 0; i < visibleArray.GetLength(0); i++)
            {
                for (int j = 0; j < visibleArray.GetLength(1); j++)
                {
                    //i + 2 since we only want the grid where we actually see the pieces
                    visibleArray[i, j] = grid[i + 2, j] != PieceType.Empty;
                }
            }

            return visibleArray;
        }

        public List<int> CheckFullRows()
        {
            var idxFullRows = new List<int>();

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                bool isFull = true;

                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j] != PieceType.Dead)
                    {
                        isFull = false;

                        break;
                    }
                }

                if (isFull)
                {
                    idxFullRows.Add(i);
                }
            }

            return idxFullRows;
        }

        public bool CheckLose()
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j] == PieceType.Dead)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void SetUpGrid()
        {
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    grid[i, j] = PieceType.Empty;
                }
            }
        }

        public void KillPiece(Piece piece)
        {
            PieceType[,] pieceArray = piece.pieceArray;

            for (int i = 0; i < pieceArray.GetLength(0); i++)
            {
                for (int j = 0; j < pieceArray.GetLength(1); j++)
                {
                    if (NotOutOfRange(piece.position.Y + i, piece.position.X + j) &&
                        grid[piece.position.Y + i, piece.position.X + j] == piece.type)
                    {
                        grid[piece.position.Y + i, piece.position.X + j] = PieceType.Dead;
                    }
                }
            }
        }

        public void RemovePiece(Piece piece)
        {
            PieceType[,] pieceArray = piece.pieceArray;

            for (int i = 0; i < pieceArray.GetLength(0); i++)
            {
                for (int j = 0; j < pieceArray.GetLength(1); j++)
                {
                    if (NotOutOfRange(piece.position.Y + i, piece.position.X + j) &&
                        grid[piece.position.Y + i, piece.position.X + j] == piece.type)
                    {
                        grid[piece.position.Y + i, piece.position.X + j] = PieceType.Empty;
                    }
                }
            }
        }

        private bool NotOutOfRange(int y, int x)
        {
            return !(y >= grid.GetLength(0) || y < 0 || x >= grid.GetLength(1) || x < 0);
        }

        public void RemoveRow(int rowIdx)
        {
            for (int i = rowIdx; i > 0; i--)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j] == PieceType.Dead || grid[i, j] == PieceType.Empty)
                    {
                        grid[i, j] = grid[i - 1, j];
                    }
                }
            }
        }

        #region Move piece

        // (1)
        /// <summary>
        /// Vérifie si la pièce active peut être déplacée dans une direction passée en paramètre.
        /// </summary>
        /// 
        /// <param name="piece">
        /// La pièce active qui sera déplacée.
        /// </param>
        /// 
        /// <param name="position">
        /// La direction dans laquelle on veut déplacer la pièce active.
        /// </param>
        /// 
        /// <returns>
        /// Retourne « vrai » si la pièce peut bouger et « faux » si elle ne peut pas.
        /// </returns>
        public bool CanPlacePiece(Piece piece, Vector2i position)
        {
            PieceType[,] pieceArray = piece.pieceArray;

            for (int i = 0; i < pieceArray.GetLength(0); i++)
            {
                for (int j = 0; j < pieceArray.GetLength(1); j++)
                {
                    if (pieceArray[i, j] != PieceType.Empty)
                    {
                        int newYPos = piece.position.Y + position.Y + i;

                        int newXPos = piece.position.X + position.X + j;

                        if (!NotOutOfRange(newYPos, newXPos) || grid[newYPos, newXPos] == PieceType.Dead)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        // (C)
        /// <summary>
        /// Déplace la pièce active dans le tableau 2D du jeu dans une direction passée en paramètre.
        /// </summary>
        /// 
        /// <param name="piece">
        /// La pièce active qui sera déplacée.
        /// </param>
        /// 
        /// <param name="position">
        /// La direction dans laquelle la pièce active sera déplacée.
        /// </param>
        public void AddPiece(Piece piece, Vector2i position)
        {
            PieceType[,] pieceArray = piece.pieceArray;


            for (int i = 0; i < pieceArray.GetLength(0); i++)
            {
                for (int j = 0; j < pieceArray.GetLength(1); j++)
                {
                    if (pieceArray[i, j] == piece.type && NotOutOfRange(piece.position.Y + position.Y + i,
                            piece.position.X + position.X + j))
                    {
                        grid[piece.position.Y + position.Y + i, piece.position.X + position.X + j] = pieceArray[i, j];
                    }
                }
            }

            piece.position += position;
        }

        // (C)
        /// <summary>
        /// Appelle la fonction qui vérifie si la pièce active peut bouger dans la 
        /// direction passée en paramètre, et ensuite appelle la fonction qui déplace 
        /// la pièce active si elle peut être déplacée. Appelle aussi les fonctions qui 
        /// gèrent la pièce fantôme.
        /// </summary>
        /// 
        /// <param name="piece">
        /// La pièce active qui sera déplacée.
        /// </param>
        /// 
        /// <param name="newPosition">
        /// La direction dans laquelle la pièce active sera déplacée.
        /// </param>
        public void MovePiece(Piece piece, Vector2i newPosition)
        {
            PieceType[,] pieceArray = piece.pieceArray;

            if (CanPlacePiece(piece, newPosition))
            {
                RemoveGhostPiece(piece);
                RemovePiece(piece);

                AddPiece(piece, newPosition);
                PlaceGhostPiece(piece);
            }
        }

        #endregion

        #region Rotate piece

        #region Kickback values

        // All of the possible kickback positions. Used in the function "CanRotatePiece".
        private static int[] noKickBack = new int[] {0, 0};
        private static int[] kickBackToRight = new int[] {0, 1};
        private static int[] kickBackToLeft = new int[] {0, -1};
        private static int[] kickBackDown = new int[] {1, 0};
        private static int[] kickTDiagLeft = new int[] {1, -1};
        private static int[] kickTDiagRight = new int[] {1, 1};
        private static int[] kickBackUp = new int[] {-1, 0};
        private static int[] kickLineToRight = new int[] {0, 2};
        private static int[] kickLineToLeft = new int[] {0, -2};
        private static int[] kickLineDown = new int[] {2, 0};
        private static int[] kickLineUp = new int[] {-2, 0};

        #endregion

        // (2)
        /// <summary>
        /// Fais tourner la pièce active en sens horaire, ou sens antihoraires.
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
        /// Retourne un tableau avec la nouvelle pièce tournée.
        /// </returns>
        private PieceType[,] RotatePieceArray(Piece piece, int directionOfRotation)
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
            int[] kickBackPosition = new int[] {0, 0};
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
                    if (NotOutOfRange(piece.position.Y + i + kickBackPosition[0],
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
            if (CheckPiecePositionValid(piece, newPieceArray, noKickBack))
            {
                kickBackPosition = noKickBack;
            }
            else if (CheckPiecePositionValid(piece, newPieceArray, kickBackToRight))
            {
                kickBackPosition = kickBackToRight;
            }
            else if (CheckPiecePositionValid(piece, newPieceArray, kickBackToLeft))
            {
                kickBackPosition = kickBackToLeft;
            }
            else if (CheckPiecePositionValid(piece, newPieceArray, kickBackDown))
            {
                kickBackPosition = kickBackDown;
            }
            else if (piece.type == PieceType.T && CheckPiecePositionValid(piece, newPieceArray, kickTDiagLeft))
            {
                kickBackPosition = kickTDiagLeft;
            }
            else if (piece.type == PieceType.T && CheckPiecePositionValid(piece, newPieceArray, kickTDiagRight))
            {
                kickBackPosition = kickTDiagRight;
            }
            else if (CheckPiecePositionValid(piece, newPieceArray, kickBackUp))
            {
                kickBackPosition = kickBackUp;
            }
            else if (piece.type == PieceType.I && CheckPiecePositionValid(piece, newPieceArray, kickLineToRight))
            {
                kickBackPosition = kickLineToRight;
            }
            else if (piece.type == PieceType.I && CheckPiecePositionValid(piece, newPieceArray, kickLineToLeft))
            {
                kickBackPosition = kickLineToLeft;
            }
            else if (piece.type == PieceType.I && CheckPiecePositionValid(piece, newPieceArray, kickLineDown))
            {
                kickBackPosition = kickLineDown;
            }
            else if (piece.type == PieceType.I && CheckPiecePositionValid(piece, newPieceArray, kickLineUp))
            {
                kickBackPosition = kickLineUp;
            }
            else
            {
                kickBackPosition = noKickBack;
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
                        if (!NotOutOfRange(newYPos, newXPos) || grid[newYPos, newXPos] == PieceType.Dead)
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
                    if (NotOutOfRange(piece.position.Y + ghostPiecePos + i, piece.position.X + j) &&
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
                    if (NotOutOfRange(i, j) && grid[i, j] == PieceType.Ghost)
                    {
                        grid[i, j] = PieceType.Empty;
                    }
                }
            }
        }

        #endregion

        #region Debug

        public void debug_print_grid()
        {
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    Console.Write(grid[i, j] + ' ');
                }

                Console.WriteLine();
            }
        }

        public void debug_change_type(int i, int j, PieceType type)
        {
            grid[i, j] = type;
        }

        public PieceType debug_get_type(int i, int j)
        {
            return grid[i, j];
        }

        #endregion
    }
}