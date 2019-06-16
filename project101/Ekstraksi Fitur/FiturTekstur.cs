using System;

namespace project101
{
    class FiturTekstur
    {
        public double[][] GLCM(byte[,] F)
        {
            //function[glcm] = glcm(F)
            //GLCM Menghasilkan fitur GLCM.
            //Masukan: F = Citra berskala keabuan Keluaran: Fitur = fitur GLCM untuk beberapa sudut.

            int tinggi = F.GetLength(0);
            int lebar = F.GetLength(1);

            //Bentuk GLCM

            double[,] GLCM0 = new double[256, 256];
            int total_piksel0 = 0;

            double[,] GLCM45 = new double[256, 256];
            int total_piksel45 = 0;

            double[,] GLCM90 = new double[256, 256];
            int total_piksel90 = 0;

            double[,] GLCM135 = new double[256, 256];
            int total_piksel135 = 0;

            for (int y = 1; y < tinggi - 1; y++)
            {
                for (int x = 1; x < lebar - 1; x++)
                {
                    //--Sudut 0
                    int a = F[y, x];
                    int b = F[y, x + 1];
                    GLCM0[a + 1, b + 1] = GLCM0[a + 1, b + 1] + 1;
                    total_piksel0 = total_piksel0 + 1;

                    //--Sudut 45
                    a = F[y, x];
                    b = F[y - 1, x + 1];
                    GLCM45[a + 1, b + 1] = GLCM45[a + 1, b + 1] + 1;
                    total_piksel45 = total_piksel45 + 1;

                    //--Sudut 90
                    a = F[y, x];
                    b = F[y - 1, x];
                    GLCM90[a + 1, b + 1] = GLCM90[a + 1, b + 1] + 1;
                    total_piksel90 = total_piksel90 + 1;

                    //--Sudut 135
                    a = F[y, x];
                    b = F[y - 1, x - 1];
                    GLCM135[a + 1, b + 1] = GLCM135[a + 1, b + 1] + 1;
                    total_piksel135 = total_piksel135 + 1;
                }
            }

            for (int i = 0; i < 256; i++)
            {
                for (int j = 0; j < 256; j++)
                {
                    GLCM0[i, j] = GLCM0[i, j] / total_piksel0;
                    GLCM45[i, j] = GLCM45[i, j] / total_piksel45;
                    GLCM90[i, j] = GLCM90[i, j] / total_piksel90;
                    GLCM135[i, j] = GLCM135[i, j] / total_piksel135;
                }
            }

            //--- Hitung ASM
            double asm0 = 0;
            double asm45 = 0;
            double asm90 = 0;
            double asm135 = 0;

            //--- Hitung kontras
            double kontras0 = 0;
            double kontras45 = 0;
            double kontras90 = 0;
            double kontras135 = 0;

            //--- Hitung IDM
            double idm0 = 0;
            double idm45 = 0;
            double idm90 = 0;
            double idm135 = 0;

            //--- Hitung entropi
            double entropi0 = 0;
            double entropi45 = 0;
            double entropi90 = 0;
            double entropi135 = 0;

            //-- Hitung kovarians
            //-- Hitung px[] dan py[] dulu
            double korelasi0 = 0;
            double px0 = 0; double py0 = 0;
            double stdevx0 = 0; double stdevy0 = 0;

            double korelasi45 = 0;
            double px45 = 0; double py45 = 0;
            double stdevx45 = 0; double stdevy45 = 0;

            double korelasi90 = 0;
            double px90 = 0; double py90 = 0;
            double stdevx90 = 0; double stdevy90 = 0;

            double korelasi135 = 0;
            double px135 = 0; double py135 = 0;
            double stdevx135 = 0; double stdevy135 = 0;

            for (int a = 0; a < 256; a++)
            {
                for (int b = 0; b < 256; b++)
                {
                    asm0 = asm0 + (GLCM0[a, b] * GLCM0[a, b]);
                    asm45 = asm45 + (GLCM45[a, b] * GLCM45[a, b]);
                    asm90 = asm90 + (GLCM90[a, b] * GLCM90[a, b]);
                    asm135 = asm135 + (GLCM135[a, b] * GLCM135[a, b]);

                    kontras0 = kontras0 + (a - b) * (a - b) * (GLCM0[a, b]);
                    kontras45 = kontras45 + (a - b) * (a - b) * (GLCM45[a, b]);
                    kontras90 = kontras90 + (a - b) * (a - b) * (GLCM90[a, b]);
                    kontras135 = kontras135 + (a - b) * (a - b) * (GLCM135[a, b]);

                    idm0 = idm0 + (GLCM0[a, b] / (1 + (a - b) * (a - b)));
                    idm45 = idm45 + (GLCM45[a, b] / (1 + (a - b) * (a - b)));
                    idm90 = idm90 + (GLCM90[a, b] / (1 + (a - b) * (a - b)));
                    idm135 = idm135 + (GLCM135[a, b] / (1 + (a - b) * (a - b)));

                    if (GLCM0[a, b] != 0)
                    { entropi0 = entropi0 - (GLCM0[a, b] * (Math.Log(GLCM0[a, b]))); }
                    if (GLCM45[a, b] != 0)
                    { entropi45 = entropi45 - (GLCM45[a, b] * (Math.Log(GLCM45[a, b]))); }
                    if (GLCM90[a, b] != 0)
                    { entropi90 = entropi90 - (GLCM90[a, b] * (Math.Log(GLCM90[a, b]))); }
                    if (GLCM135[a, b] != 0)
                    { entropi135 = entropi135 - (GLCM135[a, b] * (Math.Log(GLCM135[a, b]))); }

                    px0 = px0 + a * GLCM0[a, b];
                    py0 = py0 + b * GLCM0[a, b];

                    px45 = px45 + a * GLCM45[a, b];
                    py45 = py45 + b * GLCM45[a, b];

                    px90 = px90 + a * GLCM90[a, b];
                    py90 = py90 + b * GLCM90[a, b];

                    px135 = px135 + a * GLCM135[a, b];
                    py135 = py135 + b * GLCM135[a, b];

                    stdevx0 = stdevx0 + (a - px0) * (a - px0) * GLCM0[a, b];
                    stdevy0 = stdevy0 + (b - py0) * (b - py0) * GLCM0[a, b];

                    stdevx45 = stdevx45 + (a - px45) * (a - px45) * GLCM45[a, b];
                    stdevy45 = stdevy45 + (b - py45) * (b - py45) * GLCM45[a, b];

                    stdevx90 = stdevx90 + (a - px90) * (a - px90) * GLCM90[a, b];
                    stdevy90 = stdevy90 + (b - py90) * (b - py90) * GLCM90[a, b];

                    stdevx135 = stdevx135 + (a - px135) * (a - px135) * GLCM135[a, b];
                    stdevy135 = stdevy135 + (b - py135) * (b - py135) * GLCM135[a, b];

                    double temp0 = ((a - px0) * (b - py0) * GLCM0[a, b] / (stdevx0 * stdevy0));
                    korelasi0 = double.IsNaN(temp0) ? korelasi0 : korelasi0 = korelasi0 + temp0;

                    double temp45 = ((a - px45) * (b - py45) * GLCM45[a, b] / (stdevx45 * stdevy45));
                    korelasi45 = double.IsNaN(temp45) ? korelasi45 : korelasi45 = korelasi45 + temp45;

                    double temp90 = ((a - px90) * (b - py90) * GLCM90[a, b] / (stdevx90 * stdevy90));
                    korelasi90 = double.IsNaN(temp90) ? korelasi90 : korelasi90 = korelasi90 + temp90;

                    double temp135 = ((a - px135) * (b - py135) * GLCM135[a, b] / (stdevx135 * stdevy135));
                    korelasi135 = double.IsNaN(temp135) ? korelasi135 : korelasi135 = korelasi135 + temp135;
                }
            }

            double[] G0 = new double[] { asm0, kontras0, idm0, entropi0, korelasi0 };
            double[] G45 = new double[] { asm45, kontras45, idm45, entropi45, korelasi45 };
            double[] G90 = new double[] { asm90, kontras90, idm90, entropi90, korelasi90 };
            double[] G135 = new double[] { asm135, kontras135, idm135, entropi135, korelasi135 };

            double[][] GLCM = new double[][] { G0, G45, G90, G135 };

            return GLCM;
        }


