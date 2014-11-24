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
    }
}
