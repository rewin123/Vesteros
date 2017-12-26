using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Numerics;
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

            Bitmap map = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            BitmapData locked_data = map.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            unsafe
            {
                byte* arr = (byte*)locked_data.Scan0.ToPointer();

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        Vector2 vector = new Vector2((float)x / width, (float)y / height);
                        Place first_place = null;
                        float first_val = float.MaxValue;
                        float second_val = float.MaxValue;

                        for (int i = 0; i < places.Count; i++)
                        {
                            float val = (vector - places[i].position).Length();
                            if (val < first_val)
                            {
                                second_val = first_val;
                                first_place = places[i];
                                first_val = val;
                            }
                        }

                        if (Math.Abs(first_val - second_val) < 0.01)
                        {
                            //white
                            arr[(y * width + x) * 4] = 255;
                            arr[(y * width + x) * 4 + 1] = 255;
                            arr[(y * width + x) * 4 + 2] = 255;
                            arr[(y * width + x) * 4 + 3] = 255;
                        }
                        else
                        {
                            
                            //arr[(y * width + x) * 4] = 255;
                            arr[(y * width + x) * 4 + 1] = (byte)(first_place.isSea ? 100 : 255);
                            arr[(y * width + x) * 4] = (byte)(first_place.isSea ? 255 : 0);
                            arr[(y * width + x) * 4 + 3] = 255;
                        }
                    }
                }
            }

            map.UnlockBits(locked_data);

            Graphics gr = Graphics.FromImage(map);
            //gr.Clear(Color.Wheat);
            for(int i = 0;i < places.Count;i++)
            {
                Place place = places[i];
                if (!place.isSea)
                    gr.FillEllipse(Brushes.Green, places[i].position.X * width - 10, places[i].position.Y * height - 10, 20, 20);
                else gr.FillEllipse(Brushes.Blue, places[i].position.X * width - 10, places[i].position.Y * height - 10, 20, 20);

                for (int j = 0;j < place.links.Count;j++)
                {
                    gr.DrawLine(path_pen, place.position.X * width, place.position.Y * height,
                        place.links[j].position.X * width, place.links[j].position.Y * height);
                }
                string data = place.name;
                if(place.isSea == false)
                {
                    data += "\nB: " + place.barrelCount;
                    data += "\nP: " + place.powerCount;
                    data += "\nC: " + place.castleLevel;
                }
                gr.DrawString(data, SystemFonts.DefaultFont, name_brush, new PointF(place.position.X * width, place.position.Y * height));
            }

            return map;
        }
    }
}
