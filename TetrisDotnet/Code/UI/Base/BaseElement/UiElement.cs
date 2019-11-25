using System.Collections.Generic;
using System.Diagnostics;
using SFML.Graphics;

namespace TetrisDotnet.Code.UI.Base.BaseElement
{
	public partial class UiElement
	{
		protected readonly List<UiElement> Children = new List<UiElement>();
		private UiElement parent;

		public bool Dirty { get; protected set; }
		public bool Hidden { get; set; }

		public void SetDirty()
		{
			if (Dirty) return;

			Dirty = true;

			foreach (UiElement child in Children)
			{
				child.SetDirty();
			}
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

		public void AddChild(UiElement child)
		{
			Debug.Assert(child != this,
				$"UiElement ({child.GetType()}) added itself as its own child. This is not allowed as it would create an infinite recursion of calls.");
			child.parent = this;
			Children.Add(child);
		}

		public void Update(float deltaTime)
		{
			UpdateAnimations(deltaTime);
			
			bool fitToChildren = false;
			if (Dirty)
			{
				Refresh();
				fitToChildren = true;
			}

			foreach (UiElement child in Children)
			{
				child.Update(deltaTime);
			}

			if (fitToChildren)
			{
				FitToChildren();
			}
		}

		protected virtual void SelfDraw(RenderWindow window)
		{
		}

		public void Draw(RenderWindow window)
		{
			if (Hidden) return;
			
			SelfDraw(window);
			
			foreach (UiElement child in Children)
			{
				child.Draw(window);
			}
		}

		public void ForceRefresh()
		{
			Dirty = true;
			Update(0);
		}

		protected virtual void Refresh()
		{
			RecalculateRectangleTop();
			RecalculateRectangleHeight();
			RecalculateRectangleLeft();
			RecalculateRectangleWidth();

			Dirty = false;
		}
	}
}