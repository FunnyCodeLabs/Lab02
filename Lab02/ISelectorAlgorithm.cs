using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab02
{
    public interface ISelectorAlgorithm
    {
        List<Tuple<Vertex, Vertex>> SelectLines();
        List<Vertex> SelectVertexes();
    }
}
