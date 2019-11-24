using System.Diagnostics;
using SFML.Graphics;
using SFML.Window;
using TetrisDotnet.Code.Events.EventData;
using TetrisDotnet.Code.UI.Animations;
using TetrisDotnet.Code.UI.Animations.ElementAnimations;
using TetrisDotnet.Code.Utils.Enums;
using TetrisDotnet.Code.Utils.Extensions;
using EventType = TetrisDotnet.Code.Events.EventType;

namespace TetrisDotnet.Code.UI.Base
{
	public class TextFieldElement : RectangleElement
	{
		private readonly Color lightColor = new Color(69, 240, 31);
		private readonly Color mediumColor = new Color(29, 158, 0);
		private readonly Color darkColor = new Color(0, 0, 0);

		private readonly TextElement textElement;

		private string oldValue;
		private string newValue;
		private int cursorIndex;

		private void StartKeyCapture()
		{
			Application.EventSystem.Subscribe(EventType.InputKeyCode, OnInputKeyCode);
			Application.EventSystem.Subscribe(EventType.TextEntered, OnTextEntered);
			Application.EventSystem.ProcessEvent(EventType.ToggleKeyboardMode, new ToggleEventData(true));
		}

		private void EndKeyCapture()
		{
			Application.EventSystem.QueueUnsubscribe(EventType.InputKeyCode, OnInputKeyCode);
			Application.EventSystem.QueueUnsubscribe(EventType.TextEntered, OnTextEntered);
			Application.EventSystem.ProcessEvent(EventType.ToggleKeyboardMode, new ToggleEventData(false));
		}
		
		public TextFieldElement()
		{
			CapturesMouseClickEvents = true;
			CapturesMouseMoveEvents = true;			
			FillColor = darkColor;
			OutlineColor = mediumColor;
			StretchToFit = true;
			
			textElement = new TextElement
			{
				TopAnchor = 0.0f,
				BottomAnchor = 1.0f,
				LeftAnchor = 0.1f,
				RightAnchor = 0.9f,
				CharacterSize = 16,
				Font = AssetPool.Font,
				FillColor = mediumColor,
				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Center,
			};
			AddChild(textElement);
		}

		~TextFieldElement()
		{
			EndKeyCapture();
		}
		
		private void OnInputKeyCode(EventData eventData)
		{
			KeyPressedEventData keyPressedEventData = eventData as KeyPressedEventData;
			Debug.Assert(keyPressedEventData != null, nameof(keyPressedEventData) + " != null");
			switch (keyPressedEventData.Key)
			{
				case Keyboard.Key.Enter:
					textElement.DisplayedString = newValue;
					EndKeyCapture();
					break;
				case Keyboard.Key.Escape:
					textElement.DisplayedString = oldValue;
					EndKeyCapture();
					break;
				case Keyboard.Key.Delete:
					if (cursorIndex < newValue.Length)
					{
						newValue = newValue.Remove(cursorIndex, 1);
						textElement.DisplayedString = newValue;
					}
					break;
				case Keyboard.Key.Backspace:
					if (cursorIndex > 0)
					{
						newValue = newValue.Remove(--cursorIndex, 1);
						textElement.DisplayedString = newValue;
					}
					break;
				case Keyboard.Key.Left:
					if (cursorIndex > 0)
					{
						--cursorIndex;
					}
					break;
				case Keyboard.Key.Right:
					if (cursorIndex < newValue.Length - 1)
					{
						++cursorIndex;
					}
					break;
				default:
					if (keyPressedEventData.Key.IsNumeric())
					{

					}
					break;
			}
		}

		private void OnTextEntered(EventData eventData)
		{
			TextEnteredEventData textEnteredEventData = eventData as TextEnteredEventData;
			Debug.Assert(textEnteredEventData != null, nameof(textEnteredEventData) + " != null");
			if (float.TryParse(textEnteredEventData.Text, out float value) || textEnteredEventData.Text == ".")
			{
				if (cursorIndex < newValue.Length)
				{
					newValue = newValue.Insert(cursorIndex++, textEnteredEventData.Text);
					textElement.DisplayedString = newValue;
				}
				else
				{
					newValue += textEnteredEventData.Text;
					textElement.DisplayedString = newValue;
					++cursorIndex;
				}
			}
		}

		protected override void OnPressedOutsideElement()
		{
			EndKeyCapture();
			base.OnPressedOutsideElement();
		}

		protected override void OnButtonDown()
		{			
			Animation buttonDownAnimation = new Animation();
			buttonDownAnimation.AddElementAnimation(new FillColorAnimation(0.0f, 0.09f, textElement, mediumColor));
			buttonDownAnimation.AddElementAnimation(new FillColorAnimation(0.0f, 0.09f, this,  darkColor));
			buttonDownAnimation.AddElementAnimation(new OutlineColorAnimation(0.0f, 0.09f, this,  lightColor));
			OutlineThickness = 1;
			
			PlayAnimation(buttonDownAnimation);
			
			base.OnButtonDown();
		}

		protected override void OnButtonPressed()
		{
			StartKeyCapture();
			oldValue = textElement.DisplayedString;
			newValue = "";
			textElement.DisplayedString = newValue;
			cursorIndex = 0;
			base.OnButtonPressed();
		}

		protected override void OnButtonReleased()
		{
			Animation buttonReleasedAnimation = new Animation();
			if (MouseInside)
			{
				buttonReleasedAnimation.AddElementAnimation(new FillColorAnimation(0.0f, 0.15f, textElement, lightColor));
				buttonReleasedAnimation.AddElementAnimation(new FillColorAnimation(0.0f, 0.15f, this, mediumColor));
				buttonReleasedAnimation.AddElementAnimation(new OutlineColorAnimation(0.0f, 0.15f, this,  lightColor));
				OutlineThickness = 1;
			}
			else
			{
				buttonReleasedAnimation.AddElementAnimation(new FillColorAnimation(0.0f, 0.25f, textElement, mediumColor));
				buttonReleasedAnimation.AddElementAnimation(new FillColorAnimation(0.0f, 0.25f, this, darkColor));
				buttonReleasedAnimation.AddElementAnimation(new OutlineColorAnimation(0.0f, 0.25f, this,  mediumColor));
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
				mouseEnterAnimation.AddElementAnimation(new FillColorAnimation(0.0f, 0.15f, textElement, lightColor));
				mouseEnterAnimation.AddElementAnimation(new FillColorAnimation(0.0f, 0.15f, this, mediumColor));
				mouseEnterAnimation.AddElementAnimation(new OutlineColorAnimation(0.0f, 0.15f, this,  lightColor));
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
				mouseExitAnimation.AddElementAnimation(new FillColorAnimation(0.0f, 0.25f, textElement, mediumColor));
				mouseExitAnimation.AddElementAnimation(new FillColorAnimation(0.0f, 0.25f, this, darkColor));
				mouseExitAnimation.AddElementAnimation(new OutlineColorAnimation(0.0f, 0.25f, this,  mediumColor));
				OutlineThickness = -1;
				PlayAnimation(mouseExitAnimation);
			}
			base.OnMouseExit();
		}
	}
}