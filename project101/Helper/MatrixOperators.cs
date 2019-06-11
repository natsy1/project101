using System;

namespace project101
{
    class MatrixOperators
    {
        public int[,] ElementWiseI(int[,] M1, int[,] M2, string type)
        {
            //Dimension of M1 and M2 have to be the same
            int[,] M3 = new int[M1.GetLength(0), M1.GetLength(1)];
            switch (type)
            {
                case "add":
                    for (int i = 0; i < M1.GetLength(0); i++)
                    {
                        for (int j = 0; j < M1.GetLength(1); j++)
                        {
                            M3[i, j] = M1[i, j] + M2[i, j];
                        }
                    }
                    break;
                case "subtract":
                    for (int i = 0; i < M1.GetLength(0); i++)
                    {
                        for (int j = 0; j < M1.GetLength(1); j++)
                        {
                            M3[i, j] = M1[i, j] - M2[i, j];
                        }
                    }
                    break;
                case "multiply":
                    for (int i = 0; i < M1.GetLength(0); i++)
                    {
                        for (int j = 0; j < M1.GetLength(1); j++)
                        {
                            M3[i, j] = Convert.ToInt32(M1[i, j] * M2[i, j]);
                        }
                    }
                    break;
                case "divide":
                    for (int i = 0; i < M1.GetLength(0); i++)
                    {
                        for (int j = 0; j < M1.GetLength(1); j++)
                        {
                            M3[i, j] = Convert.ToInt32(Convert.ToDouble(M1[i, j]) / M2[i, j]);
                        }
                    }
                    break;
            }
            return M3;
        }



        public double[,] ElementWiseD(double[,] M1, double[,] M2, string type)
        {
            //Dimension of M1 and M2 have to be the same
            double[,] M3 = new double[M1.GetLength(0), M1.GetLength(1)];
            switch (type)
            {
                case "add":
                    for (int i = 0; i < M1.GetLength(0); i++)
                    {
                        for (int j = 0; j < M1.GetLength(1); j++)
                        {
                            M3[i, j] = M1[i, j] + M2[i, j];
                        }
                    }
                    break;
                case "subtract":
                    for (int i = 0; i < M1.GetLength(0); i++)
                    {
                        for (int j = 0; j < M1.GetLength(1); j++)
                        {
                            M3[i, j] = M1[i, j] - M2[i, j];
                        }
                    }
                    break;
                case "multiply":
                    for (int i = 0; i < M1.GetLength(0); i++)
                    {
                        for (int j = 0; j < M1.GetLength(1); j++)
                        {
                            M3[i, j] = M1[i, j] * M2[i, j];
                        }
                    }
                    break;
                case "divide":
                    for (int i = 0; i < M1.GetLength(0); i++)
                    {
                        for (int j = 0; j < M1.GetLength(1); j++)
                        {
                            M3[i, j] = M1[i, j] / M2[i, j];
                        }
                    }
                    break;
            }
            return M3;
        }



        public byte[,] MatrixScalarB(byte[,] M, double k, string type)
        {
            byte[,] D = new byte[M.GetLength(0), M.GetLength(1)];
            switch (type)
            {
                case "divide":
                    for (int i = 0; i < M.GetLength(0); i++)
                    {
                        for (int j = 0; j < M.GetLength(1); j++)
                        {
                            D[i, j] = Convert.ToByte(Math.Ceiling(Convert.ToDecimal(Convert.ToDouble(M[i, j]) / k)));
                        }
                    }
                    break;
                case "multiply":
                    for (int i = 0; i < M.GetLength(0); i++)
                    {
                        for (int j = 0; j < M.GetLength(1); j++)
                        {
                            D[i, j] = Convert.ToByte(M[i, j] * k);
                        }
                    }
                    break;
                case "add":
                    for (int i = 0; i < M.GetLength(0); i++)
                    {
                        for (int j = 0; j < M.GetLength(1); j++)
                        {
                            D[i, j] = Convert.ToByte(M[i, j] + Convert.ToInt32(k));
                        }
                    }
                    break;
                case "subtract":
                    for (int i = 0; i < M.GetLength(0); i++)
                    {
                        for (int j = 0; j < M.GetLength(1); j++)
                        {
                            D[i, j] = Convert.ToByte(M[i, j] - Convert.ToInt32(k));
                        }
                    }
                    break;
                case "power":
                    for (int i = 0; i < M.GetLength(0); i++)
                    {
                        for (int j = 0; j < M.GetLength(1); j++)
                        {
                            D[i, j] = Convert.ToByte(Math.Pow(Convert.ToDouble(M[i, j]), k));
                        }
                    }
                    break;
            }
            return D;
        }



