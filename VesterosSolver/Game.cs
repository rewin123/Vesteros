﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VesterosSolver
{
    public class Game
    {
        public List<Place> places = new List<Place>();
        public List<Player> players = new List<Player>();

        int moveIndex = 0;
        List<Move> inMoveActions = new List<Move>();
        public GamePhase gamePhase = GamePhase.PlaceOrders;
        int orderCount = 0;
        
        /// <summary>
        /// Выполняет пошаговый расчет мира
        /// </summary>
        public void Move()
        {
            if(inMoveActions.Count == 0)
            {
                if(gamePhase == GamePhase.PlaceOrders)
                {
                    orderCount += players[moveIndex].PlaceOrders(this, StandartOrders);
                    moveIndex++;
                    if(moveIndex == players.Count)
                    {
                        moveIndex = 0;
                        gamePhase = GamePhase.MakeOrders;
                    }
                }
                else
                {
                    orderCount -= players[moveIndex].MakeOrder(this);
                    moveIndex++;
                    if (moveIndex == players.Count)
                        moveIndex = 0;

                    if (orderCount == 0)
                        gamePhase = GamePhase.PlaceOrders;
                }
            }
            else
            {
                inMoveActions[0].player.MakeMove(this, inMoveActions[0]);
                inMoveActions.RemoveAt(0);
            }
        }

        public void MakeOrder(Player who, Place place)
        {
            var order = place.placed_order;
            place.placed_order = null;
            
            switch(order.type)
            {
                case OrderType.Attack:
                    List<Unit> units = GetToActive(place);
                    Move m = new Move
                    {
                        active_units = units,
                        active_place = place,
                        playerState = PlayerState.AttackMove,
                        player = who,
                        modifier = order.power
                    };
                    
                    inMoveActions.Add(m);
                    break;
            }
        }

        

        /// <summary>
        /// Забирает юниты из области
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public List<Unit> GetToActive(Place from)
        {
            List<Unit> units = new List<Unit>();
            units.AddRange(from.units);
            from.units.Clear();
            return units;
        }

        /// <summary>
        /// Производит ход атаки юнитами на область
        /// </summary>
        /// <param name="to"></param>
        /// <param name="units"></param>
        public void Attack(Place to, List<Unit> units)
        {
            if (to.units.Count == 0)
            {
                to.units.AddRange(units);
            }
            else if (to.units[0].player == units[0].player)
            {
                to.units.AddRange(units);
            }
            else
            {
                int attack_power = 0;
                int defence_power = 0;

                int castle_attribute = to.castleLevel > 0 ? 4 : 0;

                for (int i = 0; i < units.Count; i++)
                {
                    switch (units[i].type)
                    {
                        case UnitType.Horse:
                            attack_power += 2;
                            break;
                        case UnitType.Siege:
                            attack_power += castle_attribute;
                            break;
                        default:
                            attack_power += 1;
                            break;
                    }
                }

                List<Unit> defended_unit = to.units;
                for (int i = 0; i < defended_unit.Count; i++)
                {
                    if (!defended_unit[i].isLying)
                    {
                        switch (defended_unit[i].type)
                        {
                            case UnitType.Horse:
                                defence_power += 2;
                                break;
                            case UnitType.Siege:
                                defence_power += castle_attribute;
                                break;
                            default:
                                defence_power += 1;
                                break;
                        }
                    }
                }

                if (attack_power > defence_power)
                {
                    for (int i = 0; i < to.units.Count; i++)
                    {
                        if (to.units[i].isLying)
                        {
                            to.units.RemoveAt(i);
                            i--;
                        }
                        else to.units[i].isLying = true;
                    }

                    List<Unit> active = GetToActive(to);
                    if (active.Count > 0)
                    {
                        PlayerType p_type = active[0].player;
                        int p_index = players.FindIndex((player) => player.type == p_type);
                        Player p = players[p_index];
                        Move move = new Move
                        {
                            player = p,
                            playerState = PlayerState.RetreatMove,
                            active_units = active,
                            active_place = to
                        };

                        inMoveActions.Add(move);
                    }

                    to.units = units;
                }
                else
                {
                    for (int i = 0; i < units.Count; i++)
                        units[i].isLying = true;
                    PlayerType p_type = units[0].player;
                    Player p = players.Find((player) => player.type == p_type);

                    Move move = new Move
                    {
                        player = p,
                        playerState = PlayerState.RetreatMove,
                        active_units = units,
                        active_place = to
                    };

                    inMoveActions.Add(move);
                }
            }
        }

        /// <summary>
        /// Фозращает возможные движения юнита из этого места
        /// </summary>
        /// <param name="place"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public List<Place> GetMoves(Place place, Unit unit, bool attack = true)
        {
            if (unit.type == UnitType.Boat)
                return GetBoatMoves(place, unit, attack);
            else
            {
                return GetGroundMoves(place, unit, attack);
            }

        }

        /// <summary>
        /// Возращает возможные перемещения морского юнита
        /// </summary>
        /// <param name="place"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        List<Place> GetBoatMoves(Place place, Unit unit, bool attack)
        {
            List<Place> moves = new List<Place>();

            if (attack)
                moves.Add(place);
            else if (place.units.Count == 0)
                moves.Add(place);
            else if (place.units[0].player == unit.player)
                moves.Add(place);

            if (attack)
            {
                for (int i = 0; i < place.links.Count; i++)
                {
                    if (place.links[i].isSea == true)
                    {
                        moves.Add(place.links[i]);

                    }
                }
            }
            else
            {
                for (int i = 0; i < place.links.Count; i++)
                {
                    if (place.links[i].isSea == true)
                    {
                        if (place.links[i].units.Count == 0)
                            moves.Add(place.links[i]);
                        else if (place.links[i].units[0].player == unit.player)
                            moves.Add(place.links[i]);
                    }
                }
            }
            return moves;
        }

        /// <summary>
        /// Возращает возможные перемещения пешего юнита
        /// </summary>
        /// <param name="place"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        List<Place> GetGroundMoves(Place place, Unit unit, bool attack)
        {
            places.Unmark();
            place.mark = 1;
            List<Place> moves = new List<Place>();

            if(attack)
                moves.Add(place);
            else if (place.units.Count == 0)
                moves.Add(place);
            else if (place.units[0].player == unit.player)
                moves.Add(place);

            bool boat = unit.type == UnitType.Boat;
            for (int i = 0; i < place.links.Count; i++)
            {
                if (place.links[i].isSea)
                {
                    if (place.links[i].units.Count > 0)
                    {
                        if (place.links[i].units[0].player == unit.player)
                        {
                            moves.AddRange(RecursionSeaPath(place.links[i], unit.player, attack));
                        }
                    }
                }
                else
                {
                    if(attack)
                        moves.Add(place.links[i]);
                    else if (place.links[i].units.Count == 0)
                        moves.Add(place.links[i]);
                    else if(place.links[i].units[0].player == unit.player)
                        moves.Add(place.links[i]);

                }
            }

            return moves;
        }

        /// <summary>
        /// Передает все связанные земли с этим морем для переброски пеших войск
        /// </summary>
        /// <param name="place"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        List<Place> RecursionSeaPath(Place place, PlayerType player, bool attack)
        {
            List<Place> tos = new List<Place>();
            place.mark = 1;
            for(int i = 0;i < place.links.Count;i++)
            {
                if(place.links[i].mark == 0)
                {
                    if(place.links[i].isSea)
                    {
                        if (place.links[i].units.Count > 0)
                        {
                            if (place.links[i].units[0].player == player)
                                tos.AddRange(RecursionSeaPath(place.links[i], player, attack));
                        }
                    }
                    else
                    {
                        place.links[i].mark = 1;
                        if (!attack)
                        {
                            tos.Add(place.links[i]);
                        }
                        else
                        {
                            if(place.links[i].units.Count == 0)
                                tos.Add(place.links[i]);
                            else if(place.links[i].units[0].player == player)
                                tos.Add(place.links[i]);
                        }
                    }
                }
            }

            return tos;
        }

        public List<Order> StandartOrders
        {
            get
            {
                List<Order> orders = new List<Order>();
                orders.Add(new Order
                {
                    type = OrderType.Attack
                });

                orders.Add(new Order
                {
                    type = OrderType.Attack,
                    power = -1
                });

                return orders;
            }
        }
    }
}
