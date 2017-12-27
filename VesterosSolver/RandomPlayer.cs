using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VesterosSolver
{
    class RandomPlayer : Player
    {
        Random r = new Random();
        public override int PlaceOrders(Game game, List<Order> orders)
        {
            int orderCount = 0;
            List<Place> places = new List<Place>();
            places.AddRange(game.places);
            for(int i = 0;i < places.Count;i++)
            {
                Place place = places[i];
                if(place.units.Count != 0)
                {
                    if(place.units[0].player == type)
                    {
                        continue;
                    }
                }

                places.RemoveAt(i);
                i--;
            }

            while(orders.Count > 0 && places.Count > 0)
            {
                int place_index = r.Next(places.Count);
                int order_index = r.Next(orders.Count);

                places[place_index].placed_order = orders[order_index];
                this.orders.Add(orders[order_index]);
                places.RemoveAt(place_index);
                orders.RemoveAt(order_index);
                orderCount++;
            }

            return orderCount;
        }
    }
}
