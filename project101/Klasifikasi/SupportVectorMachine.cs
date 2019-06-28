using System;
using weka.classifiers.functions;
using weka.core.converters;
using weka.classifiers.misc;
using weka.core;
using weka.classifiers;

namespace project101
{
    class SupportVectorMachine
    {
        public double MarginClassification()
        {
            //Get single Test Instance from CSV file
            CSVLoader loader = new CSVLoader();
            loader.setSource(new java.io.File("GetMargin.csv"));
            Instances testinstances = loader.getDataSet();
            testinstances.setClassIndex(testinstances.numAttributes() - 1);
            Instance sekarang = testinstances.lastInstance();

            //Get and build saved model
            LibSVM svm = new LibSVM();
            svm = (LibSVM)SerializationHelper.read("MarginModel.model");
                       
            //Classify actual test instance
            double clsValue = svm.classifyInstance(sekarang);
            Console.WriteLine(clsValue);

            return clsValue;
        }//0=ireguler, 1=halus


        public double[] ShapeClassification()
        {
            //Get single Test Instance from CSV file
            CSVLoader loader = new CSVLoader();
            loader.setSource(new java.io.File("GetOrientation.csv"));
            Instances testinstances = loader.getDataSet();
            testinstances.setClassIndex(testinstances.numAttributes() - 1);
            Instance sekarang = testinstances.lastInstance();

            //Get and build saved model
            LibSVM modelShape = new LibSVM();
            LibSVM modelOri = new LibSVM();
            modelOri = (LibSVM)SerializationHelper.read("OrientationModel.model");
            modelShape = (LibSVM)SerializationHelper.read("ShapeModel.model");

            //Classify actual test instance
            double valueShape = modelShape.classifyInstance(sekarang);
            double valueOri = modelOri.classifyInstance(sekarang);
            Console.WriteLine(valueOri);
            Console.WriteLine(valueShape);
            double[] value = new double[] { valueShape, valueOri };

            return value;
        }//0=ireguler, 1=roundtooval
        

        //public double OrientationClassification()
        //{
        //    //Get single Test Instance from CSV file
        //    CSVLoader loader = new CSVLoader();
        //    loader.setSource(new java.io.File("GetShape.csv"));
        //    Instances testinstances = loader.getDataSet();
        //    testinstances.setClassIndex(testinstances.numAttributes() - 1);
        //    Instance sekarang = testinstances.lastInstance();

        //    //Get and build saved model
        //    LibSVM model = new LibSVM();
        //    model = (LibSVM)SerializationHelper.read("OrientationModel.model");

        //    //Classify actual test instance
        //    double clsValue = model.classifyInstance(sekarang);
        //    Console.WriteLine(clsValue);

        //    return clsValue;
        //} //0 = paralel, 1 = nonparalel
    }
}
