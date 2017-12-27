using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VesterosSolver
{
    public class Move
    {
        public Player player;
        public PlayerState playerState = PlayerState.PlaceOrders;
        public List<Unit> active_units = new List<Unit>();
        public Place active_place = null;
    }
}
