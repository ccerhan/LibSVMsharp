using LibSVMsharp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace LibSVMsharp
{
    public class SVMParameter
    {
        public SVMParameter()
        {
            Type = SVMType.C_SVC;
            Kernel = SVMKernelType.RBF;
            Degree = 3;
            Gamma = 1; // divided by num_of_features
            Coef0 = 0;
            C = 1;
            Nu = 0.5;
            P = 0.1;
            CacheSize = 100;
            Eps = 0.001;
            Shrinking = true;
            Probability = false;
            WeightLabels = new int[0];
            Weights = new double[0];
        } 

        /// <summary>
        /// Type of a SVM formulation. Possible values are: 
        /// [C_SVC] C-Support Vector Classification. n-class classification (n >= 2), allows imperfect separation of classes with penalty multiplier C for outliers. 
        /// [NU_SVC] nu-Support Vector Classification. n-class classification with possible imperfect separation. Parameter Nu (in the range 0..1, the larger the value, the smoother the decision boundary) is used instead of C. 
        /// [ONE_CLASS] Distribution Estimation (One-class SVM). All the training data are from the same class, SVM builds a boundary that separates the class from the rest of the feature space. 
        /// [EPS_SVR] epsilon-Support Vector Regression. The distance between feature vectors from the training set and the fitting hyper-plane must be less than P. For outliers the penalty multiplier C is used.
        /// [NU_SVR] nu-Support Vector Regression. Nu is used instead of p.
        /// </summary>
        public SVMType Type { get; set; }
        /// <summary>
        /// Type of a SVM kernel. Possible values are:
        /// [LINEAR] Linear kernel. No mapping is done, linear discrimination (or regression) is done in the original feature space. It is the fastest option.
        /// [POLY] Polynomial kernel.
        /// [RBF] Radial basis function (RBF), a good choice in most cases.
        /// [SIGMOID] Sigmoid kernel.
        /// </summary>
        public SVMKernelType Kernel { get; set; }
        /// <summary>
        /// Parameter degree of a kernel function (POLY).
        /// </summary>
        public int Degree { get; set; }
        /// <summary>
        /// Parameter gamma of a kernel function (POLY / RBF / SIGMOID).
        /// </summary>
        public double Gamma { get; set; }
        /// <summary>
        /// Parameter coef0 of a kernel function (POLY / SIGMOID).
        /// </summary>
        public double Coef0 { get; set; }
        /// <summary>
        /// Unit in MegaBytes.
        /// </summary>
        public double CacheSize { get; set; }
        /// <summary>
        /// Term criteria. Tolerance of the iterative SVM training procedure which solves a partial case of constrained quadratic optimization problem
        /// </summary>
        public double Eps { get; set; }
        /// <summary>
        /// Parameter C of a SVM optimization problem (C_SVC / EPS_SVR / NU_SVR).
        /// </summary>
        public double C { get; set; }
        /// <summary>
        ///  Optional weights in the C_SVC problem , assigned to particular classes.
        /// </summary>
        public int[] WeightLabels { get; set; }
        /// <summary>
        ///  Optional weights in the C_SVC problem , assigned to particular classes.
        /// </summary>
        public double[] Weights { get; set; }
        /// <summary>
        /// Parameter nu of a SVM optimization problem (NU_SVC / ONE_CLASS / NU_SVR).
        /// </summary>
        public double Nu { get; set; }
        /// <summary>
        /// Parameter epsilon of a SVM optimization problem (EPS_SVR).
        /// </summary>
        public double P { get; set; }
        /// <summary>
        /// Use the shrinking heuristics.
        /// </summary>
        public bool Shrinking { get; set; }
        /// <summary>
        /// Train a SVC or SVR model for probability estimates.
        /// </summary>
        public bool Probability { get; set; }

        public SVMParameter Clone()
        {
            SVMParameter y = new SVMParameter();
            y.Type = Type;
            y.Kernel = Kernel;
            y.Degree = Degree;
            y.Gamma = Gamma;
            y.Coef0 = Coef0;
            y.C = C;
            y.Nu = Nu;
            y.P = P;
            y.CacheSize = CacheSize;
            y.Eps = Eps;
            y.Shrinking = Shrinking;
            y.Probability = Probability;
            y.WeightLabels = WeightLabels.Select(a => a).ToArray();
            y.Weights = Weights.Select(a => a).ToArray();
            return y;
        }

        public static SVMParameter Convert(svm_parameter x)
        {
            SVMParameter y = new SVMParameter();
            y.Type = (SVMType)x.svm_type;
            y.Kernel = (SVMKernelType)x.kernel_type;
            y.Degree = x.degree;
            y.Gamma = x.gamma;
            y.Coef0 = x.coef0;
            y.CacheSize = x.cache_size;
            y.Eps = x.eps;
            y.C = x.C;
            y.Nu = x.nu;
            y.P = x.p;
            y.Shrinking = x.shrinking != 0;
            y.Probability = x.probability != 0;

            int length = x.nr_weight;
            y.WeightLabels = new int[length];
            if (length > 0)
                Marshal.Copy(x.weight_label, y.WeightLabels, 0, length);

            y.Weights = new double[length];
            if (length > 0)
                Marshal.Copy(x.weight, y.Weights, 0, length);

            return y;
        }
        public static SVMParameter Convert(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
                return null;

            svm_parameter x = (svm_parameter)Marshal.PtrToStructure(ptr, typeof(svm_parameter));
            return SVMParameter.Convert(x);
        }
        public static IntPtr Allocate(SVMParameter x)
        {
            if (x == null)
                return IntPtr.Zero;

            svm_parameter y = new svm_parameter();
            y.svm_type = (int)x.Type;
            y.kernel_type = (int)x.Kernel;
            y.degree = x.Degree;
            y.gamma = x.Gamma;
            y.coef0 = x.Coef0;
            y.cache_size = x.CacheSize;
            y.eps = x.Eps;
            y.C = x.C;
            y.nu = x.Nu;
            y.p = x.P;
            y.shrinking = x.Shrinking ? 1 : 0;
            y.probability = x.Probability ? 1 : 0;
            y.nr_weight = x.WeightLabels.Length;

            y.weight_label = IntPtr.Zero;
            if (y.nr_weight > 0)
            {
                y.weight_label = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)) * x.WeightLabels.Length);
                Marshal.Copy(x.WeightLabels, 0, y.weight_label, x.WeightLabels.Length);
            }

            y.weight = IntPtr.Zero;
            if (y.nr_weight > 0)
            {
                y.weight = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(double)) * x.Weights.Length);
                Marshal.Copy(x.Weights, 0, y.weight, x.Weights.Length);
            }

            int size = Marshal.SizeOf(y);
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(y, ptr, true);

            return ptr;
        }
        public static void Free(svm_parameter x)
        {
            Marshal.FreeHGlobal(x.weight);
            x.weight = IntPtr.Zero;

            Marshal.FreeHGlobal(x.weight_label);
            x.weight_label = IntPtr.Zero;
        }
        public static void Free(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
                return;

            svm_parameter x = (svm_parameter)Marshal.PtrToStructure(ptr, typeof(svm_parameter));

            SVMParameter.Free(x);

            Marshal.DestroyStructure(ptr, typeof(svm_parameter));
            Marshal.FreeHGlobal(ptr);
            ptr = IntPtr.Zero;
        }
    }
}
