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
        public bool isSea = false;
        public int castleLevel = 0;
        public int barrelCount = 0;
        public int powerCount = 0;

        public Place()
        {

        }


    }
}
