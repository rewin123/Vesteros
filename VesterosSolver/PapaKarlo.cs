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
        public float LastMaxScore = 0;
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
                try
                {
                    RandomPlayer my_copy = (RandomPlayer)today.players.Find((p) => p.type == type);
                    today.Move();
                    int pos = my_copy.selectedOrderToMove;

                    for (int i = 0; i < depth; i++)
                    {
                        today.Move();
                    }

                    sums[pos].Add(CalcScore(ref today));
                }
                catch(Exception excp)
                {
                    throw new GameException(today, excp);
                }
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
            LastMaxScore = max;
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

        void AttackMove(Game my_game, Move my_move)
        {
            //Для начала найдем куда атаковать и кем
            List<Unit> my_units = my_move.active_units;
            List<Place> my_places = my_game.GetMoves(my_move.active_place, my_units[0], true);
            MonteKarloSum[] place_sums = new MonteKarloSum[my_places.Count];
            MonteKarloSum[] unit_sums = new MonteKarloSum[my_units.Count];
            MonteKarloSum[] count_sum = new MonteKarloSum[my_units.Count + 1];
            bool[] attack_switch = new bool[my_units.Count];
            int[] land_moves = new int[my_units.Count];

            List<Place> my_free_place = my_game.GetMoves(my_move.active_place, my_units[0], false);
            MonteKarloSum[,] free_sum = new MonteKarloSum[my_units.Count, my_free_place.Count];

            //Инцилизируем массивы MonteKarloSum
            for (int i = 0; i < place_sums.Length; i++)
                place_sums[i] = new MonteKarloSum();

            for (int i = 0; i < count_sum.Length; i++)
                count_sum[i] = new MonteKarloSum();

            for (int i = 0; i < unit_sums.Length; i++)
                unit_sums[i] = new MonteKarloSum();

            for (int x = 0; x < my_units.Count; x++)
                for (int y = 0; y < my_free_place.Count; y++)
                    free_sum[x, y] = new MonteKarloSum();

            DateTime start = DateTime.Now;
            while ((DateTime.Now - start).TotalSeconds < SecondsToThink / 2)
            {
                for(int i = 0;i < attack_switch.Length;i++)
                {
                    attack_switch[i] = false;
                }

                Game game = my_game.CopyRandom();
                Move move = game.inMoveActions[0];

                game.SkipMove(0);

                if (move.active_units.Count == 0)
                    return;
                List<Unit> units = move.active_units;
                int attackCount = r.Next(units.Count + 1);
                List<Place> places = game.GetMoves(move.active_place, units[0], true);
                int rand_attack_place = r.Next(places.Count);
                List<Unit> attack_unit = new List<Unit>();
                for(int i = 0;i < attackCount;i++)
                {
                    int pos = r.Next(units.Count);
                    while(attack_switch[pos])
                    {
                        pos = r.Next(units.Count);
                    }

                    attack_unit.Add(units[pos]);
                    attack_switch[pos] = true;
                }
                if(attackCount > 0)
                    game.Attack(places[rand_attack_place], attack_unit);

                places = game.GetMoves(move.active_place, units[0], false);
                for (int i = 0;i < units.Count;i++)
                {
                    if(!attack_switch[i])
                    {
                        int pos = r.Next(places.Count);
                        land_moves[i] = pos;
                        places[pos].units.Add(units[i]);
                    }
                }

                for(int i = 0;i < depth;i++)
                {
                    game.Move();
                }

                int res = CalcScore(ref game);
                place_sums[rand_attack_place].Add(res);
                count_sum[attackCount].Add(res);
                for (int i = 0; i < units.Count;i++)
                {
                    if(attack_switch[i])
                    {
                        unit_sums[i].Add(res);
                    }
                    else
                    {
                        free_sum[i, land_moves[i]].Add(res);
                    }
                }
            }

            float max = 0;
            int maxCount = 0;
            for(int i = 0;i < count_sum.Length;i++)
            {
                if(count_sum[i].Medium > max)
                {
                    max = count_sum[i].Medium;
                    maxCount = i;
                }
            }

            bool[] used = new bool[my_units.Count];

            if(maxCount != 0)
            {
                List<Unit> attack_units = new List<Unit>();
                for(int i = 0;i < maxCount;i++)
                {
                    max = 0;
                    int maxIndex = 0;
                    for(int u = 0;u < my_units.Count;u++)
                    {
                        if (!used[u])
                        {
                            if (unit_sums[u].Medium > max)
                            {
                                max = unit_sums[0].Medium;
                                maxIndex = u;
                            }
                        }
                    }

                    attack_units.Add(my_units[maxIndex]);
                    used[maxIndex] = true;
                }

                max = 0;
                int maxLand = 0;
                for(int l = 0;l < my_places.Count;l++)
                {
                    if(place_sums[l].Medium > max)
                    {
                        max = place_sums[l].Medium;
                        maxLand = l;
                    }
                }

                my_game.Attack(my_places[maxLand], attack_units);
            }

            for(int i = 0;i < my_units.Count;i++)
            {
                if(!used[i])
                {
                    max = 0;
                    int maxLand = 0;
                    for (int l = 0; l < my_free_place.Count; l++)
                    {
                        if (free_sum[i,l].Medium > max)
                        {
                            max = free_sum[i,l].Medium;
                            maxLand = l;
                        }
                    }

                    my_free_place[maxLand].units.Add(my_units[i]);
                }
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
