using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chessing_UI
{
    public partial class Valhalla : UserControl
    {
        //Member Variables
        private static int _width = 8;
        private static int _height = 2;

        private int SeatLct_X = 0;
        private int SeatLct_Y = 0;

        private Color _Light;
        private Color _Shade;
        public Color LightColor {
            get { return this._Light; }
            set {
                _Light = value;
                using (Graphics g = Graphics.FromImage(BGI))
                {
                    g.Clear(_Light);
                    for (int y = 0; y < _height; y++)
                        for (int x = 0; x < _width; x++)
                        {
                            if ((x + y) % 2 == 1) g.FillRectangle(new SolidBrush(_Shade), x * Form1.Chess.chess_Size, y * Form1.Chess.chess_Size,
                                Form1.Chess.chess_Size, Form1.Chess.chess_Size);
                        }
                }
                pictureBox1.Image = BGI;
            }
        }
        public Color ShadeColor
        {
            get { return this._Shade; }
            set
            {
                _Shade = value;
                using (Graphics g = Graphics.FromImage(BGI))
                {
                    g.Clear(_Light);
                    for (int y = 0; y < _height; y++)
                        for (int x = 0; x < _width; x++)
                        {
                            if ((x + y) % 2 == 1) g.FillRectangle(new SolidBrush(_Shade), x * Form1.Chess.chess_Size, y * Form1.Chess.chess_Size,
                                Form1.Chess.chess_Size, Form1.Chess.chess_Size);
                        }
                }
                pictureBox1.Image = BGI;
            }
        }
        private Bitmap BGI = new Bitmap(_width * Form1.Chess.chess_Size, _height * Form1.Chess.chess_Size);
        private List<Form1.Chess> WarriorS = new List<Form1.Chess>();

        //Member Functions
        public Valhalla(){
            InitializeComponent();  
        }
        public void Seal(Form1.Chess Warrior) {
            Warrior.Parent = this.pictureBox1;
            Warrior.Location = new Point(Form1.Chess.chess_Size * SeatLct_X, Form1.Chess.chess_Size * SeatLct_Y);
            SeatLct_X++;
            if (SeatLct_X == 8) { SeatLct_X = 0; SeatLct_Y = 1; }
            WarriorS.Add(Warrior);
            Warrior.Enabled = false;
        }
        public void Release() {
            foreach (Form1.Chess Warrior in WarriorS) {
                Warrior.Enabled = true;
                Warrior.Parent = Form1.HintMap;
            }
            WarriorS.Clear();
            SeatLct_X = 0;
            SeatLct_Y = 0;
        }
    }
}
