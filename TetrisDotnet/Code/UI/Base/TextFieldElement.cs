using System.Diagnostics;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using TetrisDotnet.Code.Events.EventData;
using TetrisDotnet.Code.UI.Animations;
using TetrisDotnet.Code.UI.Animations.ElementAnimations;
using TetrisDotnet.Code.Utils.Enums;
using EventType = TetrisDotnet.Code.Events.EventType;

namespace TetrisDotnet.Code.UI.Base
{
	public class TextFieldElement : RectangleElement
	{
		private readonly Color lightColor = new Color(69, 240, 31);
		private readonly Color mediumColor = new Color(29, 158, 0);
		private readonly Color darkColor = new Color(0, 0, 0);

		protected readonly TextElement TextElement;
		protected readonly RectangleElement Cursor;

		protected string CurrentValue;
		protected string NewValue;
		protected int CursorIndex;

		private void StartKeyCapture()
		{
			Application.EventSystem.Subscribe(EventType.InputKeyCode, OnInputKeyCode);
			Application.EventSystem.Subscribe(EventType.TextEntered, OnTextEntered);
			Application.EventSystem.ProcessEvent(EventType.ToggleKeyboardMode, new ToggleEventData(true));
			Cursor.Hidden = false;
			
			Animation blinkingAnim = new Animation(true);
			blinkingAnim.AddElementAnimation(new WidthHeightAnimation(0.0f, 0.0f, Cursor,
				TextElement.CharacterSize * 0.5f, TextElement.CharacterSize * 0.5f, TextElement.CharacterSize * 0.05f,
				TextElement.CharacterSize * 0.05f));
			blinkingAnim.AddElementAnimation(new WidthHeightAnimation(0.4f, 0.0f, Cursor, 0.0f, 0.0f, 0.0f, 0.0f));
			blinkingAnim.AddElementAnimation(new WidthHeightAnimation(0.8f, 0.0f, Cursor, 0.0f, 0.0f, 0.0f, 0.0f));
			Cursor.PlayAnimation(blinkingAnim);
			RefreshCursor();
		}

		private void EndKeyCapture()
		{
			Application.EventSystem.QueueUnsubscribe(EventType.InputKeyCode, OnInputKeyCode);
			Application.EventSystem.QueueUnsubscribe(EventType.TextEntered, OnTextEntered);
			Application.EventSystem.ProcessEvent(EventType.ToggleKeyboardMode, new ToggleEventData(false));
			
			Cursor.Hidden = true;
			Cursor.ClearAnimations();
		}

		public TextFieldElement()
		{
			CapturesMouseClickEvents = true;
			CapturesMouseMoveEvents = true;
			FillColor = darkColor;
			OutlineColor = mediumColor;
			StretchToFit = true;
			OutlineThickness = -1;

			TextElement = new TextElement
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
			AddChild(TextElement);

			Cursor = new RectangleElement
			{
				FillColor = Color.White,
				TopAnchor = 0.5f,
				BottomAnchor = 0.5f,
				LeftAnchor = 0.0f,
				RightAnchor = 0.0f,
				TopHeight = TextElement.CharacterSize * 0.5f,
				BottomHeight = TextElement.CharacterSize * 0.5f,
				RightWidth = TextElement.CharacterSize * 0.05f,
				LeftWidth = TextElement.CharacterSize * 0.05f,
				StretchToFit = true,
				Hidden = true,
			};
			TextElement.AddChild(Cursor);
		}

		protected override void Refresh()
		{
			base.Refresh();
			RefreshCursor();
		}

		private void RefreshCursor()
		{
			TextElement.ForceRefresh();
			Vector2f desiredPos = TextElement.FindCharacterPos((uint) CursorIndex);
			Cursor.LeftAnchor = (desiredPos.X - TextElement.Left) / TextElement.Width;
			Cursor.RightAnchor = Cursor.LeftAnchor;
			Cursor.TopAnchor = (desiredPos.Y - TextElement.Top) / TextElement.Height;
			Cursor.BottomAnchor = Cursor.TopAnchor;
			Cursor.RestartCurrentAnimation();
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
					if (float.TryParse(NewValue, out float fVal))
					{
						CurrentValue = fVal.ToString();
						TextElement.DisplayedString = CurrentValue;
						OnNewValue(fVal);
					}
					else
					{
						TextElement.DisplayedString = CurrentValue;
					}
					EndKeyCapture();
					break;
				case Keyboard.Key.Escape:
					TextElement.DisplayedString = CurrentValue;
					EndKeyCapture();
					break;
				case Keyboard.Key.Delete:
					if (CursorIndex < NewValue.Length)
					{
						NewValue = NewValue.Remove(CursorIndex, 1);
						TextElement.DisplayedString = NewValue;
						RefreshCursor();
					}

					break;
				case Keyboard.Key.Backspace:
					if (CursorIndex > 0)
					{
						NewValue = NewValue.Remove(--CursorIndex, 1);
						TextElement.DisplayedString = NewValue;
						RefreshCursor();
					}

					break;
				case Keyboard.Key.Left:
					if (CursorIndex > 0)
					{
						--CursorIndex;
						RefreshCursor();
					}

					break;
				case Keyboard.Key.Right:
					if (CursorIndex <= NewValue.Length - 1)
					{
						++CursorIndex;
						RefreshCursor();
					}

					break;
			}
		}

