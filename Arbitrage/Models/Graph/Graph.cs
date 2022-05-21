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
    }
}