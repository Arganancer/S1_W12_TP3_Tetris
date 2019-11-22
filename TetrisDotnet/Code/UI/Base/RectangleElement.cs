using System;
using SFML.Graphics;
using SFML.System;
using TetrisDotnet.Code.UI.Base.BaseElement;
using TetrisDotnet.Code.Utils.Extensions;

namespace TetrisDotnet.Code.UI.Base
{
	public class RectangleElement : ShapeElement
	{
		private readonly RectangleShape rectangleShape;
		
		public override Color FillColor
		{
			get => rectangleShape.FillColor;
			set => rectangleShape.FillColor = value;
		}
		
		public Color OutlineColor
		{
			get => rectangleShape.OutlineColor;
			set => rectangleShape.OutlineColor = value;
		}
		
		public float OutlineThickness
		{
			get => rectangleShape.OutlineThickness;
			set => rectangleShape.OutlineThickness = value;
		}

		public RectangleElement()
		{
			rectangleShape = new RectangleShape();
		}

		protected override void SelfDraw(RenderWindow window)
		{
			window.Draw(rectangleShape);
			base.SelfDraw(window);
		}

		protected override void StretchShape()
		{			
			rectangleShape.Position = new Vector2f(Left, Top);
			rectangleShape.Origin = new Vector2f(0, 0);
			rectangleShape.Size = new Vector2f(Width, Height);
			
//			FloatRect localBounds = rectangleShape.GetLocalBounds();
//			const float tolerance = 1.0f;
//			if (Math.Abs(localBounds.Width) > tolerance && Math.Abs(localBounds.Height) > tolerance)
//			{
//				rectangleShape.Scale = new Vector2f(Width / localBounds.Width,
//					Height / localBounds.Height);
//			}
		}

		protected override void ResetShapeScale()
		{
			rectangleShape.Scale = new Vector2f(1, 1);
		}
		
		protected override void AlignVerticalTop()
		{		
			rectangleShape.Origin = new Vector2f(0, 0);
			rectangleShape.Position = new Vector2f(0, Top);
		}

		protected override void AlignVerticalCenter()
		{
			rectangleShape.Origin = new Vector2f(0, rectangleShape.TextureRect.Height * 0.5f);
			rectangleShape.Position = new Vector2f(0, Rectangle.Center().Y);
		}

		protected override void AlignVerticalBottom()
		{
			rectangleShape.Origin = new Vector2f(0, rectangleShape.TextureRect.Height);
			rectangleShape.Position = new Vector2f(0, Top + Height);
		}

		protected override void AlignHorizontalLeft()
		{
			rectangleShape.Origin = new Vector2f(0, rectangleShape.Origin.Y);
			rectangleShape.Position = new Vector2f(Left, rectangleShape.Position.Y);
		}

		protected override void AlignHorizontalCenter()
		{
			rectangleShape.Origin = new Vector2f(rectangleShape.TextureRect.Width * 0.5f, rectangleShape.Origin.Y);
			rectangleShape.Position = new Vector2f(Rectangle.Center().X, rectangleShape.Position.Y);
		}

		protected override void AlignHorizontalRight()
		{
			rectangleShape.Origin = new Vector2f(rectangleShape.TextureRect.Width, rectangleShape.Origin.Y);
			rectangleShape.Position = new Vector2f(Left + Width, rectangleShape.Position.Y);
		}
	}
}