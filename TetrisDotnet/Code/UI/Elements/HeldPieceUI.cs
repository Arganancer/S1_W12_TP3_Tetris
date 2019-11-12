using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;
using TetrisDotnet.Code.Game;

namespace TetrisDotnet.Code.UI.Elements
{
	// ReSharper disable once InconsistentNaming
	public class HeldPieceUI
	{
		Sprite[,] holdSprite4x4;
		Sprite[,] holdSprite1x4;
		Sprite[,] holdSprite3x3;

		public HeldPieceUI()
		{
			// Code that was in the SetUpVariables function:
			
//			AssetPool.holdSprite.Position =
//				new Vector2f(AssetPool.gridXPos /*Now in GridUI, Position*/ - AssetPool.holdTexture.Size.X - (AssetPool.blockSize.X * 2.25f),
//					AssetPool.gridYPos /*Now in GridUI, Position*/ );

			holdSprite3x3 = new Sprite[3, 3];

			for (int i = 0; i < holdSprite3x3.GetLength(0); i++)
			{
				for (int j = 0; j < holdSprite3x3.GetLength(1); j++)
				{
					holdSprite3x3[i, j] = new Sprite();

					holdSprite3x3[i, j].Position = new Vector2f(
						AssetPool.holdSprite.Position.X + AssetPool.blockSize.X * 1.5f + AssetPool.blockSize.X * j +
						AssetPool.blockSize.X * 0.5f,
						AssetPool.holdSprite.Position.Y + AssetPool.blockSize.Y * 3.5f + AssetPool.blockSize.Y * i);
				}
			}

			holdSprite4x4 = new Sprite[4, 4];

			for (int i = 0; i < holdSprite4x4.GetLength(0); i++)
			{
				for (int j = 0; j < holdSprite4x4.GetLength(1); j++)
				{
					holdSprite4x4[i, j] = new Sprite();

					holdSprite4x4[i, j].Position = new Vector2f(
						AssetPool.holdSprite.Position.X + AssetPool.blockSize.X * 1.5f + AssetPool.blockSize.X * j,
						AssetPool.holdSprite.Position.Y + AssetPool.blockSize.Y * 2.5f + AssetPool.blockSize.Y * i);
				}
			}

			holdSprite1x4 = new Sprite[1, 4];

			for (int i = 0; i < holdSprite1x4.GetLength(0); i++)
			{
				for (int j = 0; j < holdSprite1x4.GetLength(1); j++)
				{
					holdSprite1x4[i, j] = new Sprite();

					holdSprite1x4[i, j].Position = new Vector2f(
						AssetPool.holdSprite.Position.X + AssetPool.blockSize.X * 1.5f + AssetPool.blockSize.X * j,
						AssetPool.holdSprite.Position.Y + AssetPool.blockSize.Y * 2.5f + AssetPool.blockSize.Y * i +
						AssetPool.blockSize.Y * 1.5f);
				}
			}
		}

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