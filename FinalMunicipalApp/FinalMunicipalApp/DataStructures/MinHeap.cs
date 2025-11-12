using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalMunicipalApp.Models;

namespace FinalMunicipalApp.DataStructures
{
        public class MinHeap
        {
            private List<ServiceRequest> items = new List<ServiceRequest>();

            public int Count => items.Count;

            public void Insert(ServiceRequest req)
            {
                items.Add(req);
                HeapifyUp(items.Count - 1);
            }

            public ServiceRequest ExtractMin()
            {
                if (items.Count == 0) return null;
                var min = items[0];
                items[0] = items[items.Count - 1];
                items.RemoveAt(items.Count - 1);
                HeapifyDown(0);
                return min;
            }

            private void HeapifyUp(int index)
            {
                while (index > 0)
                {
                    int parent = (index - 1) / 2;
                    if (items[index].Priority < items[parent].Priority)
                    {
                        var tmp = items[parent];
                        items[parent] = items[index];
                        items[index] = tmp;
                        index = parent;
                    }
                    else break;
                }
            }

            private void HeapifyDown(int index)
            {
                int smallest = index;
                while (true)
                {
                    int left = 2 * index + 1;
                    int right = 2 * index + 2;
                    if (left < items.Count && items[left].Priority < items[smallest].Priority) smallest = left;
                    if (right < items.Count && items[right].Priority < items[smallest].Priority) smallest = right;
                    if (smallest != index)
                    {
                        var tmp = items[index];
                        items[index] = items[smallest];
                        items[smallest] = tmp;
                        index = smallest;
                    }
                    else break;
                }
            }
        }
    }

