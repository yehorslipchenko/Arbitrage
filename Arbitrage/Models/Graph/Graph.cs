using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;

namespace Arbitrage.Models.Graph
{
    public class Graph
    {
        public List<Vertex> Verticies { get; }

        public Graph()
        {
            Verticies = new List<Vertex>();
        }

        public void AddVertex(string name)
        {
            Verticies.Add(new Vertex(name));
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
                var first_vertex_name = pair.token0_address;
                var second_vertex_name = pair.token1_address;
                var firstWeight = (float) -Math.Log(pair.token1_reverse / pair.token0_reverse, 2);
                var secondWeight = (float) -Math.Log(pair.token0_reverse / pair.token1_reverse, 2);
                
                if (FindVertex(first_vertex_name) == null)
                {
                    AddVertex(first_vertex_name);
                }

                if (FindVertex(second_vertex_name) == null)
                {
                    AddVertex(second_vertex_name);
                }
                
                AddEdge(first_vertex_name, second_vertex_name, firstWeight, secondWeight);

            }
        }

        public List<Vertex> BellmanFordAlgorithm(int vertex)
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
                path[i] = new Vertex("-1");
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
             
            for (int i = 0; i < Verticies.Count; i++)
            {
                for (int j = 0; j < Verticies[i].Edges.Count; j++)
                {
                    int v = i;
                    int u = Verticies.FindIndex(x => x.Name == Verticies[i].Edges[j].Vertex.Name);
                    float weight = Verticies[i].Edges[j].Weight;

                    if (distance[v] > distance[u] + weight)
                    {
                        for (int k = 0; k < Verticies.Count - 1; k++)
                        {
                            Verticies[v] = path[v];
                        }

                        Verticies[u] = Verticies[v];

                        while (Verticies[u] != path[v])
                        {
                            answer.Add(Verticies[v]);
                            Verticies[v] = path[v];
                        }
                        
                        answer.Reverse();
                        
                    }
                }
            }

            return answer;
        }

        public List<Currency> ArbitrageOpportunity()
        {
            var currencies = new List<Currency>();
            
            return currencies;
        }
    }
}