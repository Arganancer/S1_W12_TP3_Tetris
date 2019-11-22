using TetrisDotnet.Code.UI.Base;
using TetrisDotnet.Code.UI.Generics;
using TetrisDotnet.Code.UI.SealedElements;

namespace TetrisDotnet.Code.UI.Layouts
{
	public class GameLayout : UiLayout
	{
		public GameLayout()
		{
			// Background
			Elements.Add(new SpriteElement()
			{
				Texture = AssetPool.BackDropTexture,
				StretchToFit = true
			});

			// Left Section
			UiElement leftSection = new UiElement(0.0f, 1.0f, 0.0f, 0.3f);
			Elements.Add(leftSection);
			ListContainer leftSectionContainer = new ListContainer(0.0f, 1.0f, 0.0f, 1.0f)
			{
				Orientation = Orientation.Vertical,
				Spacing = 8.0f,
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
				{TopAnchor = 0.0f, BottomAnchor = 0.0f, LeftAnchor = 0.0f, RightAnchor = 1.0f, AutoHeight = true});
			leftSectionContainer.AddChild(new LevelText
				{TopAnchor = 0.0f, BottomAnchor = 0.0f, LeftAnchor = 0.0f, RightAnchor = 1.0f, AutoHeight = true});
			leftSectionContainer.AddChild(new RealTimeText
				{TopAnchor = 0.0f, BottomAnchor = 0.0f, LeftAnchor = 0.0f, RightAnchor = 1.0f, AutoHeight = true});
			leftSectionContainer.AddChild(new PiecesPlacedText
				{TopAnchor = 0.0f, BottomAnchor = 0.0f, LeftAnchor = 0.0f, RightAnchor = 1.0f, AutoHeight = true});

			// Right Section
			UiElement rightSection = new UiElement(0.0f, 1.0f, 0.7f, 1.0f);
			Elements.Add(rightSection);
			ListContainer rightSectionContainer = new ListContainer(0.0f, 1.0f, 0.0f, 1.0f)
			{
				Orientation = Orientation.Vertical,
				Spacing = 8.0f,
				LeftPadding = 25.0f,
				TopPadding = 25.0f,
				BottomPadding = 25.0f
			};
			rightSection.AddChild(rightSectionContainer);
			rightSectionContainer.AddChild(new PieceQueueUI
			{
				TopAnchor = 0.0f, BottomAnchor = 0.0f, LeftAnchor = 0.0f, RightAnchor = 1.0f,
				BottomHeight = AssetPool.QueueTexture.Size.Y,
			});
			rightSectionContainer.AddChild(new TextButtonElement
			{
				TopAnchor = 0.0f, BottomAnchor = 0.0f, LeftAnchor = 0.2f, RightAnchor = 0.8f, BottomHeight = 24,
				DisplayedString = "Ai is On"
			});
			rightSectionContainer.AddChild(new PieceScoreElement
			{
				TopAnchor = 0.0f, BottomAnchor = 1.0f, LeftAnchor = 0.0f, RightAnchor = 1.0f,
			});

			// Center Section
			Elements.Add(new GridUI
			{
				TopAnchor = 0.0f, BottomAnchor = 1.0f, LeftAnchor = 0.3f, RightAnchor = 0.7f
			});

			// Overlays
			Elements.Add(new PauseText());
		}
	}
}