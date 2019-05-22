using System.Runtime.InteropServices;

namespace LibSVMsharp.Core
{
    [StructLayout(LayoutKind.Sequential)]
    public struct svm_node
    {
        public int index;
        public double value;
    }
}
