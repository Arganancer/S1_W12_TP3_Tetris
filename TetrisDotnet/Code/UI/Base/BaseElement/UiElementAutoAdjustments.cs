using System.Linq;

namespace TetrisDotnet.Code.UI.Base.BaseElement
{
	public partial class UiElement
	{
		private bool autoHeight;

		public bool AutoHeight
		{
			get => autoHeight;
			set
			{
				autoHeight = value;
				SetDirty();
			}
		}

		private bool autoWidth;

		public bool AutoWidth
		{
			get => autoWidth;
			set
			{
				autoWidth = value;
				SetDirty();
			}
		}

		private void FitToChildren()
		{
			bool wasDirty = Dirty;

			if (autoWidth)
			{
				RightWidth = GetMaxChildWidth() - Left;
				RecalculateRectangleWidth();
			}

			if (autoHeight)
			{
				BottomHeight = GetMaxChildHeight() - Top;
				RecalculateRectangleHeight();
			}

			Dirty = wasDirty;
		}

		protected virtual float GetMaxChildWidth()
		{
			
			return Children.Count > 0 ? Children.Select(child => child.Left + child.Width).Max() : 0;
		}

		protected virtual float GetMaxChildHeight()
		{
			
			return Children.Count > 0 ? Children.Select(child => child.Top + child.Height).Max() : 0;
		}
	}
}