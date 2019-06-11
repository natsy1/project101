using System;
using weka.classifiers.functions;
using weka.core.converters;
using weka.classifiers.misc;
using weka.core;
using weka.classifiers;

namespace project101
{
    class MultiLayerPerceptron
    {
        public string CompositionClassification()
        {
            //Inisialization of Train Data
            ConverterUtils.DataSource trainsource = new ConverterUtils.DataSource("CompTrainInstances.arff");
            Instances traininstances = trainsource.getDataSet();
            traininstances.setClassIndex(traininstances.numAttributes() - 1);

            //Get single Test Instance from CSV file
            CSVLoader loader = new CSVLoader();
            loader.setSource(new java.io.File("GetCompFeatures.csv"));
            Instances testinstances = loader.getDataSet();
            testinstances.setClassIndex(testinstances.numAttributes() - 1);
            Instance sekarang = testinstances.lastInstance();

            //Get and build saved model
            Classifier model = new MultilayerPerceptron();
            SerializationHelper.write("CompModel.model", model);
            Classifier svm = (Classifier)SerializationHelper.read("CompModel.model");
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


        public string EchogenicityClassification()
        {
            //Inisialization of Train Data
            ConverterUtils.DataSource trainsource = new ConverterUtils.DataSource("EchoTrainInstances.arff");
            Instances traininstances = trainsource.getDataSet();
            traininstances.setClassIndex(traininstances.numAttributes() - 1);

            //Get single Test Instance from CSV file
            CSVLoader loader = new CSVLoader();
            loader.setSource(new java.io.File("GetEchoFeatures.csv"));
            Instances testinstances = loader.getDataSet();
            testinstances.setClassIndex(testinstances.numAttributes() - 1);
            Instance sekarang = testinstances.lastInstance();

            //Get and build saved model
            Classifier model = new MultilayerPerceptron();
            SerializationHelper.write("EchoModel.model", model);
            Classifier svm = (Classifier)SerializationHelper.read("EchoModel.model");
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
    }
}
