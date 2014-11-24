using LibSVMsharp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibSVMsharp.Extensions
{
    public static class SVMProblemExtensions
    {
        public static string CheckParameter(this SVMProblem problem, SVMParameter parameter)
        {
            return SVM.CheckParameter(problem, parameter);
        }
        public static SVMProblem RemoveDuplicates(this SVMProblem problem)
        {
            return SVMProblemHelper.RemoveDuplicates(problem);
        }
        public static SVMProblem Normalize(this SVMProblem problem, SVMNormType type)
        {
            return SVMProblemHelper.Normalize(problem, type);
        }
        public static Dictionary<double, int> GetLabelsCount(this SVMProblem problem)
        {
            return SVMProblemHelper.GetLabelsCount(problem);
        }
        public static bool Save(this SVMProblem problem, string filename)
        {
            return SVMProblemHelper.Save(problem, filename);
        }
        public static double EvaluateClassificationProblem(this SVMProblem testset, double[] target)
        {
            return SVMHelper.EvaluateClassificationProblem(testset, target);
        }
        public static double EvaluateClassificationProblem(this SVMProblem testset, double[] target, int[] labels, out int[,] confusionMatrix)
        {
            return SVMHelper.EvaluateClassificationProblem(testset, target, labels, out confusionMatrix);
        }
        public static double EvaluateRegressionProblem(this SVMProblem testset, double[] target, out double correlation_coef)
        {
            return SVMHelper.EvaluateRegressionProblem(testset, target, out correlation_coef);
        }
        public static void CrossValidation(this SVMProblem problem, SVMParameter parameter, int nFolds, out double[] target)
        {
            SVM.CrossValidation(problem, parameter, nFolds, out target);
        }
        public static SVMModel Train(this SVMProblem problem, SVMParameter parameter)
        {
            return SVM.Train(problem, parameter);
        }
        public static double[] Predict(this SVMProblem problem, SVMModel model)
        {
            IntPtr ptr_model = SVMModel.Allocate(model);
            double[] target = problem.X.Select(x => x.Predict(ptr_model)).ToArray();
            SVMModel.Free(ptr_model);
            return target;
        }
        public static double[] PredictProbability(this SVMProblem problem, SVMModel model, out List<double[]> estimationsList)
        {
            IntPtr ptr_model = SVMModel.Allocate(model);

            List<double[]> temp = new List<double[]>();
            double[] target = problem.X.Select(x =>
            {
                double[] estimations;
                double y = x.PredictProbability(ptr_model, out estimations);
                temp.Add(estimations);
                return y;
            }).ToArray();

            SVMModel.Free(ptr_model);

            estimationsList = temp;
            return target;
        }
        public static double[] PredictValues(this SVMProblem problem, SVMModel model, out List<double[]> valuesList)
        {
            IntPtr ptr_model = SVMModel.Allocate(model);

            List<double[]> temp = new List<double[]>();
            double[] target = problem.X.Select(x =>
            {
                double[] estimations;
                double y = x.PredictProbability(ptr_model, out estimations);
                temp.Add(estimations);
                return y;
            }).ToArray();

            SVMModel.Free(ptr_model);

            valuesList = temp;
            return target;
        }
    }
}
