using System;
using Accord.Math;

namespace project101
{
    class FiturHistogram
    {
        public double[] Histogram(byte[,] F)
        {
            //function[Stat] = stattekstur(F)
            //Masukan: F = citra berskala keabuan. Keluaran: Stat = berisi statistika tekstur
            //Didasarkan pada Gonzalez, Woods, dan Eddins, 2004

            int m = F.GetLength(0);
            int n = F.GetLength(1);

            //Hitung frekuensi aras keabuan
            int L = 256;
            int[] Frek = new int[L];
            //double[,] F1 = new double[m, n];
            byte intensitas = 0;
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
            //Hitung skewness
            //Energi(Keseragaman)
            //Entropi
            double mu = Prob[0];
            double skewness = 0;
            double energi = 0;
            double entropi = 0;
            for (int i = 1; i < L; i++)
            {
                mu = mu + i * Prob[i];
                skewness = skewness + Math.Pow((i - mu), 3) * Prob[i];
                energi = energi + Math.Pow(Prob[i], 2);
                if (Prob[i] != 0)
                {
                    entropi = entropi + Prob[i] * Math.Log(Prob[i]);
                }
            }
            skewness = skewness / Math.Pow((L - 1), 2);
            entropi = -entropi;

            //Hitung deviasi standar
            //Hitung kurtosis
            double kurtosis = 0;
            double varians = 0;
            for (int i = 0; i < L; i++)
            {
                varians = varians + Math.Pow((i - mu), 2) * Prob[i];
                kurtosis = kurtosis + (Math.Pow((i - mu), 4) * Prob[i]) - 3;
            }
            double deviasi = Math.Sqrt(varians);
            double varians_n = Convert.ToDouble(varians) / Math.Pow((L - 1), 2);  //Normalisasi         

            //Hitung R atau Smoothness
            double smoothness = 1 - 1 / (1 + varians_n);

            double[] Stat = new double[] { mu, deviasi, skewness, energi, entropi, smoothness, varians_n, kurtosis };
            return Stat;
        }
    }
}
