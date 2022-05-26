using System;
using System.Collections.Generic;

namespace Arbitrage.Models.Graph
{
    public class Graph
    {
        private List<Vertex> Vertices { get; }

        public Graph()
        {
            Vertices = new List<Vertex>();
        }

        private void AddVertex(string name, int pn)
        {
            Vertices.Add(new Vertex(name, pn));
        }


        private void AddEdge(string firstName, string secondName, float firstWeight, float secondWeight)
        {
            var v1 = Vertices.Find(v => v.Name.Equals(firstName));
            var v2 = Vertices.Find(v => v.Name.Equals(secondName));

            if (v1 == null || v2 == null) return;
            v1.AddEdge(v2, firstWeight);
            v2.AddEdge(v1, secondWeight);
        }

        public void CreateGraph(IEnumerable<Currency> currencies)
        {
            foreach (var pair in currencies)
            {
                var firstWeight = (float) -Math.Log(pair.Token1Reverse / pair.Token0Reverse, 2);
                var secondWeight = (float) -Math.Log(pair.Token0Reverse / pair.Token1Reverse, 2);
                
                if (Vertices.Find(v => v.Name.Equals(pair.Token0Address)) == null)
                {
                    AddVertex(pair.Token0Address, pair.PairNumber);
                }

                if (Vertices.Find(v => v.Name.Equals(pair.Token1Address)) == null)
                {
                    AddVertex(pair.Token1Address, pair.PairNumber);
                }
                
                AddEdge(pair.Token0Address, pair.Token1Address, firstWeight, secondWeight);

            }
        }

        public int BellmanFordAlgorithm(int vertex)
        {
            // Step 1: Initialize distance from vertex to all others vertices
            var distance = new float[Vertices.Count];
            for (var i = 0; i < distance.Length; i++)
            {
                distance[i] = float.PositiveInfinity;
            }

            var path = new Vertex[Vertices.Count];
            for (var i = 0; i < path.Length; i++)
            {
                path[i] = new Vertex("-1", -1);
            }
            distance[vertex] = 0;
            
            // Step 2: Relax all edges |V| - 1 times. A simple shortest path from vertex
            // to any other vertex can have at-most |V| - 1 edges
            for (var iter = 0; iter < Vertices.Count - 1; iter++)
            {
                for (var v = 0; v < Vertices.Count; v++)
                {
                    foreach (var e in Vertices[v].Edges)
                    {
                        var u = Vertices.FindIndex(x => x.Name == e.Vertex.Name);
                        var weight = e.Weight;
                        if (!(distance[v] > distance[u] + weight)) continue;
                        distance[v] = distance[u] + weight;
                        path[v] = Vertices[u];
                    }
                }
            }

            // Step 3: Check for negative-weight cycles.
            var count = 0;
            for (var v = 0; v < Vertices.Count; v++)
            {
                foreach (var e in Vertices[v].Edges)
                {
                    var u = Vertices.FindIndex(x => x.Name == e.Vertex.Name);
                    var weight = e.Weight;
                    if (Math.Abs(distance[u] - float.MaxValue) > 1e-20 && distance[u] + weight < distance[v])
                    {
                        count++;
                    }
                }
            }

            return count;
        }
    }
}