using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LibSVMsharp.Core
{
    [StructLayout(LayoutKind.Sequential)]
    public struct svm_node
    {
        internal int index;
        internal double value;
    }
}
