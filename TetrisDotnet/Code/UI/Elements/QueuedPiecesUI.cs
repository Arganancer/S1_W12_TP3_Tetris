using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;
using TetrisDotnet.Code.Game;
using TetrisDotnet.Code.Game.World;

namespace TetrisDotnet.Code.UI.Elements
{
	// ReSharper disable once InconsistentNaming
	public class QueuedPiecesUI
	{
		Sprite[][,] queueSpriteArray4x4;
		Sprite[][,] queueSpriteArray1x4;
		Sprite[][,] queueSpriteArray3x3;

		public QueuedPiecesUI()
		{
			// Code that was in the SetUpVariables function:
			
//			AssetPool.queueSprite.Position =
//				new Vector2f(AssetPool.gridXPos /*Now in GridUI, Position*/ + AssetPool.blockSize.X * (Grid.GridHeight - 2 + 2.25f),
//					AssetPool.gridYPos /*Now in GridUI, Position*/);

			queueSpriteArray4x4 = new[]
			{
				new Sprite[4, 4],
				new Sprite[4, 4],
				new Sprite[4, 4]
			};

			for (int i = 0; i < queueSpriteArray4x4.Length; i++)
			{
				for (int j = 0; j < queueSpriteArray4x4[0].GetLength(0); j++)
				{
					for (int k = 0; k < queueSpriteArray4x4[0].GetLength(1); k++)
					{
						queueSpriteArray4x4[i][j, k] = new Sprite();

						queueSpriteArray4x4[i][j, k].Position = new Vector2f(
							AssetPool.queueSprite.Position.X + AssetPool.blockSize.X * 1.5f +
							AssetPool.blockSize.X * k,
							AssetPool.queueSprite.Position.Y + AssetPool.blockSize.Y * 2.5f +
							AssetPool.blockSize.Y * j +
							AssetPool.blockSize.Y * (queueSpriteArray4x4[0].GetLength(0) - 1) * i -
							AssetPool.blockSize.Y);
					}
				}
			}
			
			queueSpriteArray1x4 = new[]
			{
				new Sprite[1, 4],
				new Sprite[1, 4],
				new Sprite[1, 4]
			};

			for (int i = 0; i < queueSpriteArray1x4.Length; i++)
			{
				for (int j = 0; j < queueSpriteArray1x4[0].GetLength(0); j++)
				{
					for (int k = 0; k < queueSpriteArray1x4[0].GetLength(1); k++)
					{
						queueSpriteArray1x4[i][j, k] = new Sprite();

						queueSpriteArray1x4[i][j, k].Position = new Vector2f(
							AssetPool.queueSprite.Position.X + AssetPool.blockSize.X * 1.5f +
							AssetPool.blockSize.X * k,
							AssetPool.queueSprite.Position.Y + AssetPool.blockSize.Y * 2.5f +
							AssetPool.blockSize.Y * j +
							AssetPool.blockSize.Y * (queueSpriteArray4x4[0].GetLength(0) - 1) * i +
							AssetPool.blockSize.Y * 0.5f);
					}
				}
			}

			queueSpriteArray3x3 = new[]
			{
				new Sprite[3, 3],
				new Sprite[3, 3],
				new Sprite[3, 3]
			};

			for (int i = 0; i < queueSpriteArray3x3.Length; i++)
			{
				for (int j = 0; j < queueSpriteArray3x3[0].GetLength(0); j++)
				{
					for (int k = 0; k < queueSpriteArray3x3[0].GetLength(1); k++)
					{
						queueSpriteArray3x3[i][j, k] = new Sprite();

						queueSpriteArray3x3[i][j, k].Position = new Vector2f(
							AssetPool.queueSprite.Position.X + AssetPool.blockSize.X * 1.5f +
							AssetPool.blockSize.X * k + AssetPool.blockSize.X * 0.5f,
							AssetPool.queueSprite.Position.Y + AssetPool.blockSize.Y * 2.5f +
							AssetPool.blockSize.Y * j +
							AssetPool.blockSize.Y * queueSpriteArray3x3[0].GetLength(0) * i);
					}
				}
			}
		}

		public void Draw()
		{
//			List<PieceType> queueArray = pieceQueue.Get().ToList();
//
//			for (int i = 0; i < queueArray.Count; i++)
//			{
//				List<PieceType> blocks = queueArray[i].PieceType[,] queuePieceToDraw =
//					StaticVars.GetPieceArray(queueArray[i]);
//
//				if (queueArray[i] == PieceType.I)
//				{
//					for (int j = 0; j < queueSpriteArray1x4[0].GetLength(0); j++)
//					{
//						for (int k = 0; k < queuePieceToDraw.GetLength(1); k++)
//						{
//							if (queuePieceToDraw[j + 1, k] != PieceType.Empty)
//							{
//								queueSpriteArray1x4[i][j, k].Texture =
//									StaticVars.blockTextures[(int) queuePieceToDraw[j + 1, k]];
//
//								window.Draw(queueSpriteArray1x4[i][j, k]);
//							}
//						}
//					}
//				}
//				else if (queuePieceToDraw.GetLength(1) == 4)
//				{
//					for (int j = 0; j < queuePieceToDraw.GetLength(0); j++)
//					{
//						for (int k = 0; k < queuePieceToDraw.GetLength(1); k++)
//						{
//							if (queuePieceToDraw[j, k] != PieceType.Empty)
//							{
//								queueSpriteArray4x4[i][j, k].Texture =
//									StaticVars.blockTextures[(int) queuePieceToDraw[j, k]];
//
//								window.Draw(queueSpriteArray4x4[i][j, k]);
//							}
//						}
//					}
//				}
//				else
//				{
//					for (int j = 0; j < queuePieceToDraw.GetLength(0); j++)
//					{
//						for (int k = 0; k < queuePieceToDraw.GetLength(1); k++)
//						{
//							if (queuePieceToDraw[j, k] != PieceType.Empty)
//							{
//								queueSpriteArray3x3[i][j, k].Texture =
//									StaticVars.blockTextures[(int) queuePieceToDraw[j, k]];
//								window.Draw(queueSpriteArray3x3[i][j, k]);
//							}
//						}
//					}
//				}
//			}
		}
	}
}