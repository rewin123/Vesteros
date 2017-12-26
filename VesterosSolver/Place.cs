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

        public Place()
        {

        }


    }
}
