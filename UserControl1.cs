using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

using static System.Math;


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
        double XminBase = -1, XmaxBase = 1, YminBase = -1, YmaxBase = 1, Xmouse = 0, Ymouse = 0;
        double Xm1 = 0, Xm2 = 0, Ym1 = 0, Ym2 = 0;
        double Xmin, Xmax, Ymin, Ymax;
        double Kx, Ky;
        double x, y;
        double XXX;
        int X1scr, Y1scr, X2scr, Y2scr;
        //double Yscr;
        double K = 1;

        bool PointFlag = false;
        int PointNum = 0;
        double[] PointX, PointY;
        SolidBrush PointBrush;
        public int PointType = 0, PointSize = 3;

        public void AddPoint(int N, double[] PointXtemp, double[] PointYtemp, Color cl)
        {
            PointFlag = true;
            PointX = new double[N];
            PointY = new double[N];
            PointBrush = new SolidBrush(cl);
            PointNum = N;
            for (int i = 0; i < N; i++)
            {
                PointX[i] = PointXtemp[i];
                PointY[i] = PointYtemp[i];
            }
            ReDrawPicture();
        }
        public void DeletePoint()
        {
            if (PointFlag)
            {
                PointFlag = false;
                PointBrush.Dispose();
                Array.Clear(PointX, 0, PointNum);
                Array.Clear(PointY, 0, PointNum);
                ReDrawPicture();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            trackBar1.Value = 0;
            trackBar2.Value = 0;
            K = 1;
            ReDrawPicture();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AddTextBox();
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DeleteTextBox();
        }

        public void SetBaseRectangle(double X, double Y, double L, double H)
        {
            XminBase = X;
            YminBase = Y - H;
            XmaxBase = X + L;
            YmaxBase = Y;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            
            Xm2 = e.X; Ym2 = e.Y;
            label1.Left = e.X + 10;
            label1.Top = e.Y + 10;
            x = (Xmin + e.X) / Kx;
            y = (-e.Y + BM.Height - Ky * Ymin) / Ky;
            //Yscr = BM.Height-Ky*(y-Ymin)
            //y*Ky = -Yscr + BM.H - Ky*Ymin
            label1.Text = string.Format("( {0:f2}, {1:f2})", x, y);
            if (e.Button == MouseButtons.Left)
            {

                //if (Xm1 < Xm2)
                //{
                //    Xmouse += -(pictureBox1.Left-e.X)*K / Kx/250;
                //}
                //if (Xm1 > Xm2)
                //{
                //    Xmouse += (pictureBox1.Left-e.X)*K / Kx/250;
                //}
                //if (Ym1 < Ym2)
                //{
                //    Ymouse += -(pictureBox1.Top-e.Y)*K / Ky/250;
                //}
                //if (Ym1 > Ym2)
                //{
                //    Ymouse += (pictureBox1.Top-e.Y)*K / Ky/250;
                //}
                Xmouse += Xm2 - Xm1;
                Ymouse += Ym2 - Ym1;
                

                ReDrawPicture();
            }
            
            Xm1 = e.X; Ym1 = e.Y;
        }
        //private void pictureBox1_MouseButtonMove(object sender, System.Windows.Input.MouseButtonEventArgs e)
        //{

        //}

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            label1.Visible = true;
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            
            label1.Visible = false;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (FlagGraph || PointFlag)
            {
                K *= Pow(1.1, e.Delta / 100);
                ReDrawPicture();
                label1.Text = string.Format("( {0:f2}, {1:f2})", x, y);
            }
        }

        

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            ReDrawPicture();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            ReDrawPicture();
        }

        private void UserControl1_Load(object sender, EventArgs e)
        {
            pictureBox1.Left = 0;
            pictureBox1.Top = 0;
            pictureBox1.Width = this.Width - 130;
            pictureBox1.Height = this.Height - 130;

            trackBar1.Left = 0;
            trackBar1.Top = pictureBox1.Height + 5;
            trackBar1.Width = pictureBox1.Width;
            trackBar1.Height = 20;

            trackBar2.Left = pictureBox1.Width + 5;
            trackBar2.Top = 0;
            trackBar2.Height = pictureBox1.Height;
            trackBar2.Width = 20;

            button1.Left = pictureBox1.Width + 5;
            button1.Top = pictureBox1.Height + 5;
            button1.Width = 40; button1.Height = 40;

            button2.Top = trackBar1.Top + trackBar1.Height;
            button2.Left = 0;
            button2.Width = 75; button2.Height = 25;

            button3.Top = trackBar1.Top + trackBar1.Height;
            button3.Left = button2.Width + 5;
            button3.Width = 75; button3.Height = 25;
        }

        private void UserControl1_Resize(object sender, EventArgs e)
        {
            UserControl1_Load(sender, e);
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
                for (int i = 0; i <= CellX; i++) Gr.DrawLine(penCell, (int)(i * BM.Width/CellX), 0,(int)(i*BM.Width/CellX),BM.Height);

                for (int i = 0; i <= CellY; i++) Gr.DrawLine(penCell, 0, (int)(i * BM.Height / CellY), BM.Width, (int)(i * BM.Height / CellY));
            }

            if (FlagGraph)
            {
                //Xscr = Kx*(x-Xmin)
                //Yscr = BM.Height-Ky*(y-Ymin)
                Xmin = (XminBase + XmaxBase) / 2 - ((XmaxBase - XminBase) / 2) * K*Pow(1.1, trackBar1.Value);
                Xmax = (XminBase + XmaxBase) / 2 + ((XmaxBase - XminBase) / 2) * K*Pow(1.1, trackBar1.Value);
                Ymin = (YminBase + YmaxBase) / 2 - ((YmaxBase - YminBase) / 2) * K*Pow(1.1, trackBar2.Value);
                Ymax = (YminBase + YmaxBase) / 2 + ((YmaxBase - YminBase) / 2) * K*Pow(1.1, trackBar2.Value);
                Kx = BM.Width / (Xmax - Xmin);
                Ky = BM.Height / (Ymax - Ymin);

                X1scr = 0;
                x = Xmin;
                XXX = 1;
                y = 0;
                for (int i = degree; i >= 0; i--)
                {
                    y += Coeff[i] * XXX;
                    XXX *= x;
                }
                
                Y1scr = (int)(BM.Height - Ky * (y - Ymin));
                //if (Yscr < 0) Y1scr = -1;
                //else Y1scr = (int)Yscr;

                //if (Yscr > BM.Height) Y1scr = BM.Height + 1;
                //else Y1scr = (int)Yscr;

                for (int j = 1; j <= BM.Width; j++)
                {
                    X2scr = j;
                    x = Xmin + X2scr / Kx;
                    XXX = 1;y = 0;
                    for (int i = degree; i >= 0; i--)
                    {
                        y += Coeff[i] * XXX;
                        XXX *= x;
                    }
                    Y2scr = (int)(BM.Height - Ky * (y - Ymin));
                    //if (Yscr < 0) Y2scr = -1;
                    //else Y2scr = (int)Yscr;
                    
                    //if (Yscr > BM.Height) Y2scr = BM.Height + 1;
                    //else Y2scr = (int)Yscr;

                    try
                    {
                        Gr.DrawLine(PenGraph, (int)(X1scr+Xmouse), (int)(Y1scr + Ymouse), (int)(X2scr + Xmouse), (int)(Y2scr + Ymouse));
                    }
                    catch (System.OverflowException) {
                        X1scr = X2scr;
                        Y1scr = Y2scr;
                    }
                    X1scr = X2scr;
                    Y1scr = Y2scr;
                }
            }
            if (PointFlag)
            {
                Xmin = (XminBase + XmaxBase) / 2 - ((XmaxBase - XminBase) / 2) * K * Pow(1.1, trackBar1.Value);
                Xmax = (XminBase + XmaxBase) / 2 + ((XmaxBase - XminBase) / 2) * K * Pow(1.1, trackBar1.Value);
                Ymin = (YminBase + YmaxBase) / 2 - ((YmaxBase - YminBase) / 2) * K * Pow(1.1, trackBar2.Value);
                Ymax = (YminBase + YmaxBase) / 2 + ((YmaxBase - YminBase) / 2) * K * Pow(1.1, trackBar2.Value);
                Kx = BM.Width / (Xmax - Xmin);
                Ky = BM.Height / (Ymax - Ymin);
                for (int i = 0; i < PointNum; i++)
                {
                    int Xscr = (int)(Kx * (PointX[i] - Xmin));
                    int Yscr = (int)(BM.Height - Ky * (PointY[i] - Ymin));
                    switch (PointType)
                    {
                        case 0:
                            Gr.FillEllipse(PointBrush, (int)(Xscr-PointSize+Xmouse), (int)(Yscr-PointSize+Ymouse), 2*PointSize, 2*PointSize);
                            break;
                        case 1:
                            Gr.FillRectangle(PointBrush, (int)(Xscr - PointSize + Xmouse), (int)(Yscr - PointSize + Ymouse), 2 * PointSize, 2 * PointSize);
                            break;
                        case 2:
                            Point[] Triangle = new Point[3];
                            Triangle[0].X = (int)(Xscr - PointSize + Xmouse);
                            Triangle[0].Y = (int)(Yscr + PointSize + Ymouse);
                            Triangle[1].X = (int)(Xscr + PointSize + Xmouse);
                            Triangle[1].Y = (int)(Yscr + PointSize + Ymouse);
                            Triangle[2].X = (int)(Xscr + Xmouse);
                            Triangle[2].Y = (int)(Yscr - PointSize + Ymouse);
                            Gr.FillPolygon(PointBrush, Triangle);
                            break;
                    }
                }
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
            if (flagCell)
            {
                flagCell = false;
                penCell.Dispose();
                ReDrawPicture();
            }
        }
        public void AddGraph(Color cl, int W)
        {
            
            degree = N - 1;
            Coeff = new double[N];
            for (int i = 0; i < N; i++)
            {
                Coeff[i] = double.Parse(textBox[i].Text);
            }
            PenGraph = new Pen(cl, W);
            FlagGraph = true;
            ReDrawPicture();
        }
        public void DeleteGraph()
        {
            FlagGraph = false;
            PenGraph.Dispose();
            Array.Clear(Coeff, 0, degree+1);
            ReDrawPicture();
        }






    }
}
