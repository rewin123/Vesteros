using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VesterosSolver
{
    public class Player
    {
        public PlayerType type;
        public List<Order> orders = new List<Order>();
        

        public virtual void MakeMove(Move move)
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

        public virtual int MakeOrder(Game game)
        {
            return 0;
        }
    }
}
