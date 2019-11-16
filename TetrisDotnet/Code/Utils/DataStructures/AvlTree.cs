using System;
using System.Collections;
using System.Collections.Generic;

namespace TetrisDotnet.Code.Utils.DataStructures
{
	public class AvlTree<TKey, TValue> : IEnumerable<TValue>
		where TKey : IComparable<TKey> where TValue : IComparable<TValue>
	{
		private AvlNode _root;

		sealed class AvlNode
		{
			public AvlNode Parent;
			public AvlNode Left;
			public AvlNode Right;
			public TKey Key;
			public TValue Value;
			public int Balance;
		}

		public void Insert(TKey key, TValue value)
		{
			if (_root == null)
			{
				_root = new AvlNode {Key = key, Value = value};
			}
			else
			{
				AvlNode node = _root;

				while (node != null)
				{
					int compare = key.CompareTo(node.Key);

					if (compare <= 0)
					{
						AvlNode left = node.Left;

						if (left == null)
						{
							node.Left = new AvlNode {Key = key, Value = value, Parent = node};
							InsertBalance(node, 1);
							return;
						}

						node = left;
					}
					else if (compare > 0)
					{
						AvlNode right = node.Right;

						if (right == null)
						{
							node.Right = new AvlNode {Key = key, Value = value, Parent = node};
							InsertBalance(node, -1);
							return;
						}

						node = right;
					}
				}
			}
		}

		public TValue Pop()
		{
			if (_root == null)
			{
				return default(TValue);
			}

			AvlNode currentNode = _root;

			while (currentNode.Left != null)
			{
				currentNode = currentNode.Left;
			}

			return Delete(currentNode.Key, currentNode.Value);
		}

		public TValue Delete(TKey key)
		{
			AvlNode node = _root;

			while (node != null)
			{
				int compare = key.CompareTo(node.Key);

				if (compare < 0)
				{
					node = node.Left;
				}
				else if (compare > 0)
				{
					node = node.Right;
				}
				else
				{
					AvlNode left = node.Left;
					AvlNode right = node.Right;
					TValue outputValue = node.Value;

					if (left == null)
					{
						if (right == null)
						{
							if (node == _root)
							{
								_root = null;
							}
							else
							{
								AvlNode parent = node.Parent;

								if (parent.Left == node)
								{
									parent.Left = null;

									DeleteBalance(parent, -1);
								}
								else
								{
									parent.Right = null;

									DeleteBalance(parent, 1);
								}
							}
						}
						else
						{
							Replace(node, right);

							DeleteBalance(node, 0);
						}
					}
					else if (right == null)
					{
						Replace(node, left);

						DeleteBalance(node, 0);
					}
					else
					{
						AvlNode successor = right;

						if (successor.Left == null)
						{
							AvlNode parent = node.Parent;

							successor.Parent = parent;
							successor.Left = left;
							successor.Balance = node.Balance;

							if (left != null)
							{
								left.Parent = successor;
							}

							if (node == _root)
							{
								_root = successor;
							}
							else
							{
								if (parent.Left == node)
								{
									parent.Left = successor;
								}
								else
								{
									parent.Right = successor;
								}
							}

							DeleteBalance(successor, 1);
						}
						else
						{
							while (successor.Left != null)
							{
								successor = successor.Left;
							}

							AvlNode parent = node.Parent;
							AvlNode successorParent = successor.Parent;
							AvlNode successorRight = successor.Right;

							if (successorParent.Left == successor)
							{
								successorParent.Left = successorRight;
							}
							else
							{
								successorParent.Right = successorRight;
							}

							if (successorRight != null)
							{
								successorRight.Parent = successorParent;
							}

							successor.Parent = parent;
							successor.Left = left;
							successor.Balance = node.Balance;
							successor.Right = right;
							right.Parent = successor;

							if (left != null)
							{
								left.Parent = successor;
							}

							if (node == _root)
							{
								_root = successor;
							}
							else
							{
								if (parent.Left == node)
								{
									parent.Left = successor;
								}
								else
								{
									parent.Right = successor;
								}
							}

							DeleteBalance(successorParent, -1);
						}
					}

					return outputValue;
				}
			}

			return default(TValue);
		}

		public TValue Delete(TKey key, TValue value)
		{
			AvlNode node = _root;

			while (node != null)
			{
				int compare = key.CompareTo(node.Key);

				if (compare == 0 && value.Equals(node.Value))
				{
					AvlNode left = node.Left;
					AvlNode right = node.Right;
					TValue outputValue = node.Value;

					if (left == null)
					{
						if (right == null)
						{
							if (node == _root)
							{
								_root = null;
							}
							else
							{
								AvlNode parent = node.Parent;

								if (parent.Left == node)
								{
									parent.Left = null;

									DeleteBalance(parent, -1);
								}
								else
								{
									parent.Right = null;

									DeleteBalance(parent, 1);
								}
							}
						}
						else
						{
							Replace(node, right);

							DeleteBalance(node, 0);
						}
					}
					else if (right == null)
					{
						Replace(node, left);

						DeleteBalance(node, 0);
					}
					else
					{
						AvlNode successor = right;

						if (successor.Left == null)
						{
							AvlNode parent = node.Parent;

							successor.Parent = parent;
							successor.Left = left;
							successor.Balance = node.Balance;

							if (left != null)
							{
								left.Parent = successor;
							}

							if (node == _root)
							{
								_root = successor;
							}
							else
							{
								if (parent.Left == node)
								{
									parent.Left = successor;
								}
								else
								{
									parent.Right = successor;
								}
							}

							DeleteBalance(successor, 1);
						}
						else
						{
							while (successor.Left != null)
							{
								successor = successor.Left;
							}

							AvlNode parent = node.Parent;
							AvlNode successorParent = successor.Parent;
							AvlNode successorRight = successor.Right;

							if (successorParent.Left == successor)
							{
								successorParent.Left = successorRight;
							}
							else
							{
								successorParent.Right = successorRight;
							}

							if (successorRight != null)
							{
								successorRight.Parent = successorParent;
							}

							successor.Parent = parent;
							successor.Left = left;
							successor.Balance = node.Balance;
							successor.Right = right;
							right.Parent = successor;

							if (left != null)
							{
								left.Parent = successor;
							}

							if (node == _root)
							{
								_root = successor;
							}
							else
							{
								if (parent.Left == node)
								{
									parent.Left = successor;
								}
								else
								{
									parent.Right = successor;
								}
							}

							DeleteBalance(successorParent, -1);
						}
					}

					return outputValue;
				}

				if (compare <= 0)
				{
					node = node.Left;
				}
				else
				{
					node = node.Right;
				}
			}

			return default(TValue);
		}

		public TValue Search(TKey key)
		{
			AvlNode node = _root;

			while (node != null)
			{
				int compare = key.CompareTo(node.Key);

				if (compare < 0)
				{
					node = node.Left;
				}
				else if (compare > 0)
				{
					node = node.Right;
				}
				else
				{
					return node.Value;
				}
			}

			return default(TValue);
		}

		public TValue Search(TKey key, TValue value)
		{
			AvlNode node = _root;

			while (node != null)
			{
				int compare = key.CompareTo(node.Key);

				if (compare == 0 && value.CompareTo(node.Value) == 0)
				{
					return node.Value;
				}

				if (compare <= 0)
				{
					node = node.Left;
				}
				else if (compare > 0)
				{
					node = node.Right;
				}
			}

			return default(TValue);
		}
		
		public void Clear()
		{
			_root = null;
		}
		
		public IEnumerator<TValue> GetEnumerator()
		{
			return new AvlNodeEnumerator(_root);
		}

		private static AvlNode Replace(AvlNode target, AvlNode source)
		{
			AvlNode left = source.Left;
			AvlNode right = source.Right;

			target.Balance = source.Balance;
			target.Key = source.Key;
			target.Value = source.Value;
			target.Left = left;
			target.Right = right;

			if (left != null)
			{
				left.Parent = target;
			}

			if (right != null)
			{
				right.Parent = target;
			}

			return target;
		}

		private void DeleteBalance(AvlNode node, int balance)
		{
			while (node != null)
			{
				balance = (node.Balance += balance);

				if (balance == 2)
				{
					if (node.Left.Balance >= 0)
					{
						node = RotateRight(node);

						if (node.Balance == -1)
						{
							return;
						}
					}
					else
					{
						node = RotateLeftRight(node);
					}
				}
				else if (balance == -2)
				{
					if (node.Right.Balance <= 0)
					{
						node = RotateLeft(node);

						if (node.Balance == 1)
						{
							return;
						}
					}
					else
					{
						node = RotateRightLeft(node);
					}
				}
				else if (balance != 0)
				{
					return;
				}

				AvlNode parent = node.Parent;

				if (parent != null)
				{
					balance = parent.Left == node ? -1 : 1;
				}

				node = parent;
			}
		}

		private void InsertBalance(AvlNode node, int balance)
		{
			while (node != null)
			{
				balance = (node.Balance += balance);

				if (balance == 0)
				{
					return;
				}

				if (balance == 2)
				{
					if (node.Left.Balance == 1)
					{
						RotateRight(node);
					}
					else
					{
						RotateLeftRight(node);
					}

					return;
				}

				if (balance == -2)
				{
					if (node.Right.Balance == -1)
					{
						RotateLeft(node);
					}
					else
					{
						RotateRightLeft(node);
					}

					return;
				}

				AvlNode parent = node.Parent;

				if (parent != null)
				{
					balance = parent.Left == node ? 1 : -1;
				}

				node = parent;
			}
		}

		private AvlNode RotateLeft(AvlNode node)
		{
			AvlNode right = node.Right;
			AvlNode rightLeft = right.Left;
			AvlNode parent = node.Parent;

			right.Parent = parent;
			right.Left = node;
			node.Right = rightLeft;
			node.Parent = right;

			if (rightLeft != null)
			{
				rightLeft.Parent = node;
			}

			if (node == _root)
			{
				_root = right;
			}
			else if (parent.Right == node)
			{
				parent.Right = right;
			}
			else
			{
				parent.Left = right;
			}

			right.Balance++;
			node.Balance = -right.Balance;

			return right;
		}

		private AvlNode RotateRight(AvlNode node)
		{
			AvlNode left = node.Left;
			AvlNode leftRight = left.Right;
			AvlNode parent = node.Parent;

			left.Parent = parent;
			left.Right = node;
			node.Left = leftRight;
			node.Parent = left;

			if (leftRight != null)
			{
				leftRight.Parent = node;
			}

			if (node == _root)
			{
				_root = left;
			}
			else if (parent.Left == node)
			{
				parent.Left = left;
			}
			else
			{
				parent.Right = left;
			}

			left.Balance--;
			node.Balance = -left.Balance;

			return left;
		}

		private AvlNode RotateLeftRight(AvlNode node)
		{
			AvlNode left = node.Left;
			AvlNode leftRight = left.Right;
			AvlNode parent = node.Parent;
			AvlNode leftRightRight = leftRight.Right;
			AvlNode leftRightLeft = leftRight.Left;

			leftRight.Parent = parent;
			node.Left = leftRightRight;
			left.Right = leftRightLeft;
			leftRight.Left = left;
			leftRight.Right = node;
			left.Parent = leftRight;
			node.Parent = leftRight;

			if (leftRightRight != null)
			{
				leftRightRight.Parent = node;
			}

			if (leftRightLeft != null)
			{
				leftRightLeft.Parent = left;
			}

			if (node == _root)
			{
				_root = leftRight;
			}
			else if (parent.Left == node)
			{
				parent.Left = leftRight;
			}
			else
			{
				parent.Right = leftRight;
			}

			if (leftRight.Balance == -1)
			{
				node.Balance = 0;
				left.Balance = 1;
			}
			else if (leftRight.Balance == 0)
			{
				node.Balance = 0;
				left.Balance = 0;
			}
			else
			{
				node.Balance = -1;
				left.Balance = 0;
			}

			leftRight.Balance = 0;

			return leftRight;
		}

		private AvlNode RotateRightLeft(AvlNode node)
		{
			AvlNode right = node.Right;
			AvlNode rightLeft = right.Left;
			AvlNode parent = node.Parent;
			AvlNode rightLeftLeft = rightLeft.Left;
			AvlNode rightLeftRight = rightLeft.Right;

			rightLeft.Parent = parent;
			node.Right = rightLeftLeft;
			right.Left = rightLeftRight;
			rightLeft.Right = right;
			rightLeft.Left = node;
			right.Parent = rightLeft;
			node.Parent = rightLeft;

			if (rightLeftLeft != null)
			{
				rightLeftLeft.Parent = node;
			}

			if (rightLeftRight != null)
			{
				rightLeftRight.Parent = right;
			}

			if (node == _root)
			{
				_root = rightLeft;
			}
			else if (parent.Right == node)
			{
				parent.Right = rightLeft;
			}
			else
			{
				parent.Left = rightLeft;
			}

			if (rightLeft.Balance == 1)
			{
				node.Balance = 0;
				right.Balance = -1;
			}
			else if (rightLeft.Balance == 0)
			{
				node.Balance = 0;
				right.Balance = 0;
			}
			else
			{
				node.Balance = 1;
				right.Balance = 0;
			}

			rightLeft.Balance = 0;

			return rightLeft;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		private sealed class AvlNodeEnumerator : IEnumerator<TValue>
		{
			private AvlNode _root;
			private Action _action;
			private AvlNode _current;
			private AvlNode _right;

			public AvlNodeEnumerator(AvlNode root)
			{
				_right = _root = root;
				_action = _root == null ? Action.End : Action.Right;
			}

			public bool MoveNext()
			{
				switch (_action)
				{
					case Action.Right:
						_current = _right;

						while (_current.Left != null)
						{
							_current = _current.Left;
						}

						_right = _current.Right;
						_action = _right != null ? Action.Right : Action.Parent;

						return true;

					case Action.Parent:
						while (_current.Parent != null)
						{
							AvlNode previous = _current;

							_current = _current.Parent;

							if (_current.Left == previous)
							{
								_right = _current.Right;
								_action = _right != null ? Action.Right : Action.Parent;

								return true;
							}
						}

						_action = Action.End;

						return false;

					default:
						return false;
				}
			}

			public void Reset()
			{
				_right = _root;
				_action = _root == null ? Action.End : Action.Right;
			}

			public TValue Current
			{
				get { return _current.Value; }
			}

			object IEnumerator.Current
			{
				get { return Current; }
			}

			public void Dispose()
			{
			}

			enum Action
			{
				Parent,
				Right,
				End
			}
		}
	}
}