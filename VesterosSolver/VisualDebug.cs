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
        public static Pen path_pen = new Pen(Color.Violet, 3);
        public static Brush name_brush = new SolidBrush(Color.Black);
        
        public static Bitmap DrawPlaces(List<Place> places, int width, int height)
        {

            Bitmap map = new Bitmap(width, height);

            Graphics gr = Graphics.FromImage(map);
            gr.Clear(Color.Wheat);
            for(int i = 0;i < places.Count;i++)
            {
                Place place = places[i];
                gr.FillEllipse(Brushes.Green, places[i].position.X * width - 10, places[i].position.Y * height - 10, 20, 20);

                for(int j = 0;j < place.links.Count;j++)
                {
                    gr.DrawLine(path_pen, place.position.X * width, place.position.Y * height,
                        place.links[j].position.X * width, place.links[j].position.Y * height);
                }

                gr.DrawString(place.name, SystemFonts.DefaultFont, name_brush, new PointF(place.position.X * width, place.position.Y * height));
            }

            return map;
        }
    }
}
