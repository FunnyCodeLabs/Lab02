using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab02
{
    public class BiconnectedComponentsSearch : ISelectorAlgorithm
    {
        private Graph graph;
        private List<Vertex> biconnectedComponents;
        private List<List<int>> graphRepresentation;
        bool[] used;
        int timer;
        int[] tin, fup;

        void DFS(int v, int p = -1)
        {
	        used[v] = true;
	        tin[v] = fup[v] = timer++;
	        int children = 0;

            for (int i = 0; i < graphRepresentation[v].Count; i++) 
            {
                int to = graphRepresentation[v][i];
		        if (to == p)  continue;
		        if (used[to])
			        fup[v] = Math.Min(fup[v], tin[to]);
		        else 
                {
			        DFS (to, v);
			        fup[v] = Math.Min(fup[v], fup[to]);
                    if (fup[to] >= tin[v] && p != -1)
                    {
                        Vertex vertex = graph.Vertexes[v];
                        if (!biconnectedComponents.Contains(vertex))
                            biconnectedComponents.Add(vertex);
                    }
			        ++children;
		        }
	        }

            if (p == -1 && children > 1)
            {
                Vertex vertex = graph.Vertexes[v];
                if (!biconnectedComponents.Contains(vertex))
                    biconnectedComponents.Add(vertex);
            }
        }




        private List<Vertex> FindBiconnectedComponents()
        {
            if (graph.Vertexes.Count > 0)
            {
                timer = 0;
                DFS(0);
            }

            return biconnectedComponents; 
        }


        public List<Vertex> SelectVertexes()
        {
            return FindBiconnectedComponents();
        }

        private List<Link> emptyLinesList = new List<Link>();
        public List<Link> SelectLines()
        {
            return emptyLinesList;
        }


        public void Initialize(Graph gr)
        {
            biconnectedComponents = new List<Vertex>();
            graph = gr;

            used = new bool[graph.Vertexes.Count];

            tin = new int[graph.Vertexes.Count];
            fup = new int[graph.Vertexes.Count];

            graphRepresentation = new List<List<int>>();

            foreach (var v in graph.Vertexes)
            {
                graphRepresentation.Add(new List<int>());
            }
            foreach (var pair in graph.Links)
            {
                graphRepresentation[graph.Vertexes.IndexOf(pair.Item1)].Add(graph.Vertexes.IndexOf(pair.Item2));
            }
        }
        
        public override string ToString()
        {
            return "Biconnected components search";
        }
    }
}
