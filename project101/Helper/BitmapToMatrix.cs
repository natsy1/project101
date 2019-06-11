using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace project101
{
    public class BitmapToMatrix
    {
        public Bitmap tempBMP;

        public byte[,] BitmaptoMatrix(Bitmap picture)
        {
            byte[,] imgMatrix = new byte[picture.Height, picture.Width];
            //Loop through the images pixels to get gray values.
            for (int x = 0; x < picture.Height; x++)
            {
                for (int y = 0; y < picture.Width; y++)
                {
                    Color pixelColor = picture.GetPixel(y, x);
                    byte greyValue = (byte)((pixelColor.R * 0.3) + (pixelColor.G * 0.59) + (pixelColor.B * 0.11));
                    imgMatrix[x, y] = greyValue;
                }
            }
            return imgMatrix;
        }


        public Bitmap getBMP(byte[,] ImageMatrix)
        {
            int Height = ImageMatrix.GetLength(0);
            int Width = ImageMatrix.GetLength(1);

            Bitmap ImageBMP = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);

            unsafe
            {
                BitmapData bmd = ImageBMP.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, ImageBMP.PixelFormat);
                int nWidth = 0;
                nWidth = Width * 3;
                int nOffset = bmd.Stride - nWidth;
                byte* p = (byte*)bmd.Scan0;
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        p[0] = p[1] = p[2] = ImageMatrix[i, j];
                        p += 3;
                    }

                    p += nOffset;
                }
                ImageBMP.UnlockBits(bmd);
                return ImageBMP;
            }
        }

        public static void DisplayImage(byte[,] ImageMatrix, PictureBox PicBox)
        {
            int Height = ImageMatrix.GetLength(0);
            int Width = ImageMatrix.GetLength(1);

            Bitmap ImageBMP = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);

            unsafe
            {
                BitmapData bmd = ImageBMP.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, ImageBMP.PixelFormat);
                int nWidth = 0;
                nWidth = Width * 3;
                int nOffset = bmd.Stride - nWidth;
                byte* p = (byte*)bmd.Scan0;
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        p[0] = p[1] = p[2] = ImageMatrix[i, j];
                        p += 3;
                    }

                    p += nOffset;
                }
                ImageBMP.UnlockBits(bmd);
                PicBox.Image = ImageBMP;
            }
        }

        public Bitmap ImageMasking(Bitmap image)
        {
            Bitmap maskedImg = new Bitmap(image.Width, image.Height);
            for (int y = 0; y <= image.Height - 1; y++)
            {
                for (int x = 0; x <= image.Width - 1; x++)
                {
                    if (image.GetPixel(x, y).A != 255)
                    {
                        maskedImg.SetPixel(x, y, Color.FromArgb(255 - image.GetPixel(x, y).A, 255, 0, 0));
                    }
                }
            }
            //PictureBox1.Image = maskedImg;
            return maskedImg;
        }
    }
}
