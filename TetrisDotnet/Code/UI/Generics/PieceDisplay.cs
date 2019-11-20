using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;
using TetrisDotnet.Code.Game;
using TetrisDotnet.Code.UI.Base;
using TetrisDotnet.Code.Utils.Enums;
using TetrisDotnet.Code.Utils.Extensions;

namespace TetrisDotnet.Code.UI.Generics
{
	public class PieceDisplay : UiElement
	{
		private List<Sprite> pieceBlocks;
		private PieceType pieceType;

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

		public PieceDisplay()
		{
			InitializePieceDisplay();
		}

		public PieceDisplay(float topAnchor, float bottomAnchor, float leftAnchor, float rightAnchor) : base(topAnchor,
			bottomAnchor, leftAnchor, rightAnchor)
		{
			InitializePieceDisplay();
		}

		public PieceDisplay(float topAnchor, float bottomAnchor, float leftAnchor, float rightAnchor, float topHeight,
			float bottomHeight, float leftWidth, float rightWidth) : base(topAnchor, bottomAnchor, leftAnchor,
			rightAnchor, topHeight, bottomHeight, leftWidth, rightWidth)
		{
			InitializePieceDisplay();
		}

		private void InitializePieceDisplay()
		{
			pieceBlocks = new List<Sprite>();
			pieceType = PieceType.Empty;
		}

		protected override void Refresh()
		{
			base.Refresh();
			AlignPiece();
		}

		public void UpdateDisplayedPiece(PieceType pieceType)
		{
			this.pieceType = pieceType;
			AlignPiece();
		}

		private void AlignPiece()
		{
			Vector2f origin;
			Vector2f position;

			switch (VerticalAlignment)
			{
				case VerticalAlignment.Top:
					origin = new Vector2f(0, 0);
					position = new Vector2f(0, Rectangle.Top);
					break;
				case VerticalAlignment.Center:
					origin = new Vector2f(0, AssetPool.BlockSize.Y * 0.5f);
					position = new Vector2f(0, Rectangle.Center().Y);
					break;
				case VerticalAlignment.Bottom:
					origin = new Vector2f(0, AssetPool.BlockSize.Y);
					position = new Vector2f(0, Rectangle.Top + Rectangle.Height);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			switch (HorizontalAlignment)
			{
				case HorizontalAlignment.Left:
					origin = new Vector2f(0, origin.Y);
					position = new Vector2f(Rectangle.Left, position.Y);
					break;
				case HorizontalAlignment.Center:
					origin = new Vector2f(AssetPool.BlockSize.X * 0.5f, origin.Y);
					position = new Vector2f(Rectangle.Center().X, position.Y);
					break;
				case HorizontalAlignment.Right:
					origin = new Vector2f(AssetPool.BlockSize.X, origin.Y);
					position = new Vector2f(Rectangle.Left + Rectangle.Width, position.Y);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			pieceBlocks = UpdateBlocks(position, origin);
		}

		private List<Sprite> UpdateBlocks(Vector2f position, Vector2f origin)
		{
			List<Sprite> sprites = new List<Sprite>();

			Vector2f offset = GetOffset();
			
			foreach (Vector2i pos in PieceTypeUtils.GetPieceTypeBlocks(pieceType))
			{
				Sprite sprite = new Sprite
				{
					Texture = AssetPool.BlockTextures[(int) pieceType],
					Origin = new Vector2f(origin.X - (pos.X + offset.X) * AssetPool.BlockSize.X, origin.Y - (pos.Y + offset.Y) * AssetPool.BlockSize.Y),
					Position = position,
				};
				sprites.Add(sprite);
			}

			return sprites;
		}

		private Vector2f GetOffset()
		{
			Vector2f offset = pieceType switch
			{
				PieceType.I => new Vector2f(0.5f, 1.5f),
				PieceType.O => new Vector2f(0.5f, 1.0f),
				_ => new Vector2f(1.0f, 1.0f)
			};

			offset.Y += VerticalAlignment switch
			{
				VerticalAlignment.Center => -1.0f,
				VerticalAlignment.Bottom => -2.0f,
				_ => 0.0f,
			};
					
			offset.X += HorizontalAlignment switch
			{
				HorizontalAlignment.Center => -2.0f,
				HorizontalAlignment.Right => -4.0f,
				_ => 0.0f,
			};

			return offset;
		}

		public override void Draw(RenderWindow window)
		{
			foreach (Sprite pieceBlock in pieceBlocks)
			{
				window.Draw(pieceBlock);
			}

			base.Draw(window);
		}
	}
}