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
        public string MarginClassification()
        {
            //Inisialization of Train Data
            ConverterUtils.DataSource trainsource = new ConverterUtils.DataSource("MarginTrainInstances.arff");
            Instances traininstances = trainsource.getDataSet();
            traininstances.setClassIndex(traininstances.numAttributes() - 1);

            //Get single Test Instance from CSV file
            CSVLoader loader = new CSVLoader();
            loader.setSource(new java.io.File("GetMarginFeatures.csv"));
            Instances testinstances = loader.getDataSet();
            testinstances.setClassIndex(testinstances.numAttributes() - 1);
            Instance sekarang = testinstances.lastInstance();

            //Get and build saved model
            Classifier model = new LibSVM();
            SerializationHelper.write("MarginModel.model", model);
            Classifier svm = (Classifier)SerializationHelper.read("MarginModel.model");
            InputMappedClassifier imc = new InputMappedClassifier();
            imc.setClassifier(svm);
            imc.buildClassifier(traininstances);
            //svm.setOptions();

            //Classify actual test instance
            double clsValue = imc.classifyInstance(sekarang);
            sekarang.setClassValue(clsValue);
            double angka = sekarang.classValue();
            string kelas = sekarang.toString(sekarang.numAttributes() - 1);
            Console.WriteLine(sekarang.classValue());
            Console.WriteLine(sekarang.toString(sekarang.numAttributes() - 1));

            //Evaluation eval = new Evaluation(traininstances);
            //eval.evaluateModel(svm, testinstances);
            //Console.WriteLine(eval.toSummaryString());
            return kelas;
        }


        public string ShapeClassification()
        {
            //Inisialization of Train Data
            ConverterUtils.DataSource trainsource = new ConverterUtils.DataSource("ShapeTrainInstances.arff");
            Instances traininstances = trainsource.getDataSet();
            traininstances.setClassIndex(traininstances.numAttributes() - 1);

            //Get single Test Instance from CSV file
            CSVLoader loader = new CSVLoader();
            loader.setSource(new java.io.File("GetShapeFeatures.csv"));
            Instances testinstances = loader.getDataSet();
            testinstances.setClassIndex(testinstances.numAttributes() - 1);
            Instance sekarang = testinstances.lastInstance();

            //Get and build saved model
            Classifier model = new LibSVM();
            SerializationHelper.write("ShapeModel.model", model);
            Classifier svm = (Classifier)SerializationHelper.read("ShapeModel.model");
            InputMappedClassifier imc = new InputMappedClassifier();
            imc.setClassifier(svm);
            imc.buildClassifier(traininstances);
            //svm.setOptions();

            //Classify actual test instance
            double clsValue = imc.classifyInstance(sekarang);
            sekarang.setClassValue(clsValue);
            double angka = sekarang.classValue();
            string kelas = sekarang.toString(sekarang.numAttributes() - 1);
            Console.WriteLine(sekarang.classValue());
            Console.WriteLine(sekarang.toString(sekarang.numAttributes() - 1));

            //Evaluation eval = new Evaluation(traininstances);
            //eval.evaluateModel(svm, testinstances);
            //Console.WriteLine(eval.toSummaryString());
            return kelas;
        }
        

        public void OrientationClassification(double kelasShape)
        {
            //string kelasOri;
            //if (kelasShape == 0) { kelasOri = ""; }
            //else if (kelasShape == 1) { kelasOri = ""; }
            //return kelasOri;
        }
    }
}
