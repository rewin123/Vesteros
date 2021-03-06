﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VesterosSolver
{
     public class RandomPlayer : Player
    {
        public int selectedOrderToMove = 0;
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
                orders[order_index].place = places[place_index];

                this.orders.Add(orders[order_index]);
                places.RemoveAt(place_index);
                orders.RemoveAt(order_index);
                orderCount++;
            }

            return orderCount;
        }

        public override int MakeOrder(Game game)
        {
            if (orders.Count > 0)
            {
                int pos = r.Next(orders.Count);

                selectedOrderToMove = pos;

                orders[pos].place.placed_order = orders[pos];

                game.MakeOrder(this, orders[pos].place);

                orders.RemoveAt(pos);
                return 1;
            }
            else return 0;
        }

        public override void MakeMove(Game game, Move move)
        {
            switch(move.playerState)
            {
                case PlayerState.AttackMove:
                    AttackMove(game, move);
                    break;
                case PlayerState.RetreatMove:
                    RetreatMove(game, move);
                    break;
            }
        }

        /// <summary>
        /// Внутрифазовый ход аттаки
        /// </summary>
        /// <param name="game"></param>
        /// <param name="move"></param>
        void AttackMove(Game game, Move move)
        {
            if (move.active_units.Count == 0)
                return;
            List<Unit> units = move.active_units;
            int attackCount = r.Next(units.Count + 1);
            List<Place> places = game.GetMoves(move.active_place, units[0], false);
            while(units.Count > attackCount)
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

        void RetreatMove(Game game, Move move)
        {
            var units = move.active_units;
            var places = game.GetMoves(move.active_place, units[0], false);
            if(places.Count > 0)
            {
                places[r.Next(places.Count)].units.AddRange(units);
            }
        }
    }
}