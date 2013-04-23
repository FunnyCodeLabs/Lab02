using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFExtensions.AttachedBehaviours;

namespace Lab02
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Style el_template;
        private Style l_template;
        private Graph uses_graph;
        private Random random = new Random();

        public MainWindow()
        {
            InitializeComponent();
            el_template = FindResource("Ellipse_Template") as Style;
            l_template = FindResource("Line_Style") as Style;
            UsesGraph();
            RedrawUsesGraph(uses_graph);
            
        }

        void UsesGraph()
        {
            uses_graph = new Graph();
            Vertex v1 = uses_graph.AddVertex();
            Vertex v2 = uses_graph.AddVertex();
            Vertex v3 = uses_graph.AddVertex();
            Vertex v4 = uses_graph.AddVertex();
            uses_graph.AddLink(v1, v2);
            uses_graph.AddLink(v2, v3);
            uses_graph.AddLink(v3, v4);
            uses_graph.AddLink(v4, v1);
            uses_graph.GraphChanged += RedrawUsesGraph;
        }

        void RedrawUsesGraph(Graph gr)
        {
            cl_VasaField.Children.Clear();
            foreach (var v in gr.Vertexes)
            {
                AddVertexToCanvas(v);
            }
            foreach (var link in gr.Links)
            {
                AddLinkToCanvas(link.Item1, link.Item2);
            }
        }
        
        void AddVertexToCanvas(Vertex v)
        {
            if (!v.HasCoords)
            {
                v.X = random.Next(0, (int)this.Width);
                v.Y = random.Next(0, (int)this.Height);
            }
            Ellipse el = new Ellipse();
            el.DataContext = v;
            el.Style = el_template;
            cl_VasaField.Children.Add(el);
        }

        void AddLinkToCanvas(Vertex v1, Vertex v2)
        {
            Line l = new Line();
            Tuple<Vertex, Vertex> pair = new Tuple<Vertex, Vertex>(v1, v2);
            l.DataContext = pair;
            l.Style = l_template;
            cl_VasaField.Children.Add(l);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                RedrawUsesGraph(uses_graph);
        }
    }
}
