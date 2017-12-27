using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace VesterosSolver
{
    public class Unit
    {
        public UnitType type = UnitType.Soldat;
        public bool isLying = false;
        public PlayerType player = PlayerType.Black;

        public void Draw(Graphics gr, float x, float y, int size)
        {
            Brush brush = Brushes.White;
            
            switch(type)
            {
                case UnitType.Soldat:
                    gr.FillEllipse(brush, x - size, y - size, size * 2, size * 2);
                    break;
                case UnitType.Boat:
                    gr.FillRectangle(brush, x - size, y - size / 2, size * 2, size);
                    break;
            }
        }

        
    }
}
