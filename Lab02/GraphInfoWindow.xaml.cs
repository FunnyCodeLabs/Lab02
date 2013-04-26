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
using System.Windows.Shapes;

namespace Lab02
{
    /// <summary>
    /// Interaction logic for GraphInfoWindow.xaml
    /// </summary>
    public partial class GraphInfoWindow : Window
    {
        private Graph uses_graph;

        public GraphInfoWindow(Graph graph)
        {
            if (graph == null)
                throw new ArgumentNullException("graph");

            InitializeComponent();
            uses_graph = graph;
        }

    }
}
