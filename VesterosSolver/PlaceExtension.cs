using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VesterosSolver
{
    public static class PlaceExtension
    {
        public static void AddLink(this List<Place> places, int pos1, int pos2)
        {
            places[pos1].links.Add(places[pos2]);
            places[pos2].links.Add(places[pos1]);
        }

        public static void AddLink(this List<Place> places, string name1, string name2)
        {
            int pos1 = places.FindIndex((pl) => pl.name == name1);
            int pos2 = places.FindIndex((pl) => pl.name == name2);

            places.AddLink(pos1, pos2);
        }

        public static void Unmark(this List<Place> places)
        {
            for(int i = 0;i < places.Count;i++)
            {
                places[i].mark = 0;
            }
        }
    }
}
