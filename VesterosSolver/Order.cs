using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VesterosSolver
{
    public class Order
    {
        public OrderType type;
        public sbyte power = 0;
        public Place place = null;

        public override string ToString()
        {
            return type.ToString() + ":" + power;
        }
    }
}
