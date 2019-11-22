namespace TetrisDotnet.Code.UI.Base.BaseElement
{
	public abstract class ShapeElement : AlignableElement
	{
		private bool stretchToFit;
		public bool StretchToFit
		{
			get => stretchToFit;
			set
			{
				if (stretchToFit != value)
				{
					stretchToFit = value;
					SetDirty();
				}
			}
		}
		
		protected override void Refresh()
		{
			base.Refresh();
			if (StretchToFit)
			{
				StretchShape();
			}
			else
			{
				ResetShapeScale();
				AlignElement();
			}
		}

		protected abstract void StretchShape();

		protected abstract void ResetShapeScale();
	}
}