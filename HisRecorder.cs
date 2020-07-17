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
    public partial class HisRecorder : UserControl
    {
        public List<String> Items = new List<string>();
        int SelectedIndex = -60229;
        public static int Std_Offset = 19;

        Bitmap BG_Bitmap_Odd = new Bitmap(300, Std_Offset * 10);
        Bitmap BG_Bitmap_Even = new Bitmap(300, Std_Offset * 10);
        Bitmap bufferBitmap;

        Color Light = Color.FromArgb(219, 192, 122);
        Color Shade = Color.FromArgb(181, 153, 82);
        Color Mark = Color.FromArgb(125, 115, 243);

        public HisRecorder()
        {
            InitializeComponent();
            WriteRegion.MouseWheel+= new MouseEventHandler(ScrolledOnList);

            using (Graphics g = Graphics.FromImage(BG_Bitmap_Odd))
            {
                for (int offset = 0; offset < Std_Offset * 10; offset += Std_Offset)
                    if (offset % (Std_Offset * 2) == 0) g.FillRectangle(new SolidBrush(Shade), 0, offset, 300, Std_Offset);
                    else g.FillRectangle(new SolidBrush(Light), 0, offset, 300, Std_Offset);
            }

            using (Graphics g = Graphics.FromImage(BG_Bitmap_Even))
            {
                for (int offset = 0; offset < Std_Offset * 10; offset += Std_Offset)
                    if (offset % (Std_Offset * 2) == 0) g.FillRectangle(new SolidBrush(Light), 0, offset, 300, Std_Offset);
                    else g.FillRectangle(new SolidBrush(Shade), 0, offset, 300, Std_Offset);
            }

            WriteRegion.Image = BG_Bitmap_Odd;
        }
        public void AddRecord(String _record)
        {

            Items.Add(_record);

            if (Items.Count > 10)
            {
               DrScroll.Enabled = true;
               DrScroll.Maximum++;
               DrScroll.Value = Items.Count;
            }
            else if (Items.Count == 1) WriteRegion.Text = _record;
            else
            {
                WriteRegion.Text += ("\n" + _record);
            }
        }
        private void ScrolledOnList(object sender, MouseEventArgs e)
        {
            if (!DrScroll.Enabled) return;
            int _delta = -(e.Delta / 120);

            if (DrScroll.Value == DrScroll.Minimum && _delta < 0) return;
            else if (DrScroll.Value == DrScroll.Maximum && _delta > 0) return;

            DrScroll.Value += _delta;
        }
        private void DrScroll_ValueChanged(object sender, EventArgs e)
        {
            WriteRegion.Text = Items.ElementAt(DrScroll.Value - 10);
            for (int i = DrScroll.Value - 9; i < DrScroll.Value; i++)
                WriteRegion.Text += ("\n" + Items.ElementAt(i));

            if (DrScroll.Value % 2 == 1) bufferBitmap = new Bitmap(BG_Bitmap_Odd);
            else bufferBitmap = new Bitmap(BG_Bitmap_Even);

            if (SelectedIndex < DrScroll.Value && (SelectedIndex > (DrScroll.Value - 11)))
            {
                using (Graphics g = Graphics.FromImage(bufferBitmap))
                {
                    g.FillRectangle(new SolidBrush(Mark), 0, (SelectedIndex + 10 - DrScroll.Value) * Std_Offset, 300, Std_Offset);
                }
            }

            WriteRegion.Image = bufferBitmap;
        }
        private void WriteRegion_MouseEnter(object sender, EventArgs e)
        {
            WriteRegion.Focus();
        }
        private void WriteRegion_MouseClick(object sender, MouseEventArgs e)
        {
            int TargetIndex = DrScroll.Value - 10 + (e.Y / Std_Offset);

            if (TargetIndex > Items.Count - 1) return;

            SelectedIndex = TargetIndex;

            if (DrScroll.Value % 2 == 1) bufferBitmap = new Bitmap(BG_Bitmap_Odd);
            else WriteRegion.Image = bufferBitmap = new Bitmap(BG_Bitmap_Even);

            using (Graphics g = Graphics.FromImage(bufferBitmap))
            {
                g.FillRectangle(new SolidBrush(Mark), 0, (TargetIndex + 10 - DrScroll.Value) * Std_Offset, 300, Std_Offset);
            }

            WriteRegion.Image = bufferBitmap;
        }
        public void Clear()
        {
            SelectedIndex = -60229;
            WriteRegion.Image = BG_Bitmap_Odd;
            DrScroll.Enabled = false;
            DrScroll.Maximum = 10;
            DrScroll.Value = 10;
            WriteRegion.Text = "";
            Items.Clear();
        }
        
    }
}
