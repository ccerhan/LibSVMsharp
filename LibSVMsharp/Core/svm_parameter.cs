using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LibSVMsharp.Core
{
    [StructLayout(LayoutKind.Sequential)]
    public struct svm_parameter
    {
        public int svm_type;
        public int kernel_type;
        public int degree;
        public double gamma;
        public double coef0;
        public double cache_size;
        public double eps;
        public double C;
        public int nr_weight;
        public IntPtr weight_label; // int*
        public IntPtr weight; // double*
        public double nu;
        public double p;
        public int shrinking;
        public int probability;
    }
}
