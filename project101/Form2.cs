using System;
using System.Drawing;
using System.Windows.Forms;

namespace project101
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        Point lastPoint;
        private void Form2_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Left += e.X - lastPoint.X;
                Top += e.Y - lastPoint.Y;
            }
        }

        private void Form2_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }
        
        private void back_Click(object sender, EventArgs e)
        {
            Close();
        }

        public string marginValue
        {
            get { return marginResult.Text; }
            set { marginResult.Text = value; }
        }
        public string shapeValue
        {
            get { return shapeResult.Text; }
            set { shapeResult.Text = value; }
        }
        public string orientationValue
        {
            get { return orientResult.Text; }
            set { orientResult.Text = value; }
        }
        public string echogenityValue
        {
            get { return echoResult.Text; }
            set { echoResult.Text = value; }
        }
        public string compositionValue
        {
            get { return compoResult.Text; }
            set { compoResult.Text = value; }
        }
    }
}
