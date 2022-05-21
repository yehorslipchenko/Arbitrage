namespace Arbitrage.Models.Graph
{
    public class Edge
    {
        public Vertex Vertex { get; }
        public float Weight { get; }
        
        public Edge(Vertex vertex, float weight)
        {
            Vertex = vertex;
            Weight = weight;
        }
        
    }
}