using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VesterosSolver;

namespace Vesteros
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Game game = VesterosBuilder.VesterosMap();
            var places = game.places;

            pictureBox1.Image = VisualDebug.DrawPlaces(places, pictureBox1.Width, pictureBox1.Height);
        }
    }
}
