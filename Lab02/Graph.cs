using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lab02
{
    public class Graph
    {
        private List<List<Vertex>> adjacencyList;

        int n;

        public List<Vertex> Vertexes
        {
            get 
            {
                return adjacencyList.Select(o => o.First()).ToList();
            }
        }

        public List<Tuple<Vertex, Vertex>> Links
        {
            get
            {
                List<Tuple<Vertex, Vertex>> links = new List<Tuple<Vertex, Vertex>>();
                foreach (var list in adjacencyList)
                {
                    for (int i = 1; i < list.Count; i++)
                    {
                        links.Add(new Tuple<Vertex,Vertex>(list[0], list[i]));
                    }
                }
                return links;
            }
        }

        public bool ChangesSaved
        {
            get;
            set;
        }

        public Graph()
        {
            adjacencyList = new List<List<Vertex>>();
            n = 0;
            ChangesSaved = true;
        }

        public Graph(GraphSerializable gr)
        {
            LoadSerializableClone(gr);

            ChangesSaved = true;
        }

        private List<Vertex> GetAdjacent(Vertex v)
        {
            return (from o in adjacencyList
                    where o[0] == v
                    select o).FirstOrDefault();
        }

        public Vertex AddVertex(Point? coords = null)
        {
            Vertex nv = new Vertex(coords);
            adjacencyList.Add(new List<Vertex>() { nv });
            n++;
            OnGraphChanged();
            return nv;
        }

        public void AddLink(Vertex v1, Vertex v2)
        {
            List<Vertex> v1Adjacent = GetAdjacent(v1);
            List<Vertex> v2Adjacent = GetAdjacent(v2);

            if (v1Adjacent == null || v2Adjacent == null)
                return;

            v1Adjacent.Add(v2);
            v2Adjacent.Add(v1);

            OnGraphChanged();
        }

        public bool IsLinked(Vertex v1, Vertex v2)
        {
            return Links.Contains(new Tuple<Vertex,Vertex>(v1,v2));
        }

        public Vertex AddLinkedVertex(List<Vertex> adjacent)
        {
            adjacencyList.Add(new List<Vertex>(adjacent));
            OnGraphChanged();
            return adjacencyList.Last<List<Vertex>>().First<Vertex>();
        }

        public void DeleteVertex(Vertex v)
        {
            adjacencyList.Remove(GetAdjacent(v));
            foreach (var adjacent in adjacencyList)
            {
                adjacent.Remove(v);
            }
            OnGraphChanged();
        }

        public void DeleteLink(Vertex v1, Vertex v2)
        {
            GetAdjacent(v1).Remove(v2);
            GetAdjacent(v2).Remove(v1);
            OnGraphChanged();
        }

        public List<Vertex> GetLinkedVertexes(Vertex v)
        {
            return GetAdjacent(v).Skip(1).ToList();
        }

        public delegate void GraphChangedEventHandler(Graph sender);

        public event GraphChangedEventHandler GraphChanged;

        private void OnGraphChanged()
        {
            ChangesSaved = false;

            if (GraphChanged != null)
                GraphChanged(this);
        }

        private void LoadSerializableClone(GraphSerializable gr)
        {
            adjacencyList = gr.adjacencyList;
            n = gr.n;
        }

        public GraphSerializable GetSerializableClone()
        {
            return new GraphSerializable() { adjacencyList = this.adjacencyList, n = this.n};
        }
    }
}