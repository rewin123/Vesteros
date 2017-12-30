using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace VesterosSolver
{
    public class Place
    {
        public string name = "";
        public Vector2 position = new Vector2();
        public List<Place> links = new List<Place>();
        public List<string> names = new List<string>();
        public bool isSea = false;
        public int castleLevel = 0;
        public int barrelCount = 0;
        public int powerCount = 0;
        public List<Unit> units = new List<Unit>();
        public Order placed_order = null;
        public PlayerType powerType = PlayerType.None;

        public int mark = 0;

        public Place()
        {

        }

        public override string ToString()
        {
            return name + ":" + (isSea ? "sea" : "land");
        }

        public Place CopyUnlinked()
        {
            Place place = new Place();
            place.mark = mark;
            place.barrelCount = barrelCount;
            place.castleLevel = castleLevel;
            place.isSea = isSea;
            place.name = name;
            place.position = new Vector2(position.X, position.Y);
            place.powerCount = powerCount;
            place.powerType = powerType;
            place.names = Links();
            
            if(placed_order != null)
            {
                place.placed_order = placed_order.CopyWithoutLand();
                place.placed_order.place = place;
            }

            for(int i = 0;i < units.Count;i++)
            {
                place.units.Add(units[i].Copy());
            }

            return place;
        }

        public List<string> Links()
        {
            List<string> names = new List<string>();
            for(int i = 0;i < links.Count;i++)
            {
                names.Add(links[i].name);
            }

            return names;
        }
    }
}
