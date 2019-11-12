using System.Collections.Generic;
using SFML.Graphics;
using TetrisDotnet.Code.Game;

namespace TetrisDotnet.Code.UI.Elements
{
	// ReSharper disable once InconsistentNaming
	public class HeldPieceUI
	{
		Sprite[,] holdSprite4x4;
		Sprite[,] holdSprite1x4;
		Sprite[,] holdSprite3x3;

		public void Draw()
		{
//			PieceType[,] pieceToDraw = StaticVars.GetPieceArray(holdManager.currentPiece);
//
//			if (holdManager.currentPiece == PieceType.I)
//			{
//				for (int j = 0; j < holdSprite1x4.GetLength(0); j++)
//				{
//					for (int k = 0; k < pieceToDraw.GetLength(1); k++)
//					{
//						if (pieceToDraw[j + 1, k] != PieceType.Empty)
//						{
//							holdSprite1x4[j, k].Texture = StaticVars.blockTextures[(int) pieceToDraw[j + 1, k]];
//
//							window.Draw(holdSprite1x4[j, k]);
//						}
//					}
//				}
//			}
//			else if (pieceToDraw.GetLength(1) == 4)
//			{
//				for (int j = 0; j < pieceToDraw.GetLength(0); j++)
//				{
//					for (int k = 0; k < pieceToDraw.GetLength(1); k++)
//					{
//						if (pieceToDraw[j, k] != PieceType.Empty)
//						{
//							holdSprite4x4[j, k].Texture = StaticVars.blockTextures[(int) pieceToDraw[j, k]];
//
//							window.Draw(holdSprite4x4[j, k]);
//						}
//					}
//				}
//			}
//			else
//			{
//				for (int j = 0; j < pieceToDraw.GetLength(0); j++)
//				{
//					for (int k = 0; k < pieceToDraw.GetLength(1); k++)
//					{
//						if (pieceToDraw[j, k] != PieceType.Empty)
//						{
//							holdSprite3x3[j, k].Texture = StaticVars.blockTextures[(int) pieceToDraw[j, k]];
//							window.Draw(holdSprite3x3[j, k]);
//						}
//					}
//				}
//			}
		}
	}
}