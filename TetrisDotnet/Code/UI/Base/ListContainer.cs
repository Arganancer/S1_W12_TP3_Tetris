namespace TetrisDotnet.Code.UI.Base
{
	public enum Orientation
	{
		Horizontal,
		Vertical,
	}

	public class ListContainer : UiElement
	{
		public Orientation Orientation;

		public float Spacing;

		public ListContainer()
		{
		}

		public ListContainer(float topAnchor, float bottomAnchor, float leftAnchor, float rightAnchor) : base(topAnchor,
			bottomAnchor, leftAnchor, rightAnchor)
		{
		}

		public ListContainer(float topAnchor, float bottomAnchor, float leftAnchor, float rightAnchor, float topHeight,
			float bottomHeight, float leftWidth, float rightWidth) : base(topAnchor, bottomAnchor, leftAnchor,
			rightAnchor, topHeight, bottomHeight, leftWidth, rightWidth)
		{
		}

		protected override void Refresh()
		{
			RecalculateRectangleTop();
			RecalculateRectangleHeight();
			RecalculateRectangleLeft();
			RecalculateRectangleWidth();

			Align();
			Dirty = false;
		}

		private void Align()
		{
			switch (Orientation)
			{
				case Orientation.Horizontal:
					HorizontalAlign();
					break;
				case Orientation.Vertical:
					VerticalAlign();
					break;
			}
		}

		private void VerticalAlign()
		{
			float nextTopAnchor = Spacing / (Rectangle.Height - TopPadding - BottomPadding);
			foreach (UiElement child in Children)
			{
				if (child.Dirty)
				{
					child.ForceRefresh();
				}

				float height = child.Height;

				child.TopAnchor = nextTopAnchor;
				child.BottomAnchor = nextTopAnchor;

				nextTopAnchor += (Spacing + height) / (Rectangle.Height - TopPadding - BottomPadding);
			}
		}

		private void HorizontalAlign()
		{
			float nextLeftAnchor = Spacing / (Rectangle.Width - LeftPadding - RightPadding);
			foreach (UiElement child in Children)
			{
				if (child.Dirty)
				{
					child.ForceRefresh();
				}
				
				float width = child.Width;

				child.LeftAnchor = nextLeftAnchor;
				child.RightAnchor = nextLeftAnchor;

				nextLeftAnchor += (Spacing + width) / (Rectangle.Width - LeftPadding - RightPadding);
			}
		}
	}
}