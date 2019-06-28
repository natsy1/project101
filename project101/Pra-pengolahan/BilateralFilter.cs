using System;
using Emgu.CV;
using Emgu.CV.Structure;

namespace project101
{
    class BilateralFilter
    {
        MatrixOperators opt = new MatrixOperators();

        public byte[,] Bilateral(byte[,] A)
        {
            //Implements bilateral filtering for grayscale images.
            //function B = bfltGray(A, w, sigma_d, sigma_r)

            double[,] newA = new double[A.GetLength(0), A.GetLength(1)];
            for (int i = 0; i < newA.GetLength(0); i++)
            {
                for (int j = 0; j < newA.GetLength(1); j++)
                {
                    newA[i, j] = Convert.ToDouble(A[i, j]) / 255;
                }
            }

            int w = 5;       //bilateral filter half-width
            double sigma_d = 3;  //bilateral filter standard deviations
            double sigma_r = 0.1;

            //Pre - compute Gaussian distance weights.
            //[X, Y] = meshgrid(-w:w, -w:w);
            int grid = w * 2 + 1;
            double[,] X = new double[grid, grid];
            for (int i = 0; i < grid; i++)
            {
                for (int j = 0, k = -w; j < grid; j++, k++)
                {
                    X[i, j] = k;
                }
            }
            double[,] Y = Accord.Math.Matrix.Transpose(X);

            //X.^2 dan Y.^2
            X = opt.MatrixScalarD(X, 2, "power");
            Y = opt.MatrixScalarD(Y, 2, "power");
            //2*sigma_d^2
            double pembagiD = 2 * Math.Pow(sigma_d, 2);
            //2*sigma_r^2
            double pembagiR = 2 * Math.Pow(sigma_r, 2);

            //G = exp(-(X.^2+Y.^2)/(2*sigma_d^2));
            double[,] G = new double[grid, grid];
            for (int i = 0; i < grid; i++)
            {
                for (int j = 0; j < grid; j++)
                {
                    double pangkat = (Y[i, j] - X[i, j]) / pembagiD;
                    G[i, j] = Math.Pow(Math.E, -pangkat);
                }
            }

            //apply filter
            int r = newA.GetLength(0);
            int c = newA.GetLength(1);
            byte[,] B = new byte[r, c];
            for (int i = 0; i < r; i++)
            {
                for (int j = 0; j < c; j++)
                {
                    //Extract local region.
                    int iMin = Math.Max(i - w, 0);
                    int iMax = Math.Min(i + w, r);
                    int jMin = Math.Max(j - w, 0);
                    int jMax = Math.Min(j + w, c);
                    double[,] I = new double[iMax - iMin + 1, jMax - jMin + 1];
                    double[,] Ip = new double[I.GetLength(0), I.GetLength(1)];
                    double[,] H = new double[I.GetLength(0), I.GetLength(1)];
                    double[,] F = new double[I.GetLength(0), I.GetLength(1)];
                    double[,] g = new double[I.GetLength(0), I.GetLength(1)];

                    //Extract local region
                    I = getI(newA, iMin, iMax, jMin, jMax, i, j);

                    //Compute Gaussian intessity weights
                    H = getH(I, newA, i, j, pembagiR);

                    //Calculate bilateral filter response
                    g = getG(I, G, iMin, iMax, jMin, jMax, i, j);
                    F = getF(H, g);
                    B[i, j] = getB(I, F);

                    #region Break
                    ////Compute Gaussian intensity weights.
                    //for (int m = 0; m < I.GetLength(0); m++)
                    //{
                    //    for (int n = 0; n < I.GetLength(1); n++)
                    //    {
                    //        I[m, n] = newA[m, n];
                    //        Ip[m, n] = Math.Pow((I[m, n] - newA[i, j]), 2);

                    //        //H = exp(-(I-A(i,j)).^2/(2*sigma_r^2));
                    //        double pangkat = Ip[m, n] / pembagiR;
                    //        H[m, n] = Math.Pow(Math.E, -pangkat);

                    //        //Calculate bilateral filter response.
                    //        //F = H.* G((iMin: iMax) - i + w + 1, (jMin: jMax) - j + w + 1);
                    //        double[,] g = new double[I.GetLength(0), I.GetLength(1)];
                    //        for (int x = iMin - i + w, k = 0; x < iMax - i + w + 1; x++, k++)
                    //        {
                    //            for (int y = jMin - j + w, l = 0; y < jMax - j + w + 1; y++, l++)
                    //            {
                    //                g[k, l] = G[x, y];
                    //            }
                    //        }
                    //        F[m, n] = H[m, n] * g[m, n];
                    //    }
                    //}
                    ////B(i,j) = sum(F(:).*I(:))/sum(F(:));
                    //double atas = 0;
                    //double bawah = 0;
                    //for (int m = 0; m < I.GetLength(0); m++)
                    //{
                    //    for (int n = 0; n < I.GetLength(1); n++)
                    //    {
                    //        atas = atas + (F[m, n] * I[m, n]);
                    //        bawah = bawah + F[m, n];
                    //    }
                    //}
                    //double hasil = atas / bawah * 255;
                    //if (hasil >= 255) { B[i, j] = 255; }
                    //else if (hasil < 0 || double.IsNaN(hasil)) { B[i, j] = 255; }
                    //else { B[i, j] = Convert.ToByte(hasil); }
                    #endregion
                }
            }
            return B;
        }

