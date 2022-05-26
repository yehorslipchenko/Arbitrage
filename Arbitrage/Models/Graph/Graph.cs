using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;

namespace Arbitrage.Models.Graph
{
    public class Graph
    {
        public List<Vertex> Verticies { get; }

        public Graph()
        {
            Verticies = new List<Vertex>();
        }

        public void AddVertex(string name, int pn)
        {
            Verticies.Add(new Vertex(name, pn));
        }

        public Vertex FindVertex(string name)
        {
            Vertex ver = null;
            foreach (var v in Verticies)
            {
                if (v.Name.Equals(name))
                {
                    ver = v;
                }
            }

            return ver;
        }

        public void AddEdge(string first_name, string second_name, float firstWeight, float secondWeight)
        {
            var v1 = FindVertex(first_name);
            var v2 = FindVertex(second_name);

            if (v1 == null || v2 == null) return;
            v1.AddEdge(v2, firstWeight);
            v2.AddEdge(v1, secondWeight);
        }

        public void CreateGraph(List<Currency> currencies)
        {
            foreach (var pair in currencies)
            {
                var pairNumber = pair.PairNumber;
                var first_vertex_name = pair.Token0Address;
                var second_vertex_name = pair.Token1Address;
                var firstWeight = (float) -Math.Log(pair.Token1Reverse / pair.Token0Reverse, 2);
                var secondWeight = (float) -Math.Log(pair.Token0Reverse / pair.Token1Reverse, 2);
                
                if (FindVertex(first_vertex_name) == null)
                {
                    AddVertex(first_vertex_name, pairNumber);
                }

                if (FindVertex(second_vertex_name) == null)
                {
                    AddVertex(second_vertex_name, pairNumber);
                }
                
                AddEdge(first_vertex_name, second_vertex_name, firstWeight, secondWeight);

            }
        }

        public async Task<int> BellmanFordAlgorithm(int vertex)
        {
            var answer = new List<Vertex>();
            // Step 1: Initialize distance from vertex to all others vertices
            var distance = new float[Verticies.Count];
            for (int i = 0; i < distance.Length; i++)
            {
                distance[i] = float.PositiveInfinity;
            }

            Vertex[] path = new Vertex[Verticies.Count];
            for (int i = 0; i < path.Length; i++)
            {
                path[i] = new Vertex("-1", -1);
            }
            distance[vertex] = 0;
            
            // Step 2: Relax all edges |V| - 1 times. A simple shortest path from vertex
            // to any other vertex can have at-most |V| - 1 edges
            for (int s = 0; s < Verticies.Count - 1; s++)
            {
                for (int i = 0; i < Verticies.Count; i++)
                {
                    for (int j = 0; j < Verticies[i].Edges.Count; j++)
                    {
                        int v = i;
                        int u = Verticies.FindIndex(x => x.Name == Verticies[i].Edges[j].Vertex.Name);
                        float weight = Verticies[i].Edges[j].Weight;
                        if (distance[v] > distance[u] + weight)
                        {
                            distance[v] = distance[u] + weight;
                            path[v] = Verticies[u];
                        }
                    }
                }
            }

            // Step 3: Check for negative-weight cycles.
            int count = 0;
            for (int i = 0; i < Verticies.Count; i++)
            {
                for (int j = 0; j < Verticies[i].Edges.Count; j++)
                {
                    int v = i;
                    int u = Verticies.FindIndex(x => x.Name == Verticies[i].Edges[j].Vertex.Name);
                    float weight = Verticies[i].Edges[j].Weight;
                    if (distance[u] != float.MaxValue && distance[u] + weight < distance[v])
                    {
                        count++;
                        Console.WriteLine("Graph conteins negative cycle!");
                    }
                }
            }

            return count;
        }

        public List<Currency> ArbitrageOpportunity()
        {
            var currencies = new List<Currency>();
            
            return currencies;
        }
    }
}