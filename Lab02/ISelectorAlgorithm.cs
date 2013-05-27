using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab02
{
    public interface ISelectorAlgorithm
    {
        void Initialize(Graph gr);
        List<Link> SelectLines();
        List<Vertex> SelectVertexes();
    }
}
