using System.Collections.Generic;
using SFML.Graphics;
using TetrisDotnet.Code.Game;

namespace TetrisDotnet.Code.UI.Elements
{
	// ReSharper disable once InconsistentNaming
	public class QueuedPiecesUI
	{
		Sprite[][,] queueSpriteArray4x4;
		Sprite[][,] queueSpriteArray1x4;
		Sprite[][,] queueSpriteArray3x3;

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