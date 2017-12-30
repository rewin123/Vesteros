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

        public OrderData CopyUnlinked()
        {
            OrderData data = new OrderData
            {
                type = type,
                power = power,
                place = place.name
            };

            return data;
        }

        public Order CopyWithoutLand()
        {
            return new Order
            {
                type = type,
                power = power
            };
        }
    }
    
    
    public class OrderData
    {
        public OrderType type;
        public sbyte power = 0;
        public string place;
    }
}
