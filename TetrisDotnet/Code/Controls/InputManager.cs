using SFML.Window;

namespace TetrisDotnet.Code.Controls
{
	public class InputManager
	{
		public void OnKeyPressed(object sender, KeyEventArgs e)
		{
			if (gameState == GameState.Playing)
			{
				switch (e.Code)
				{
					// Debug function to level up automagically.
					case Keyboard.Key.L:
						levelText.LevelUp();
						break;

					// Rotate active piece clockwise.
					case Keyboard.Key.Up:
					case Keyboard.Key.W:
						if (grid.RotatePiece(activePiece, Rotation.Clockwise))
						{
							dropTime -= levelText.sideMoveSpeed;
						}

						break;

					// Rotate active piece counter clockwise.
					case Keyboard.Key.E:
						if (grid.RotatePiece(activePiece, Rotation.CounterClockwise))
						{
							dropTime -= levelText.sideMoveSpeed;
						}

						break;

					// Move active piece left.
					case Keyboard.Key.Left:
					case Keyboard.Key.A:
						if (!grid.CanPlacePiece(activePiece, Vector2iUtils.down))
						{
							dropTime -= levelText.sideMoveSpeed;
						}

						grid.MovePiece(activePiece, Vector2iUtils.left);

						break;

					// Move active piece down.
					case Keyboard.Key.Down:
					case Keyboard.Key.S:
						if (grid.CanPlacePiece(activePiece, Vector2iUtils.down))
						{
							grid.MovePiece(activePiece, Vector2iUtils.down);
							scoreText.AddScore(1);
						}
						else
						{
							dropTime = 1;
						}

						break;

					// Move active piece right.
					case Keyboard.Key.Right:
					case Keyboard.Key.D:
						if (!grid.CanPlacePiece(activePiece, Vector2iUtils.down))
						{
							dropTime -= levelText.sideMoveSpeed;
						}

						grid.MovePiece(activePiece, Vector2iUtils.right);
						break;

					// Holds the active piece (if a held piece exists, it will be selected as the new active piece).
					case Keyboard.Key.LShift:
					case Keyboard.Key.C:
						if (holdManager.canSwap)
						{
							PieceType oldPiece = activePiece.type;

							grid.RemovePiece(activePiece);

							if (holdManager.currentPiece == PieceType.Empty)
							{
								NewPiece();
							}
							else
							{
								NewPiece(holdManager.currentPiece);
							}

							holdManager.currentPiece = oldPiece;
							holdManager.canSwap = false;
						}

						break;

					// Hard drop active piece.
					case Keyboard.Key.Space:
						int spacesMoved = grid.DetermineDropdownPosition(activePiece);
						scoreText.AddScore(2 * spacesMoved);

						grid.MovePiece(activePiece, Vector2iUtils.down * (spacesMoved));

						dropTime = levelText.dropSpeed;

						DrawActivePieceHardDrop();
						break;

					// Opens the menu.
					case Keyboard.Key.Escape:
//						soundplayer.Stop();
						Quit();
						break;

					// Pause game.
					case Keyboard.Key.P:
						gameState = GameState.Pause;
						break;
				}
			}
			else if (gameState == GameState.Pause)
			{
				switch (e.Code)
				{
					case Keyboard.Key.Escape:
					case Keyboard.Key.P:
						gameState = GameState.Playing;
						break;
				}
			}
		}
	}
}