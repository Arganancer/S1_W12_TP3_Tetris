using SFML.Graphics;
using SFML.System;
using TetrisDotnet.Code.UI.Elements;

namespace TetrisDotnet.Code.UI.Layouts
{
	public class GameLayout : UiLayout
	{
		private readonly GridUI gridUi = new GridUI();
		private ScoreText scoreText;
		private LevelText levelText;
		private RealTimeText realTimeText;
		private StatsTextBlock statsTextBlock;
		private readonly ControlsText controlsText;
		private readonly HeldPieceUI heldPieceUi;
		private PieceQueueUI pieceQueueUi;

		public GameLayout()
		{
			scoreText = new ScoreText();
			levelText = new LevelText();
			realTimeText = new RealTimeText();
			controlsText = new ControlsText();

			heldPieceUi = new HeldPieceUI();
			pieceQueueUi = new PieceQueueUI();
			statsTextBlock = new StatsTextBlock();
			Elements.Add(new PauseText());
			
			AssetPool.DrawGridSprite.Position = new Vector2f(GridUI.Position.X - AssetPool.BlockSize.X * 1.5f,
				GridUI.Position.Y - AssetPool.BlockSize.Y * 2f);

		}

		public override void Update(float deltaTime)
		{
			realTimeText.RealTime += deltaTime;
			base.Update(deltaTime);
		}

		public override void Draw(RenderWindow window)
		{			
			window.Draw(AssetPool.BackDrop);
			window.Draw(AssetPool.HoldSprite);
			window.Draw(AssetPool.QueueSprite);
			window.Draw(AssetPool.DrawGridSprite);
			gridUi.Draw(window);
			heldPieceUi.Draw(window);
			pieceQueueUi.Draw(window);
			window.Draw(scoreText);
			window.Draw(levelText);
			window.Draw(realTimeText);
			window.Draw(controlsText);
			window.Draw(AssetPool.StatsSprite);
			
			foreach (Text statTextBlock in statsTextBlock.GetDrawable())
			{
				window.Draw(statTextBlock);
			}
			
			base.Draw(window);
		}
	}
}