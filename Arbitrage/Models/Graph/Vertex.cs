using System.Collections.Generic;

namespace Arbitrage.Models.Graph
{
    public class Vertex
    {
        public string Name { get; }
        public List<Edge> Edges { get; }
        
        public Vertex(string name)
        {
            Name = name;
            Edges = new List<Edge>();
        }

        public void AddEdge(Edge edge)
        {
            Edges.Add(edge);
        }

        public void AddEdge(Vertex vertex, float weight)
        {
            AddEdge(new Edge(vertex, weight));
        }
    }
}