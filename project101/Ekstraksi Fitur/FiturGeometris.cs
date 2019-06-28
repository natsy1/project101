using System;
using System.Collections.Generic;
using System.Linq;

namespace project101
{
    class FiturGeometris
    {
        public double RasioTL(byte[,] F)
        {
            //function rasioHW = bboxcitra(F)
            //BBOXCITRA Mencari kotak terkecil yang melingkupi citra.
            //Masukan: F = Citra berskala keabuan. Keluaran: Nilai X dan Y terkecil dan terbesar

            int m = F.GetLength(0);
            int n = F.GetLength(1);
            int min_y = m;
            int max_y = 0;
            int min_x = n;
            int max_x = 0;

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
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
            //rasio = luas / ((max_y - min_y) * (max_x - min_x));
            double height = max_y - min_y;
            double width = max_x - min_x;
            double rasioHW = height / width;

            return rasioHW;
        }


        public double Rectangularity(byte[,] F)
        {
            //function rasio = bboxobjek(F)
            //BBOXOBJEK Mencari kotak terkecil yang melingkupi citra.
            //Masukan: F = Citra berskala keabuan yang mengandung suatu objek. Keluaran: Nilai alpha dan beta terkecil dan terbesar

            int jum_baris = F.GetLength(0);
            int jum_kolom = F.GetLength(1);

            //gambar(3:end - 2, 3:end - 2) = F;
            byte[,] gambar = new byte[jum_baris + 4, jum_kolom + 4];
            for (int i1 = 0, i2 = 2; i1 < jum_baris; i1++, i2++)
            {
                for (int j1 = 0, j2 = 2; j1 < jum_kolom; j1++, j2++)
                {
                    gambar[i2, j2] = F[i1, j1];
                }
            }
            F = gambar;

            int[][] Kontur = InboundTracing(F);
            int jum = Kontur.Length;

            //Cari nilai alpha dan beta terbesar dan terkecil
            double max_a = 0;
            double min_a = Math.Pow(10, 300);
            double max_b = 0;
            double min_b = min_a;

            double[] arr = Centroid(F);
            double xc = arr[0];
            double yc = arr[1];

            double theta = 0.5 * Math.Atan(2 * MomenPusat(F, 1, 1) / (MomenPusat(F, 2, 0) - MomenPusat(F, 0, 2)));
            for (int i = 0; i < jum; i++)
            {
                int x = Kontur[i][1];
                int y = Kontur[i][0];
                double alpha = x * Math.Cos(theta) + y * Math.Sin(theta);
                double beta = -x * Math.Sin(theta) + y * Math.Cos(theta);
                if (min_b > beta) { min_b = beta; }
                if (max_b < beta) { max_b = beta; }
                if (min_a > alpha) { min_a = alpha; }
                if (max_a < alpha) { max_a = alpha; }
            }

            //Hitung luas
            int m = F.GetLength(0);
            int n = F.GetLength(1);

            double luas = 0;
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (F[i, j] != 0) { luas = luas + 1; }
                }
            }

            double rasio;
            //Hitung rasio
            if (luas == 0) { rasio = 0; }
            else { rasio = luas / ((max_a - min_a) * (max_b - min_b)); }

