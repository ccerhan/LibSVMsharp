using LibSVMsharp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace LibSVMsharp
{
    public class SVMModel
    {
        public enum CreationType
        {
            TRAIN = 0,
            LOAD_MODEL = 1
        }

        public SVMModel()
        {
            Parameter = new SVMParameter();
            ClassCount = 0;
            TotalSVCount = 0;
            SV = null;
            SVCoefs = null;
            Rho = null;
            ProbabilityA = null;
            ProbabilityB = null;
            SVIndices = null;
            Labels = null;
            SVCounts = null;
            Creation = CreationType.LOAD_MODEL;
        }

        /// <summary>
        /// SVM parameter.
        /// </summary>
        public SVMParameter Parameter { get; set; }
        /// <summary>
        /// Number of classes, = 2 in regression/one class svm.
        /// </summary>
        public int ClassCount { get; set; }
        /// <summary>
        /// Total support vector count.
        /// </summary>
        public int TotalSVCount { get; set; }
        /// <summary>
        /// Support vectors (SV[TotalSVCount])
        /// </summary>
        public List<SVMNode[]> SV { get; set; }
        /// <summary>
        /// Coefficients for SVs in decision functions (sv_coef[ClassCount-1][TotalSVCount])
        /// </summary>
        public List<double[]> SVCoefs { get; set; }
        /// <summary>
        /// Constants in decision functions (rho[ClassCount*(ClassCount-1)/2])
        /// </summary>
        public double[] Rho { get; set; }
        /// <summary>
        /// Pariwise probability information (A)
        /// </summary>
        public double[] ProbabilityA { get; set; }
        /// <summary>
        /// Pariwise probability information (B)
        /// </summary>
        public double[] ProbabilityB { get; set; }
        /// <summary>
        /// SVIndices[0,...,SVCounts-1] are values in [1,...,num_traning_data] to indicate SVs in the training set.
        /// </summary>
        public int[] SVIndices { get; set; }
        /// <summary>
        /// Label of each class (Labels[ClassCount]).
        /// </summary>
        public int[] Labels { get; set; }
        /// <summary>
        /// Number of SVs for each class (SVCounts[ClassCount]). SVCounts[0] + SVCounts[1] + ... + SVCounts[ClassCount-1] = TotalSVCount.
        /// </summary>
        public int[] SVCounts { get; set; }
        /// <summary>
        /// Creation type of the model.
        /// </summary>
        public CreationType Creation { get; set; }

        public SVMModel Clone()
        {
            SVMModel y = new SVMModel();

            if (Parameter != null)
                y.Parameter = Parameter.Clone();

            y.ClassCount = ClassCount;
            y.TotalSVCount = TotalSVCount;

            if (SV != null)
                y.SV = SV.Select(a => a.Select(b => b.Clone()).ToArray()).ToList();

            if (SVCoefs != null)
                y.SVCoefs = SVCoefs.Select(a => a.Select(b => b).ToArray()).ToList();

            if (Rho != null)
                y.Rho = Rho.Select(a => a).ToArray();

            if (ProbabilityA != null)
                y.ProbabilityA = ProbabilityA.Select(a => a).ToArray();

            if (ProbabilityB != null)
                y.ProbabilityB = ProbabilityB.Select(a => a).ToArray();

            if (SVIndices != null)
                y.SVIndices = SVIndices.Select(a => a).ToArray();

            if (Labels != null)
                y.Labels = Labels.Select(a => a).ToArray();

            if (SVCounts != null)
                y.SVCounts = SVCounts.Select(a => a).ToArray();

            y.Creation = Creation;
            return y;
        }

        public static SVMModel Convert(svm_model x)
        {
            SVMModel y = new SVMModel();
            y.Creation = (CreationType)x.free_sv;
            y.ClassCount = x.nr_class;
            y.TotalSVCount = x.l;

            if (y.Creation == CreationType.LOAD_MODEL)
            {
                y.Parameter = new SVMParameter();
                y.Parameter.Type = (SVMType)x.param.svm_type;
                y.Parameter.Kernel = (SVMKernelType)x.param.kernel_type;
                switch (y.Parameter.Kernel)
                {
                    case SVMKernelType.LINEAR:
                        break;
                    case SVMKernelType.POLY:
                        y.Parameter.Gamma = x.param.gamma;
                        y.Parameter.Coef0 = x.param.coef0;
                        y.Parameter.Degree = x.param.degree;
                        break;
                    case SVMKernelType.RBF:
                        y.Parameter.Gamma = x.param.gamma;
                        break;
                    case SVMKernelType.SIGMOID:
                        y.Parameter.Gamma = x.param.gamma;
                        y.Parameter.Coef0 = x.param.coef0;
                        break;
                }
            }
            else
            {
                y.Parameter = SVMParameter.Convert(x.param);
            }

            int problemCount = (int)(y.ClassCount * (y.ClassCount - 1) * 0.5);

            y.Rho = new double[problemCount];
            Marshal.Copy(x.rho, y.Rho, 0, y.Rho.Length);

            y.ProbabilityA = null;
            if (x.probA != IntPtr.Zero)
            {
                y.ProbabilityA = new double[problemCount];
                Marshal.Copy(x.probA, y.ProbabilityA, 0, y.ProbabilityA.Length);
            }

            y.ProbabilityB = null;
            if (x.probB != IntPtr.Zero)
            {
                y.ProbabilityB = new double[problemCount];
                Marshal.Copy(x.probB, y.ProbabilityB, 0, y.ProbabilityB.Length);
            }

            if (x.nSV != IntPtr.Zero)
            {
                y.SVCounts = new int[y.ClassCount];
                Marshal.Copy(x.nSV, y.SVCounts, 0, y.SVCounts.Length);

                y.Labels = new int[y.ClassCount];
                Marshal.Copy(x.label, y.Labels, 0, y.Labels.Length);
            }

            y.SVCoefs = new List<double[]>(y.ClassCount - 1);
            IntPtr i_ptr_svcoef = x.sv_coef;
            for (int i = 0; i < y.ClassCount - 1; i++)
            {
                y.SVCoefs.Add(new double[y.TotalSVCount]);
                IntPtr coef_ptr = (IntPtr)Marshal.PtrToStructure(i_ptr_svcoef, typeof(IntPtr));
                Marshal.Copy(coef_ptr, y.SVCoefs[i], 0, y.SVCoefs[i].Length);
                i_ptr_svcoef = IntPtr.Add(i_ptr_svcoef, Marshal.SizeOf(typeof(IntPtr)));
            }

            y.SVIndices = null;
            if (x.sv_indices != IntPtr.Zero)
            {
                y.SVIndices = new int[y.TotalSVCount];
                Marshal.Copy(x.sv_indices, y.SVIndices, 0, y.SVIndices.Length);
            }

            y.SV = new List<SVMNode[]>();
            IntPtr i_ptr_sv = x.SV;
            for (int i = 0; i < x.l; i++)
            {
                IntPtr ptr_nodes = (IntPtr)Marshal.PtrToStructure(i_ptr_sv, typeof(IntPtr));
                SVMNode[] nodes = SVMNode.Convert(ptr_nodes);
                y.SV.Add(nodes);
                i_ptr_sv = IntPtr.Add(i_ptr_sv, Marshal.SizeOf(typeof(IntPtr)));
            }

            return y;
        }
        public static SVMModel Convert(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
                return null;

            svm_model x = (svm_model)Marshal.PtrToStructure(ptr, typeof(svm_model));
            return SVMModel.Convert(x);
        }
        public static IntPtr Allocate(SVMModel x)
        {
            if (x == null || x.ClassCount < 1 || x.Parameter == null || x.Rho == null || x.Rho.Length < 1 ||
                x.SVCoefs == null || x.SVCoefs.Count < 1 || x.TotalSVCount < 1)
            {
                return IntPtr.Zero;
            }

            if (x.Parameter.Type != SVMType.EPSILON_SVR && x.Parameter.Type != SVMType.NU_SVR &&
                x.Parameter.Type != SVMType.ONE_CLASS &&
                (x.Labels == null || x.Labels.Length < 1 || x.SVCounts == null || x.SVCounts.Length < 1))
            {
                return IntPtr.Zero;
            }

            svm_model y = new svm_model();
            y.nr_class = x.ClassCount;
            y.l = x.TotalSVCount;
            y.free_sv = (int)x.Creation;

            // Allocate model.parameter
            IntPtr ptr_param = SVMParameter.Allocate(x.Parameter);
            y.param = (svm_parameter)Marshal.PtrToStructure(ptr_param, typeof(svm_parameter));
            SVMParameter.Free(ptr_param);
            
            // Allocate model.rho
            y.rho = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(double)) * x.Rho.Length);
            Marshal.Copy(x.Rho, 0, y.rho, x.Rho.Length);

            // Allocate model.probA
            y.probA = IntPtr.Zero;
            if (x.ProbabilityA != null)
            {
                y.probA = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(double)) * x.ProbabilityA.Length);
                Marshal.Copy(x.ProbabilityA, 0, y.probA, x.ProbabilityA.Length);
            }

            // Allocate model.probB
            y.probB = IntPtr.Zero;
            if (x.ProbabilityB != null)
            {
                y.probB = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(double)) * x.ProbabilityB.Length);
                Marshal.Copy(x.ProbabilityB, 0, y.probB, x.ProbabilityB.Length);
            }

            if (x.Parameter.Type != SVMType.EPSILON_SVR && x.Parameter.Type != SVMType.NU_SVR &&
                x.Parameter.Type != SVMType.ONE_CLASS)
            {
                // Allocate model.label
                y.label = Marshal.AllocHGlobal(Marshal.SizeOf(typeof (int))*x.Labels.Length);
                Marshal.Copy(x.Labels, 0, y.label, x.Labels.Length);

                // Allocate model.nSV
                y.nSV = Marshal.AllocHGlobal(Marshal.SizeOf(typeof (int))*x.SVCounts.Length);
                Marshal.Copy(x.SVCounts, 0, y.nSV, x.SVCounts.Length);
            }
            else
            {
                y.label = IntPtr.Zero;
                y.nSV = IntPtr.Zero;
            }

            // Allocate model.sv_coef
            y.sv_coef = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)) * x.SVCoefs.Count);
            IntPtr i_ptr_svcoef = y.sv_coef;
            for (int i = 0; i < x.SVCoefs.Count; i++)
            {
                IntPtr temp = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(double)) * x.SVCoefs[i].Length);
                Marshal.Copy(x.SVCoefs[i], 0, temp, x.SVCoefs[i].Length);
                Marshal.StructureToPtr(temp, i_ptr_svcoef, true);
                i_ptr_svcoef = IntPtr.Add(i_ptr_svcoef, Marshal.SizeOf(typeof(IntPtr)));
            }

            // Allocate model.sv_indices
            y.sv_indices = IntPtr.Zero;
            if (x.SVIndices != null)
            {
                y.sv_indices = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)) * x.SVIndices.Length);
                Marshal.Copy(x.SVIndices, 0, y.sv_indices, x.SVIndices.Length);
            }

            // Allocate model.SV
            y.SV = IntPtr.Zero;
            if (x.SV != null)
            {
                y.SV = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)) * x.SV.Count);
                IntPtr i_ptr_sv = y.SV;
                for (int i = 0; i < x.SV.Count; i++)
                {
                    // Prepare each node array 
                    // 1) All nodes containing zero value is removed 
                    // 2) A node which index is -1 is added to the end
                    List<SVMNode> temp = x.SV[i].Where(a => a.Value != 0).ToList();
                    temp.Add(new SVMNode(-1, 0));
                    SVMNode[] nodes = temp.ToArray();

                    // Allocate node array
                    IntPtr ptr_nodes = SVMNode.Allocate(nodes);
                    Marshal.StructureToPtr(ptr_nodes, i_ptr_sv, true);

                    i_ptr_sv = IntPtr.Add(i_ptr_sv, Marshal.SizeOf(typeof(IntPtr)));
                }
            }

            // Allocate the model
            int size = Marshal.SizeOf(y);
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(y, ptr, true);

            return ptr;
        }
        public static void Free(svm_model x)
        {
            Marshal.FreeHGlobal(x.rho);
            x.rho = IntPtr.Zero;

            Marshal.FreeHGlobal(x.probA);
            x.probA = IntPtr.Zero;

            Marshal.FreeHGlobal(x.probB);
            x.probB = IntPtr.Zero;

            Marshal.FreeHGlobal(x.sv_indices);
            x.sv_indices = IntPtr.Zero;

            Marshal.FreeHGlobal(x.label);
            x.label = IntPtr.Zero;

            Marshal.FreeHGlobal(x.nSV);
            x.nSV = IntPtr.Zero;

            SVMParameter.Free(x.param);

            IntPtr i_ptr_sv = x.SV;
            for (int i = 0; i < x.l; i++)
            {
                IntPtr ptr_nodes = (IntPtr)Marshal.PtrToStructure(i_ptr_sv, typeof(IntPtr));
                SVMNode.Free(ptr_nodes);

                i_ptr_sv = IntPtr.Add(i_ptr_sv, Marshal.SizeOf(typeof(IntPtr)));
            }

            Marshal.FreeHGlobal(x.SV);
            x.SV = IntPtr.Zero;

            IntPtr i_ptr_svcoef = x.sv_coef;
            for (int i = 0; i < x.nr_class - 1; i++)
            {
                IntPtr temp = (IntPtr)Marshal.PtrToStructure(i_ptr_svcoef, typeof(IntPtr));
                Marshal.FreeHGlobal(temp);
                temp = IntPtr.Zero;

                i_ptr_svcoef = IntPtr.Add(i_ptr_svcoef, Marshal.SizeOf(typeof(IntPtr)));
            }

            Marshal.FreeHGlobal(x.sv_coef);
            x.sv_coef = IntPtr.Zero;
        }
        public static void Free(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
                return;

            svm_model x = (svm_model)Marshal.PtrToStructure(ptr, typeof(svm_model));

            SVMModel.Free(x);

            Marshal.DestroyStructure(ptr, typeof(svm_model));
            Marshal.FreeHGlobal(ptr);
            ptr = IntPtr.Zero;
        }
    }
}
