﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab02
{
    class BridgeSearch : ISelectorAlgorithm
    {
        Graph graph;
        List<int>[] g;
        bool[] used;
        int timer;
        int[] tin, fup;

        private List<Link> bridges;

        void dfs(int v, int p = -1)
        {
            used[v] = true;
            tin[v] = fup[v] = timer++;
            for (int i = 0; i < g[v].Count; ++i)
            {
                int to = g[v][i];
                if (to == p) continue;
                if (used[to])
                    fup[v] = Math.Min(fup[v], tin[to]);
                else
                {
                    dfs(to, v);
                    fup[v] = Math.Min(fup[v], fup[to]);
                    if (fup[to] > tin[v])
                    {
                        bridges.Add(new Link(graph.Vertexes[v], graph.Vertexes[to]));
                    }
                }
            }
        }

        private List<Link> FindBridges()
        {
            timer = 0;
            for (int i = 0; i < g.Length; ++i)
                if (!used[i])
                    dfs(i);

            for (int i = 0, n = bridges.Count; i < n; i++)
                bridges.Add(new Link(bridges[i].Item2, bridges[i].Item1));
            return bridges;
        }

        public List<Link> SelectLines()
        {
            return FindBridges();
        }

        private List<Vertex> emptyVertexList = new List<Vertex>();

        public List<Vertex> SelectVertexes()
        {
            return emptyVertexList;
        }

        public void Initialize(Graph gr)
        {
            bridges = new List<Link>();
            graph = gr;

            used = new bool[gr.Vertexes.Count];
            tin = new int[gr.Vertexes.Count];
            fup = new int[gr.Vertexes.Count];

            g = new List<int>[gr.Vertexes.Count];
            int i = 0;
            foreach (var v in graph.Vertexes)
            {
                g[i++] = new List<int>();
            }
            foreach (var pair in graph.Links)
            {
                g[graph.Vertexes.IndexOf(pair.Item1)].Add(graph.Vertexes.IndexOf(pair.Item2));
            }
        }

        public override string ToString()
        {
            return "Bridge search";
        }
    }
}