        MatrixOperators opt = new MatrixOperators();
        public double[] GLRLM(byte[,] I)
        {
            //ini yang dari matlab
            int rows = I.GetLength(0);
            int cols = I.GetLength(1);

            double quantize = 16;
            double mini = 1;
            double maxi = 0;
            byte[,] mask = new byte[rows, cols];
            double[,] img = new double[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    mask[i, j] = 1;
                    img[i, j] = Convert.ToDouble(I[i, j]) / 255;
                    if (img[i, j] <= mini) { mini = img[i, j]; }
                    if (img[i, j] >= maxi) { maxi = img[i, j]; }
                }
            }

            img = opt.MatrixScalarD(img, mini, "subtract");
            maxi = maxi - mini;

            //double lvStep = maxi / quantize;
            double[] levels = new double[Convert.ToInt32(quantize)];
            for (int i = 0; i < levels.Length; i++)
            {
                levels[i] = (maxi / quantize) * (i + 1);
            }

            //imquantize 0-15 (16 kuantisasi)
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (img[i, j] <= levels[0]) { img[i, j] = 0; }
                    else if (levels[levels.Length - 1] <= img[i, j]) { img[i, j] = levels.Length; }
                    else
                    {
                        for (int k = 1; k < levels.Length; k++) // index 1-15
                        {
                            if (levels[k - 1] < img[i, j] && img[i, j] < levels[k]) { img[i, j] = k; break; }
                        }
                    }
                }
            }

            int maxImgsize = Math.Max(rows, cols);

            int[,] p0 = new int[Convert.ToInt32(quantize), maxImgsize];
            int[,] p90 = new int[Convert.ToInt32(quantize), maxImgsize];

            //step padarray[1 1] skipped
            //imrotate
            int[,] image0 = opt.MatrixDToI(img);

            for (int q = 0; q < Convert.ToInt32(quantize); q++) //0-15
            {
                int[,] BW = new int[rows, cols];
                int r = BW.GetLength(0);
                int c = BW.GetLength(1);

                //BW = int8(img == i);
                for (int a = 0; a < r; a++)
                {
                    for (int b = 0; b < c; b++)
                    {
                        BW[a, b] = image0[a, b] == q ? 1 : 0;
                    }
                }

                int runlength = 0;

                for (int i = 0; i < r; i++)
                {
                    for (int j = 0; j < c;)
                    {
                        runlength = 0;
                        if (BW[i, j] == 1)
                        {
                            runlength++;
                            j++;
                            while (j < c && runlength < c - 1)
                            {
                                if (BW[i, j] == 1) { runlength++; }
                                else { break; }
                                j++;
                            }
                            p0[q, runlength]++;
                        }
                        j++;
                    }
                }

                for (int j = 0; j < c; j++)
                {
                    for (int i = 0; i < r;)
                    {
                        runlength = 0;
                        if (BW[i, j] == 1)
                        {
                            runlength++;
                            i++;
                            while (i < r && runlength < r - 1)
                            {
                                if (BW[i, j] == 1) { runlength++; }
                                else { break; }
                                i++;
                            }
                            p90[q, runlength]++;
                        }
                        i++;
                    }
                }
            }

            int[,] maskI = opt.MatrixBToI(mask);
            int[,] p = opt.ElementWiseI(p0, p90, "add");
            int totSum = opt.SumI(p);

            //1:maximgS.^ 2
            int[] pembagi0 = new int[maxImgsize];
            for (int i = 0; i < maxImgsize; i++)
            {
                pembagi0[i] = Convert.ToInt32(Math.Pow(Convert.ToDouble(i + 1), 2));
            }

            //sum(p, 1)
            int[] sump1 = opt.SumRowI(p);
            //sum(p, 2)
            int[,] sump2 = opt.SumColI(p);

            //sump2 .^ 2
            int[,] sump2pow = opt.MatrixScalarI(sump2, 2, "power");

            //(1:quantize)' .^ 2
            int[,] pembagi1 = new int[Convert.ToInt32(quantize), 1];
            for (int i = 0; i < quantize; i++)
            {
                pembagi1[i, 0] = Convert.ToInt32(Math.Pow(Convert.ToDouble(i + 1), 2));
            }

            //glrlm.SRE = sum(sum(p, 1)./ ((1:maximgS).^ 2)) / totSum;
            double SRE = Convert.ToDouble(opt.SumVectorI(opt.ElementWiseVector(sump1, pembagi0, "divide"))) / totSum;
            //glrlm.LRE = sum(sum(p, 1).* ((1:maximgS).^ 2)) / totSum;
            double LRE = Convert.ToDouble(opt.SumVectorI(opt.ElementWiseVector(sump1, pembagi0, "multiply"))) / totSum;
            //glrlm.RLN = sum(sum(p, 1).^ 2) / totSum;
            double RLN = Convert.ToDouble(opt.SumVectorI(opt.VectorScalarOpI(sump1, 2, "power"))) / totSum;
            //glrlm.RP = totSum / sum(mask(:));
            double RP = Convert.ToDouble(totSum) / Convert.ToDouble(opt.SumI(maskI));
            //glrlm.GLN = sum(sum(p, 2).^ 2) / totSum;
            double GLN = Convert.ToDouble(opt.SumI(sump2pow)) / Convert.ToDouble(totSum);
            //glrlm.LGRE = sum(sum(p, 2) .* ((1:quantize)'.^2)) / totSum;
            double LGRE = Convert.ToDouble(opt.SumI(opt.ElementWiseI(sump2, pembagi1, "multiply"))) / totSum;
            //glrlm.HGRE = sum(sum(p, 2).^ 2) / totSum;
            double HGRE = Convert.ToDouble(opt.SumI(sump2pow)) / Convert.ToDouble(totSum);

            double[] glrlm = new double[] { SRE, LRE, RLN, RP, GLN, LGRE, HGRE };
            return glrlm;
        }


        public double[] Lacunarity(byte[,] GR)
        {
            //function[H] = lacunarity(GR)
            //LACUNARITY Berguna untuk memperoleh fitur lacunarity.
            //Masukan: RGB = Citra berwarna. Keluaran: H = Nilai balik berupa lacunarity

            int tinggi = GR.GetLength(0);
            int lebar = GR.GetLength(1);

            //Hitung warna rata-rata R, G, dan B
            double jumls_atas = 0;
            double jumls_bawah = 0;
            double jum_piksel = 0;

            for (int Baris = 0; Baris < tinggi; Baris++)
            {
                for (int Kolom = 0; Kolom < lebar; Kolom++)
                {
                    jum_piksel = jum_piksel + 1;
                    jumls_atas = jumls_atas + Math.Pow(GR[Baris, Kolom], 2);
                    jumls_bawah = jumls_bawah + GR[Baris, Kolom];
                }
            }

            double jumla = 0;
            double juml2 = 0;
            double juml4 = 0;
            double juml6 = 0;
            double juml8 = 0;
            double juml10 = 0;

            for (int Baris = 0; Baris < tinggi; Baris++)
            {
                for (int Kolom = 0; Kolom < lebar; Kolom++)
                {
                    jumla = jumla + GR[Baris, Kolom] / (jumls_bawah / jum_piksel) - 1;
                    juml2 = juml2 + GR[Baris, Kolom] / Math.Pow((jumls_bawah / jum_piksel) - 1, 2);
                    juml4 = juml4 + GR[Baris, Kolom] / Math.Pow((jumls_bawah / jum_piksel) - 1, 4);
                    juml6 = juml6 + GR[Baris, Kolom] / Math.Pow((jumls_bawah / jum_piksel) - 1, 6);
                    juml8 = juml8 + GR[Baris, Kolom] / Math.Pow((jumls_bawah / jum_piksel) - 1, 8);
                    juml10 = juml10 + GR[Baris, Kolom] / Math.Pow((jumls_bawah / jum_piksel) - 1, 10);
                }
            }

            double ls = (jumls_atas / jum_piksel) / Math.Pow((jumls_bawah / jum_piksel), 2) - 1;
            double la = jumla / jum_piksel;
            double l2 = Math.Sqrt(juml2 / jum_piksel);
            double l4 = Math.Pow((juml4 / jum_piksel), 0.25);
            double l6 = Math.Pow((juml6 / jum_piksel), 0.16666666666666666);
            double l8 = Math.Pow((juml8 / jum_piksel), 0.125);
            double l10 = Math.Pow((juml10 / jum_piksel), 0.1);

            double[] H = new double[] { ls, la, l2, l4, l6, l8, l10 };
            return H;
        }
    }
}
