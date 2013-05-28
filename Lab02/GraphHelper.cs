using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Win32;
using System.Windows;

namespace Lab02
{
    public class GraphHelper
    {
        #region Constants

        private static readonly string DEFAULT_FILENAME = "graph";
        private static readonly string DEFAULT_EXTENSION = ".gr";
        private static readonly string DEFAULT_FILEFILTER = "Graph files (*.gr)|*" + DEFAULT_EXTENSION;
        private static readonly string DEFAULT_DIRECTORY = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private static readonly string OPENDIALOG_TITLE = "Open existing graph";
        private static readonly string SAVEDIALOG_TITLE = "Save graph";

        #endregion

        public static void SerializeGraph(Graph graph, string path_to_file)
        {
            using (Stream stream = File.Open(path_to_file, FileMode.OpenOrCreate))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, graph.GetSerializableClone());
            }
        }

        public static Graph DeSerializeGraph(string path_to_file)
        {
            using (Stream stream = File.Open(path_to_file, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                GraphSerializable gr = (GraphSerializable)formatter.Deserialize(stream);
                return new Graph(gr);
            }
        }

        public static Graph OpenGraphFromFile(Window owner)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Multiselect = false;
            openDialog.InitialDirectory = DEFAULT_DIRECTORY;
            openDialog.Filter = DEFAULT_FILEFILTER;
            openDialog.Title = OPENDIALOG_TITLE;

            var result = openDialog.ShowDialog(owner);
            if (result.Value)
            {
                string fileName = openDialog.FileName;

                return DeSerializeGraph(fileName);
            }

            return null;
        }

        public static void SaveGraphToFile(Window owner, Graph graph)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();

            saveDialog.OverwritePrompt = true;
            saveDialog.FileName = DEFAULT_FILENAME;
            saveDialog.DefaultExt = DEFAULT_EXTENSION;
            saveDialog.AddExtension = true;
            saveDialog.CheckPathExists = true;
            saveDialog.InitialDirectory = DEFAULT_DIRECTORY;
            saveDialog.Filter = DEFAULT_FILEFILTER;
            saveDialog.Title = SAVEDIALOG_TITLE;

            var result = saveDialog.ShowDialog();
            if (result.Value)
            {
                SerializeGraph(graph, saveDialog.FileName);
                graph.ChangesSaved = true;
            }
        }

        public static MessageBoxResult DoYouWantToSaveQuestion(Window owner)
        {
            var result = MessageBox.Show(owner, "Do you want to save changes?", "Save graph", 
                                         MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

            return result;
        }

        /// <summary>
        /// Asks "Do you want to save question" if graph is unsaved. Returns true if next operation is allowed 
        /// </summary>
        public static bool CheckIfUnsaved(Window owner, Graph graph)
        {
            if (!graph.ChangesSaved)
            {
                var result = DoYouWantToSaveQuestion(owner);

                if (result == MessageBoxResult.Yes)
                    SaveGraphToFile(owner, graph);
                else if (result == MessageBoxResult.Cancel)
                    return false;
            }

            return true;
        }
    }
}
