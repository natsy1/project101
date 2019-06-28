using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project101
{
    class ActiveContour
    {
        public ActiveContour()
        {
            //Find Contour
            //Image<Gray, byte> binaryContour = grayBf.Convert<Gray, byte>().ThresholdBinaryInv(new Gray(65), new Gray(255));
            //Image<Gray, byte> grayContour = grayBf;
            //VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            //Mat hier = new Mat();

            //CvInvoke.FindContours(binaryContour, contours, hier, RetrType.List, ChainApproxMethod.ChainApproxNone);
            //double maxArea = 0; int idx = 0;
            //for (int i = 0; i < contours.Size; i++)
            //{
            //    double area = CvInvoke.ContourArea(contours[i]);
            //    if (maxArea < area) { maxArea = area; idx = i; }
            //}

            //VectorOfPoint hull = new VectorOfPoint();
            //double eps = 0.0015;
            //while (true)
            //{
            //    double epsilon = eps * CvInvoke.ArcLength(contours[idx], true);
            //    CvInvoke.ApproxPolyDP(contours[idx], hull, epsilon, true);
            //    if (hull.Size >= 225) { break; }
            //    else { eps = eps * 0.99; }
            //}
            ////CvInvoke.ConvexHull(contours[idx], hull, false, true);
            ////CvInvoke.DrawContours(grayOne, contours, idx, new MCvScalar(255, 0, 0), 2);
            //grayContour.DrawPolyline(hull.ToArray(), true, new Gray(255));

            //OutputBox.Image = grayContour.Bitmap;
            //InputBox.Image = grayBf.Bitmap;
            //OutputBox.Refresh(); InputBox.Refresh();
        }
    }
}
