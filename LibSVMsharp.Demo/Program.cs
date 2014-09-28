using LibSVMsharp;
using LibSVMsharp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibSVMsharp.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            SVMProblem problem = SVMProblemHelper.Load(@"Datasets\wine.txt");
            problem = SVMProblemHelper.Normalize(problem, SVMNormType.L2); // Optional

            SVMParameter parameter = new SVMParameter();
            parameter.Type = SVMType.C_SVC;
            parameter.Kernel = SVMKernelType.RBF;
            parameter.C = 1;
            parameter.Gamma = 1;

            // Do 10-fold cross validation
            double[] target;
            SVM.CrossValidation(problem, parameter, 10, out target);

            double crossValidationAccuracy = SVMHelper.EvaluateClassificationProblem(problem, target);
            
            // Train the model
            SVMModel model = SVM.Train(problem, parameter);

            double correct = 0;
            for (int i = 0; i < problem.Length; i++)
            {
                double y = SVM.Predict(model, problem.X[i]);
                if (y == problem.Y[i])
                    correct++;
            }

            double trainingAccuracy = correct / (double)problem.Length;

            Console.WriteLine("\nCross validation accuracy: " + crossValidationAccuracy);
            Console.WriteLine("\nTraining accuracy: " + trainingAccuracy);

            Console.ReadLine();
        }
    }
}
