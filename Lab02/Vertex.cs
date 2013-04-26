using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lab02
{
    [Serializable]
    public class Vertex
    {
        public bool HasCoords { get; set; }
        private int x;
        private int y;

        public int X 
        { 
            get 
            {
                if (!HasCoords)
                    throw new ArgumentException("Coords are not initialized! (x)");
                return x;
            } 
            set { x = value; HasCoords = true; } 
        }

        public int Y 
        {
            get 
            {
                if (!HasCoords)
                    throw new ArgumentException("Coords are not initialized! (y)");
                return y; 
            } 
            set { y = value; HasCoords = true; } 
        }

        public Vertex(Point? coords = null)
        {
            if (coords.HasValue)
            {
                X = (int)coords.Value.X;
                Y = (int)coords.Value.Y;
            }
            else
                HasCoords = false;
        }
    }
}
