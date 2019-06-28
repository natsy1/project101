namespace project101
{
    class InternalFeatures
    {
        FiturHistogram hist = new FiturHistogram();
        FiturTekstur tekstur = new FiturTekstur();
        FiturLaws law = new FiturLaws();

        byte[,] BW;
        double[][] glcm;
        double[] glrlm;
        double[] lacunarity;
        double[] HF;
        byte[][,] Laws;

        public InternalFeatures(byte[,] matrixNodule)
        {
            BW = matrixNodule;
            glcm = tekstur.GLCM(BW);
            glrlm = tekstur.GLRLM(BW);
            lacunarity = tekstur.Lacunarity(BW);
            HF = hist.Histogram(BW);
            Laws = law.Laws(BW);
        }


        #region CompositionFeatures
        public double[][] GLCM()
        {
            //double[][] glcm = tekstur.GLCM(BW);
            return glcm;
        }
        
        public double[] GLRLM()
        {
            //double[] glrlm = tekstur.GLRLM(BW);
            return glrlm;
        }
        
        public double[] Lacunarity()
        {
            //double[] lacunarity = tekstur.Lacunarity(BW);
            return lacunarity;
        }
        #endregion


        #region EchogenicityFeatures
        public double[] FiturHistogram()
        {
            //double[] HF = hist.Histogram(BW);
            return HF;
        }

        public double[][] FiturLaws()
        {
            //byte[][,] Laws = law.Laws(BW);
            double[] law0 = law.LawsStatistik(Laws[0]);
            double[] law1 = law.LawsStatistik(Laws[1]);
            double[] law2 = law.LawsStatistik(Laws[2]);
            double[] law3 = law.LawsStatistik(Laws[3]);
            double[] law4 = law.LawsStatistik(Laws[4]);
            double[] law5 = law.LawsStatistik(Laws[5]);
            double[] law6 = law.LawsStatistik(Laws[6]);
            double[] law7 = law.LawsStatistik(Laws[7]);
            double[] law8 = law.LawsStatistik(Laws[8]);

            double[][] L = new double[][] { law0, law1, law2, law3, law4, law5, law6, law7, law8 };
            return L;
        }
        #endregion
    }
}
