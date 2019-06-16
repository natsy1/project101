using System;
using System.Linq;
using System.Drawing;
using System.Data;
using Accord.Math;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace project101
{
    class MomenCitra
    {
        FiturGeometris geo = new FiturGeometris();
        BitmapToMatrix conv = new BitmapToMatrix();
        
        public double[] MomenHu(byte[,] F)
        {
            //function[Momen] = momenhu(F)
            //MOMENHU Menghitung momen HU.
            //Masukan: F = citra berskala keabuan; Keluaran: Momen = 7 momen Hu

            double norm_20 = Normomen(F, 2, 0);
            double norm_02 = Normomen(F, 0, 2);
            double norm_11 = Normomen(F, 1, 1);
            double norm_30 = Normomen(F, 3, 0);
            double norm_12 = Normomen(F, 1, 2);
            double norm_21 = Normomen(F, 2, 1);
            double norm_03 = Normomen(F, 0, 3);

            double m1 = norm_20 + norm_02;
            double m2 = Math.Pow((norm_20 - norm_02), 2) + 4 * Math.Pow(norm_11, 2);
            double m3 = Math.Pow((norm_30 + 3 * norm_12), 2) + Math.Pow((3 * norm_21 - norm_03), 2);
            double m4 = Math.Pow((norm_30 + norm_12), 2) + Math.Pow((norm_21 + norm_03), 2);
            double m5 = (norm_30 - 3 * norm_12) * (norm_30 + norm_12) *
                (Math.Pow((norm_30 + norm_12), 2) - 3 * Math.Pow((norm_21 + norm_03), 2)) +
                (3 * norm_21 - norm_03) * (norm_21 + norm_03) *
                (3 * Math.Pow((norm_30 + norm_12), 2) - Math.Pow((norm_21 + norm_03), 2));
            double m6 = (norm_20 - norm_02) *
                (Math.Pow((norm_30 + norm_12), 2) - Math.Pow((norm_21 + norm_03), 2)) +
                4 * norm_11 * (norm_30 + norm_12) * (norm_21 + norm_03);
            double m7 = (3 * norm_21 + norm_30) * (norm_30 + norm_12) *
                (Math.Pow((norm_30 + norm_12), 2) - 3 * Math.Pow((norm_21 + norm_03), 2)) +
                (norm_30 - 3 * norm_12) * (norm_21 + norm_03) *
                (3 * Math.Pow((norm_30 + norm_12), 2) - Math.Pow((norm_21 + norm_03), 2));

            double[] Momen = new double[] { m1, m2, m3, m4, m5, m6, m7 };
            return Momen;
        }

        public double Normomen(byte[,] F, int p, int q)
        {
            //function[hasil] = normomen(F, p, q)
            //Masukan : F citra biner, p dan q = orde momen

            //F = double(F);
            double[,] A = new double[F.GetLength(0), F.GetLength(1)];
            for (int i = 0; i < F.GetLength(0); i++)
            {
                for (int j = 0; j < F.GetLength(1); j++)
                {
                    A[i, j] = Convert.ToDouble(F[i, j]);
                }
            }

            double m00 = MomenSpasial(F, 0, 0);
            double normalisasi = Math.Pow(m00, ((p + q + 2) / 2.0));
            double hasil = MomenPusat(F, p, q) / normalisasi;
            return hasil;
        }
        public double MomenSpasial(byte[,] F, int p, int q)
        {
            //function[hasil] = momen_spasial(F, p, q)
            //MOMEN_SPASIAL menghitung momen spasial berorde(p, q)
            int m = F.GetLength(0);
            int n = F.GetLength(1);
            double momenPQ = 0;
            for (int y = 0; y < m; y++)
            {
                for (int x = 0; x < n; x++)
                {
                    if (F[y, x] != 0)
                    {
                        momenPQ = momenPQ + Math.Pow(x, p) * Math.Pow(y, q);
                    }
                }
            }
            return momenPQ;
        }
        public double MomenPusat(byte[,] F, int p, int q)
        {
            //function[hasil] = momen_pusat(F, p, q)
            //momen_pusat menghitung momen pusat berorde p, q
            int m = F.GetLength(0);
            int n = F.GetLength(1);
            double m00 = MomenSpasial(F, 0, 0);

            double xc = MomenSpasial(F, 1, 0) / m00;
            double yc = MomenSpasial(F, 0, 1) / m00;

            double mpq = 0;
            for (int y = 0; y < m; y++)
            {
                for (int x = 0; x < n; x++)
                {
                    if (F[y, x] != 0)
                    {
                        mpq = mpq + Math.Pow((x - xc), p) * Math.Pow((y - yc), q);
                    }
                }
            }
            double hasil = mpq;
            return hasil;
        }

                       

        public double[] MomenZernike(byte[,] F, int orde)
        {
            //function A = zermoment(F, orde, tampil)
            //Salin yang ada pada kotak pembatas
            int[] arrd = KotakPembatas(F);
            int min_y = arrd[0];
            int max_y = arrd[1];
            int min_x = arrd[2];
            int max_x = arrd[3];

            //luas = sum(sum(B)); //Luas objek
            int luas = 0;
            //B = F(min_y: max_y, min_x: max_x);
            byte[,] B = new byte[max_y - min_y + 1, max_x - min_x + 1];
            for (int i = min_y, i1 = 0; i < max_y + 1; i++, i1++)
            {
                for (int j = min_x, j1 = 0; j < max_x + 1; j++, j1++)
                {
                    if (F[i, j] == 255) { luas = luas + 1; B[i1, j1] = 255; }
                }
            }

            int m = B.GetLength(0);
            int n = B.GetLength(1);
            int beta = 20000;   //Parameter untuk mengatur penyekalaan citra baru: 200 x 200

            //Tentukan citra yang memenuhi perbandingan beta dengan luas citra B
            int m1 = Convert.ToInt32(Math.Truncate(m * Math.Sqrt(Convert.ToDouble(beta) / luas)));
            int n1 = Convert.ToInt32(Math.Truncate(n * Math.Sqrt(Convert.ToDouble(beta) / luas)));

            //imresize
            Bitmap Bbmp = conv.getBMP(B);
            Bitmap Cbmp = ResizeImage(Bbmp, n1, m1);
            //Bitmap Cbmp = new Bitmap(Bbmp, new Size(n1, m1));
            byte[,] C = new byte[B.GetLength(0) * m / m1, B.GetLength(1) * n / n1];
            C = conv.BitmaptoMatrix(Cbmp);

            m = C.GetLength(0);
            n = C.GetLength(1);
            //Atur ukuran gambar untuk kepentingan penyajian dalam bentuk lingkaran 
            int maks_mn = Math.Max(m, n);
            int m_baru = Convert.ToInt32(Math.Round(Math.Sqrt(2) * maks_mn));
            int n_baru = m_baru;

            byte[,] D = new byte[m_baru, n_baru];
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    D[i, j] = C[i, j];
                    if (C[i, j] > 120) { D[i, j] = 255; }
                    else { D[i, j] = 0; }
                }
            }

            m = m_baru;
            n = n_baru;

            //Peroleh pusat massa dan letakkan di tengah
            double[] arrb = geo.Centroid(D);
            int xc = Convert.ToInt32(Math.Round(arrb[1]));
            int yc = Convert.ToInt32(Math.Round(arrb[0]));

            xc = xc - Convert.ToInt32(Math.Round(Convert.ToDouble(n) / 2));
            yc = yc - Convert.ToInt32(Math.Round(Convert.ToDouble(m) / 2));

            //Atur gambar ke G
            byte[,] G = new byte[m, n];
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (!((j - xc < 0) || (i - yc < 0) || (i - yc > m - 1) || (j - xc > n - 1)))
                    {
                        G[i - yc, j - xc] = D[i, j];
                    }
                    if (G[i, j] == 255) { G[i, j] = 1; }
                }
            }

            //Bentuk grid untuk menentukan koordinat dengan tengah citra sebagai titik pusat
            double selang = Convert.ToDouble(2) / (m - 1);
            int dim = Convert.ToInt32(Convert.ToDouble(2) / selang + 1);
            int ii = -1;
            double[,] X = new double[dim, dim];
            double[,] Y = new double[dim, dim];
            for (double i = -1.0; i <= 1.0; i += selang) //i = -1:selang:1
            {
                ii++;
                int jj = -1;
                for (double j = -1.0; j <= 1.0; j += selang) //j = -1:selang:1
                {
                    jj++;
                    X[ii, jj] = j;
                }
            }

            Y = Accord.Math.Matrix.Transpose(X);

            //Hitung sudut, rho dan lingkaran
            double[,] Theta = new double[m, n];
            double[,] Rho = new double[m, n];
            double[,] L = new double[m, n];

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Theta[i, j] = Math.Atan2(Y[i, j], X[i, j]);
                    if (Theta[i, j] < 0)
                    {
                        Theta[i, j] = Theta[i, j] + 2 * Math.PI;
                    }
                    double jarak2 = Math.Pow(X[i, j], 2) + Math.Pow(Y[i, j], 2);
                    Rho[i, j] = Math.Sqrt(jarak2);
                    L[i, j] = jarak2;
                }
            }

            //Bentuk Lingkaran
            //DidalamL = find(L <= 1);
            int luas1 = 0;
            double[,] Lingkaran = new double[m, n];
            //Lingkaran(DidalamL) = 1;
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (L[i, j] <= 1)
                    {
                        Lingkaran[i, j] = 1;
                        luas1 = luas1 + 1;
                    }
                    else { continue; }
                }
            }

            //Hitung koefisien momen zernike
            double[] A = new double[orde * 2 - 1];
            int indeks = 0;
            double zpq_real = 0, zpq_imaj = 0;
            for (int p = 2; p < orde + 1; p++)
            {
                for (int q = p; q >= 0; q -= 2) //q = p:-2:0
                {
                    zpq_real = 0;
                    zpq_imaj = 0;
                    for (int i = 1; i < m; i++)
                    {
                        for (int j = 1; j < n; j++)
                        {
                            if (Lingkaran[i, j] == 1)
                            {
                                double vpq = FBZernike(p, q, Rho[i, j]);
                                zpq_real = zpq_real + G[i, j] * vpq * Math.Cos(q * Theta[i, j]);
                                zpq_imaj = zpq_imaj + G[i, j] * vpq * Math.Sin(q * Theta[i, j]);
                            }
                        }
                    }
                    zpq_real = zpq_real * (p + 1) / Math.PI;
                    zpq_imaj = zpq_imaj * (p + 1) / Math.PI;

                    A[indeks] = Math.Sqrt(Math.Pow(zpq_real, 2) + Math.Pow(zpq_imaj, 2));
                    indeks = indeks + 1;
                }
            }

            //Normalisasi koefisien
            double m00 = MomenSpasial(G, 0, 0);
            A = A.Select(r => r / m00).ToArray();
            return A;
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
        public double FBZernike(int n, int l, double rho)
        {
            //function p = fb_zernike(n, l, rho)
            double p = 0;
            if (n == 2)
            {
                switch (l)
                {
                    case 0:
                        p = 2 * Math.Pow(rho, 2) - 1;
                        break;
                    case 2:
                        p = Math.Pow(rho, 2);
                        break;
                }
            }
            else if (n == 3)
            {
                switch (l)
                {
                    case 1:
                        p = 3 * Math.Pow(rho, 3) - 2 * rho;
                        break;
                    case 3:
                        p = Math.Pow(rho, 3);
                        break;
                }
            }
            else if (n == 4)
            {
                switch (l)
                {
                    case 0:
                        p = 6 * Math.Pow(rho, 4) - 6 * Math.Pow(rho, 2) + 1;
                        break;
                    case 2:
                        p = 4 * Math.Pow(rho, 4) - 3 * Math.Pow(rho, 2);
                        break;
                    case 4:
                        p = Math.Pow(rho, 4);
                        break;
                }
            }
            else if (n == 5)
            {
                switch (l)
                {
                    case 1:
                        p = 10 * Math.Pow(rho, 5) - 12 * Math.Pow(rho, 3) + 3 * rho;
                        break;
                    case 3:
                        p = 5 * Math.Pow(rho, 5) - 4 * Math.Pow(rho, 3);
                        break;
                    case 5:
                        p = Math.Pow(rho, 5);
                        break;
                }
            }
            else if (n == 6)
            {
                switch (l)
                {
                    case 0:
                        p = 20 * Math.Pow(rho, 6) - 30 * Math.Pow(rho, 4) + 12 * Math.Pow(rho, 2) - 1;
                        break;
                    case 2:
                        p = 15 * Math.Pow(rho, 6) - 20 * Math.Pow(rho, 4) + 6 * Math.Pow(rho, 2);
                        break;
                    case 4:
                        p = 6 * Math.Pow(rho, 6) - 5 * Math.Pow(rho, 4);
                        break;
                    case 6:
                        p = Math.Pow(rho, 6);
                        break;
                }
            }
            else if (n == 7)
            {
                switch (l)
                {
                    case 1:
                        p = 35 * Math.Pow(rho, 7) - 60 * Math.Pow(rho, 5) + 30 * Math.Pow(rho, 3) - 4 * rho;
                        break;
                    case 3:
                        p = 21 * Math.Pow(rho, 7) - 30 * Math.Pow(rho, 5) + 10 * Math.Pow(rho, 3);
                        break;
                    case 5:
                        p = 7 * Math.Pow(rho, 7) - 6 * Math.Pow(rho, 5);
                        break;
                    case 7:
                        p = Math.Pow(rho, 7);
                        break;
                }
            }
            else if (n == 8)
            {
                switch (l)
                {
                    case 0:
                        p = 70 * Math.Pow(rho, 8) - 140 * Math.Pow(rho, 6) + 90 * Math.Pow(rho, 4) - 20 * Math.Pow(rho, 2) + 1;
                        break;
                    case 2:
                        p = 56 * Math.Pow(rho, 8) - 105 * Math.Pow(rho, 6) + 60 * Math.Pow(rho, 4) - 10 * Math.Pow(rho, 2);
                        break;
                    case 4:
                        p = 28 * Math.Pow(rho, 8) - 42 * Math.Pow(rho, 6) + 15 * Math.Pow(rho, 4);
                        break;
                    case 6:
                        p = 8 * Math.Pow(rho, 8) - 7 * Math.Pow(rho, 6);
                        break;
                    case 8:
                        p = Math.Pow(rho, 8);
                        break;
                }
            }
            else if (n == 9)
            {
                switch (l)
                {
                    case 1:
                        p = 126 * Math.Pow(rho, 9) - 280 * Math.Pow(rho, 7) + 210 * Math.Pow(rho, 5) - 60 * Math.Pow(rho, 3) + 5 * rho;
                        break;
                    case 3:
                        p = 84 * Math.Pow(rho, 9) - 168 * Math.Pow(rho, 7) + 105 * Math.Pow(rho, 5) - 20 * Math.Pow(rho, 3);
                        break;
                    case 5:
                        p = 36 * Math.Pow(rho, 9) - 56 * Math.Pow(rho, 7) + 21 * Math.Pow(rho, 5);
                        break;
                    case 7:
                        p = 9 * Math.Pow(rho, 9) - 8 * Math.Pow(rho, 7);
                        break;
                    case 9:
                        p = Math.Pow(rho, 9);
                        break;
                }
            }
            else if (n == 10)
            {
                switch (l)
                {
                    case 0:
                        p = 252 * Math.Pow(rho, 10) - 630 * Math.Pow(rho, 8) + 560 * Math.Pow(rho, 6) - 210 * Math.Pow(rho, 4) + 30 * Math.Pow(rho, 2) - 1;
                        break;
                    case 2:
                        p = 210 * Math.Pow(rho, 10) - 504 * Math.Pow(rho, 8) + 420 * Math.Pow(rho, 6) - 140 * Math.Pow(rho, 4) + 15 * Math.Pow(rho, 2);
                        break;
                    case 4:
                        p = 129 * Math.Pow(rho, 10) - 252 * Math.Pow(rho, 8) + 168 * Math.Pow(rho, 6) - 35 * Math.Pow(rho, 4);
                        break;
                    case 6:
                        p = 45 * Math.Pow(rho, 10) - 72 * Math.Pow(rho, 8) + 28 * Math.Pow(rho, 6);
                        break;
                    case 8:
                        p = 10 * Math.Pow(rho, 10) - 9 * Math.Pow(rho, 8);
                        break;
                    case 10:
                        p = Math.Pow(rho, 10);
                        break;
                }
            }
            else if (n == 11)
            {
                switch (l)
                {
                    case 1:
                        p = 462 * Math.Pow(rho, 11) - 1260 * Math.Pow(rho, 9) + 1260 * Math.Pow(rho, 7) - 560 * Math.Pow(rho, 5) + 105 * Math.Pow(rho, 3) - 6 * rho;
                        break;
                    case 3:
                        p = 330 * Math.Pow(rho, 11) - 840 * Math.Pow(rho, 9) + 756 * Math.Pow(rho, 7) - 280 * Math.Pow(rho, 5) + 35 * Math.Pow(rho, 3);
                        break;
                    case 5:
                        p = 165 * Math.Pow(rho, 11) - 360 * Math.Pow(rho, 9) + 252 * Math.Pow(rho, 7) - 56 * Math.Pow(rho, 5);
                        break;
                    case 7:
                        p = 55 * Math.Pow(rho, 11) - 90 * Math.Pow(rho, 9) + 36 * Math.Pow(rho, 7);
                        break;
                    case 9:
                        p = 11 * Math.Pow(rho, 11) - 10 * Math.Pow(rho, 9);
                        break;
                    case 11:
                        p = Math.Pow(rho, 11);
                        break;
                }
            }
            else if (n == 12)
            {
                switch (l)
                {
                    case 0:
                        p = 924 * Math.Pow(rho, 12) - 2772 * Math.Pow(rho, 10) + 3150 * Math.Pow(rho, 8) - 1680 * Math.Pow(rho, 6) + 420 * Math.Pow(rho, 4) - 42 * Math.Pow(rho, 2) + 1;
                        break;
                    case 2:
                        p = 792 * Math.Pow(rho, 12) - 2310 * Math.Pow(rho, 10) + 2520 * Math.Pow(rho, 8) - 1260 * Math.Pow(rho, 6) + 280 * Math.Pow(rho, 4) - 21 * Math.Pow(rho, 2);
                        break;
                    case 4:
                        p = 495 * Math.Pow(rho, 12) - 1320 * Math.Pow(rho, 10) + 1260 * Math.Pow(rho, 8) - 504 * Math.Pow(rho, 6) + 70 * Math.Pow(rho, 4);
                        break;
                    case 6:
                        p = 220 * Math.Pow(rho, 12) - 495 * Math.Pow(rho, 10) + 360 * Math.Pow(rho, 8) - 84 * Math.Pow(rho, 6);
                        break;
                    case 8:
                        p = 66 * Math.Pow(rho, 12) - 110 * Math.Pow(rho, 10) + 45 * Math.Pow(rho, 8);
                        break;
                    case 10:
                        p = 12 * Math.Pow(rho, 12) - 11 * Math.Pow(rho, 10);
                        break;
                    case 12:
                        p = Math.Pow(rho, 12);
                        break;
                }
            }
            return p;
        }
        public int[] KotakPembatas(byte[,] F)
        {
            //function[min_x, max_x, min_y, max_y] = kotak_pembatas(F)
            //Mencari koordinat kotak yang membatasi citra F
            int m0 = F.GetLength(0);
            int n0 = F.GetLength(1);
            int min_y = m0;
            int max_y = 1;
            int min_x = n0;
            int max_x = 1;

            for (int i = 0; i < m0; i++)
            {
                for (int j = 0; j < n0; j++)
                {
                    if (F[i, j] == 255)
                    {
                        if (min_y > i) { min_y = i; }
                        if (max_y < i) { max_y = i; }
                        if (min_x > j) { min_x = j; }
                        if (max_x < j) { max_x = j; }
                    }
                }
            }
            int[] arrd = new int[] { min_y, max_y, min_x, max_x };
            return arrd;
        }
    }
}
