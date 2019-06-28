namespace project101
{
    class ExternalFeatures
    {
        FiturGeometris geo = new FiturGeometris();
        MomenCitra momen = new MomenCitra();

        byte[,] a;
        double rasiotl;
        double rec;
        double dispersion;
        double compactness;
        double circularity;
        double convex;
        double tortuosity;
        double aspectratio;
        double nilaiCC;
        double[] momenHu;
        double[] momenZer;

        public ExternalFeatures(byte[,] matrixMasking)
        {
            a = matrixMasking;
            rasiotl = geo.RasioTL(a);
            rec = geo.Rectangularity(a);
            dispersion = geo.Dispersion(a);
            compactness = geo.Compactness(a);
            circularity = geo.Circularity(a);
            convex = geo.Convexity(a);
            tortuosity = geo.Tortuosity(a);
            aspectratio = geo.AspectRatio(a);
            nilaiCC = geo.SelisihKodeRantai(a);
            momenHu = momen.MomenHu(a);
            momenZer = momen.MomenZernike(a, 4);
        }

        public double[] getMargin()
        {
            double[] MarginFeatures = new double[] { rasiotl, rec, dispersion, compactness, circularity, convex, tortuosity, aspectratio, nilaiCC };
            //double[][] extFeature = new double[][] { MarginFeatures, ShapeFeatures };
            return MarginFeatures;
        }

        public double[] getShape()
        {
            double[] ShapeFeatures = new double[]
            {
                rasiotl, rec, dispersion, compactness, circularity, convex, tortuosity,
                momenHu[0], momenHu[1], momenHu[2], momenHu[3], momenHu[4], momenHu[5], momenHu[6],
                momenZer[0], momenZer[1], momenZer[2], momenZer[3], momenZer[4], momenZer[5], momenZer[0]
            };
            return ShapeFeatures;
        }

        #region MarginFeatures
        public double RasioTL(byte[,] a)
        {
            rasiotl = geo.RasioTL(a);
            return rasiotl;
        }

        public double Rectangularity(byte[,] a)
        {
            rec = geo.Rectangularity(a);
            return rec;
        }

        public double Dispersion(byte[,] a)
        {
            dispersion = geo.Dispersion(a);
            return dispersion;
        }

        public double Compactness(byte[,] a)
        {
            compactness = geo.Compactness(a);
            return compactness;
        }

        public double Circularity(byte[,] a)
        {
            circularity = geo.Circularity(a);
            return circularity;
        }

        public double Convexity(byte[,] a)
        {
            convex = geo.Convexity(a);
            return convex;
        }

        public double Tortuosity(byte[,] a)
        {
            tortuosity = geo.Tortuosity(a);
            return tortuosity;
        }

        public double AspectRatio(byte[,] a)
        {
            aspectratio = geo.AspectRatio(a);
            return aspectratio;
        }

        public double SelisihKodeRantai(byte[,] a)
        {
            nilaiCC = geo.SelisihKodeRantai(a);
            return nilaiCC;
        }
        #endregion
        
        #region ShapeFeatures
        public double[] MomenHu(byte[,] BW)
        {
            momenHu = momen.MomenHu(BW);
            return momenHu;
        }

        public double[] MomenZernike(byte[,] BW)
        {
            momenZer = momen.MomenZernike(BW, 4);
            return momenZer;
        }
        #endregion
    }
}
