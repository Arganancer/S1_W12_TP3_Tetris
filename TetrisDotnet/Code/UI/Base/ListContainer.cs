using System.Linq;
using TetrisDotnet.Code.UI.Base.BaseElement;

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
			float nextTopAnchor = Spacing / (Height - TopPadding - BottomPadding);
			foreach (UiElement child in Children)
			{
				if (child.Dirty)
				{
					child.ForceRefresh();
				}

				float height = child.Height;

				child.TopAnchor = nextTopAnchor;
				child.BottomAnchor = nextTopAnchor;

				nextTopAnchor += (Spacing + height) / (Height - TopPadding - BottomPadding);
			}
		}

		private void HorizontalAlign()
		{
			float nextLeftAnchor = Spacing / (Width - LeftPadding - RightPadding);
			foreach (UiElement child in Children)
			{
				if (child.Dirty)
				{
					child.ForceRefresh();
				}

				float width = child.Width;

				child.LeftAnchor = nextLeftAnchor;
				child.RightAnchor = nextLeftAnchor;

				nextLeftAnchor += (Spacing + width) / (Width - LeftPadding - RightPadding);
			}
		}
	}
}