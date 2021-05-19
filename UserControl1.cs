using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lib1
{
    public partial class UserControl1: UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }
        Bitmap BM;
        Graphics Gr;
        int CellX=1, CellY=1;
        bool flagCell=false;
        Pen penCell;


        bool FlagGraph = false;
        Pen PenGraph;
        int degree; double[] Coeff;
        double XminBase = -1, XmaxBase = 1, YminBase = -1, YmaxBase = 1;
        double Xmin, Xmax, Ymin, Ymax;
        double Kx, Ky;
        double x, y;
        double XXX;
        int X1scr, Y1scr, X2scr, Y2scr;
       
        private void UserControl1_Load(object sender, EventArgs e)
        {
            pictureBox1.Left = 0;
            pictureBox1.Top = 0;
            pictureBox1.Width = this.Width - 50;
            pictureBox1.Height = this.Height - 50;


            trackBar1.Left = 0;
            trackBar1.Top = pictureBox1.Height + 5;
            trackBar1.Width = pictureBox1.Width;


            trackBar2.Left = pictureBox1.Width + 5;
            trackBar2.Top = 0;
            trackBar2.Height = pictureBox1.Height;
        }

        private void UserControl1_Resize(object sender, EventArgs e)
        {
            pictureBox1.Left = 0;
            pictureBox1.Top = 0;
            pictureBox1.Width = this.Width - 50;
            pictureBox1.Height = this.Height - 50;


            trackBar1.Left = 0;
            trackBar1.Top = pictureBox1.Height + 5;
            trackBar1.Width = pictureBox1.Width;


            trackBar2.Left = pictureBox1.Width + 5;
            trackBar2.Top = 0;
            trackBar2.Height = pictureBox1.Height;

        }

        public void InitGraph()
        {
            BM = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Gr = Graphics.FromImage(BM);
        }

        private void ReDrawPicture()
        {
            Gr.Clear(pictureBox1.BackColor);

            if(flagCell)
            {
                for (int i = 0; i <= CellX; i++) Gr.DrawLine(penCell, (int)(i * BM.Width/CellX), 0,(int)(i*BM.Width/CellX),BM.Height);/* */

                for (int i = 0; i <= CellY; i++) Gr.DrawLine(penCell, 0, (int)(i * BM.Height / CellY), BM.Width, (int)(i * BM.Height / CellY));
            }

            pictureBox1.Image = BM;
        }

        public void SetBackColor(Color cl)
        {
            pictureBox1.BackColor = cl;
        }

        public void AddCell(int Nx,int Ny,Color Cl,int w)
        {
            flagCell = true;
            CellX = Nx;
            CellY = Ny;
            penCell = new Pen(Cl,w);
            ReDrawPicture();
        }

        public void DelCell()
        {
            flagCell = false;
            penCell.Dispose();
            ReDrawPicture();
        }
        public void AddGraph(int N, double[] A, Color cl, int W)
        {
            degree = N;
            Coeff = new double[N + 1];
            for (int i = 0; i <= N; i++)
            {
                Coeff[i] = A[i];
            }
        }
        public void DeleteGraph()
        {

        }






    }
}
