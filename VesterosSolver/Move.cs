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
        public int modifier = 0;

        public override string ToString()
        {
            return playerState + ":" + player.type;
        }

        public MoveData CopyUnlinked()
        {
            return new MoveData(this);
        }
    }

    public class MoveData
    {
        public PlayerType player;
        public List<Unit> active_units;
        public string active_place;
        public int modifier;
        public PlayerState playerState;

        public MoveData(Move m)
        {
            player = m.player.type;
            active_place = m.active_place.name;
            modifier = m.modifier;
            playerState = m.playerState;

            active_units = new List<Unit>();
            for(int i = 0;i < m.active_units.Count;i++)
            {
                active_units.Add(m.active_units[i].Copy());
            }
        }
    }
}
