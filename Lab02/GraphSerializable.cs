using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab02
{
    [Serializable]
    public class GraphSerializable
    {
        public List<List<Vertex>> adjacencyList;
        public int n;
    }
}
