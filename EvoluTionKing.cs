using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chessing_UI
{
    public partial class EvoluTionKing : Form
    {
        public int choise = 0;

        public EvoluTionKing()
        {
            InitializeComponent();
        }

        private void EvolutionRock_Click(object sender, EventArgs e)
        {
            choise = 2; this.Close();
        }

        private void EvolutionKnight_Click(object sender, EventArgs e)
        {
            choise = 1; this.Close();
        }

        private void EvolutionBishop_Click(object sender, EventArgs e)
        {
            choise = 3; this.Close();
        }

        private void EvolutionQueen_Click(object sender, EventArgs e)
        {
            choise = 4; this.Close();
        }

    }
}
