using System;
using SFML.Graphics;

namespace TetrisDotnet.Code.UI.Base.BaseElement
{
	public partial class UiElement
	{
		protected FloatRect Rectangle;

		public float Top
		{
			get => Rectangle.Top;
			private set => Rectangle.Top = value;
		}

		public float Left
		{
			get => Rectangle.Left;
			private set => Rectangle.Left = value;
		}

		public float Height
		{
			get => Rectangle.Height;
			private set => Rectangle.Height = value;
		}

		public float Width
		{
			get => Rectangle.Width;
			private set => Rectangle.Width = value;
		}

		public float Bottom => Top + Height;
		public float Right => Left + Width;

		public float InnerHeight => Height - TopPadding - BottomPadding;
		public float InnerWidth => Width - leftPadding - RightPadding;

		public float InnerTop => Top + TopPadding;
		public float InnerLeft => Left + LeftPadding;

		#region Anchors

		private float topAnchor;
		private float bottomAnchor;
		private float leftAnchor;
		private float rightAnchor;

		public float TopAnchor
		{
			get => topAnchor;
			set
			{
				topAnchor = value;
				SetDirty();
			}
		}

		public float BottomAnchor
		{
			get => bottomAnchor;
			set
			{
				bottomAnchor = value;
				SetDirty();
			}
		}

		public float LeftAnchor
		{
			get => leftAnchor;
			set
			{
				leftAnchor = value;
				SetDirty();
			}
		}

		public float RightAnchor
		{
			get => rightAnchor;
			set
			{
				rightAnchor = value;
				SetDirty();
			}
		}

		#endregion

		#region Heights

		private float topHeight;
		private float bottomHeight;
		private float leftWidth;
		private float rightWidth;

		public float TopHeight
		{
			get => topHeight;
			set
			{
				topHeight = value;
				SetDirty();
			}
		}

		public float BottomHeight
		{
			get => bottomHeight;
			set
			{
				bottomHeight = value;
				SetDirty();
			}
		}

		public float LeftWidth
		{
			get => leftWidth;
			set
			{
				leftWidth = value;
				SetDirty();
			}
		}

		public float RightWidth
		{
			get => rightWidth;
			set
			{
				rightWidth = value;
				SetDirty();
			}
		}

		#endregion

		#region Padding

		private float topPadding;
		private float bottomPadding;
		private float leftPadding;
		private float rightPadding;

		public float TopPadding
		{
			get => topPadding;
			set
			{
				topPadding = value;
				SetDirty();
			}
		}

		public float BottomPadding
		{
			get => bottomPadding;
			set
			{
				bottomPadding = value;
				SetDirty();
			}
		}

		public float LeftPadding
		{
			get => leftPadding;
			set
			{
				leftPadding = value;
				SetDirty();
			}
		}

		public float RightPadding
		{
			get => rightPadding;
			set
			{
				rightPadding = value;
				SetDirty();
			}
		}

		#endregion

		public UiElement()
		{
			TopAnchor = 0.0f;
			BottomAnchor = 1.0f;
			LeftAnchor = 0.0f;
			RightAnchor = 1.0f;

			TopHeight = 0.0f;
			BottomHeight = 0.0f;
			LeftWidth = 0.0f;
			RightWidth = 0.0f;

			SetDirty();
		}

		protected void RecalculateRectangleTop()
		{
			if (parent != null)
			{
				Top = Parent.InnerTop - TopHeight + (Parent.InnerHeight * TopAnchor);
			}
			else
			{
				Top = Math.Max(-TopHeight + (Application.WindowHeight * TopAnchor), 0.0f);
			}
		}

		protected void RecalculateRectangleLeft()
		{
			if (parent != null)
			{
				Left = Parent.InnerLeft - LeftWidth + (Parent.InnerWidth * LeftAnchor);
			}
			else
			{
				Left = Math.Max(-LeftWidth + (Application.WindowWidth * LeftAnchor), 0.0f);
			}
		}

		protected void RecalculateRectangleHeight()
		{
			if (parent != null)
			{
				Height = Parent.InnerTop + (Parent.InnerHeight * BottomAnchor) + BottomHeight - Top;
			}
			else
			{
				Height = Math.Min((Application.WindowHeight * BottomAnchor) + BottomHeight - Top,
					Application.WindowHeight);
			}
		}

		protected void RecalculateRectangleWidth()
		{
			if (parent != null)
			{
				Width = Parent.InnerLeft + (Parent.InnerWidth * RightAnchor) + RightWidth - Left;
			}
			else
			{
				Width = Math.Min(Application.WindowWidth * RightAnchor + RightWidth - Left, Application.WindowWidth);
			}
		}
	}
}