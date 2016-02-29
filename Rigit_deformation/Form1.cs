using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Rigit_deformation.Триангуляция_и_контур;

namespace Rigit_deformation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }

        private void открытьКартинкуToolStripMenuItem_Click(object sender, EventArgs e)
        {   Graphics gBitmap = pictureBox1.CreateGraphics();
            gBitmap.Clear(Color.White);

            string fileName;

            this.openFileDialog1.Filter = "Image | *.png";
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fileName = this.openFileDialog1.FileName;

                Bitmap myBitmap = new Bitmap(fileName);

                //pictureBox1.Image = Image.FromFile(fileName);
                Border border = new Border(myBitmap);
                
                gBitmap.DrawImage(Image.FromFile(fileName), 0, 0);
                border.DrawExactBorder(gBitmap);
                border.DrawShortCutBorder(gBitmap);
                //pictureBox1.Invalidate();
            }
        }
    }
}
