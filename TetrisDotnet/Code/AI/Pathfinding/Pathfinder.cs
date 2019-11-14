using System;
using System.Collections.Generic;
using System.Linq;
using SFML.System;
using TetrisDotnet.Code.AI.Pathfinding.Lists;
using TetrisDotnet.Code.Game;
using TetrisDotnet.Code.Game.World;
using TetrisDotnet.Code.Utils.Extensions;

namespace TetrisDotnet.Code.AI.Pathfinding
{
	class Pathfinder
	{
		public static Pathfinder pathfinder { get; } = new Pathfinder();

		private readonly OpenList openList = new OpenList();
		private readonly ClosedHashSet closedHashSet = new ClosedHashSet();

		private readonly Vector2i[] validDirections =
		{
			new Vector2i(-1, 0),
			new Vector2i(0, 1),
			new Vector2i(1, 0)
		};

		private bool PositionIsValid(State state, Piece piece)
		{
			foreach (Vector2i point in piece.getGlobalBlocks)
			{
				if (point.X < 0 || point.X > Grid.GridWidth - 1 || point.Y < 0 || point.Y > Grid.GridHeight - 1)
				{
					return false;
				}

				if (state.GetBlock(point.X, point.Y) && !state.currentPiece.ContainsPoint(point))
				{
					return false;
				}
			}

			return true;
		}

		private IEnumerable<Vector2i> GetNeighbors(State state, Piece endPiece, Vector2i currentCell)
		{
			return validDirections.Select(dir => dir + currentCell).Where(final =>
				PositionIsValid(state, new Piece(endPiece) {position = final}));
		}

		public Stack<PathNode> FindPath(State state, Piece startPiece, Piece endPiece)
		{
			openList.Clear();
			closedHashSet.Clear();
			
			openList.Add(new PathNode(startPiece.position));

			while (openList.Count > 0)
			{
				PathNode currentNode = openList.Pop();

				if (currentNode.Position == endPiece.position)
				{
					return ReconstructPath(currentNode, endPiece);
				}

				closedHashSet.Add(currentNode);

				foreach (Vector2i neighbor in GetNeighbors(state, endPiece, currentNode.Position))
				{
					if (closedHashSet.Contains(neighbor))
					{
						continue;
					}

					float gValue = currentNode.g + neighbor.Distance(currentNode.Position);

					if (openList.Update(neighbor, gValue))
					{
						continue;
					}

					float hValue = GetH(neighbor, endPiece.position);

					PathNode currentNeighbor = new PathNode(neighbor)
					{
						g = gValue,
						h = hValue,
						f = gValue + hValue,
						Parent = currentNode
					};

					openList.Add(currentNeighbor);
				}
			}

			return null;
		}

		private Stack<PathNode> ReconstructPath(PathNode lastNode, Piece endPiece)
		{
			bool isRemovingLastStraightForDropdown = true;
			Stack<PathNode> path = new Stack<PathNode>();
			PathNode current = lastNode;
			while (current != null)
			{
				if (isRemovingLastStraightForDropdown)
				{
					if (current.Parent != null && current.Position.Y == current.Parent.Position.Y)
					{
						current = current.Parent;
						continue;
					}

					isRemovingLastStraightForDropdown = false;
				}
				current.Rotation = endPiece.rotationIndex;
				path.Push(current);
				current = current.Parent;
			}

			return path;
		}

		private float GetH(Vector2i origin, Vector2i destination)
		{
			float x = origin.X - destination.X;
			float y = origin.Y - destination.Y;

			// We want the AI to align itself horizontally before going down.
			x *= 5f;

			return (float) Math.Sqrt(x * x + y * y);
		}
	}
}