using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Lab02
{
    public class Link : Tuple<Vertex, Vertex>, INotifyPropertyChanged
    {
        private int length;
        public int Length 
        {
            get { return length; }
            set { length = value; NotifyPropertyChanged(); }
        }

        public Link(Vertex v1, Vertex v2) : base(v1,v2)
        {
            v1.CoordsChanged += Vertexes_CoordsChanged;
            v2.CoordsChanged += Vertexes_CoordsChanged;
            Vertexes_CoordsChanged();
        }

        void Vertexes_CoordsChanged()
        {
            int x1 = Item1.X;
            int y1 = Item1.Y;
            int x2 = Item2.X;
            int y2 = Item2.Y;

            Length = (int)Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }

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
