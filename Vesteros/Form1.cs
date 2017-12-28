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
        List<Func<bool, Bitmap>> unit_tests = new List<Func<bool, Bitmap>>();
        List<Action> speed_test = new List<Action>();
        Game game = new Game();
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

        private void button2_Click(object sender, EventArgs e)
        {
            game = new Game();
            game.players.Add(new RandomPlayer());
            game.places.Add(new Place
            {
                position = new Vector2(0.1f, 0.5f),
                name = "1",
            });

            game.places.Add(new Place
            {
                position = new Vector2(0.9f, 0.5f),
                name = "2"
            });

            game.places.Add(new Place
            {
                position = new Vector2(0.5f, 0.3f),
                name = "3"
            });

            game.places.Add(new Place
            {
                position = new Vector2(0.1f, 0.3f),
                name = "sea",
                isSea = true
            });

            game.places.AddLink("1", "2");
            game.places.AddLink("1", "sea");
            game.places.AddLink("3", "sea");

            game.places[0].units.Add(new Unit
            {
                
            });

            game.places[0].units.Add(new Unit
            {

            });

            game.places[3].units.Add(new Unit
            {
                type = UnitType.Boat
            });


            Bitmap map = VisualDebug.DrawPlaces(game.places, pictureBox1.Width, pictureBox1.Height);
            VisualDebug.DrawMoves(ref map, game, game.places[0], game.places[0].units[0], map.Width, map.Height);
            pictureBox1.Image = map;

            DateTime start = DateTime.Now;
            int count = 0;
            while((DateTime.Now - start).TotalSeconds < 1)
            {
                count++;
                game.GetMoves(game.places[0], game.places[0].units[0]);
            }

            listBox1.Items.Insert(0, count + ":" + 1.0f/count);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            game.Move();
            pictureBox1.Image = VisualDebug.DrawPlaces(game.places, pictureBox1.Width, pictureBox1.Height);
            listBox1.Items.Insert(0, game.gamePhase);
        }
    }
}
