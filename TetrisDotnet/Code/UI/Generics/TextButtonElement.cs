using SFML.Graphics;
using TetrisDotnet.Code.Events;
using TetrisDotnet.Code.Events.EventData;
using TetrisDotnet.Code.UI.Animations;
using TetrisDotnet.Code.UI.Animations.ElementAnimations;
using TetrisDotnet.Code.UI.Base;
using TetrisDotnet.Code.Utils.Enums;

namespace TetrisDotnet.Code.UI.Generics
{
	public sealed class TextButtonElement : RectangleElement
	{
		private readonly Color lightColor = new Color(69, 240, 31);
		private readonly Color mediumColor = new Color(29, 158, 0);
		private readonly Color darkColor = new Color(19, 107, 0);

		private readonly Color lightFadeColor = new Color(255, 145, 229);
		private readonly Color mediumFadeColor = new Color(179, 66, 152);
		private readonly Color darkFadeColor = new Color(127, 47, 109);

		private bool aiIsActive = true;

		private readonly TextElement textElement;

		public string DisplayedString
		{
			get => textElement.DisplayedString;
			set => textElement.DisplayedString = value;
		}

		public TextButtonElement()
		{			
			CapturesMouseClickEvents = true;
			CapturesMouseMoveEvents = true;
			FillColor = darkColor;
			OutlineColor = mediumColor;
			StretchToFit = true;
			
			textElement = new TextElement
			{
				CharacterSize = 16,
				Font = AssetPool.Font,
				FillColor = mediumColor,
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center,
			};
			AddChild(textElement);
		}

		protected override void OnButtonDown()
		{			
			Animation buttonDownAnimation = new Animation();
			buttonDownAnimation.AddElementAnimation(new FillColorAnimation(0.0f, 0.09f, textElement, aiIsActive ? mediumColor : mediumFadeColor));
			buttonDownAnimation.AddElementAnimation(new FillColorAnimation(0.0f, 0.09f, this,  aiIsActive ? darkColor : darkFadeColor));
			buttonDownAnimation.AddElementAnimation(new OutlineColorAnimation(0.0f, 0.09f, this,  aiIsActive ? lightColor : lightFadeColor));
			OutlineThickness = 1;
			
			PlayAnimation(buttonDownAnimation);
			
			base.OnButtonDown();
		}

		protected override void OnButtonPressed()
		{
			aiIsActive = !aiIsActive;
			Application.EventSystem.ProcessEvent(EventType.ToggleAi, new ToggleEventData(aiIsActive));
			DisplayedString = $"Ai is {(aiIsActive ? "On" : "Off")}";
			base.OnButtonPressed();
		}

		protected override void OnButtonReleased()
		{
			Animation buttonReleasedAnimation = new Animation();
			if (MouseInside)
			{
				buttonReleasedAnimation.AddElementAnimation(new FillColorAnimation(0.0f, 0.15f, textElement, aiIsActive ?  lightColor : lightFadeColor));
				buttonReleasedAnimation.AddElementAnimation(new FillColorAnimation(0.0f, 0.15f, this,  aiIsActive ? mediumColor : mediumFadeColor));
				buttonReleasedAnimation.AddElementAnimation(new OutlineColorAnimation(0.0f, 0.15f, this,  aiIsActive ? lightColor : lightFadeColor));
				OutlineThickness = 1;
			}
			else
			{
				buttonReleasedAnimation.AddElementAnimation(new FillColorAnimation(0.0f, 0.25f, textElement, aiIsActive ?  mediumColor : mediumFadeColor));
				buttonReleasedAnimation.AddElementAnimation(new FillColorAnimation(0.0f, 0.25f, this,  aiIsActive ? darkColor : darkFadeColor));
				buttonReleasedAnimation.AddElementAnimation(new OutlineColorAnimation(0.0f, 0.25f, this,  aiIsActive ? mediumColor : mediumFadeColor));
				OutlineThickness = -1;
			}
			
			PlayAnimation(buttonReleasedAnimation);
			
			base.OnButtonReleased();
		}

		protected override void OnMouseEnter()
		{
			if (!ButtonDown)
			{
				Animation mouseEnterAnimation = new Animation();
				mouseEnterAnimation.AddElementAnimation(new FillColorAnimation(0.0f, 0.25f, textElement, aiIsActive ?  lightColor : lightFadeColor));
				mouseEnterAnimation.AddElementAnimation(new FillColorAnimation(0.0f, 0.25f, this,  aiIsActive ? mediumColor : mediumFadeColor));
				mouseEnterAnimation.AddElementAnimation(new OutlineColorAnimation(0.0f, 0.25f, this,  aiIsActive ? lightColor : lightFadeColor));
				OutlineThickness = 1;
				PlayAnimation(mouseEnterAnimation);
			}

			base.OnMouseEnter();
		}

		protected override void OnMouseExit()
		{			
			if (!ButtonDown)
			{
				Animation mouseExitAnimation = new Animation();
				mouseExitAnimation.AddElementAnimation(new FillColorAnimation(0.0f, 0.25f, textElement, aiIsActive ?  mediumColor : mediumFadeColor));
				mouseExitAnimation.AddElementAnimation(new FillColorAnimation(0.0f, 0.25f, this,  aiIsActive ? darkColor : darkFadeColor));
				mouseExitAnimation.AddElementAnimation(new OutlineColorAnimation(0.0f, 0.25f, this,  aiIsActive ? mediumColor : mediumFadeColor));
				OutlineThickness = -1;
				PlayAnimation(mouseExitAnimation);
			}
			base.OnMouseExit();
		}
	}
}