using System.Diagnostics;
using SFML.Window;
using TetrisDotnet.Code.Events.EventData;
using TetrisDotnet.Code.Utils.Extensions;

namespace TetrisDotnet.Code.UI.Base
{
	public class ButtonElement : UiElement
	{
		private bool mouseInside;

		public bool MouseInside
		{
			get => mouseInside;
			set
			{
				if (mouseInside != value)
				{
					mouseInside = value;
					if (mouseInside)
					{
						OnMouseEnter();
					}
					else
					{
						OnMouseExit();
					}
				}
			}
		}

		private bool buttonDown;

		public bool ButtonDown
		{
			get => buttonDown;
			set
			{
				if (buttonDown && !value)
				{
					OnButtonReleased();
				}
				else if (!buttonDown && value)
				{
					OnButtonDown();
				}

				buttonDown = value;
			}
		}

		public ButtonElement()
		{
			CapturesMouseClickEvents = true;
			CapturesMouseMoveEvents = true;
		}

		protected override void OnMouseMove(EventData eventData)
		{
			MouseMovedEventData mouseMovedEventData = eventData as MouseMovedEventData;
			Debug.Assert(mouseMovedEventData != null, nameof(mouseMovedEventData) + " != null");
			MouseInside = Rectangle.Contains(mouseMovedEventData.MousePosition);
			base.OnMouseMove(eventData);
		}

		protected override void OnMouseClick(EventData eventData)
		{
			MouseButtonEventData mouseButtonEventData = eventData as MouseButtonEventData;
			Debug.Assert(mouseButtonEventData != null, nameof(mouseButtonEventData) + " != null");
			if (Rectangle.Contains(mouseButtonEventData.MousePosition) &&
			    mouseButtonEventData.MouseButton == Mouse.Button.Left)
			{
				if (mouseButtonEventData.IsPressed)
				{
					ButtonDown = true;
				}
				else if (ButtonDown)
				{
					OnButtonPressed();
					ButtonDown = false;
				}
			}
			else if (ButtonDown)
			{
				ButtonDown = false;
			}

			base.OnMouseClick(eventData);
		}

		protected virtual void OnButtonReleased()
		{
		}

		protected virtual void OnButtonDown()
		{
		}

		protected virtual void OnButtonPressed()
		{
		}

		protected virtual void OnMouseEnter()
		{
		}

		protected virtual void OnMouseExit()
		{
		}
	}
}