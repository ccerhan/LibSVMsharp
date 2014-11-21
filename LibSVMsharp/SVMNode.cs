using LibSVMsharp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace LibSVMsharp
{
    public class SVMNode : IEquatable<SVMNode>
    {
        public SVMNode(int index, double value)
        {
            Index = index;
            Value = value;
        }
        public SVMNode()
            : this(0, 0)
        {

        }

        public int Index { get; set; }
        public double Value { get; set; }

        public bool Equals(SVMNode x)
        {
            if (x == null) return false;
            return Index.Equals(x.Index) && Value.Equals(x.Value);
        }
        public override int GetHashCode()
        {
            return Index * (int) (Value * 100000000.0);
        }
        public SVMNode Clone()
        {
            SVMNode y = new SVMNode();
            y.Index = Index;
            y.Value = Value;
            return y;
        }

        public static SVMNode[] Convert(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
                return null;

            List<SVMNode> nodes = new List<SVMNode>();
            IntPtr i_ptr_nodes = ptr;
            while (true)
            {
                svm_node node = (svm_node)Marshal.PtrToStructure(i_ptr_nodes, typeof(svm_node));
                i_ptr_nodes = IntPtr.Add(i_ptr_nodes, Marshal.SizeOf(typeof(svm_node)));
                if (node.index > 0)
                {
                    nodes.Add(new SVMNode(node.index, node.value));
                }
                else
                {
                    break;
                }
            }

            return nodes.ToArray();
        }
        public static IntPtr Allocate(SVMNode[] x)
        {
            if (x == null)
                return IntPtr.Zero;

            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(svm_node)) * x.Length);
            IntPtr i_ptr_nodes = ptr;
            for (int i = 0; i < x.Length; i++)
            {
                svm_node node = new svm_node();
                node.index = x[i].Index;
                node.value = x[i].Value;
                Marshal.StructureToPtr(node, i_ptr_nodes, true);
                i_ptr_nodes = IntPtr.Add(i_ptr_nodes, Marshal.SizeOf(typeof(svm_node)));
            }

            return ptr;
        }
        public static void Free(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
                return;

            Marshal.DestroyStructure(ptr, typeof(IntPtr));
            Marshal.FreeHGlobal(ptr);
            ptr = IntPtr.Zero;
        }
    }
}
