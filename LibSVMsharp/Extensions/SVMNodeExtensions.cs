using LibSVMsharp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibSVMsharp.Extensions
{
    public static class SVMNodeExtensions
    {
        public static SVMNode[] Normalize(this SVMNode[] x, SVMNormType type)
        {
            return SVMNodeHelper.Normalize(x, type);
        }
        public static double Predict(this SVMNode[] x, SVMModel model)
        {
            return SVM.Predict(model, x);
        }
        public static double Predict(this SVMNode[] x, IntPtr ptr_model)
        {
            return SVM.Predict(ptr_model, x);
        }
        public static double PredictProbability(this SVMNode[] x, SVMModel model, out double[] estimations)
        {
            return SVM.PredictProbability(model, x, out estimations);
        }
        public static double PredictProbability(this SVMNode[] x, IntPtr ptr_model, out double[] estimations)
        {
            return SVM.PredictProbability(ptr_model, x, out estimations);
        }
        public static double PredictValues(this SVMNode[] x, SVMModel model, out double[] values)
        {
            return SVM.PredictValues(model, x, out values);
        }
        public static double PredictValues(this SVMNode[] x, IntPtr ptr_model, out double[] values)
        {
            return SVM.PredictValues(ptr_model, x, out values);
        }
    }
}
