using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Text;
using System.IO;
using Emgu.CV;
using Emgu.CV.Structure;

namespace project101
{
    public partial class Form1 : Form
    {        
        public Form1()
        { 
            InitializeComponent();
        }

        #region VariableConstructor
        bool mouseClicked;
        Point startPoint = new Point();
        Point endPoint = new Point();
        Rectangle rectCropArea;

        Bitmap inputImg;
        Bitmap cropped;

        double persentase;
        string marginCls;
        string shapeCls;
        string oriCls;
        string echoCls;
        string compCls;

        BitmapToMatrix conv = new BitmapToMatrix();
        Form2 form2 = new Form2();
        #endregion


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
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }

        private void Crop_Click(object sender, EventArgs e)
        {
            cropped = new Bitmap(OutputBox.Width, OutputBox.Height);
            using (Bitmap sourceBitmap = new Bitmap(InputBox.Image, InputBox.Width, InputBox.Height))
            {
                using (Graphics g = Graphics.FromImage(cropped))
                {
                    g.DrawImage(sourceBitmap, new Rectangle(0, 0, OutputBox.Width, OutputBox.Height), rectCropArea, GraphicsUnit.Pixel);
                }
            }
            if (OutputBox.Image != null) OutputBox.Image.Dispose();
            OutputBox.Image = cropped;

            OutputBox.Refresh();
        }

