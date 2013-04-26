using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Lab02
{
    class GraphHelper
    {
        public static void SerializeGraph(Graph graph, string path_to_file)
        {
            using (Stream stream = File.Open(path_to_file, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, graph);
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
    }
}
