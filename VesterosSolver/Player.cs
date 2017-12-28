using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VesterosSolver
{
    public class Player
    {
        public PlayerType type;
        public List<Order> orders = new List<Order>();
        
        public static Brush PlayerBrush(PlayerType type)
        {
            switch(type)
            {
                case PlayerType.Black:
                    return Brushes.Black;
                case PlayerType.Red:
                    return Brushes.Red;
                default:
                    return Brushes.Wheat;
            }
        }

        public virtual void MakeMove(Game game, Move move)
        {

        }

        /// <summary>
        /// Размещает приказы на поле
        /// </summary>
        /// <param name="game"></param>
        /// <param name="orders"></param>
        /// <returns></returns>
        public virtual int PlaceOrders(Game game, List<Order> orders)
        {
            return 0;
        }

        /// <summary>
        /// Выполняет приказ положенный на стол
        /// </summary>
        /// <param name="game"></param>
        /// <returns>Кол-во выполненный приказов</returns>
        public virtual int MakeOrder(Game game)
        {
            return 0;
        }
    }
}
