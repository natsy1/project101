using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Math;

namespace project101
{
    class FiturLaws
    {
        MatrixOperators opt = new MatrixOperators();
        public byte[][,] Laws(byte[,] I)
        {
            //function[mapz] = laws(image)
            //LAWS Laws image filters. Law's image filters applied to input image

            int rows = I.GetLength(0);
            int cols = I.GetLength(1);

            //define fiters
            int[] filter = new int[] { 1, 4, 6, 4, 2 };
            int[] filter1 = new int[] { -1, -2, 0, 2, 1 };
            int[] filter2 = new int[] { -1, 0, 2, 0, -1 };
            int[] filter3 = new int[] { 1, -4, 6, -4, 1 };
            int[][] filters = { filter, filter1, filter2, filter3 };

            int[][,] filterst = { Matrix.Transpose(filter), Matrix.Transpose(filter1), Matrix.Transpose(filter2), Matrix.Transpose(filter3) };

            //define filters and apply to images
            int[,][,] filtered2D = new int[4, 4][,];

            for (int p = 0; p < 4; p++)
            {
                for (int q = 0; q < 4; q++)
                {
                    //filter' * filter
                    int[,] temp = new int[5, 5];
                    for (int i = 0; i < 5; i++)
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            temp[i, j] = filterst[p][i, 0] * filters[q][j];
                        }
                    }
                    //correlation filter image and temp
                    int[,] filtered = new int[rows + 4, cols + 4];
                    for (int a = 0; a < filtered.GetLength(0); a++)
                    {
                        for (int b = 0; b < filtered.GetLength(1); b++)
                        {
                            for (int c = 0; c < 5; c++)
                            {
                                if (a - c < 0 || a - c > rows - 1) { break; }
                                for (int d = 0; d < 5; d++)
                                {
                                    if (b - d < 0 || b - d > cols - 1) { break; }
                                    filtered[a, b] = filtered[a, b] + I[a - c, b - d] * temp[4 - c, 4 - d];
                                }
                            }
                        }
                    }
                    for (int i = 0; i < filtered.GetLength(0); i++)
                    {
                        for (int j = 0; j < filtered.GetLength(1); j++)
                        {
                            filtered[i, j] = filtered[i, j] > 255 ? 255 : filtered[i, j];
                            filtered[i, j] = filtered[i, j] < 0 ? 0 : filtered[i, j];
                        }
                    }
                    filtered2D[p, q] = filtered;
                }
            }

            //get resulting 9 maps
            int[][,] map = new int[9][,];
            map[0] = opt.MatrixScalarI(opt.ElementWiseI(filtered2D[0, 0], filtered2D[0, 3], "add"), 2, "divide");
            map[1] = opt.MatrixScalarI(opt.ElementWiseI(filtered2D[0, 2], filtered2D[2, 3], "add"), 2, "divide");
            map[2] = opt.MatrixScalarI(opt.ElementWiseI(filtered2D[1, 1], filtered2D[2, 0], "add"), 2, "divide");
            map[3] = filtered2D[2, 1];
            map[4] = filtered2D[3, 2];
            map[5] = opt.MatrixScalarI(opt.ElementWiseI(filtered2D[0, 1], filtered2D[1, 3], "add"), 2, "divide");
            map[6] = filtered2D[1, 0];
            map[7] = opt.MatrixScalarI(opt.ElementWiseI(filtered2D[1, 2], filtered2D[3, 0], "add"), 2, "divide");
            map[8] = opt.MatrixScalarI(opt.ElementWiseI(filtered2D[2, 2], filtered2D[3, 1], "add"), 2, "divide");

            byte[][,] mapz = new byte[9][,];
            mapz[0] = opt.MatrixIToB(map[0]);
            mapz[1] = opt.MatrixIToB(map[1]);
            mapz[2] = opt.MatrixIToB(map[2]);
            mapz[3] = opt.MatrixIToB(map[3]);
            mapz[4] = opt.MatrixIToB(map[4]);
            mapz[5] = opt.MatrixIToB(map[5]);
            mapz[6] = opt.MatrixIToB(map[6]);
            mapz[7] = opt.MatrixIToB(map[7]);
            mapz[8] = opt.MatrixIToB(map[8]);

            return mapz;
        }

        public double[] LawsStatistik(byte[,] F)
        {
            int m = F.GetLength(0);
            int n = F.GetLength(1);

            //Hitung frekuensi aras keabuan
            int L = 256;
            int[] Frek = new int[L];
            //double[,] F1 = new double[m, n];
            int intensitas = 0;
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    //F1[i, j] = Convert.ToDouble(F[i, j]);
                    intensitas = F[i, j];
                    Frek[intensitas] = Frek[intensitas] + 1;
                }
            }

            //Hitung probabilitas
            int jum_piksel = m * n;
            double[] Prob = new double[L];

            for (int i = 0; i < L; i++)
            {
                Prob[i] = Convert.ToDouble(Frek[i]) / jum_piksel;
            }

            //Hitung mu
            double mu = Prob[0];
            for (int i = 1; i < L; i++)
            {
                mu = mu + i * Prob[i];
            }

            //Hitung deviasi standar
            double varians = 0;
            for (int i = 0; i < L; i++)
            {
                varians = varians + Math.Pow((i - mu), 2) * Prob[i];
            }
            double deviasi = Math.Sqrt(varians);
            //double varians_n = Convert.ToDouble(varians) / Math.Pow((L - 1), 2);  //Normalisasi

            double[] statLaw = { mu, varians };
            return statLaw;
        }
    }
}
