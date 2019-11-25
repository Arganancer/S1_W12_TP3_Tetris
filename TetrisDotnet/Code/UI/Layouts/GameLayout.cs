using SFML.Graphics;
using TetrisDotnet.Code.UI.Base;
using TetrisDotnet.Code.UI.Base.BaseElement;
using TetrisDotnet.Code.UI.Generics;
using TetrisDotnet.Code.UI.SealedElements;
using TetrisDotnet.Code.Utils.Enums;

namespace TetrisDotnet.Code.UI.Layouts
{
	public class GameLayout : UiLayout
	{
		public GameLayout()
		{
			// Background
			Elements.Add(new SpriteElement
			{
				Texture = AssetPool.BackDropTexture,
				StretchToFit = true
			});

			// Left Section
			UiElement leftSection = new UiElement
			{
				TopAnchor = 0.0f,
				BottomAnchor = 1.0f,
				LeftAnchor = 0.0f,
				RightAnchor = 0.3f,
			};
			Elements.Add(leftSection);
			ListContainer leftSectionContainer = new ListContainer
			{
				TopAnchor = 0.0f,
				BottomAnchor = 1.0f,
				LeftAnchor = 0.0f,
				RightAnchor = 1.0f,
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
			leftSectionContainer.AddChild(new PieceScoreElement
			{
				TopAnchor = 0.0f, BottomAnchor = 1.0f, LeftAnchor = 0.0f, RightAnchor = 1.0f,
			});

			// Right Section
			UiElement rightSection = new UiElement
			{
				TopAnchor = 0.0f,
				BottomAnchor = 1.0f,
				LeftAnchor = 0.7f,
				RightAnchor = 1.0f,
			};
			Elements.Add(rightSection);
			ListContainer rightSectionContainer = new ListContainer
			{
				TopAnchor = 0.0f,
				BottomAnchor = 1.0f,
				LeftAnchor = 0.0f,
				RightAnchor = 1.0f,
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

			rightSectionContainer.AddChild(CreateLabelTextFieldElement("Ai Tick", new AiTickSpeedTextFieldElement()));
			rightSectionContainer.AddChild(CreateLabelTextFieldElement("Ai Bumpiness", new AiBumpinessTextFieldElement()));
			rightSectionContainer.AddChild(CreateLabelTextFieldElement("Ai Aggregate",
				new AiAggregateHeightTextFieldElement()));
			rightSectionContainer.AddChild(CreateLabelTextFieldElement("Ai Top Height",
				new AiTopHeightTextFieldElement()));
			rightSectionContainer.AddChild(CreateLabelTextFieldElement("Ai Holes", new AiNbOfHolesTextFieldElement()));

			// Center Section
			Elements.Add(new GridUI
			{
				TopAnchor = 0.0f, BottomAnchor = 1.0f, LeftAnchor = 0.3f, RightAnchor = 0.7f
			});

			// Overlays
			Elements.Add(new PauseText());
		}

		private UiElement CreateLabelTextFieldElement(string labelText, TextFieldElement textField)
		{
			float split = 0.4f;
			UiElement element = new UiElement
			{
				TopAnchor = 0.0f, BottomAnchor = 0.0f, LeftAnchor = 0.2f, RightAnchor = 0.8f, BottomHeight = 24,
			};
			element.AddChild(new TextElement
			{
				TopAnchor = 0.0f, BottomAnchor = 1.0f, LeftAnchor = 0.0f, RightAnchor = split, LeftWidth = 0.0f,
				DisplayedString = labelText, HorizontalAlignment = HorizontalAlignment.Right, Font = AssetPool.Font,
				CharacterSize = 14,
				FillColor = Color.Green, RightWidth = -10,
			});
			textField.TopAnchor = 0.0f;
			textField.BottomAnchor = 1.0f;
			textField.LeftAnchor = split;
			textField.RightAnchor = 1.0f;

			element.AddChild(textField);

			return element;
		}
	}
}