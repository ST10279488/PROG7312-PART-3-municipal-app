using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using FinalMunicipalApp.Models;

namespace FinalMunicipalApp.DataStructures
{

   
        public class BSTNode
        {
            public ServiceRequest Value;
            public BSTNode Left;
            public BSTNode Right;
            public BSTNode(ServiceRequest value) { Value = value; }
        }

        public class BinarySearchTree
        {
            public BSTNode Root;

            public void Insert(ServiceRequest request)
            {
                Root = Insert(Root, request);
            }

            private BSTNode Insert(BSTNode node, ServiceRequest val)
            {
                if (node == null) return new BSTNode(val);

                int cmp = string.Compare(val.RequestId, node.Value.RequestId, StringComparison.Ordinal);
                if (cmp < 0) node.Left = Insert(node.Left, val);
                else if (cmp > 0) node.Right = Insert(node.Right, val);
                else
                {
                    // If same ID, replace
                    node.Value = val;
                }
                return node;
            }

            public ServiceRequest Find(string requestId)
            {
                var node = Root;
                while (node != null)
                {
                    int cmp = string.Compare(requestId, node.Value.RequestId, StringComparison.Ordinal);
                    if (cmp == 0) return node.Value;
                    node = cmp < 0 ? node.Left : node.Right;
                }
                return null;
            }

            public List<ServiceRequest> InOrder()
            {
                var list = new List<ServiceRequest>();
                TraverseInOrder(Root, list);
                return list;
            }

            private void TraverseInOrder(BSTNode node, List<ServiceRequest> list)
            {
                if (node == null) return;
                TraverseInOrder(node.Left, list);
                list.Add(node.Value);
                TraverseInOrder(node.Right, list);
            }
        }
    }
