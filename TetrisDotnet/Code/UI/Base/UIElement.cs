using System;
using System.Collections.Generic;
using SFML.Graphics;
using TetrisDotnet.Code.Events;
using TetrisDotnet.Code.Events.EventData;

namespace TetrisDotnet.Code.UI.Base
{
	public class UiElement
	{
		private UiElement parent;

		public bool Dirty { get; protected set; }
		public bool Hidden { get; protected set; }

		public void SetDirty()
		{
			if (Dirty) return;
			
			Dirty = true;

			foreach (UiElement child in Children)
			{
				child.SetDirty();
			}
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
		
		protected virtual void OnMouseMove(EventData eventData)
		{
		}
		
		protected virtual void OnMouseClick(EventData eventData)
		{
		}

		public UiElement Parent
		{
			get => parent;
			set
			{
				parent = value;
				SetDirty();
			}
		}

		protected readonly List<UiElement> Children = new List<UiElement>();

		public UiElement()
		{
			topAnchor = 0.0f;
			bottomAnchor = 1.0f;
			leftAnchor = 0.0f;
			rightAnchor = 1.0f;

			topHeight = 0.0f;
			bottomHeight = 0.0f;
			leftWidth = 0.0f;
			rightWidth = 0.0f;

			SetDirty();
		}

		public UiElement(float topAnchor, float bottomAnchor, float leftAnchor, float rightAnchor)
		{
			this.topAnchor = topAnchor;
			this.bottomAnchor = bottomAnchor;
			this.leftAnchor = leftAnchor;
			this.rightAnchor = rightAnchor;

			topHeight = 0.0f;
			bottomHeight = 0.0f;
			leftWidth = 0.0f;
			rightWidth = 0.0f;

			SetDirty();
		}

		public UiElement(float topAnchor, float bottomAnchor, float leftAnchor, float rightAnchor, float topHeight,
			float bottomHeight, float leftWidth, float rightWidth)
		{
			this.topAnchor = topAnchor;
			this.bottomAnchor = bottomAnchor;
			this.leftAnchor = leftAnchor;
			this.rightAnchor = rightAnchor;

			this.topHeight = topHeight;
			this.bottomHeight = bottomHeight;
			this.leftWidth = leftWidth;
			this.rightWidth = rightWidth;

			SetDirty();
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

		public virtual void AddChild(UiElement child)
		{
			child.parent = this;
			Children.Add(child);
		}

		public FloatRect Rectangle;
		public float Height => Rectangle.Height;
		public float Width => Rectangle.Width;

		// Anchors
		private float topAnchor;
		public float TopAnchor
		{
			get => topAnchor;
			set
			{
				topAnchor = value;
				SetDirty();
			}
		}

		private float bottomAnchor;
		public float BottomAnchor
		{
			get => bottomAnchor;
			set
			{
				bottomAnchor = value;
				SetDirty();
			}
		}

		private float leftAnchor;
		public float LeftAnchor
		{
			get => leftAnchor;
			set
			{
				leftAnchor = value;
				SetDirty();
			}
		}

		private float rightAnchor;
		public float RightAnchor
		{
			get => rightAnchor;
			set
			{
				rightAnchor = value;
				SetDirty();
			}
		}

		// Size
		private float topHeight;
		public float TopHeight
		{
			get => topHeight;
			set
			{
				topHeight = value;
				SetDirty();
			}
		}

		private float bottomHeight;
		public float BottomHeight
		{
			get => bottomHeight;
			set
			{
				bottomHeight = value;
				SetDirty();
			}
		}

		private float leftWidth;
		public float LeftWidth
		{
			get => leftWidth;
			set
			{
				leftWidth = value;
				SetDirty();
			}
		}

		private float rightWidth;
		public float RightWidth
		{
			get => rightWidth;
			set
			{
				rightWidth = value;
				SetDirty();
			}
		}

		private float topPadding;
		public float TopPadding
		{
			get => topPadding;
			set
			{
				topPadding = value;
				SetDirty();
			}
		}

		private float bottomPadding;
		public float BottomPadding
		{
			get => bottomPadding;
			set
			{
				bottomPadding = value;
				SetDirty();
			}
		}

		private float leftPadding;
		public float LeftPadding
		{
			get => leftPadding;
			set
			{
				leftPadding = value;
				SetDirty();
			}
		}

		private float rightPadding;
		public float RightPadding
		{
			get => rightPadding;
			set
			{
				rightPadding = value;
				SetDirty();
			}
		}

		public void ForceRefresh()
		{
			Refresh();
		}

		protected virtual void Refresh()
		{
			RecalculateRectangleTop();
			RecalculateRectangleHeight();
			RecalculateRectangleLeft();
			RecalculateRectangleWidth();

			Dirty = false;
		}

		protected void RecalculateRectangleTop()
		{
			if (parent != null)
			{
				float parentInnerHeight = Parent.Height - Parent.TopPadding - Parent.BottomPadding;
				float parentInnerTop = Parent.Rectangle.Top + Parent.TopPadding;
				Rectangle.Top = parentInnerTop - TopHeight + parentInnerHeight * topAnchor;
			}
			else
			{
				Rectangle.Top = Math.Max(-TopHeight + Application.WindowHeight * topAnchor, 0.0f);
			}
		}

		protected void RecalculateRectangleLeft()
		{
			if (parent != null)
			{
				float parentInnerWidth = Parent.Width - Parent.LeftPadding - Parent.RightPadding;
				float parentInnerLeft = Parent.Rectangle.Left + Parent.LeftPadding;
				Rectangle.Left = parentInnerLeft - LeftWidth + parentInnerWidth * leftAnchor;
			}
			else
			{
				Rectangle.Left = Math.Max(-LeftWidth + Application.WindowWidth * leftAnchor, 0.0f);
			}
		}

		protected void RecalculateRectangleHeight()
		{
			if (parent != null)
			{
				float parentInnerHeight = Parent.Height - Parent.TopPadding - Parent.BottomPadding;
				float parentInnerTop = Parent.Rectangle.Top + Parent.TopPadding;
				Rectangle.Height = parentInnerTop + parentInnerHeight * bottomAnchor + bottomHeight - Rectangle.Top;
			}
			else
			{
				Rectangle.Height = Math.Min(Application.WindowHeight * bottomAnchor + bottomHeight - Rectangle.Top,
					Application.WindowHeight);
			}
		}

		protected void RecalculateRectangleWidth()
		{
			if (parent != null)
			{
				float parentInnerWidth = Parent.Width - Parent.LeftPadding - Parent.RightPadding;
				float parentInnerLeft = Parent.Rectangle.Left + Parent.LeftPadding;
				Rectangle.Width = parentInnerLeft + parentInnerWidth * rightAnchor + rightWidth - Rectangle.Left;
			}
			else
			{
				Rectangle.Width = Math.Min(Application.WindowWidth * rightAnchor + rightWidth - Rectangle.Left,
					Application.WindowWidth);
			}
		}

		public virtual void Draw(RenderWindow window)
		{
			foreach (UiElement child in Children)
			{
				child.Draw(window);
			}
		}

		public virtual void Update()
		{
			if (Dirty)
			{
				Refresh();
			}

			foreach (UiElement child in Children)
			{
				child.Update();
			}
		}
	}
}