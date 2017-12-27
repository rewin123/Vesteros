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
    public static class VisualDebug
    {
        public static Pen path_pen = new Pen(Color.Violet, 3);
        public static Brush name_brush = new SolidBrush(Color.Black);
        public static int unit_size = 10;
        
        public static void DrawMosaic(List<Place> places, ref Bitmap map)
        {
            int width = map.Width;
            int height = map.Height;
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
                            arr[(y * width + x) * 4 + 1] = (byte)(first_place.isSea ? 100 : 180);
                            arr[(y * width + x) * 4] = (byte)(first_place.isSea ? 255 : 0);
                            arr[(y * width + x) * 4 + 3] = 255;
                        }
                    }
                }
            }

            map.UnlockBits(locked_data);
        }
        
        
        public static Bitmap DrawPlaces(List<Place> places, int width, int height)
        {

            Bitmap map = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            DrawMosaic(places, ref map);

            Graphics gr = Graphics.FromImage(map);
            //gr.Clear(Color.Wheat);
            for(int i = 0;i < places.Count;i++)
            {
                Place place = places[i];
                

                for (int j = 0;j < place.links.Count;j++)
                {
                    gr.DrawLine(path_pen, place.position.X * width, place.position.Y * height,
                        place.links[j].position.X * width, place.links[j].position.Y * height);
                }

                for(int j = 0;j < place.units.Count;j++)
                {
                    place.units[j].Draw(gr, place.position.X * width, place.position.Y * height + unit_size * 2 * (j + 1), unit_size);
                }
                

                string data = place.name;
                if(place.isSea == false)
                {
                    data += "\nB: " + place.barrelCount;
                    data += "\nP: " + place.powerCount;
                    data += "\nC: " + place.castleLevel;
                }
                gr.DrawString(data, SystemFonts.DefaultFont, name_brush, new PointF(place.position.X * width, place.position.Y * height));

                if (!place.isSea)
                    gr.FillEllipse(Brushes.Green, places[i].position.X * width - 3, places[i].position.Y * height - 3, 6, 6);
                else gr.FillEllipse(Brushes.Blue, places[i].position.X * width - 3, places[i].position.Y * height - 3, 6, 6);
            }

            return map;
        }

        /// <summary>
        /// Рисует линии от юнита до тех мест куда он может сейчас сходить
        /// </summary>
        /// <param name="map"></param>
        /// <param name="game"></param>
        /// <param name="from"></param>
        /// <param name="who"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void DrawMoves(ref Bitmap map, Game game, Place from, Unit who, int width, int height)
        {
            int pos = from.units.FindIndex((unit) => unit == who);
            Pen pen = new Pen(Color.Wheat, 3);
            Graphics gr = Graphics.FromImage(map);
            List<Place> to = game.GetMoves(from, who);
            for(int i = 0;i < to.Count;i++)
            {
                gr.DrawLine(pen, from.position.X * width, from.position.Y * height + (pos + 1) * unit_size * 2, to[i].position.X * width, to[i].position.Y * height);
            }
        }
    }
}