            return rasio;
        }


        public double Dispersion(byte[,] a)
        {
            double hasil_jarakmaks = GetDiameter(a);
            double hasil_luas = GetLuas(a);
            double dispersion = hasil_jarakmaks / hasil_luas;
            return dispersion;
        }
        public double GetDiameter(byte[,] BW)
        {
            //function[diameter, x1, y1, x2, y2] = peroleh_diameter(BW)
            //PEROLEH_DIAMETER Digunakan untuk menghitung panjang objek pada citra BW(citra biner).
            //Hasil: diameter : panjang objek; x1, y1, x2, y2: menyatakan dua titik yang mewakili panjang tersebut

            int[][] U = InboundTracing(BW);
            int n = U.Length;
            double jarak_maks = 0;
            int piksel1 = 0;
            int piksel2 = 0;
            int p = U[0][0];
            int q = U[0][1];

            for (int i = p; i < (n - 1); i++)
            {
                for (int j = (p + 1); j < n; j++)
                {
                    double jarak = Math.Sqrt(Math.Pow((U[i][0] - U[j][0]), 2) + Math.Pow((U[i][1] - U[j][1]), 2));
                    if (jarak > jarak_maks)
                    {
                        jarak_maks = jarak;
                        piksel1 = i;
                        piksel2 = j;
                    }
                }
            }

            int y1 = U[piksel1][0];
            int x1 = U[piksel1][1];
            int y2 = U[piksel2][0];
            int x2 = U[piksel2][1];

            double diameter = jarak_maks;
            return diameter;
        }
        public double GetLuas(byte[,] BW)
        {
            //function hasil_luas1 = luas(BW)
            //LUAS Untuk menghitung luas citra BW(citra biner)

            int jum_baris = BW.GetLength(0);
            int jum_kolom = BW.GetLength(1);

            //gambar(3:end - 2, 3:end - 2) = BW;
            byte[,] gambar = new byte[jum_baris + 4, jum_kolom + 4];
            for (int i = 2; i < jum_baris + 2; i++)
            {
                for (int j = 2; j < jum_kolom + 2; j++)
                {
                    gambar[i, j] = BW[i - 2, j - 2];
                }
            }
            BW = gambar;

            jum_baris = BW.GetLength(0);
            jum_kolom = BW.GetLength(1);

            double hasil_luas1 = 0;
            for (int p = 0; p < jum_baris; p++)
            {
                for (int q = 0; q < jum_kolom; q++)
                {
                    if (BW[p, q] == 255) { hasil_luas1 = hasil_luas1 + 1; }
                }
            }
            return hasil_luas1;
        }


        public double Compactness(byte[,] A)
        {
            //function cf = peroleh_compactness(A)
            double p = CCPerimeter(A);
            double a = CCLuas(A);
            double rasio = 4 * Math.PI * a / Math.Pow(p, 2);
            double cf = 1 - rasio;
            return cf;
        }
        public double CCPerimeter(byte[,] BW)
        {
            //function hasil_perim2 = perim2(BW)
            //PERIM2 Untuk menghitung perimeter suatu objek pada BW(citra biner) dengan menggunakan chain code
            //hasil menyatakan hasil perhitungan perimeter

            int[][] U = InboundTracing(BW);
            int[][] kode_rantai = ConvexHull(U);

            int jum_genap = 0;
            int jum_ganjil = 0;
            for (int p = 0; p < kode_rantai.Length; p++)
            {
                int kode = kode_rantai[p][0];
                if ((kode == '0') || (kode == '2') || (kode == '4') || (kode == '6') || (kode == '8'))
                {
                    jum_genap = jum_genap + 1;
                }
                else
                {
                    jum_ganjil = jum_ganjil + 1;
                }
            }
            double hasil_perim2 = jum_genap + jum_ganjil * Math.Sqrt(2);
            return hasil_perim2;
        }
        public double CCLuas(byte[,] BW)
        {
            //function hasil_luas2 = luas2(BW)
            //LUAS2 Untuk menghitung luas citra BW(citra biner) melalui kode rantai

            int tinggi_luas2 = BW.GetLength(0);
            int lebar_luas2 = BW.GetLength(1);
            int[][] U = InboundTracing(BW);
            int[][] kode_rantai = ConvexHull(U);

            double hasil_luas2 = 0;
            for (int p = 0; p < kode_rantai.Length; p++)
            {
                int kode = kode_rantai[p][0];
                double y = tinggi_luas2 + 1 - U[p][0];

                switch (kode)
                {
                    case '0':
                        hasil_luas2 = hasil_luas2 + y;
                        break;
                    case '1':
                        hasil_luas2 = hasil_luas2 + y + 0.5;
                        break;
                    case '2':
                        hasil_luas2 = hasil_luas2;
                        break;
                    case '3':
                        hasil_luas2 = hasil_luas2 - y - 0.5;
                        break;
                    case '4':
                        hasil_luas2 = hasil_luas2 - y;
                        break;
                    case '5':
                        hasil_luas2 = hasil_luas2 - y + 0.5;
                        break;
                    case '6':
                        hasil_luas2 = hasil_luas2;
                        break;
                    case '7':
                        hasil_luas2 = hasil_luas2 + y - 0.5;
                        break;
                }
            }
            return hasil_luas2;
        }


        public double Circularity(byte[,] BW)
        {
            //function[c] = peroleh_circularity(BW)
            int jum_baris = BW.GetLength(0);
            int jum_kolom = BW.GetLength(1);

            byte[,] gambar = new byte[jum_baris + 4, jum_kolom + 4];
            for (int i1 = 0, i2 = 2; i1 < jum_baris; i1++, i2++)
            {
                for (int j1 = 0, j2 = 2; j1 < jum_kolom; j1++, j2++)
                {
                    gambar[i2, j2] = BW[i1, j1];
                }
            }
            BW = gambar;
            jum_baris = jum_baris + 4;
            jum_kolom = jum_kolom + 4;

            double[] p = Centroid(BW);
            double py = p[0];
            double px = p[1];

            int[][] Kontur = InboundTracing(BW);

            //Hapus elemen terakhir
            int[][] newKontur = new int[Kontur.Length - 1][];
            List<int> list = Kontur.SelectMany(T => T).ToList();
            list.RemoveAt(Kontur.Length * 2 - 1);
            list.RemoveAt(Kontur.Length * 2 - 2);
            for (int k = 0, l = 0; k < Kontur.Length - 1; k++, l += 2)
            {
                newKontur[k] = new int[] { list[l], list[l + 1] };
            }
            int jum = newKontur.Length;

            //Hitung mu
            double total = 0;
            double xVal = 0; double yVal = 0;
            for (int i = 0; i < jum; i++)
            {
                xVal = Math.Pow((newKontur[i][1] - px), 2);
                yVal = Math.Pow((newKontur[i][0] - py), 2);
                total = total + Math.Sqrt(yVal + xVal);
            }
            double mu = total / jum;

            //Hitung sigma
            total = 0; xVal = 0; yVal = 0;
            for (int i = 0; i < jum; i++)
            {
                yVal = Math.Pow((newKontur[i][0] - py), 2);
                xVal = Math.Pow((newKontur[i][1] - px), 2);
                double sq = Math.Sqrt(yVal + xVal) - mu;
                total = total + Math.Pow(sq, 2);
                //total = total + Math.Sqrt(Math.Pow((Math.Pow(((yVal - py), 2) + (Math.Pow((xVal - px), 2)) - mu)), 2));
            }
            double sigma = total / jum;
            double c = mu / sigma;

            return c;
        }


        public double Convexity(byte[,] a)
        {
            double hasil_perim1 = GetPerimeter(a);
            double hasil_perim2 = CCPerimeter(a);
            double convex = hasil_perim2 / hasil_perim1;
            return convex;
        }
        public double GetPerimeter(byte[,] BW)
        {
            //function hasil_perim1 = perim1(BW)
            //PERIM1 Untuk menghitung perimeter suatu objek pada BW(citra biner)
            //hasil menyatakan hasil perhitungan perimeter
            int[][] U = InboundTracing(BW);

            double hasil_perim1 = U.Length - 1;
            return hasil_perim1;
        }


        public double Tortuosity(byte[,] a)
        {
            double hasil_jarakmaks = GetDiameter(a);
            double hasil_perim1 = GetPerimeter(a);
            double tortuosity = (2 * hasil_jarakmaks) / hasil_perim1;
            return tortuosity;
        }


        public double AspectRatio(byte[,] a)
        {
            double[] arrd = GetLebar(a);
            double panjang = arrd[0];
            double lebar = arrd[1];
            double aspectratio = lebar / panjang;
            return aspectratio;
        }
        public double[] GetLebar(byte[,] BW)
        {
            //function[panjang, lebar, x1, y1, x2, y2, x3, y3, x4, y4] = peroleh_lebar(BW)
            //PEROLEH_LEBAR Digunakan untuk memperoleh panjang dan lebar objek yang terdapat pada citra biner BW.
            //Hasil:
            //panjang = panjang objek; lebar = lebar objek
            //(x1, y1, x2, y2) = menyatakan posisi lebar objek
            //(x3, y3, x4, y4) = menyatakan posisi panjang objek

            int jum_baris = BW.GetLength(0);
            int jum_kolom = BW.GetLength(1);

            byte[,] gambar = new byte[jum_baris + 4, jum_kolom + 4];
            for (int i1 = 0, i2 = 2; i1 < jum_baris; i1++, i2++)
            {
                for (int j1 = 0, j2 = 2; j1 < jum_kolom; j1++, j2++)
                {
                    gambar[i2, j2] = BW[i1, j1];
                }
            }
            BW = gambar;

            int[][] U = InboundTracing(BW);
            int n = U.Length;
            double jarak_maks = 0;
            int piksel1 = 0;
            int piksel2 = 0;
            double jarak;

            for (int p = 0; p < (n - 1); p++)
            {
                for (int q = (p + 1); q < n; q++)
                {
                    jarak = Math.Sqrt(Math.Pow((U[p][0] - U[q][0]), 2) + Math.Pow((U[p][1] - U[q][1]), 2));
                    if (jarak > jarak_maks) { jarak_maks = jarak; piksel1 = p; piksel2 = q; }
                }
            }

            int y1 = U[piksel1][0];
            int x1 = U[piksel1][1];
            int y2 = U[piksel2][0];
            int x2 = U[piksel2][1];

            double panjang = jarak_maks;

            //Cari dua titik terpanjang yang tegak lurus dengan garis terpanjang

            double maks = 0;
            int posx3 = -1;
            int posx4 = -1;
            int posy3 = -1;
            int posy4 = -1;
            int x3 = 0, y3 = 0, x4 = 0, y4 = 0;

            if ((x1 != x2) && (y1 != y2))
            {
                //Kedua titik tidak pada kolom atau baris yang sama
                double grad1 = Convert.ToDouble(y1 - y2) / (x1 - x2);
                double grad2 = -1 / grad1;
                for (int p = 0; p < (n - 1); p++)
                {
                    for (int q = p + 1; q < n; q++)
                    {
                        x3 = U[p][1];
                        y3 = U[p][0];
                        x4 = U[q][1];
                        y4 = U[q][0];
                        int pembagi = x4 - x3;
                        if (pembagi == 0) { continue; }

                        double grad3 = Convert.ToDouble(y4 - y3) / (x4 - x3);
                        if (Math.Abs(grad3 - grad2) < 0.1 * Math.Abs(grad2))
                        {
                            jarak = Math.Sqrt(Math.Pow((x3 - x4), 2) + Math.Pow((y3 - y4), 2));
                            if (jarak > maks)
                            {
                                maks = jarak;
                                posx3 = x3;
                                posx4 = x4;
                                posy3 = y3;
                                posy4 = y4;
                            }
                        }
                    }
                }
            }

            else if (y1 == y2)
            {
                //kalau kedua titik pada baris yang sama
                //double grad1 = 0;
                //double grad2 = double.PositiveInfinity;

                for (int p = 0; p < (n - 1); p++)
                {
                    for (int q = p + 1; q < n; q++)
                    {
                        x3 = U[p][1]; y3 = U[p][0];
                        x4 = U[q][1]; y4 = U[q][0];
                        int deltax = (x4 - x3);
                        if ((deltax < 0.01) || (deltax > 0.01)) { continue; }

                        jarak = Math.Sqrt(Math.Pow((x3 - x4), 2) + Math.Pow((y3 - y4), 2));
                        if (jarak > maks)
                        {
                            maks = jarak;
                            posx3 = x3;
                            posx4 = x4;
                            posy3 = y3;
                            posy4 = y4;
                        }
                    }
                }
            }

            else
            {
                //kalau kedua titik pada kolom yang berbeda
                //double grad1 = double.PositiveInfinity;
                //double grad2 = 0;

                for (int p = 0; p < (n - 1); p++)
                {
                    for (int q = p + 1; q < n; q++)
                    {
                        x3 = U[p][1]; y3 = U[p][0];
                        x4 = U[q][1]; y4 = U[q][0];
                        int deltay = (y3 - y4);
                        if ((deltay < 1.0) || (deltay > 1.0)) { continue; }

                        jarak = Math.Sqrt(Math.Pow((x4 - x3), 2) + Math.Pow((y4 - y3), 2));
                        if (jarak > maks)
                        {
                            maks = jarak;
                            posx3 = x3;
                            posx4 = x4;
                            posy3 = y3;
                            posy4 = y4;
                        }
                    }
                }
            }

            x3 = posx3;
            y3 = posy3;
            x4 = posx4;
            y4 = posy4;
            double lebar = maks;

            double[] arrd = new double[] { panjang, lebar, x1, y1, x2, y2, x3, y3, x4, y4 };

            return arrd;
        }


        public double SelisihKodeRantai(byte[,] A)
        {
            //function nilaiCC = selisih_kode_rantai(A)
            //DERIV_CHAINCODE untuk menghitung selisih antara kode rantai n + 1 dengan kode rantai n, 
            //lalu dijumlahkan dan dibagi sejumlah kode rantai yang terbentuk untuk mengetahui seberapa besar perubahan perjalanan kode rantai tersebut

            int[] C = ChainCode(A);
            int TotalSelisih = 0;
            int LC = C.Length - 1;

            for (int p = 1; p < LC; p++)
            {
                int selisih = C[p + 1] - C[p];
                TotalSelisih = TotalSelisih + selisih;
            }

            double nilaiCC = Convert.ToDouble(TotalSelisih) / LC;
            return nilaiCC;
        }
        public int[] ChainCode(byte[,] A)
        {
            //function koderantai = chain_code(A)
            //CHAIN_CODE Digunakan untuk mendapatkan titik awal(x, y) dan kode rantai dari kontur U yang datanya telah terurutkan misalnya melalui get_contour
            //Kode 1 2 3 4 5 6 7 8 9

            int[][] U = InboundTracing(A);

            //string[] Kode = new string[] { "3", "2", "1", "4", "0", "0", "5", "6", "7" };
            int[] Kode = new int[] { 3, 2, 1, 4, 0, 0, 5, 6, 7 };
            //int xawal = U[0][1];
            //int yawal = U[0][0];
            //string[] koderantai = new string[U.Length];
            int[] koderantai = new int[U.Length];
            for (int p = 1; p < U.Length; p++)
            {
                int deltay = U[p][0] - U[p - 1][0];
                int deltax = U[p][1] - U[p - 1][1];
                int indeks = 3 * deltay + deltax + 4;
                koderantai[p] = Kode[indeks];
            }
            //int[] kodeRantai = new int[U.Length];
            //Converter(koderantai);
            return koderantai;
        }



        #region Helpers
        public int[][] InboundTracing(byte[,] BW)
        {
            //function[Kontur] = inbound_tracing(BW)
            //INBOUND_TRACING memperoleh kontur yang telah diurutkan dengan algoritma pelacakan kontur Moore

            int jum_baris = BW.GetLength(0);
            int jum_kolom = BW.GetLength(1);

            byte[,] gambar = new byte[jum_baris + 4, jum_kolom + 4];
            for (int i1 = 0, i2 = 2; i1 < jum_baris; i1++, i2++)
            {
                for (int j1 = 0, j2 = 2; j1 < jum_kolom; j1++, j2++)
                {
                    gambar[i2, j2] = BW[i1, j1];
                }
            }
            BW = gambar;
            jum_baris = jum_baris + 4;
            jum_kolom = jum_kolom + 4;

            //peroleh piksel awal
            bool selesai = false;
            //int[] b0 = new int[2];
            MyStruct1 b0 = new MyStruct1();
            for (int p = 0; p < jum_baris; p++)
            {
                for (int q = 0; q < jum_kolom; q++)
                {
                    if (BW[p, q] == 255)
                    {
                        b0.y = p;
                        b0.x = q;
                        selesai = true;
                        break;
                    }
                }
                if (selesai == true)
                {
                    break;
                }
            }

            //periksa 8 tetangga dan cari piksel pertama yang bernilai 255 (putih)
            int c0 = 4, c1 = 5; //pengecekan pertama ke arah barat
            MyStruct1 b1 = new MyStruct1();
            for (int p = 0; p < 8; p++)
            {
                int[] delta_piksel = DeltaPiksel(c0);
                int dy = delta_piksel[0];
                int dx = delta_piksel[1];

                if (BW[b0.y + dy, b0.x + dx] == 255)
                {
                    b1.y = b0.y + dy;
                    b1.x = b0.x + dx;
                    c1 = Sebelum(c0);
                    break;
                }
                else
                {
                    c0 = Berikut(c0);
                }
            }

            List<MyStruct1> Kontur = new List<MyStruct1>
            {
                b0,
                b1
            };

            MyStruct1 b = b1;
            int c = c1;
            // % Ulangi sampai berakhir
            while (true)
            {
                for (int p = 0; p < 8; p++)
                {
                    int[] delta_piksel = DeltaPiksel(c);
                    int dy = delta_piksel[0];
                    int dx = delta_piksel[1];
                    if (BW[b.y + dy, b.x + dx] == 255)
                    {
                        b.y = b.y + dy;
                        b.x = b.x + dx;
                        //if (Kontur.Contains(b)) { continue; }
                        c = Sebelum(c);
                        Kontur.Add(b);
                        break;
                    }
                    else
                    {
                        c = Berikut(c);
                    }
                }
                //kondisi pengakhir pengulangan
                if ((b.y == b0.y) && (b.x == b0.x)) { break; }
            }
            Kontur.TrimExcess();
            int[] xValue = Kontur.Select(item => item.x).ToArray();
            int[] yValue = Kontur.Select(item => item.y).ToArray();
            int[][] KonturArray = new int[xValue.Length][];
            for (int i = 0; i < yValue.Length; i++)
            {
                KonturArray[i] = new int[2] { yValue[i], xValue[i] };
            }
            return KonturArray;
        }

        public int Berikut(int x)
        {
            int b;
            if (x == 0) { b = 7; }
            else { b = x - 1; }
            return b;
        }
        public int Sebelum(int x)
        {
            int s;
            if (x == 7) { s = 0; }
            else { s = x + 1; }

            if (s < 2) { s = 2; }
            else if (s < 4) { s = 4; }
            else if (s < 6) { s = 6; }
            else { s = 0; }

            return s;
        }
        public int[] DeltaPiksel(int n)
        {
            //function[dy, dx] = delta_piksel(n)
            int dx = 0, dy = 0;
            if (n == 0) { dx = 1; dy = 0; }
            else if (n == 1) { dx = 1; dy = -1; }
            else if (n == 2) { dx = 0; dy = -1; }
            else if (n == 3) { dx = -1; dy = -1; }
            else if (n == 4) { dx = -1; dy = 0; }
            else if (n == 5) { dx = -1; dy = 1; }
            else if (n == 6) { dx = 0; dy = 1; }
            else if (n == 7) { dx = 1; dy = 1; }

            int[] arrd = new int[] { dy, dx };
            return arrd;
        }
        public interface IMyStructModifier1
        {
            int x { set; }
            int y { set; }
            double sudut { set; }
            double jarak { set; }
        }
        public interface IMyStructModifier
        {
            int x { set; }
            int y { set; }
        }
        public struct MyStruct : IMyStructModifier1
        {
            public int y { get; set; }
            public int x { get; set; }
            public double sudut { get; set; }
            public double jarak { get; set; }
        }
        public struct MyStruct1 : IMyStructModifier
        {
            public int y { get; set; }
            public int x { get; set; }
        }



        public int[][] ConvexHull(int[][] Kontur)
        {
            //function[CH] = convexhull2(Kontur)
            //CONVEXHULL Digunakan untuk mendapatkan convex hull dari suatu objek menggunakan metode 'Graham Scan'.
            //Masukan: Kontur = kontur objek, yamg berdimensi dua dengan kolom pertama berisi data Y dan kolom kedua berisi data X.
            //Keluaran: CH = Convex hull

            int jum = Kontur.Length;

            //Cari titik jangkar atau pivot 
            int terkecil = 0;
            for (int a = 1; a < jum; a++)
            {
                if (Kontur[a][0] == Kontur[terkecil][0])
                {
                    if (Kontur[a][1] < Kontur[terkecil][1])
                    {
                        terkecil = a;
                    }
                }
                else if (Kontur[a][0] < Kontur[terkecil][0])
                {
                    terkecil = a;
                }
            }

            //Susun data dengan menyertakan sudut dan panjang, kecuali titik dengan posisi = terkecil
            int indeks = 0;
            List<MyStruct> Piksel = new List<MyStruct>();
            for (int a = 0; a < jum; a++)
            {
                if (a == terkecil)
                {
                    continue;
                }
                MyStruct ms = new MyStruct
                {
                    y = Kontur[a][0],
                    x = Kontur[a][1],
                    jarak = Jarak(Kontur[terkecil], Kontur[a]),
                    sudut = Sudut(Kontur[terkecil], Kontur[a])
                };
                Piksel.Insert(indeks, ms);
                indeks = indeks + 1;
            }
            Piksel.TrimExcess();

            int jum_piksel = indeks;
            MyStruct x;
            //Lakukan pengurutan menurut sudut dan jarak
            for (int p = 1; p < jum_piksel; p++)
            {
                x = Piksel[p];
                //Sisipkan x ke dalam data[1..p - 1]
                int q = p - 1;
                bool ketemu = false;

                while ((q >= 0) && (!ketemu))
                {
                    if (x.sudut < Piksel[q].sudut)
                    {
                        Piksel[q + 1] = Piksel[q];
                        q = q - 1;
                    }
                    else
                    {
                        ketemu = true;
                    }
                    Piksel[q + 1] = x;
                }
            }

            //Kalau ada sejumlah piksel dengan nilai sudut sama maka hanya yang jaraknya terbesar yang akan dipertahankan
            List<MyStruct> Piksel1 = Unik(Piksel);
            Piksel1.TrimExcess();
            jum_piksel = Piksel1.Count;

            //Siapkan tumpukan
            List<MyStruct1> H = new List<MyStruct1>();
            MyStruct1 titik = new MyStruct1();

            //Proses pemindaian. Mula - mula sisipkan dua titik
            H.Add(new MyStruct1 { y = Kontur[terkecil][0], x = Kontur[terkecil][1] });
            H.Add(new MyStruct1 { y = Piksel1[0].y, x = Piksel1[0].x });

            MyStruct1 A = new MyStruct1();
            MyStruct1 B = new MyStruct1();

            int i = 1; int top = 1;
            while (i < jum_piksel)
            {
                titik.x = Piksel1[i].x;
                titik.y = Piksel1[i].y;

                //Ambil dua data pertama pada tumpukan H tanpa membuangnya
                A.x = H[top].x;
                A.y = H[top].y;

                B.x = H[top - 1].x;
                B.y = H[top - 1].y;

                //top = top + 1;
                bool putar = PutarKanan(A.x, A.y, B.x, B.y, titik.x, titik.y);
                if (putar == true)
                {
                    //Pop data pada tumpukan H
                    top = top - 1;
                }
                else
                {
                    //Tumpuk titik ke tumpukan H
                    top = top + 1;
                    if (H.Count - 1 <= top) { H.Add(A); }
                    H[top] = titik;
                    i = i + 1;
                }
            }
            H.RemoveAt(H.Count - 1);
            H.TrimExcess();

            //Ambil data dari tumpukan H
            List<MyStruct1> C = new List<MyStruct1>();
            indeks = 0;
            while (top != -1)
            {
                //Pop data dari tumpukan H
                //C[indeks] = H[top];
                C.Insert(indeks, H[top]);
                top = top - 1;
                //indeks = indeks + 1;
            }
            C.TrimExcess();

            int[] xValue = C.Select(a => a.x).ToArray();
            int[] yValue = C.Select(a => a.y).ToArray();
            int[][] ConvexHull = new int[xValue.GetLength(0)][];
            for (int t = 0; t < xValue.GetLength(0); t++)
            {
                ConvexHull[t] = new int[2] { yValue[t], xValue[t] };
            }
            return ConvexHull;
        }

        public bool PutarKanan(int p1x, int p1y, int p2x, int p2y, int p3x, int p3y)
        {
            //function[stat] = berputar_ke_kanan(p1, p2, p3)           
            bool stat = ((p2x - p1x) * (p3y - p1y) - (p3x - p1x) * (p2y - p1y)) > 0;
            return stat;
        }
        public List<MyStruct> Unik(List<MyStruct> Pikselin)
        {
            //function[P] = unik(Piksel)
            //List<MyStruct> temp = new List<MyStruct>();
            List<MyStruct> P = new List<MyStruct>();

            int jum = Pikselin.Count;
            double sudut = -1;
            double jarak = 0;

            //Tandai jarak dengan -1 kalau titik tidak terpakai
            for (int i = 0; i < jum; i++)
            {
                if (sudut != Pikselin[i].sudut)
                {
                    sudut = Pikselin[i].sudut;
                    jarak = Pikselin[i].jarak;
                }
                else if (jarak < Pikselin[i].jarak)
                {
                    MyStruct temp = new MyStruct
                    {
                        y = Pikselin[i].y,
                        x = Pikselin[i].x,
                        jarak = -1,
                        sudut = Pikselin[i].sudut
                    };
                    Pikselin[i] = temp;
                }
            }

            int indeks = 0;
            for (int i = 0; i < jum; i++)
            {
                if (Pikselin[i].jarak != -1)
                {
                    P.Insert(indeks, Pikselin[i]);
                    indeks = indeks + 1;
                }
            }
            return P;
        }
        public double Sudut(int[] T1, int[] T2)
        {
            //function[s] = sudut(T1, T2)
            double dy = T1[0] - T2[0];
            double dx = T1[1] - T2[1];

            if (dx == 0)
            {
                dx = 0.00000001;
            }

            double s = Math.Atan(dy / dx);
            if (s < 0)
            {
                s = s + Math.PI;
            }

            return s;
        }
        public double Jarak(int[] T1, int[] T2)
        {
            //function[j] = jarak(T1, T2)
            double j = Math.Pow((T1[0] - T2[0]), 2) + Math.Pow((T1[1] - T2[1]), 2);
            return j;
        }



        public double[] Centroid(byte[,] BW)
        {
            //CENTROID Untuk memperoleh pusat masa sebuah objek yang terletak pada citra biner BW
            double pusat_x = 0;
            double pusat_y = 0;
            double luas = 0;

            for (int q = 0; q < BW.GetLength(0); q++)
            {
                for (int p = 0; p < BW.GetLength(1); p++)
                {
                    if (BW[q, p] == 255)
                    {
                        luas = luas + 1;
                        pusat_x = pusat_x + p;
                        pusat_y = pusat_y + q;
                    }
                }
            }
            pusat_x = pusat_x / luas;
            pusat_y = pusat_y / luas;

            double[] arr = { pusat_y, pusat_x };
            return arr;
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
        #endregion
    }
}
