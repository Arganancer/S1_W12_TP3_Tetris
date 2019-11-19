using SFML.Graphics;
using SFML.System;
using TetrisDotnet.Code.UI.Base;
using TetrisDotnet.Code.UI.Elements;
using TetrisDotnet.Code.Utils.Enums;

namespace TetrisDotnet.Code.UI.Layouts
{
	public class GameLayout : UiLayout
	{
		private readonly GridUI gridUi = new GridUI();

		public GameLayout()
		{
			// Left Section
			UiElement leftSection = new UiElement(0.0f, 1.0f, 0.0f, 0.3f);
			Elements.Add(leftSection);
			ListContainer leftSectionContainer = new ListContainer(0.0f, 1.0f, 0.0f, 1.0f)
			{
				Orientation = Orientation.Vertical,
				Spacing = 5.0f,
				LeftPadding = 25.0f,
				TopPadding = 25.0f,
				BottomPadding = 25.0f,
			};
			leftSection.AddChild(leftSectionContainer);

			leftSectionContainer.AddChild(new HeldPieceUI
			{
				TopAnchor = 0.0f, BottomAnchor = 0.0f, LeftAnchor = 0.0f, RightAnchor = 1.0f,
				BottomHeight = AssetPool.HoldTexture.Size.Y,
			});

			leftSectionContainer.AddChild(new ScoreText
				{TopAnchor = 0.0f, BottomAnchor = 0.0f, LeftAnchor = 0.0f, RightAnchor = 1.0f, BottomHeight = 16});

			leftSectionContainer.AddChild(new TextElement
			{
				DisplayedString = "Test 1", Font = AssetPool.Font, CharacterSize = 16, TopAnchor = 0.0f,
				BottomAnchor = 0.0f, LeftAnchor = 0.0f, RightAnchor = 1.0f, BottomHeight = 16, TextHorizontalAlignment = HorizontalAlignment.Left
			});
			
			leftSectionContainer.AddChild(new TextElement
			{
				DisplayedString = "Test 2 Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.", 
				Font = AssetPool.Font, CharacterSize = 16, TopAnchor = 0.0f,
				BottomAnchor = 0.0f, LeftAnchor = 0.0f, RightAnchor = 1.0f, BottomHeight = 16, TextHorizontalAlignment = HorizontalAlignment.Left, TextVerticalAlignment = VerticalAlignment.Top, Wrap = true
			});
			
			leftSectionContainer.AddChild(new TextElement
			{
				DisplayedString = "Test 3", Font = AssetPool.Font, CharacterSize = 16, TopAnchor = 0.0f,
				BottomAnchor = 0.0f, LeftAnchor = 0.0f, RightAnchor = 1.0f, BottomHeight = 16, TextHorizontalAlignment = HorizontalAlignment.Right
			});

//			levelText = new LevelText();
//			realTimeText = new RealTimeText();
//			controlsText = new ControlsText();

			// Center Section

			// Right Section
			UiElement rightSection = new UiElement(0.0f, 1.0f, 0.7f, 1.0f);
			Elements.Add(rightSection);
			rightSection.AddChild(new PieceQueueUI());

//			statsTextBlock = new StatsTextBlock();

			// Overlays
			Elements.Add(new PauseText());

			AssetPool.DrawGridSprite.Position = new Vector2f(GridUI.Position.X - AssetPool.BlockSize.X * 1.5f,
				GridUI.Position.Y - AssetPool.BlockSize.Y * 2f);
		}

		public override void Update(float deltaTime)
		{
			base.Update(deltaTime);
		}

		public override void Draw(RenderWindow window)
		{
			window.Draw(AssetPool.BackDrop);
			window.Draw(AssetPool.DrawGridSprite);
			gridUi.Draw(window);

			base.Draw(window);
		}
	}
}