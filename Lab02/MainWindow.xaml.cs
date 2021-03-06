﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
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
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private Style el_template;
        private Style l_template;
        private Style el_mouse_over;
        private Style el_selected;
        private Style l_selected;
        private Graph uses_graph;
        private Random random = new Random();
        private Ellipse curOverEllipse
        {
            get
            {
                Ellipse ellipse = null;

                foreach (var child in cl_VasaField.Children)
                {
                    if (child is Ellipse && ((Ellipse)child).IsMouseOver)
                    {
                        ellipse = (Ellipse)child;
                        break;
                    }
                }

                return ellipse;
            }
        }
        private Ellipse previouslySelectedEllipse = null;

        private List<Link> selectedLines = new List<Link>();
        private List<Vertex> selectedVertexes = new List<Vertex>();
       
        private bool isDraggingEnabled = true;
        public bool IsDraggingEnabled { get { return isDraggingEnabled; } set { isDraggingEnabled = value; NotifyPropertyChanged(); } }


        private List<ISelectorAlgorithm> algorithms = new List<ISelectorAlgorithm>();
        public List<ISelectorAlgorithm> Algorithms
        {
            get { return algorithms; }
        }
        private ISelectorAlgorithm curAlogirthm;
        public ISelectorAlgorithm CurAlogirthm
        {
            get { return curAlogirthm; }
            set { curAlogirthm = value; RedrawUsesGraph(uses_graph); }
        }

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            el_template = FindResource("Ellipse_Template") as Style;
            l_template = FindResource("Line_Style") as Style;
            el_mouse_over = FindResource("MouseOverEllipseStyle") as Style;
            el_selected = FindResource("SelectedEllipseStyle") as Style;
            l_selected = FindResource("SelectedLineStyle") as Style;
            UsesGraphInit(new Graph());

            algorithms.Add(new BridgeSearch());
            algorithms.Add(new NonBridgeSearch());
            algorithms.Add(new BiconnectedComponentsSearch());
            algorithms.Add(new MinLineCovering());
            curAlogirthm = algorithms.FirstOrDefault();

            RedrawUsesGraph(uses_graph);
        }

        void UsesGraphInit(Graph newGraph)
        {
            uses_graph = newGraph;
            uses_graph.GraphChanged += RedrawUsesGraph;
        }

        void RedrawUsesGraph(Graph gr)
        {
            cl_VasaField.Children.Clear();
            curAlogirthm.Initialize(uses_graph);
            selectedLines = curAlogirthm.SelectLines();
            selectedVertexes = curAlogirthm.SelectVertexes();

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
            
            if (!selectedVertexes.Contains(v))
                el.Style = el_template;
            else
                el.Style = el_selected;

            el.MouseDown += el_MouseDown;
            cl_VasaField.Children.Add(el);
        }
        
        void AddLinkToCanvas(Vertex v1, Vertex v2)
        {
            Line l = new Line();
            Link pair = new Link(v1, v2);
            l.DataContext = pair;
            if (!selectedLines.Contains(pair))
                l.Style = l_template;
            else
                l.Style = l_selected;
            cl_VasaField.Children.Add(l);
        }

        #region Event handlers

        //Adding links between 2 ellipses by clicking on them (mouse right button)
        private void el_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Ellipse el = sender as Ellipse;
            if (el != null)
                if (previouslySelectedEllipse == null)
                {
                    previouslySelectedEllipse = el;
                    previouslySelectedEllipse.Style = el_mouse_over;
                }
                else
                {
                    Vertex v1 = previouslySelectedEllipse.DataContext as Vertex;
                    Vertex v2 = el.DataContext as Vertex;
                    previouslySelectedEllipse.Style = el_template;
                    previouslySelectedEllipse = null;

                    if (v1 == v2)
                        return;

                    if (!uses_graph.IsLinked(v1, v2))
                        uses_graph.AddLink(v1, v2);
                    else
                        uses_graph.DeleteLink(v1, v2);
                }
        }

        #region  Drag disabling while Ctrl is holded
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
                IsDraggingEnabled = false;
        }
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
            {
                IsDraggingEnabled = true;
                previouslySelectedEllipse = null;
            }
        }
        #endregion

        //Adding new ellipse by mouse double clicking
        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (IsDraggingEnabled && e.ChangedButton == MouseButton.Left)
            {
                Ellipse ellipse = curOverEllipse;
                if (ellipse == null)
                    uses_graph.AddVertex(Mouse.GetPosition(cl_VasaField));
                else
                    uses_graph.DeleteVertex(ellipse.DataContext as Vertex);
            }
        }

        #region Toolbar buttons
        private void NewGraph_Click(object sender, RoutedEventArgs e)
        {
            if (!GraphHelper.CheckIfUnsaved(this, uses_graph))
                return;

            UsesGraphInit(new Graph());
            RedrawUsesGraph(uses_graph);
        }

        private void ClearGraph_Click(object sender, RoutedEventArgs e)
        {
            if (!GraphHelper.CheckIfUnsaved(this, uses_graph))
                return;

            uses_graph.Clear();
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            //SettingsWindow should be opened
        }

        private void OpenFromFile_Click(object sender, RoutedEventArgs e)
        {
            Graph openedGraph = GraphHelper.OpenGraphFromFile(this);
            if (openedGraph != null)
            {
                if (!GraphHelper.CheckIfUnsaved(this, uses_graph))
                    return;

                UsesGraphInit(openedGraph);
                RedrawUsesGraph(uses_graph);
            }
        }
        
        private void SaveToFile_Click(object sender, RoutedEventArgs e)
        {
            GraphHelper.SaveGraphToFile(this, uses_graph);
        }
        
        private void GetInfo_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
