using System.Diagnostics;
using SFML.Window;
using TetrisDotnet.Code.Events.EventData;
using TetrisDotnet.Code.Utils.Extensions;
using EventType = TetrisDotnet.Code.Events.EventType;

namespace TetrisDotnet.Code.UI.Base.BaseElement
{
	public partial class UiElement
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

		protected virtual void OnMouseMove(EventData eventData)
		{
			MouseMovedEventData mouseMovedEventData = eventData as MouseMovedEventData;
			Debug.Assert(mouseMovedEventData != null, nameof(mouseMovedEventData) + " != null");
			MouseInside = Rectangle.Contains(mouseMovedEventData.MousePosition);
		}

		protected virtual void OnMouseClick(EventData eventData)
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
				OnPressedOutsideElement();
			}
		}

		protected virtual void OnPressedOutsideElement()
		{
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

		private bool capturesMouseMoveEvents;

		protected bool CapturesMouseMoveEvents
		{
			get => capturesMouseMoveEvents;
			set
			{
				if (value != capturesMouseMoveEvents)
				{
					capturesMouseMoveEvents = value;
					if (capturesMouseMoveEvents)
					{
						Application.EventSystem.Subscribe(EventType.MouseMove, OnMouseMove);
					}
					else
					{
						Application.EventSystem.Unsubscribe(EventType.MouseMove, OnMouseMove);
					}
				}
			}
		}

		private bool capturesMouseClickEvents;

		protected bool CapturesMouseClickEvents
		{
			get => capturesMouseClickEvents;
			set
			{
				if (value != capturesMouseClickEvents)
				{
					capturesMouseClickEvents = value;
					if (capturesMouseClickEvents)
					{
						Application.EventSystem.Subscribe(EventType.MouseButton, OnMouseClick);
					}
					else
					{
						Application.EventSystem.Unsubscribe(EventType.MouseButton, OnMouseClick);
					}
				}
			}
		}

		~UiElement()
		{
			if (capturesMouseMoveEvents)
			{
				Application.EventSystem.Unsubscribe(EventType.MouseMove, OnMouseMove);
			}

			if (capturesMouseClickEvents)
			{
				Application.EventSystem.Unsubscribe(EventType.MouseButton, OnMouseClick);
			}
		}
	}
}