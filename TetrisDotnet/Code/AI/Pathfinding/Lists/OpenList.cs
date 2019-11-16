using SFML.System;
using TetrisDotnet.Code.Utils;
using TetrisDotnet.Code.Utils.DataStructures;

namespace TetrisDotnet.Code.AI.Pathfinding.Lists
{
	class OpenList
	{
		public long Count => size;

		private long size = 0;
		private AvlTree<Vector2I, PathNode> posTree = new AvlTree<Vector2I, PathNode>();
		private AvlTree<float, PathNode> fTree = new AvlTree<float, PathNode>();

		public void Add(PathNode pathNode)
		{
			fTree.Insert(pathNode.f, pathNode);
			posTree.Insert(pathNode.Position, pathNode);
			++size;
		}

		public PathNode Remove(PathNode pathNode)
		{
			PathNode node = posTree.Delete(pathNode.Position);
			fTree.Delete(node.f, node);
			--size;
			return node;
		}

		public bool Update(Vector2i position, float gValue)
		{
			PathNode pathNode = Get(position);
			if (pathNode != null)
			{
				if (pathNode.g > gValue)
				{
					Remove(pathNode);
					pathNode.g = gValue;
					pathNode.f = gValue + pathNode.h;
					Add(pathNode);
				}
				return true;
			}
			return false;
		}

		public PathNode Pop()
		{
			PathNode output = fTree.Pop();
			posTree.Delete(output.Position);
			--size;
			return output;
		}

		public bool Contains(Vector2i position)
		{
			return posTree.Search(position) != null;
		}

		public PathNode Get(Vector2i position)
		{
			return posTree.Search(position);
		}

		public void Clear()
		{
			posTree.Clear();
			fTree.Clear();
			size = 0;
		}
	}
}