using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using System.IO;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace project101
{
    public partial class Form1 : Form
    {
        public Form1()
        { 
            InitializeComponent();
        }


        #region ImageCrop
        private void PicBox_MouseUp(object sender, MouseEventArgs e)
        {
            mouseClicked = false;

            if (endPoint.X != -1)
            {
                Point currentPoint = new Point(e.X, e.Y);
            }
            endPoint.X = -1;
            endPoint.Y = -1;
            startPoint.X = -1;
            startPoint.Y = -1;
        }

        private void PicBox_MouseDown(object sender, MouseEventArgs e)
        {
            mouseClicked = true;

            startPoint.X = e.X;
            startPoint.Y = e.Y;

            endPoint.X = -1;
            endPoint.Y = -1;

            rectCropArea = new Rectangle(new Point(e.X, e.Y), new Size());
        }

        private void PicBox_MouseMove(object sender, MouseEventArgs e)
        {
            Point ptCurrent = new Point(e.X, e.Y);

            if (mouseClicked)
            {
                endPoint = ptCurrent;

                if (e.X > startPoint.X && e.Y > startPoint.Y)
                {
                    rectCropArea.Width = e.X - startPoint.X;
                    rectCropArea.Height = e.Y - startPoint.Y;
                }
                else if (e.X < startPoint.X && e.Y > startPoint.Y)
                {
                    rectCropArea.Width = startPoint.X - e.X;
                    rectCropArea.Height = e.Y - startPoint.Y;
                    rectCropArea.X = e.X;
                    rectCropArea.Y = startPoint.Y;
                }
                else if (e.X > startPoint.X && e.Y < startPoint.Y)
                {
                    rectCropArea.Width = e.X - startPoint.X;
                    rectCropArea.Height = startPoint.Y - e.Y;
                    rectCropArea.X = startPoint.X;
                    rectCropArea.Y = e.Y;
                }
                else
                {
                    rectCropArea.Width = startPoint.X - e.X;
                    rectCropArea.Height = startPoint.Y - e.Y;
                    rectCropArea.X = e.X;
                    rectCropArea.Y = e.Y;
                }
                InputBox.Refresh();
            }
        }

        private void PicBox_Paint(object sender, PaintEventArgs e)
        {
            Pen drawLine = new Pen(Color.Red)
            {
                DashStyle = DashStyle.Dash
            };
            e.Graphics.DrawRectangle(drawLine, rectCropArea);
        }

        private void ROI_Click(object sender, EventArgs e)
        {
            mouseClicked = false;

            // This is needed so that it does not flicker on repaints
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }

        private void Crop_Click(object sender, EventArgs e)
        {
            OutputBox.Refresh();

            Bitmap sourceBitmap = new Bitmap(InputBox.Image, InputBox.Width, InputBox.Height);
            Graphics g = OutputBox.CreateGraphics();

            g.DrawImage(sourceBitmap, new Rectangle(0, 0, OutputBox.Width, OutputBox.Height), rectCropArea, GraphicsUnit.Pixel);
            sourceBitmap.Dispose();
        }

        private void InputBox_Click(object sender, EventArgs e)
        {
            InputBox.Refresh();
        }

        #endregion


        #region FormNavigation
        private void Close_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void Maximize_Click(object sender, EventArgs e)
        {
            if (WindowState.ToString() == "Normal")
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }
        }
        
        Point lastPoint;
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }
        #endregion
        

        #region VariableConstructor
        bool mouseClicked;
        Point startPoint = new Point();
        Point endPoint = new Point();
        Rectangle rectCropArea;

        Bitmap inputImg;

        BitmapToMatrix conv = new BitmapToMatrix();

        #endregion


        public void Browse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Image Files(*.bmp)|*.bmp"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //Open the browsed image and display it
                Bitmap original = new Bitmap(ofd.FileName);
                InputBox.Image = original;
                OutputBox.Image = original;
                //Display image file path  
                directory.Text = ofd.FileName;

                inputImg = original;
            }
        }
        
        private void Help_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.ShowDialog();
        }

        private void Details_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();
        }

        private void Run_Click(object sender, EventArgs e)
        {
            Run.Text = "Processing...";
            Run.Refresh();
            //Adaptive Median Filter
            byte[,] matrixInput = conv.BitmaptoMatrix(inputImg);
            AdaptiveMedianFilter adpMedian = new AdaptiveMedianFilter(matrixInput, 17);
            adpMedian.Filter();
            byte[,] matrixAmf = adpMedian.GetResult();
            Bitmap bitmapAmf = conv.getBMP(matrixAmf);
            OutputBox.Image = conv.getBMP(matrixAmf);
            OutputBox.Refresh(); InputBox.Refresh();

            //Bilateral Filter
            BilateralFilter bf = new BilateralFilter();
            Image<Gray, byte> grayBf = bf.bilateralFilter(new Image<Gray, byte>(bitmapAmf));
            OutputBox.Image = grayBf.Bitmap;
            OutputBox.Refresh(); InputBox.Refresh();

            //Active Contour




            //Feature Extraction
            //byte[,] matrixMasking;
            //byte[,] matrixNodule;

            //ExternalFeatures ext = new ExternalFeatures(matrixMasking);


            Run.Text = "Finished";
            Run.Refresh();
        }
    }
}
