using LibSVMsharp.Helpers;
using LibSVMsharp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace LibSVMsharp.Examples.RapidPrediction
{
    class Program
    {
        static Stopwatch sw = new Stopwatch();

        static void Main(string[] args)
        {
            SVMProblem testSet = SVMProblemHelper.Load(@"Dataset\wine.txt"); // Same as the training set
            SVMModel model = SVM.LoadModel(@"Model\wine_model.txt");

            Console.WriteLine("Feature count in one instance: " + model.SV[0].Length +"\n\n");

            // Test 1: Predict instances with SVMProblem's Predict extension method.

            sw.Start();

            double[] target = testSet.Predict(model);

            sw.Stop();
            double elapsedTimeInTest1 = (double)sw.ElapsedMilliseconds / (double)testSet.Length;

            Console.WriteLine("> Test 1: \nPredict instances with SVMProblem's Predict extension method.\n");
            Console.WriteLine("\tAverage elapsed time of one prediction: " + elapsedTimeInTest1 + " ms\n");

            // Test 2: Predict instances with RapidPreditor class which is an explicit implementation of the method used in Test 1.

            using (RapidPredictor predictor = new RapidPredictor(model)) // It needs to be Disposed
            {
                sw.Start();

                target = new double[testSet.Length];
                for (int i = 0; i < testSet.Length; i++)
                    target[i] = predictor.Predict(testSet.X[i]);

                sw.Stop();
            }
            double elapsedTimeInTest2 = (double)sw.ElapsedMilliseconds / (double)testSet.Length;

            Console.WriteLine("> Test 2: \nPredict instances with RapidPreditor class which is an explicit implementation of the method used in Test 1.\n");
            Console.WriteLine("\tAverage elapsed time of one prediction: " + elapsedTimeInTest2 + " ms\n");

            // Test 3: Predict instances with standard SVM.Predict method or SVMNode[]'s predict extension method.

            sw.Start();

            target = new double[testSet.Length];
            for (int i = 0; i < testSet.Length; i++)
                target[i] = SVM.Predict(model, testSet.X[i]);

            sw.Stop();
            double elapsedTimeInTest3 = (double)sw.ElapsedMilliseconds / (double)testSet.Length;

            Console.WriteLine("> Test 3: \nPredict instances with standard SVM.Predict method or SVMNode[]'s Predict extension method.\n");
            Console.WriteLine("\tAverage elapsed time of one prediction: " + elapsedTimeInTest3 + " ms\n");

            // Print the results
            Console.WriteLine("\nExplanation:\n");
            Console.WriteLine(
                "In standard SVM.Predict method, the SVMModel object is allocated and deallocated every time when the method called. " + 
                "Also the SVMNode[]'s Predict extension methods directly calls the SVM.Predict. " + 
                "However, the model is allocated once and is used to predict whole instances with its pointer in SVMProblem's " +
                "Predict extension method as implemented in the RapidPredictor class. You can take or modify this class in order " +
                "to use in your applications, if you have performance considerations. " +
                "I am not suggesting that SVMProblem's Predict extension method is used in real-time, because the model is allocated" +
                "in every method call.");

            Console.WriteLine("\n\nPress any key to quit...");
            Console.ReadLine();
        }
    }
}
