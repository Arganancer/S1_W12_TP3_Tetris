using System;
using SFML.Graphics;
using SFML.System;

namespace TetrisDotnet.Code.UI.Base
{
	public class SpriteElement : UiElement
	{
		private Sprite sprite;

		public Texture Texture
		{
			get => sprite.Texture;
			set => sprite.Texture = value;
		}

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

		public SpriteElement()
		{
			InitializeSpriteElement();
		}

		public SpriteElement(float topAnchor, float bottomAnchor, float leftAnchor, float rightAnchor) : base(topAnchor, bottomAnchor, leftAnchor, rightAnchor)
		{
			InitializeSpriteElement();
		}

		public SpriteElement(float topAnchor, float bottomAnchor, float leftAnchor, float rightAnchor, float topHeight, float bottomHeight, float leftWidth, float rightWidth) : base(topAnchor, bottomAnchor, leftAnchor, rightAnchor, topHeight, bottomHeight, leftWidth, rightWidth)
		{
			InitializeSpriteElement();
		}

		private void InitializeSpriteElement()
		{
			sprite = new Sprite();
		}

		protected override void Refresh()
		{
			base.Refresh();
			sprite.Position = new Vector2f(Rectangle.Left, Rectangle.Top);
			if (StretchToFit)
			{
				FloatRect globalRect = sprite.GetGlobalBounds();
				const float tolerance = 1.0f;
				if (Math.Abs(globalRect.Width) > tolerance && Math.Abs(globalRect.Height) > tolerance)
				{
					sprite.Scale = new Vector2f(Rectangle.Width / globalRect.Width,
						Rectangle.Height / globalRect.Height);
				}
			}
			else
			{
				sprite.Scale = new Vector2f(1, 1);
			}
		}

		public override void Draw(RenderWindow window)
		{
			window.Draw(sprite);
			base.Draw(window);
		}
	}
}