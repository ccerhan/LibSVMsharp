using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LibSVMsharp.Core
{
    [StructLayout(LayoutKind.Sequential)]
    public struct svm_model
    {
        [MarshalAs(UnmanagedType.Struct, SizeConst = 96)]
        public svm_parameter param;
        public int nr_class;
        public int l;
        public IntPtr SV; // svm_node**
        public IntPtr sv_coef; // double**
        public IntPtr rho; // double*
        public IntPtr probA;	// double*
        public IntPtr probB; // double*
        public IntPtr sv_indices; // int*
        public IntPtr label; // int*	
        public IntPtr nSV; // int*
        public int free_sv;
    }
}
