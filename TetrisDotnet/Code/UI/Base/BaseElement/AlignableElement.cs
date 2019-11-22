using System;
using TetrisDotnet.Code.Utils.Enums;

namespace TetrisDotnet.Code.UI.Base.BaseElement
{
	public abstract class AlignableElement : DrawableElement
	{
		private HorizontalAlignment horizontalAlignment;
		public HorizontalAlignment HorizontalAlignment
		{
			get => horizontalAlignment;
			set
			{
				horizontalAlignment = value;
				SetDirty();
			}
		}

		private VerticalAlignment verticalAlignment;
		public VerticalAlignment VerticalAlignment
		{
			get => verticalAlignment;
			set
			{
				verticalAlignment = value;
				SetDirty();
			}
		}

		protected void AlignElement()
		{
			AlignElementVertically();
			AlignElementHorizontally();
		}

		private void AlignElementVertically()
		{
			switch (VerticalAlignment)
			{
				case VerticalAlignment.Top:
					AlignVerticalTop();
					break;
				case VerticalAlignment.Center:
					AlignVerticalCenter();
					break;
				case VerticalAlignment.Bottom:
					AlignVerticalBottom();
					break;
			}
		}

		protected abstract void AlignVerticalTop();
		protected abstract void AlignVerticalCenter();
		protected abstract void AlignVerticalBottom();

		private void AlignElementHorizontally()
		{
			switch (HorizontalAlignment)
			{
				case HorizontalAlignment.Left:
					AlignHorizontalLeft();
					break;
				case HorizontalAlignment.Center:
					AlignHorizontalCenter();
					break;
				case HorizontalAlignment.Right:
					AlignHorizontalRight();
					break;
			}
		}

		protected abstract void AlignHorizontalLeft();
		protected abstract void AlignHorizontalCenter();
		protected abstract void AlignHorizontalRight();
	}
}