        public double[,] MatrixScalarD(double[,] M, double k, string type)
        {
            double[,] D = new double[M.GetLength(0), M.GetLength(1)];
            switch (type)
            {
                case "divide":
                    for (int i = 0; i < M.GetLength(0); i++)
                    {
                        for (int j = 0; j < M.GetLength(1); j++)
                        {
                            D[i, j] = M[i, j] / k;
                        }
                    }
                    break;
                case "multiply":
                    for (int i = 0; i < M.GetLength(0); i++)
                    {
                        for (int j = 0; j < M.GetLength(1); j++)
                        {
                            D[i, j] = M[i, j] * k;
                        }
                    }
                    break;
                case "add":
                    for (int i = 0; i < M.GetLength(0); i++)
                    {
                        for (int j = 0; j < M.GetLength(1); j++)
                        {
                            D[i, j] = M[i, j] + k;
                        }
                    }
                    break;
                case "subtract":
                    for (int i = 0; i < M.GetLength(0); i++)
                    {
                        for (int j = 0; j < M.GetLength(1); j++)
                        {
                            D[i, j] = M[i, j] - k;
                        }
                    }
                    break;
                case "power":
                    for (int i = 0; i < M.GetLength(0); i++)
                    {
                        for (int j = 0; j < M.GetLength(1); j++)
                        {
                            D[i, j] = Math.Pow(M[i, j], k);
                        }
                    }
                    break;
            }
            return D;
        }



        public int[,] MatrixScalarI(int[,] M, double k, string type)
        {
            int[,] D = new int[M.GetLength(0), M.GetLength(1)];
            switch (type)
            {
                case "divide":
                    for (int i = 0; i < M.GetLength(0); i++)
                    {
                        for (int j = 0; j < M.GetLength(1); j++)
                        {
                            D[i, j] = Convert.ToInt32(M[i, j] / k);
                        }
                    }
                    break;
                case "multiply":
                    for (int i = 0; i < M.GetLength(0); i++)
                    {
                        for (int j = 0; j < M.GetLength(1); j++)
                        {
                            D[i, j] = Convert.ToInt32(M[i, j] * k);
                        }
                    }
                    break;
                case "add":
                    for (int i = 0; i < M.GetLength(0); i++)
                    {
                        for (int j = 0; j < M.GetLength(1); j++)
                        {
                            D[i, j] = Convert.ToInt32(M[i, j] + k);
                        }
                    }
                    break;
                case "subtract":
                    for (int i = 0; i < M.GetLength(0); i++)
                    {
                        for (int j = 0; j < M.GetLength(1); j++)
                        {
                            D[i, j] = Convert.ToInt32(M[i, j] - k);
                        }
                    }
                    break;
                case "power":
                    for (int i = 0; i < M.GetLength(0); i++)
                    {
                        for (int j = 0; j < M.GetLength(1); j++)
                        {
                            D[i, j] = Convert.ToInt32(Math.Pow(Convert.ToDouble(M[i, j]), k));
                        }
                    }
                    break;
            }
            return D;
        }



        public int[,] MatrixBToI(byte[,] M)
        {
            int[,] I = new int[M.GetLength(0), M.GetLength(1)];
            for (int i = 0; i < M.GetLength(0); i++)
            {
                for (int j = 0; j < M.GetLength(1); j++)
                {
                    I[i, j] = Convert.ToInt32(M[i, j]);
                }
            }
            return I;
        }



        public byte[,] MatrixIToB(int[,] M)
        {
            byte[,] B = new byte[M.GetLength(0), M.GetLength(1)];
            for (int i = 0; i < M.GetLength(0); i++)
            {
                for (int j = 0; j < M.GetLength(1); j++)
                {
                    B[i, j] = Convert.ToByte(M[i, j]);
                }
            }
            return B;
        }



        public byte[,] MatrixDToB(double[,] M)
        {
            byte[,] B = new byte[M.GetLength(0), M.GetLength(1)];
            for (int i = 0; i < M.GetLength(0); i++)
            {
                for (int j = 0; j < M.GetLength(1); j++)
                {
                    B[i, j] = Convert.ToByte(M[i, j]);
                }
            }
            return B;
        }



        public int[,] MatrixDToI(double[,] M)
        {
            int[,] I = new int[M.GetLength(0), M.GetLength(1)];
            for (int i = 0; i < M.GetLength(0); i++)
            {
                for (int j = 0; j < M.GetLength(1); j++)
                {
                    I[i, j] = Convert.ToInt32(M[i, j]);
                }
            }
            return I;
        }



