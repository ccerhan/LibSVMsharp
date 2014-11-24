using LibSVMsharp;
using LibSVMsharp.Helpers;
using LibSVMsharp.Extensions;
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
            //problem = problem.Normalize(SVMNormType.L2); // Extension method usage

            SVMParameter parameter = new SVMParameter();
            parameter.Type = SVMType.C_SVC;
            parameter.Kernel = SVMKernelType.RBF;
            parameter.C = 1;
            parameter.Gamma = 1;

            // Do 10-fold cross validation
            double[] target;
            SVM.CrossValidation(problem, parameter, 10, out target);
            //problem.CrossValidation(parameter, 10, out target); // Extension method usage

            double crossValidationAccuracy = SVMHelper.EvaluateClassificationProblem(problem, target);
            //double crossValidationAccuracy = problem.EvaluateClassificationProblem(target); // Extension method usage
            
            // Train the model
            SVMModel model = SVM.Train(problem, parameter);
            //SVMModel model = problem.Train(parameter); // Extension method usage

            double correct = 0;
            for (int i = 0; i < problem.Length; i++)
            {
                double y = SVM.Predict(model, problem.X[i]);
                //double y = problem.X[i].Predict(model); // Extension method usage
                if (y == problem.Y[i])
                    correct++;
            }
            //double[] target = problem.Predict(model); // Extension method usage

            double trainingAccuracy = correct / (double)problem.Length;

            Console.WriteLine("\nCross validation accuracy: " + crossValidationAccuracy);
            Console.WriteLine("\nTraining accuracy: " + trainingAccuracy);

            Console.ReadLine();
        }
    }
}
