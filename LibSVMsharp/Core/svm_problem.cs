using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LibSVMsharp.Core
{
    [StructLayout(LayoutKind.Sequential)]
    public struct svm_problem
    {
        public int l;
        public IntPtr y; // double*
        public IntPtr x; // svm_node**
    }
}
