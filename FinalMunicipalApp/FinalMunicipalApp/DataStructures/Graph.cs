using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalMunicipalApp.DataStructures
{
        public class Graph
        {
            // nodes as strings
            private readonly Dictionary<string, List<(string to, int weight)>> adj = new();

            public void AddEdge(string from, string to, int weight = 1, bool undirected = true)
            {
                if (!adj.ContainsKey(from)) adj[from] = new List<(string, int)>();
                if (!adj.ContainsKey(to)) adj[to] = new List<(string, int)>();
                adj[from].Add((to, weight));
                if (undirected) adj[to].Add((from, weight));
            }

            public List<string> BFS(string start)
            {
                var visited = new HashSet<string>();
                var q = new Queue<string>();
                var order = new List<string>();
                if (!adj.ContainsKey(start)) return order;
                q.Enqueue(start);
                visited.Add(start);
                while (q.Count > 0)
                {
                    var u = q.Dequeue();
                    order.Add(u);
                    foreach (var (v, _) in adj[u])
                    {
                        if (!visited.Contains(v))
                        {
                            visited.Add(v);
                            q.Enqueue(v);
                        }
                    }
                }
                return order;
            }

            // returns edges selected
            public List<(string u, string v, int w)> PrimsMST()
            {
                var result = new List<(string, string, int)>();
                if (adj.Count == 0) return result;

                var start = adj.Keys.First();
                var inMST = new HashSet<string> { start };
                var edges = new List<(string u, string v, int w)>();
                foreach (var (v, w) in adj[start]) edges.Add((start, v, w));

                while (inMST.Count < adj.Count)
                {
                    var candidate = edges.OrderBy(e => e.w).FirstOrDefault(e => !inMST.Contains(e.v));
                    if (candidate.v == null) break;
                    result.Add(candidate);
                    inMST.Add(candidate.v);
                    // add edges from candidate.v
                    foreach (var (nv, w) in adj[candidate.v])
                    {
                        if (!inMST.Contains(nv))
                            edges.Add((candidate.v, nv, w));
                    }
                    edges = edges.Where(e => !inMST.Contains(e.v)).ToList();
                }
                return result;
            }
        }
    }

