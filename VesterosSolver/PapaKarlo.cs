using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VesterosSolver
{
    public class PapaKarlo : Player
    {
        Random r = new Random();
        public double SecondsToThink = 1;
        public int depth = 100;
        public int lastGames = 0;
        public override int PlaceOrders(Game game, List<Order> orders)
        {
            int orderCount = 0;
            List<Place> places = game.GetPlacesForOrders(type);

            List<MonteKarloSum[]> orderResults = new List<MonteKarloSum[]>();
            for (int i = 0; i < places.Count; i++)
            {
                MonteKarloSum[] sums = new MonteKarloSum[orders.Count];
                for (int j = 0; j < orders.Count; j++)
                {
                    sums[j] = new MonteKarloSum();
                }
                orderResults.Add(sums);
            }

            lastGames = 0;
            DateTime start = DateTime.Now;
            
            while ((DateTime.Now - start).TotalSeconds < SecondsToThink)
            {
                Game today = game.CopyRandom();
                Player my_copy = today.players.Find((player) => player.type == type);
                List<Place> places_copy = today.GetPlacesForOrders(type);
                
                List<Order> orders_copy = new List<Order>();
                for(int i = 0;i < orders.Count;i++)
                {
                    orders_copy.Add(orders[i].CopyWithoutLand());
                }

                lastGames++;

                List<KeyValuePair<int, int>> positions = new List<KeyValuePair<int, int>>();
                today.Move();
                for(int i = 0;i < my_copy.orders.Count;i++)
                {
                    Order my_copy_order = my_copy.orders[i];
                    int order_index = orders.FindIndex((order) => order.ToString() == my_copy_order.ToString());

                    int landIndex = places.FindIndex((place) => place.name == my_copy_order.place.name);
                    positions.Add(new KeyValuePair<int, int>(order_index, landIndex));
                }

                

                for(int i = 0;i < depth;i++)
                {
                    today.Move();
                }

                int my_lands = 0;
                for(int i = 0;i < today.places.Count;i++)
                {
                    my_lands += today.places[i].powerType == type ? 1 : 0;
                }

                for (int i = 0; i < positions.Count; i++)
                {
                    orderResults[positions[i].Value][positions[i].Key].Add(my_lands);
                }
            }

            
            int placed_orders = 0;
            int placed_lands = 0;

            bool[] skip = new bool[orders.Count];
            bool[] skip_land = new bool[places.Count];

            while (placed_orders < orders.Count && placed_lands < places.Count)
            {
                float max = 0;
                int land_max = 0;
                int order_max = 0;
                for (int i = 0; i < places.Count; i++)
                {
                    if (skip_land[i])
                        continue;

                    for (int j = 0; j < orders.Count; j++)
                    {
                        if (skip[j])
                            continue;

                        float avr = orderResults[i][j].Medium;
                        if(!float.IsNaN(avr))
                        {
                            if(avr > max)
                            {
                                max = avr;
                                land_max = i;
                                order_max = j;
                            }
                        }
                    }
                }

                skip[order_max] = true;
                skip_land[land_max] = true;
                placed_lands++;
                placed_orders++;
                orderCount++;

                this.orders.Add(orders[order_max]);
                this.orders.Last().place = places[land_max];

                places[land_max].placed_order = this.orders.Last();
            }

            return orderCount;
        }

        public override int MakeOrder(Game game)
        {
            lastGames = 0;
            MonteKarloSum[] sums = new MonteKarloSum[orders.Count];
            for (int i = 0; i < sums.Length; i++)
                sums[i] = new MonteKarloSum();
            DateTime start = DateTime.Now;
            while ((DateTime.Now - start).TotalSeconds < SecondsToThink)
            {
                lastGames++;
                Game today = game.CopyRandom();
                RandomPlayer my_copy = (RandomPlayer)today.players.Find((p) => p.type == type);
                today.Move();
                int pos = my_copy.selectedOrderToMove;

                for(int i = 0;i < depth;i++)
                {
                    today.Move();
                }

                sums[pos].Add(CalcScore(ref today));
            }

            float max = 0;
            int indexMax = 0;
            for(int i = 0;i < orders.Count;i++)
            {
                float avr = sums[i].Medium;
                if(!float.IsNaN(avr))
                {
                    if(avr > max)
                    {
                        max = avr;
                        indexMax = i;
                    }
                }
            }

            game.MakeOrder(this, orders[indexMax].place);
            orders.RemoveAt(indexMax);

            return 1;
        }

        public override void MakeMove(Game game, Move move)
        {
           switch(move.playerState)
            {
                case PlayerState.AttackMove:
                    AttackMove(game, move);
                    break;
            }
        }

        void AttackMove(Game game, Move move)
        {
            if (move.active_units.Count == 0)
                return;
            List<Unit> units = move.active_units;
            int attackCount = r.Next(units.Count + 1);
            List<Place> places = game.GetMoves(move.active_place, units[0], false);
            while (units.Count > attackCount)
            {
                int pos = r.Next(units.Count);
                int land = r.Next(places.Count);
                places[land].units.Add(units[pos]);
                units.RemoveAt(pos);
            }

            if (attackCount > 0)
            {
                places = game.GetMoves(move.active_place, units[0], true);
                int land = r.Next(places.Count);
                game.Attack(places[land], units);
            }
        }

        int CalcScore(ref Game game)
        {
            int my_lands = 0;
            for (int i = 0; i < game.places.Count; i++)
            {
                my_lands += game.places[i].powerType == type ? 1 : 0;
            }

            return my_lands;
        }
    }
}
