using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VesterosSolver
{
    public class GameException : Exception
    {
        public Game game;
        public Exception exception;
        public GameException(Game game, Exception exception, string message = "GameException") : base(message)
        {
            this.game = game;
            this.exception = exception;
        }
    }
}
