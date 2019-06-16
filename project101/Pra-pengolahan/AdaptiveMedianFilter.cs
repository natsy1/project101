using System;

namespace project101
{
    public class AdaptiveMedianFilter
    {
        public const int w = 3;
        public const int wMax = 17;
        int Zxy, Zmax, Zmin, Zmed;
        byte A1, A2, B1, B2, NewPixelVal;
        int x, y;
        byte[] ImgWindow;
        byte[,] imageMatrix;
        readonly byte[,] InitImage;

        public AdaptiveMedianFilter(byte[,] image, int wMax)
        {
            imageMatrix = new byte[image.GetLength(0), image.GetLength(1)];
            InitImage = InitArray(image, wMax);
        }


        public void Filter()
        {
            int x_init = wMax / 2;
            int y_init = wMax / 2;

            for (int X = 0; X < imageMatrix.GetLength(0); X++)
            {
                for (int Y = 0; Y < imageMatrix.GetLength(1); Y++)
                {
                    FindMedian(Window(x_init + X, y_init + Y, InitImage, w, wMax), w, InitImage, x_init + X, y_init + Y);

                    imageMatrix[X, Y] = NewPixelVal;
                }
            }
        }


        private byte[] Window(int X, int Y, byte[,] original, int w, int wMax)
        {
            ImgWindow = new byte[w * w];

            x = X - (w / 2);
            y = Y - (w / 2);
            int k = 0;
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    ImgWindow[k] = original[x, y];
                    y++;
                    k++;
                }
                y = Y - (w / 2);
                x++;
            }
            return ImgWindow;
        }


        private void FindMedian(byte[] ImgWindow, int w, byte[,] image, int X, int Y)
        {
            int right = ImgWindow.Length - 1;
            QuickSort_Recursive(ImgWindow, 0, right);

            Zxy = image[X, Y];
            Zmax = ImgWindow[(w * w) - 1];
            Zmin = ImgWindow[0];
            Zmed = ImgWindow[(int)((w * w) / 2)];
            A1 = (byte)(Zmed - Zmin);
            A2 = (byte)(Zmax - Zmed);
            if (A1 > 0 && A2 > 0)
            {
                B1 = (byte)(Zxy - Zmin);
                B2 = (byte)(Zmax - Zxy);
                if (B1 > 0 && B2 > 0)
                {
                    NewPixelVal = (byte)Zxy;
                }
                else
                {
                    NewPixelVal = (byte)Zmed;
                }
            }
            else
            {
                w += 2;
                if (w <= wMax)
                {
                    FindMedian(Window(X, Y, image, w, wMax), w, image, X, Y);
                }
                else
                {
                    NewPixelVal = (byte)Zmed;
                }
            }

        }


        public byte[,] GetResult()
        {
            Console.WriteLine(imageMatrix);
            return imageMatrix;
        }


        public byte[,] InitArray(byte[,] image, int wMax)
        {
            int X_mask1 = wMax;
            int X_mask2 = 1;
            int Y_mask1 = wMax;
            int Y_mask2 = 1;
            byte[,] initImage = new byte[image.GetLength(0) + (wMax) - 1, image.GetLength(1) + (wMax) - 1];

            for (int i = 0; i < initImage.GetLength(0); i++)
            {
                Y_mask1 = wMax;
                Y_mask2 = 1;

                for (int j = 0; j < initImage.GetLength(1); j++)
                {
                    if (i < wMax / 2 && j >= wMax / 2 && i < initImage.GetLength(0) - (wMax / 2) && j < initImage.GetLength(1) - (wMax / 2))
                    {
                        initImage[i, j] = image[X_mask1, j - (wMax / 2)];
                    }
                    else if (i >= initImage.GetLength(0) - (wMax / 2) && j >= wMax / 2 && j < initImage.GetLength(1) - (wMax / 2))
                    {
                        initImage[i, j] = image[i - (wMax / 2) - X_mask2, j - (wMax / 2)];
                    }

                    else if (j < wMax / 2 && i >= wMax / 2 && i < initImage.GetLength(0) - (wMax / 2))
                    {
                        initImage[i, j] = image[i - (wMax / 2), Y_mask1];
                    }
                    else if (j >= initImage.GetLength(1) - (wMax / 2) && i >= wMax / 2 && i < initImage.GetLength(0) - (wMax / 2))
                    {
                        initImage[i, j] = image[i - (wMax / 2), j - (wMax / 2) - Y_mask2];
                    }

                    else if (i >= wMax / 2 && j >= wMax / 2 && i < initImage.GetLength(0) - (wMax / 2) && j < initImage.GetLength(1) - (wMax / 2))
                    {
                        initImage[i, j] = image[i - (wMax / 2), j - (wMax / 2)];
                    }

                    else
                    {
                        initImage[i, j] = 0;
                    }

                    Y_mask1--;
                    if (j >= initImage.GetLength(1) - (wMax / 2))
                    { Y_mask2++; }

                }
                X_mask1--;
                if (i >= initImage.GetLength(0) - (wMax / 2))
                { X_mask2++; }

            }
            return initImage;
        }


        static public int Partition(byte[] numbers, int left, int right)
        {
            int pivot = numbers[left];
            int i = left;
            int j = right;
            while (true)
            {
                if (numbers[i] <= pivot)
                    i++;
                if (numbers[j] >= pivot)
                    j--;
                if (i < j && numbers[i] > numbers[j])
                {
                    Swap(ref numbers[i], ref numbers[j]);
                }
                else if (i > j)
                    break;
            }
            Swap(ref numbers[j], ref numbers[left]);
            return j;
        }

        static public void QuickSort_Recursive(byte[] arr, int left, int right)
        {
            // For Recusrion
            if (left < right)
            {
                int pivot = Partition(arr, left, right);

                if (pivot > 1)
                    QuickSort_Recursive(arr, left, pivot - 1);

                if (pivot + 1 < right)
                    QuickSort_Recursive(arr, pivot + 1, right);
            }
        }

        public static void Swap<T>(ref T lhs, ref T rhs)
        {
            T temp;
            temp = lhs;
            lhs = rhs;
            rhs = temp;
        }
    }

}