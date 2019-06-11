using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project101
{
    class ExternalFeatures
    {
        FiturGeometris geo = new FiturGeometris();
        MomenCitra momen = new MomenCitra();


        #region MarginFeatures
        public double RasioTL(byte[,] a)
        {
            double rasiotl = geo.RasioTL(a);
            return rasiotl;
        }

        public double Rectangularity(byte[,] a)
        {
            double rec = geo.Rectangularity(a);
            return rec;
        }

        public double Dispersion(byte[,] a)
        {
            double dispersion = geo.Dispersion(a);
            return dispersion;
        }

        public double Compactness(byte[,] a)
        {
            double compactness = geo.Compactness(a);
            return compactness;
        }

        public double Circularity(byte[,] a)
        {
            double c = geo.Circularity(a);
            return c;
        }

        public double Convexity(byte[,] a)
        {
            double convex = geo.Convexity(a);
            return convex;
        }

        public double Tortuosity(byte[,] a)
        {
            double tortuosity = geo.Tortuosity(a);
            return tortuosity;
        }

        public double AspectRatio(byte[,] a)
        {
            double aspectratio = geo.AspectRatio(a);
            return aspectratio;
        }

        public double SelisihKodeRantai(byte[,] a)
        {
            double nilaiCC = geo.SelisihKodeRantai(a);
            return nilaiCC;
        }
        #endregion


        #region ShapeFeatures
        public double[] MomenHu(byte[,] BW)
        {
            double[] momenHu = momen.MomenHu(BW);
            return momenHu;
        }

        public double[] MomenZernike(byte[,] BW)
        {
            double[] momenZer = momen.MomenZernike(BW, 4);
            return momenZer;
        }
        #endregion
    }
}
