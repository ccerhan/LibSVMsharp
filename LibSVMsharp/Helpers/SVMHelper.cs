using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibSVMsharp.Helpers
{
    public static class SVMHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="testset"></param>
        /// <param name="target"></param>
        /// <returns>Accuracy for C_SVC, NU_SVC and ONE_CLASS.</returns>
        public static double EvaluateClassificationProblem(SVMProblem testset, double[] target)
        {
            if (testset.Length != target.Length)
            {
                return -1;
            }

            int total_correct = 0;
            for (int i = 0; i < testset.Length; i++)
            {
                int y = (int)testset.Y[i];
                int v = (int)target[i];

                if (y == v)
                {
                    ++total_correct;
                }
            }

            return 100.0 * ((double)total_correct / (double)testset.Length);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="testset"></param>
        /// <param name="target"></param>
        /// <param name="labels"></param>
        /// <param name="confusionMatrix"></param>
        /// <returns>Accuracy for C_SVC, NU_SVC and ONE_CLASS.</returns>
        public static double EvaluateClassificationProblem(SVMProblem testset, double[] target, int[] labels, out int[,] confusionMatrix)
        {
            if (testset.Length != target.Length)
            {
                confusionMatrix = null;
                return -1;
            }

            Dictionary<int, int> indexes = new Dictionary<int, int>();
            for (int i = 0; i < labels.Length; i++)
            {
                indexes.Add(labels[i], i);
            }
            
            confusionMatrix = new int[labels.Length, labels.Length];

            int total_correct = 0;
            for (int i = 0; i < testset.Length; i++)
            {
                int y = (int)testset.Y[i];
                int v = (int)target[i];

                confusionMatrix[indexes[y], indexes[v]]++;

                if (y == v)
                {
                    ++total_correct;
                }
            }

            return 100.0 * ((double)total_correct / (double)testset.Length);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="testset"></param>
        /// <param name="target"></param>
        /// <param name="correlationCoeff">Squared correlation coefficient for EPSILON_SVR and NU_SVR.</param>
        /// <returns>Mean squared error for EPSILON_SVR and NU_SVR.</returns>
        public static double EvaluateRegressionProblem(SVMProblem testset, double[] target, out double correlationCoeff)
        {
            double total_error = 0;
            double sumv = 0, sumy = 0, sumvv = 0, sumyy = 0, sumvy = 0;
            for (int i = 0; i < testset.Length; i++)
            {
                double y = testset.Y[i];
                double v = target[i];
                total_error += (v - y) * (v - y);
                sumv += v;
                sumy += y;
                sumvv += v * v;
                sumyy += y * y;
                sumvy += v * y;
            }

            double mean_squared_error = total_error / (double)testset.Length;
            correlationCoeff =
                (((double)testset.Length * sumvy - sumv * sumy) * ((double)testset.Length * sumvy - sumv * sumy)) /
                (((double)testset.Length * sumvv - sumv * sumv) * ((double)testset.Length * sumyy - sumy * sumy));

            return mean_squared_error;
        }
    }
}
