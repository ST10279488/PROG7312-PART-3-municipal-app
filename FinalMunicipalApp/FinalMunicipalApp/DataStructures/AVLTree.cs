using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalMunicipalApp.Models;

namespace FinalMunicipalApp.DataStructures
{

        public class AVLNode
        {
            public ServiceRequest Value;
            public AVLNode Left;
            public AVLNode Right;
            public int Height;
            public AVLNode(ServiceRequest value) { Value = value; Height = 1; }
        }

        public class AVLTree
        {
            public AVLNode Root;

            private int Height(AVLNode node) => node?.Height ?? 0;

            private int BalanceFactor(AVLNode node) => node == null ? 0 : Height(node.Left) - Height(node.Right);

            private void UpdateHeight(AVLNode node)
            {
                node.Height = 1 + Math.Max(Height(node.Left), Height(node.Right));
            }

            private AVLNode RightRotate(AVLNode y)
            {
                AVLNode x = y.Left;
                AVLNode T2 = x.Right;

                x.Right = y;
                y.Left = T2;

                UpdateHeight(y);
                UpdateHeight(x);

                return x;
            }

            private AVLNode LeftRotate(AVLNode x)
            {
                AVLNode y = x.Right;
                AVLNode T2 = y.Left;

                y.Left = x;
                x.Right = T2;

                UpdateHeight(x);
                UpdateHeight(y);

                return y;
            }

            public void Insert(ServiceRequest val)
            {
                Root = Insert(Root, val);
            }

            private AVLNode Insert(AVLNode node, ServiceRequest val)
            {
                if (node == null) return new AVLNode(val);

                int cmp = string.Compare(val.RequestId, node.Value.RequestId, StringComparison.Ordinal);
                if (cmp < 0) node.Left = Insert(node.Left, val);
                else if (cmp > 0) node.Right = Insert(node.Right, val);
                else node.Value = val;

                UpdateHeight(node);
                int balance = BalanceFactor(node);

                // Left Left
                if (balance > 1 && string.Compare(val.RequestId, node.Left.Value.RequestId, StringComparison.Ordinal) < 0)
                    return RightRotate(node);

                // Right Right
                if (balance < -1 && string.Compare(val.RequestId, node.Right.Value.RequestId, StringComparison.Ordinal) > 0)
                    return LeftRotate(node);

                // Left Right
                if (balance > 1 && string.Compare(val.RequestId, node.Left.Value.RequestId, StringComparison.Ordinal) > 0)
                {
                    node.Left = LeftRotate(node.Left);
                    return RightRotate(node);
                }

                // Right Left
                if (balance < -1 && string.Compare(val.RequestId, node.Right.Value.RequestId, StringComparison.Ordinal) < 0)
                {
                    node.Right = RightRotate(node.Right);
                    return LeftRotate(node);
                }

                return node;
            }

            public ServiceRequest Find(string id)
            {
                var n = Root;
                while (n != null)
                {
                    int cmp = string.Compare(id, n.Value.RequestId, StringComparison.Ordinal);
                    if (cmp == 0) return n.Value;
                    n = cmp < 0 ? n.Left : n.Right;
                }
                return null;
            }

            public void InOrder(AVLNode node, List<ServiceRequest> acc)
            {
                if (node == null) return;
                InOrder(node.Left, acc);
                acc.Add(node.Value);
                InOrder(node.Right, acc);
            }
        }

    }