		protected virtual void OnNewValue(float value)
		{
		}

		private void OnTextEntered(EventData eventData)
		{
			TextEnteredEventData textEnteredEventData = eventData as TextEnteredEventData;
			Debug.Assert(textEnteredEventData != null, nameof(textEnteredEventData) + " != null");
			if (float.TryParse(textEnteredEventData.Text, out float value) || textEnteredEventData.Text == "." || textEnteredEventData.Text == "-")
			{
				if (CursorIndex < NewValue.Length)
				{
					NewValue = NewValue.Insert(CursorIndex++, textEnteredEventData.Text);
					TextElement.DisplayedString = NewValue;
				}
				else
				{
					NewValue += textEnteredEventData.Text;
					TextElement.DisplayedString = NewValue;
					++CursorIndex;
				}

				RefreshCursor();
			}
		}

		protected override void OnPressedOutsideElement()
		{
			if (Cursor.Hidden) return;
			EndKeyCapture();
			TextElement.DisplayedString = CurrentValue;
			base.OnPressedOutsideElement();
		}

		protected override void OnButtonDown()
		{
			Animation buttonDownAnimation = new Animation();
			buttonDownAnimation.AddElementAnimation(new FillColorAnimation(0.0f, 0.09f, TextElement, mediumColor));
			buttonDownAnimation.AddElementAnimation(new FillColorAnimation(0.0f, 0.09f, this, darkColor));
			buttonDownAnimation.AddElementAnimation(new OutlineColorAnimation(0.0f, 0.09f, this, lightColor));
			OutlineThickness = 1;

			PlayAnimation(buttonDownAnimation);

			base.OnButtonDown();
		}

		protected override void OnButtonPressed()
		{
			StartKeyCapture();
			NewValue = "";
			TextElement.DisplayedString = NewValue;
			CursorIndex = 0;
			base.OnButtonPressed();
		}

		protected override void OnButtonReleased()
		{
			Animation buttonReleasedAnimation = new Animation();
			if (MouseInside)
			{
				buttonReleasedAnimation.AddElementAnimation(
					new FillColorAnimation(0.0f, 0.15f, TextElement, lightColor));
				buttonReleasedAnimation.AddElementAnimation(new FillColorAnimation(0.0f, 0.15f, this, mediumColor));
				buttonReleasedAnimation.AddElementAnimation(new OutlineColorAnimation(0.0f, 0.15f, this, lightColor));
				OutlineThickness = 1;
			}
			else
			{
				buttonReleasedAnimation.AddElementAnimation(new FillColorAnimation(0.0f, 0.25f, TextElement,
					mediumColor));
				buttonReleasedAnimation.AddElementAnimation(new FillColorAnimation(0.0f, 0.25f, this, darkColor));
				buttonReleasedAnimation.AddElementAnimation(new OutlineColorAnimation(0.0f, 0.25f, this, mediumColor));
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
				mouseEnterAnimation.AddElementAnimation(new FillColorAnimation(0.0f, 0.15f, TextElement, lightColor));
				mouseEnterAnimation.AddElementAnimation(new FillColorAnimation(0.0f, 0.15f, this, mediumColor));
				mouseEnterAnimation.AddElementAnimation(new OutlineColorAnimation(0.0f, 0.15f, this, lightColor));
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
				mouseExitAnimation.AddElementAnimation(new FillColorAnimation(0.0f, 0.25f, TextElement, mediumColor));
				mouseExitAnimation.AddElementAnimation(new FillColorAnimation(0.0f, 0.25f, this, darkColor));
				mouseExitAnimation.AddElementAnimation(new OutlineColorAnimation(0.0f, 0.25f, this, mediumColor));
				OutlineThickness = -1;
				PlayAnimation(mouseExitAnimation);
			}

			base.OnMouseExit();
		}
	}
}