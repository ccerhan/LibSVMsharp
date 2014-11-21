using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibSVMsharp.Extensions
{
    public static class SVMModelExtensions
    {
        public static bool CheckProbabilityModel(this SVMModel model)
        {
            return SVM.CheckProbabilityModel(model);
        }
        public static double PredictProbability(this SVMModel model, SVMNode[] x, out double[] estimations)
        {
            return SVM.PredictProbability(model, x, out estimations);
        }
        public static double Predict(this SVMModel model, SVMNode[] x)
        {
            return SVM.Predict(model, x);
        }
        public static double PredictValues(this SVMModel model, SVMNode[] x, out double[] values)
        {
            return PredictValues(model, x, out values);
        }
        public static bool SaveModel(this SVMModel model, string filename)
        {
            return SVM.SaveModel(model, filename);
        }
    }
}