        public double[,] MatrixBToD(byte[,] M)
        {
            double[,] D = new double[M.GetLength(0), M.GetLength(1)];
            for (int i = 0; i < M.GetLength(0); i++)
            {
                for (int j = 0; j < M.GetLength(1); j++)
                {
                    D[i, j] = Convert.ToDouble(M[i, j]);
                }
            }
            return D;
        }



        public int[] ElementWiseVector(int[] M1, int[] M2, string type)
        {
            //Dimension of M1 and M2 have to be the same
            int[] M3 = new int[M1.Length];
            switch (type)
            {
                case "add":
                    for (int i = 0; i < M1.GetLength(0); i++)
                    {
                        M3[i] = M1[i] + M2[i];
                    }
                    break;
                case "subtract":
                    for (int i = 0; i < M1.GetLength(0); i++)
                    {
                        M3[i] = M1[i] - M2[i];
                    }
                    break;
                case "multiply":
                    for (int i = 0; i < M1.GetLength(0); i++)
                    {
                        M3[i] = Convert.ToInt32(M1[i] * M2[i]);
                    }
                    break;
                case "divide":
                    for (int i = 0; i < M1.GetLength(0); i++)
                    {
                        M3[i] = Convert.ToInt32(Convert.ToDouble(M1[i]) / M2[i]);
                    }
                    break;
            }
            return M3;
        }



        public int[] VectorScalarOpI(int[] M, double k, string type)
        {
            int[] D = new int[M.Length];
            switch (type)
            {
                case "divide":
                    for (int i = 0; i < M.Length; i++)
                    {
                        D[i] = Convert.ToInt32(M[i] / k);
                    }
                    break;
                case "multiply":
                    for (int i = 0; i < M.Length; i++)
                    {
                        D[i] = Convert.ToInt32(M[i] * k);
                    }
                    break;
                case "add":
                    for (int i = 0; i < M.Length; i++)
                    {
                        D[i] = Convert.ToInt32(M[i] + k);
                    }
                    break;
                case "subtract":
                    for (int i = 0; i < M.Length; i++)
                    {
                        D[i] = Convert.ToInt32(M[i] - k);
                    }
                    break;
                case "power":
                    for (int i = 0; i < M.Length; i++)
                    {
                        D[i] = Convert.ToInt32(Math.Pow(Convert.ToDouble(M[i]), k));
                    }
                    break;
            }
            return D;
        }



        public int SumI(int[,] M)
        {
            int sum = 0;
            for (int i = 0; i < M.GetLength(0); i++)
            {
                for (int j = 0; j < M.GetLength(1); j++)
                {
                    sum = sum + M[i, j];
                }
            }
            return sum;
        }



        public int[] SumRowI(int[,] M)
        {
            int[] sum = new int[M.GetLength(1)];
            for (int i = 0; i < M.GetLength(1); i++)
            {
                for (int j = 0; j < M.GetLength(0); j++)
                {
                    sum[i] = sum[i] + M[j, i];
                }
            }
            return sum;
        }



        public int[,] SumColI(int[,] M)
        {
            int[,] sum = new int[M.GetLength(0), 1];
            for (int i = 0; i < M.GetLength(0); i++)
            {
                for (int j = 0; j < M.GetLength(1); j++)
                {
                    sum[i, 0] = sum[i, 0] + M[i, j];
                }
            }
            return sum;
        }



        public int SumVectorI(int[] M)
        {
            int sum = 0;
            for (int i = 0; i < M.Length; i++)
            {
                sum = sum + M[i];
            }
            return sum;
        }



        public double SumD(double[,] M)
        {
            double sum = 0;
            for (int i = 0; i < M.GetLength(0); i++)
            {
                for (int j = 0; j < M.GetLength(1); j++)
                {
                    sum = sum + M[i, j];
                }
            }
            return sum;
        }


        //public byte padarray(byte[,] M, int pad, byte content)
        //{
        //    int row = M.GetLength(0);
        //    int col = M.GetLength(1);
        //    byte[,] newM = new byte[row + pad, col + pad];

        //    int i;
        //    //atas
        //    for(i = 0; i < newM.GetLength(1); i++)
        //    {
        //        newM[i, j] = content;
        //    }

        //    //bawah
        //    for(i = Array.GetUpperBound(newM); a)

        //    for(int i = 0; i < pad; i++)
        //    {
        //        for(int j = 0; j < newM.GetLength(1); j++)
        //        {
        //            newM[i, j] = content;
        //        }
        //    }

        //    //bawah
        //    for (int i = 0; i < pad; i++)
        //    {
        //        for (int j = 0; j < newM.GetLength(1); j++)
        //        {
        //            newM[i, j] = content;
        //        }
        //    }
        //}
    }
}
