using SFML.Graphics;
using SFML.System;
using TetrisDotnet.Code.UI.Base;

namespace TetrisDotnet.Code.UI.Generics
{
	public class Rectangle : UiElement
	{
		public RectangleShape RectangleShape;

		public Rectangle(RectangleShape rectangleShape)
		{
			RectangleShape = rectangleShape;
			RectangleShape.Origin = new Vector2f(0, 0);
		}

		public Rectangle(RectangleShape rectangleShape, float topAnchor, float bottomAnchor, float leftAnchor, float rightAnchor) :
			base(topAnchor, bottomAnchor, leftAnchor, rightAnchor)
		{
			RectangleShape = rectangleShape;
			RectangleShape.Origin = new Vector2f(0, 0);
		}

		public Rectangle(RectangleShape rectangleShape, float topAnchor, float bottomAnchor, float leftAnchor, float rightAnchor, float topHeight,
			float bottomHeight, float leftWidth, float rightWidth) :
			base(topAnchor, bottomAnchor, leftAnchor, rightAnchor, topHeight, bottomHeight, leftWidth, rightWidth)
		{
			RectangleShape = rectangleShape;
			RectangleShape.Origin = new Vector2f(0, 0);
		}

		protected override void Refresh()
		{
			base.Refresh();
			RectangleShape.Position = new Vector2f(Rectangle.Left, Rectangle.Top);
			RectangleShape.Size = new Vector2f(Rectangle.Width, Rectangle.Height);
		}

		public override void Draw(RenderWindow window)
		{
			window.Draw(RectangleShape);
			base.Draw(window);
		}
	}
}