        private double[,] getI(double[,] A, int iMin, int iMax, int jMin, int jMax, int i, int j)
        {
            double[,] I = new double[iMax - iMin + 1, jMax - jMin + 1];
            for(int m = 0; m < I.GetLength(0); m++)
            {
                for(int n = 0; n < I.GetLength(1); n++)
                {
                    I[m, n] = Math.Pow((A[m, n] - A[i, j]), 2);
                }
            }
            return I;
        }

        private double[,] getH(double[,] I, double[,] A, int i, int j, double pembagiR)
        {
            double[,] H = new double[I.GetLength(0), I.GetLength(1)];
            for (int m = 0; m < H.GetLength(0); m++)
            {
                for (int n = 0; n < H.GetLength(1); n++)
                {
                    H[m, n] = Math.Pow(Math.E, -(I[m, n] / pembagiR));
                }
            }
            return H;
        }

        private double[,] getG(double[,] I, double[,] G, int iMin, int iMax, int jMin, int jMax, int i, int j)
        {
            int w = 5;
            double[,] g = new double[I.GetLength(0), I.GetLength(1)];
            for (int x = iMin - i + w, k = 0; x < iMax - i + w + 1; x++, k++)
            {
                for (int y = jMin - j + w, l = 0; y < jMax - j + w + 1; y++, l++)
                {
                    g[k, l] = G[x, y];
                }
            }
            return g;
        }

        private double[,] getF(double[,] H, double[,] g)
        {
            double[,] F = new double[H.GetLength(0), H.GetLength(1)];
            for (int m = 0; m < H.GetLength(0); m++)
            {
                for (int n = 0; n < H.GetLength(1); n++)
                {
                    F[m, n] = H[m, n] * g[m, n];
                }
            }
            return F;
        }

        private byte getB(double[,] I, double[,] F)
        {
            double atas = 0;
            double bawah = 0;
            for (int m = 0; m < I.GetLength(0); m++)
            {
                for (int n = 0; n < I.GetLength(1); n++)
                {
                    atas = atas + (F[m, n] * I[m, n]);
                    bawah = bawah + F[m, n];
                }
            }
            double hasil = atas / bawah * 255;
            byte B;
            if (hasil >= 255) { B = 255; }
            else if (hasil < 0 || double.IsNaN(hasil)) { B = 255; }
            else { B = Convert.ToByte(hasil); }
            return B;
        }
        
        public Image<Gray, byte> bilateralFilter(Image<Gray, byte> grayImage)
        {
            //bilateral filter dari EmguCV
            //CvInvoke.BilateralFilter(inputArray, outputArray, int diameter of each pixel neighborhood, double sigmaColor, double sigmaSpace, opt borderType)
            var threshImage = grayImage.CopyBlank();
            CvInvoke.BilateralFilter(grayImage, threshImage, 7, 25, 60);
            return threshImage;
        }
    }
}
