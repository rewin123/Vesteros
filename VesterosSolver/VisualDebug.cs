using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VesterosSolver
{
    public class VisualDebug
    {
        public static Bitmap DrawPlaces(List<Place> places, int width, int height)
        {
            Bitmap map = new Bitmap(width, height);

            Graphics gr = Graphics.FromImage(map);
            for(int i = 0;i < places.Count;i++)
            {
                gr.FillEllipse(Brushes.Green, places[i].position.X * width - 10, places[i].position.Y * height - 10, 20, 20);
            }
        }
    }
}