        private void InputBox_MouseClick(object sender, EventArgs e)
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
            if (marginCls != null)
            {
                form2.marginValue = marginCls.ToString();
                form2.shapeValue = shapeCls.ToString();
                form2.orientationValue = oriCls.ToString();
                form2.echogenityValue = echoCls.ToString();
                form2.compositionValue = compCls.ToString();
            }
            form2.ShowDialog();
        }

        private void Run_Click(object sender, EventArgs e)
        {
            if(inputImg != null)
            {
                Run.Text = "Pra-pengolahan...";
                Run.TextAlign = ContentAlignment.MiddleCenter;
                Run.Refresh();

                #region Pre-processing
                if (cropped != null) { inputImg = cropped; }
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
                InputBox.Image = bitmapAmf;
                OutputBox.Refresh(); InputBox.Refresh();
                #endregion

                Run.Text = " Ekstraksi Fitur...";
                Run.TextAlign = ContentAlignment.MiddleCenter;
                Run.Refresh();

                #region Feature Extraction
                //Input Internal
                Bitmap bitmapNodule = inputImg;
                MessageBox.Show("Tolong masukkan citra keabuan nodul", "Ekstraksi Fitur Internal", MessageBoxButtons.OK);
                OpenFileDialog ofd = new OpenFileDialog()
                {
                    Filter = "Image Files(*.bmp)|*.bmp"
                };

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    //Open the browsed image and display it
                    Bitmap original = new Bitmap(ofd.FileName);
                    InputBox.Image = original;
                    InputBox.Refresh();

                    bitmapNodule = original;
                }

                byte[,] matrixNodule = conv.BitmaptoMatrix(bitmapNodule);
                InternalFeatures intn = new InternalFeatures(matrixNodule);

                //EKOGENITAS
                double[] histogramFtr = intn.FiturHistogram();
                double[][] lawsFtr = intn.FiturLaws();

                string Echofile = "GetEchogenity.csv";
                using (StreamWriter sw = new StreamWriter(new FileStream(Echofile, FileMode.Append), Encoding.UTF8))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat(string.Format("mu, deviasi, skewness, energi, entropi, smoothness, varians_n, kurtosis, mu1, v1, mu2, v2, mu3, v3, mu4, v4, mu5, v5, mu6, v6, mu7, v7, mu8, v8, mu9, v9, Class"));
                    sb.Clear();
                    sb.AppendLine(string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8} ,{9}, {10}, {11}, {12}, {13}, {14}, {15}, {16}, {17}, {18}, {19}, {20}, {21}, {22}, {23}, {24}, {25}, {26}",
                                                histogramFtr[0], histogramFtr[1], histogramFtr[2], histogramFtr[3], histogramFtr[4], histogramFtr[5], histogramFtr[6], histogramFtr[7],
                                                lawsFtr[0][0], lawsFtr[0][1],
                                                lawsFtr[1][0], lawsFtr[1][1],
                                                lawsFtr[2][0], lawsFtr[2][1],
                                                lawsFtr[3][0], lawsFtr[3][1],
                                                lawsFtr[4][0], lawsFtr[4][1],
                                                lawsFtr[5][0], lawsFtr[5][1],
                                                lawsFtr[6][0], lawsFtr[6][1],
                                                lawsFtr[7][0], lawsFtr[7][1],
                                                lawsFtr[8][0], lawsFtr[8][1], "e"));
                    sw.WriteAsync(sb.ToString());
                }

                //KOMPOSISI
                double[] glrlm = intn.GLRLM();
                double[][] glcm = intn.GLCM();

                string Compfile = "GetComposition.csv";
                using (StreamWriter sw = new StreamWriter(new FileStream(Compfile, FileMode.Append), Encoding.UTF8))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat(string.Format("mu, deviasi, skewness, energi, entropi, smoothness, varians_n, kurtosis, SRE, LRE, RLN, RP, GLN, LGRE, HGRE, asm0, kontras0, idm0, entropi0, korelasi0, asm45, kontras45, idm45, entropi45, korelasi45, asm90, kontras90, idm90, entropi90, korelasi90, asm135, kontras135, idm135, entropi135, korelasi135"));
                    sb.Clear();
                    sb.AppendLine(string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}, {16}, {17}, {18}, {19}, {20}, {21}, {22}, {23}, {24}, {25}, {26}, {27}, {28}, {29}, {30}, {31}, {32}, {33}, {34}, {35}",
                                                histogramFtr[0], histogramFtr[1], histogramFtr[2], histogramFtr[3], histogramFtr[4], histogramFtr[5], histogramFtr[6], histogramFtr[7],
                                                glrlm[0], glrlm[1], glrlm[2], glrlm[3], glrlm[4], glrlm[5], glrlm[6],
                                                glcm[0][0], glcm[0][1], glcm[0][2], glcm[0][3], glcm[0][4],
                                                glcm[1][0], glcm[1][1], glcm[1][2], glcm[1][3], glcm[1][4],
                                                glcm[2][0], glcm[2][1], glcm[2][2], glcm[2][3], glcm[2][4],
                                                glcm[3][0], glcm[3][1], glcm[3][2], glcm[3][3], glcm[3][4], "e"));
                    sw.WriteAsync(sb.ToString());
                }


                //Input Eksternal
                Bitmap bitmapMasking = inputImg;
                MessageBox.Show("Tolong masukkan citra masking nodul tiroid", "Ekstraksi Fitur Eksternal", MessageBoxButtons.OK);
                OpenFileDialog ofd2 = new OpenFileDialog()
                {
                    Filter = "Image Files(*.bmp)|*.bmp"
                };

                if (ofd2.ShowDialog() == DialogResult.OK)
                {
                    //Open the browsed image and display it
                    Bitmap original = new Bitmap(ofd2.FileName);
                    InputBox.Image = original;
                    InputBox.Refresh();

                    bitmapMasking = original;
                }

                byte[,] matrixMasking = conv.BitmaptoMatrix(bitmapMasking);
                ExternalFeatures ext = new ExternalFeatures(matrixMasking);

                //MARGIN
                double[] MarginFeature = ext.getMargin();

                string marginFile = "GetMargin.csv";
                using (StreamWriter sw = new StreamWriter(new FileStream(marginFile, FileMode.Append), Encoding.UTF8))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat(string.Format("Ratio HW, Rectangularity, Dispersy, Compactness, Circularity, Convexity, Tortuosity, Aspect Ratio, Chain Code, Class"));
                    sb.Clear();
                    sb.AppendLine(string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}",
                                                MarginFeature[0], MarginFeature[1], MarginFeature[2], MarginFeature[3], MarginFeature[4],
                                                MarginFeature[5], MarginFeature[6], MarginFeature[7], MarginFeature[8], "e"));
                    sw.WriteAsync(sb.ToString());
                }

                //SHAPE AND ORIENTATION
                double[] ShapeFeature = ext.getShape();

                string shapeFile = "GetOrientation.csv";
                using (StreamWriter sw = new StreamWriter(new FileStream(shapeFile, FileMode.Append), Encoding.UTF8))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat(string.Format("Ratio HW, Rectangularity, Dispersy, Compactness, Circularity, Convexity, Tortuosity, hu1, hu2, hu3, hu4, hu5, hu6, hu7, zer1, zer2, zer3, zer4, zer5, zer6, zer7, Class"));
                    sb.Clear();
                    sb.AppendLine(string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8} ,{9}, {10}, {11}, {12}, {13}, {14}, {15}, {16}, {17}, {18}, {19}, {20}, {21}",
                                               ShapeFeature[0], ShapeFeature[1], ShapeFeature[2], ShapeFeature[3], ShapeFeature[4], ShapeFeature[5], ShapeFeature[6],
                                                ShapeFeature[7], ShapeFeature[8], ShapeFeature[9], ShapeFeature[10], ShapeFeature[11], ShapeFeature[12], ShapeFeature[13],
                                                ShapeFeature[14], ShapeFeature[15], ShapeFeature[16], ShapeFeature[17], ShapeFeature[18], ShapeFeature[19], ShapeFeature[20], "e"));
                    sw.WriteAsync(sb.ToString());
                }
                #endregion

                Run.Text = "   Klasifikasi...";
                Run.TextAlign = ContentAlignment.MiddleCenter;
                Run.Refresh();

                #region Classification
                //CLASSIFICATION
                int poin = 0;
                MultiLayerPerceptron mlp = new MultiLayerPerceptron();
                double echo = mlp.EchogenicityClassification();
                double comp = mlp.CompositionClassification();

                //string echoCls; string compCls;
                if (comp == 0) { compCls = "Kistik"; }
                else if (comp == 1) { compCls = "Padat"; poin += 2; }
                else if (comp == 2) { compCls = "Kompleks"; poin += 1; }
                else { compCls = "tidak dapat diidentifikasi"; }

                if (echo == 0) { echoCls = "Anekoik"; }
                else if (echo == 1) { echoCls = "Isoekoik"; poin += 1; }
                else if (echo == 2) { echoCls = "Hipoekoik"; poin += 2; }
                else if (echo == 3) { echoCls = "Sangat Hipoekoik"; poin += 3; }
                else { echoCls = "tidak dapat diidentifikasi"; }

                SupportVectorMachine svm = new SupportVectorMachine();
                double margin = svm.MarginClassification();
                double[] shape = svm.ShapeClassification();

                //string marginCls; string shapeCls; string oriCls;
                if (margin == 0) { marginCls = "Ireguler"; poin += 2; }
                else if (margin == 1) { marginCls = "Halus"; }
                else { marginCls = "tidak dapat diidentifikasi"; }

                if (shape[0] == 0) { shapeCls = "Ireguler"; poin += 3; }
                else if (shape[0] == 1) { shapeCls = "Bulat ke Oval"; }
                else { shapeCls = "tidak dapat diidentifikasi"; }

                if (shape[1] == 0) { oriCls = "Paralel"; }
                else if (shape[1] == 1) { oriCls = "Non-paralel"; poin += 2; }
                else { oriCls = "tidak dapat diidentifikasi"; }

                persentase = Convert.ToDouble(poin) / 12 * 100;
                malignRate.Value = Convert.ToInt32(persentase);
                if (poin < 4) { resultLabel.Text = "Jinak"; }
                else if (poin >= 4 && poin < 7) { resultLabel.Text = "Mencurigakan"; }
                else if (poin > 7) { resultLabel.Text = "Ganas"; }
                #endregion

                Run.Text = "   Selesai";
                Run.TextAlign = ContentAlignment.MiddleCenter;
                Run.Refresh();
            }
            else { MessageBox.Show("Tolong masukkan citra USG nodul tiroid terlebih dahulu", "File Error", MessageBoxButtons.OK); }
        }

        private void Reset_Click(object sender, EventArgs e)
        {
            InputBox.Image = null;
            OutputBox.Image = null;
            malignRate.Value = 0;
            resultLabel.Text = "Status";
            directory.Text = null;
            Run.Text = "   Mulai Proses";
            form2.marginValue = "";
            form2.shapeValue = "";
            form2.orientationValue = "";
            form2.echogenityValue = "";
            form2.compositionValue = "";
        }

        private void SaveAs_Click(object sender, EventArgs e)
        {
            string label = Convert.ToInt32(persentase).ToString() + "%-" + resultLabel.Text;
           
            PointF firstLocation = new PointF(0, 0);

            if (inputImg != null)
            {
                Bitmap newBitmap;
                using (var bitmap = inputImg)//load the image file
                {
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        using (Font arialFont = new Font("Times New Roman", 18))
                        {
                            graphics.DrawString(label, arialFont, Brushes.Gold, firstLocation);
                        }
                    }
                    newBitmap = new Bitmap(bitmap);
                }
                
                SaveFileDialog sfd = new SaveFileDialog
                {
                    Filter = "Image Files(*.bmp)|*.bmp",
                    InitialDirectory = directory.ToString(),
                    FileName = ".bmp"
                };

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    newBitmap.Save(sfd.FileName);//save the image file
                }
                newBitmap.Dispose();
            }
            else
            {
                MessageBox.Show("Tidak terdapat masukan citra!", "File Error", MessageBoxButtons.OK);
            }
        }
         
    }
}
