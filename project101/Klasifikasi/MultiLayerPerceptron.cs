using weka.classifiers.functions;
using weka.core.converters;
using weka.core;

namespace project101
{
    class MultiLayerPerceptron
    {
        public double CompositionClassification()
        {
            //Get single Test Instance from CSV file
            CSVLoader loader = new CSVLoader();
            loader.setSource(new java.io.File("GetComposition.csv"));
            Instances testinstances = loader.getDataSet();
            testinstances.setClassIndex(testinstances.numAttributes() - 1);
            Instance sekarang = testinstances.lastInstance();

            //Get and build saved model
            MultilayerPerceptron model = new MultilayerPerceptron();
            model = (MultilayerPerceptron)SerializationHelper.read("CompositionModel.model");

            //Classify actual test instance
            double clsValue = model.classifyInstance(sekarang);
            
            return clsValue;
        }

        public double EchogenicityClassification()
        {
            //Get single Test Instance from CSV file
            CSVLoader loader = new CSVLoader();
            loader.setSource(new java.io.File("GetEchogenity.csv"));
            Instances testinstances = loader.getDataSet();
            testinstances.setClassIndex(testinstances.numAttributes() - 1);
            Instance sekarang = testinstances.lastInstance();

            //Get and build saved model
            MultilayerPerceptron model = new MultilayerPerceptron();
            model = (MultilayerPerceptron)SerializationHelper.read("EchogenityModel.model");

            //Classify actual test instance
            double clsValue = model.classifyInstance(sekarang);
            
            return clsValue;
        }
    }
}
