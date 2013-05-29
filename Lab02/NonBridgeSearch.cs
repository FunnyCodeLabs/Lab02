using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab02
{
    class NonBridgeSearch : ISelectorAlgorithm
    {
        Graph graph;
        BridgeSearch bridges = new BridgeSearch();

        private List<Link> nonBridges;

        private List<Link> FindNonBridges()
        {
            List<Link> b = bridges.SelectLines();
            return graph.Links.Except(b).ToList();
        }

        public List<Link> SelectLines()
        {
            return FindNonBridges();
        }


        private List<Vertex> emptyVertexList = new List<Vertex>();
        public List<Vertex> SelectVertexes()
        {
            return emptyVertexList;
        }

        public void Initialize(Graph gr)
        {
            graph = gr;
            bridges.Initialize(gr);
        }

        public override string ToString()
        {
            return "Non bridge search";
        }
    }
}