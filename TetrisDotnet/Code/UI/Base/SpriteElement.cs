using System;
using SFML.Graphics;
using SFML.System;
using TetrisDotnet.Code.Utils.Enums;
using TetrisDotnet.Code.Utils.Extensions;

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
		
		private HorizontalAlignment spriteHorizontalAlignment;
		public HorizontalAlignment SpriteHorizontalAlignment
		{
			get => spriteHorizontalAlignment;
			set
			{
				spriteHorizontalAlignment = value;
				SetDirty();
			}
		}

		private VerticalAlignment spriteVerticalAlignment;
		public VerticalAlignment SpriteVerticalAlignment
		{
			get => spriteVerticalAlignment;
			set
			{
				spriteVerticalAlignment = value;
				SetDirty();
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
			if (StretchToFit)
			{
				sprite.Position = new Vector2f(Rectangle.Left, Rectangle.Top);
				sprite.Origin = new Vector2f(0, 0);

				FloatRect localBounds = sprite.GetLocalBounds();
				const float tolerance = 1.0f;
				if (Math.Abs(localBounds.Width) > tolerance && Math.Abs(localBounds.Height) > tolerance)
				{
					sprite.Scale = new Vector2f(Rectangle.Width / localBounds.Width,
						Rectangle.Height / localBounds.Height);
				}
			}
			else
			{
				sprite.Scale = new Vector2f(1, 1);
				AlignSprite();
			}
		}

		public override void Draw(RenderWindow window)
		{
			window.Draw(sprite);
			base.Draw(window);
		}

		private void AlignSprite()
		{
			switch (SpriteVerticalAlignment)
			{
				case VerticalAlignment.Top:
					sprite.Origin = new Vector2f(0, 0);
					sprite.Position = new Vector2f(0, Rectangle.Top);
					break;
				case VerticalAlignment.Center:
					sprite.Origin = new Vector2f(0, sprite.TextureRect.Height * 0.5f);
					sprite.Position = new Vector2f(0, Rectangle.Center().Y);
					break;
				case VerticalAlignment.Bottom:
					sprite.Origin = new Vector2f(0, sprite.TextureRect.Height);
					sprite.Position = new Vector2f(0, Rectangle.Top + Rectangle.Height);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			switch (SpriteHorizontalAlignment)
			{
				case HorizontalAlignment.Left:
					sprite.Origin = new Vector2f(0, sprite.Origin.Y);
					sprite.Position = new Vector2f(Rectangle.Left, sprite.Position.Y);
					break;
				case HorizontalAlignment.Center:
					sprite.Origin = new Vector2f(sprite.TextureRect.Width * 0.5f, sprite.Origin.Y);
					sprite.Position = new Vector2f(Rectangle.Center().X, sprite.Position.Y);
					break;
				case HorizontalAlignment.Right:
					sprite.Origin = new Vector2f(sprite.TextureRect.Width, sprite.Origin.Y);
					sprite.Position = new Vector2f(Rectangle.Left + Rectangle.Width, sprite.Position.Y);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}