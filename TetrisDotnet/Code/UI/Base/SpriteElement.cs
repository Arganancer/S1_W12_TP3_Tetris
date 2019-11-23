using System;
using SFML.Graphics;
using SFML.System;
using TetrisDotnet.Code.UI.Base.BaseElement;
using TetrisDotnet.Code.Utils.Extensions;

namespace TetrisDotnet.Code.UI.Base
{
	public class SpriteElement : ShapeElement
	{
		private readonly Sprite sprite;

		public Texture Texture
		{
			get => sprite.Texture;
			set => sprite.Texture = value;
		}

		public override Color FillColor
		{
			get => sprite.Color;
			set => sprite.Color = value;
		}

		public SpriteElement()
		{
			sprite = new Sprite();
		}

		protected override void SelfDraw(RenderWindow window)
		{
			window.Draw(sprite);
			base.SelfDraw(window);
		}

		protected override void StretchShape()
		{
			sprite.Position = new Vector2f(Left, Top);
			sprite.Origin = new Vector2f(0, 0);

			FloatRect localBounds = sprite.GetLocalBounds();
			const float tolerance = 1.0f;
			if (Math.Abs(localBounds.Width) > tolerance && Math.Abs(localBounds.Height) > tolerance)
			{
				sprite.Scale = new Vector2f(Width / localBounds.Width,
					Height / localBounds.Height);
			}
		}

		protected override void ResetShapeScale()
		{
			sprite.Scale = new Vector2f(1, 1);
		}

		protected override void AlignVerticalTop()
		{
			sprite.Origin = new Vector2f(0, 0);
			sprite.Position = new Vector2f(0, MathF.Round(Top));
		}

		protected override void AlignVerticalCenter()
		{
			sprite.Origin = new Vector2f(0, MathF.Round(sprite.TextureRect.Height * 0.5f));
			sprite.Position = new Vector2f(0, MathF.Round(Rectangle.Center().Y));
		}

		protected override void AlignVerticalBottom()
		{
			sprite.Origin = new Vector2f(0, MathF.Round(sprite.TextureRect.Height));
			sprite.Position = new Vector2f(0, MathF.Round(Top + Height));
		}

		protected override void AlignHorizontalLeft()
		{
			sprite.Origin = new Vector2f(0, sprite.Origin.Y);
			sprite.Position = new Vector2f(MathF.Round(Left), sprite.Position.Y);
		}

		protected override void AlignHorizontalCenter()
		{
			sprite.Origin = new Vector2f(MathF.Round(sprite.TextureRect.Width * 0.5f), sprite.Origin.Y);
			sprite.Position = new Vector2f(MathF.Round(Rectangle.Center().X), sprite.Position.Y);
		}

		protected override void AlignHorizontalRight()
		{
			sprite.Origin = new Vector2f(MathF.Round(sprite.TextureRect.Width), sprite.Origin.Y);
			sprite.Position = new Vector2f(MathF.Round(Left + Width), sprite.Position.Y);
		}
	}
}