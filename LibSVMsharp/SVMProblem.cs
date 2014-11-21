using LibSVMsharp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace LibSVMsharp
{
    public class SVMProblem
    {
        public SVMProblem()
        {
            Y = new List<double>();
            X = new List<SVMNode[]>();
        }

        public int Length { get { return Y.Count; } }
        public List<double> Y { get; private set; }
        public List<SVMNode[]> X { get; private set; }

        public void Add(SVMNode[] x, double y)
        {
            if (x.Length > 0)
            {
                SVMNode[] nodes = x.OrderBy(a => a.Index).ToArray();
                X.Add(nodes);
                Y.Add(y);
            }
        }
        public void RemoveAt(int index)
        {
            if (index < Length)
            {
                Y.RemoveAt(index);
                X.RemoveAt(index);
            }
        }
        public void Insert(int index, SVMNode[] x, double y)
        {
            if (x.Length > 0)
            {
                SVMNode[] nodes = x.OrderBy(a => a.Index).ToArray();
                X.Insert(index, x);
                Y.Insert(index, y);
            }
        }
        public SVMProblem Clone()
        {
            SVMProblem y = new SVMProblem();
            for (int i = 0; i < Length; i++)
            {
                SVMNode[] nodes = X[i].Select(x => x.Clone()).ToArray();
                y.Add(nodes, Y[i]);
            }
            return y;
        }

        public static SVMProblem Convert(svm_problem x)
        {
            double[] y_array = new double[x.l];
            Marshal.Copy(x.y, y_array, 0, y_array.Length);

            List<SVMNode[]> x_array = new List<SVMNode[]>();
            IntPtr i_ptr_x = x.x;
            for (int i = 0; i < x.l; i++)
            {
                IntPtr ptr_nodes = (IntPtr)Marshal.PtrToStructure(i_ptr_x, typeof(IntPtr));
                SVMNode[] nodes = SVMNode.Convert(ptr_nodes);
                x_array.Add(nodes);
                i_ptr_x = IntPtr.Add(i_ptr_x, Marshal.SizeOf(typeof(IntPtr)));
            }

            SVMProblem y = new SVMProblem();
            for (int i = 0; i < x.l; i++)
            {
                y.Add(x_array[i], y_array[i]);
            }

            return y;
        }
        public static SVMProblem Convert(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
                return null;

            svm_problem x = (svm_problem)Marshal.PtrToStructure(ptr, typeof(svm_problem));
            return SVMProblem.Convert(x);
        }
        public static IntPtr Allocate(SVMProblem x)
        {
            if (x == null || x.X == null || x.Y == null || x.Length < 1)
            {
                return IntPtr.Zero;
            }

            svm_problem y = new svm_problem();
            y.l = x.Length;

            // Allocate problem.y
            y.y = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(double)) * x.Y.Count);
            Marshal.Copy(x.Y.ToArray(), 0, y.y, x.Y.Count);

            // Allocate problem.x
            y.x = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)) * x.X.Count);
            IntPtr i_ptr_x = y.x;
            for (int i = 0; i < x.X.Count; i++)
            {
                // Prepare each node array 
                // 1) All nodes containing zero value is removed 
                // 2) A node which index is -1 is added to the end
                List<SVMNode> temp = x.X[i].Where(a => a.Value != 0).ToList();
                temp.Add(new SVMNode(-1, 0));
                SVMNode[] nodes = temp.ToArray();

                // Allocate node array
                IntPtr ptr_nodes = SVMNode.Allocate(nodes);
                Marshal.StructureToPtr(ptr_nodes, i_ptr_x, true);

                i_ptr_x = IntPtr.Add(i_ptr_x, Marshal.SizeOf(typeof(IntPtr)));
            }

            // Allocate the problem
            int size = Marshal.SizeOf(y);
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(y, ptr, true);

            return ptr;
        }
        public static void Free(svm_problem x)
        {
            Marshal.FreeHGlobal(x.y);
            x.y = IntPtr.Zero;

            IntPtr i_ptr_x = x.x;
            for (int i = 0; i < x.l; i++)
            {
                IntPtr ptr_nodes = (IntPtr)Marshal.PtrToStructure(i_ptr_x, typeof(IntPtr));
                SVMNode.Free(ptr_nodes);

                i_ptr_x = IntPtr.Add(i_ptr_x, Marshal.SizeOf(typeof(IntPtr)));
            }

            Marshal.FreeHGlobal(x.x);
            x.x = IntPtr.Zero;
        }
        public static void Free(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
                return;

            svm_problem x = (svm_problem)Marshal.PtrToStructure(ptr, typeof(svm_problem));

            SVMProblem.Free(x);

            Marshal.DestroyStructure(ptr, typeof(svm_problem));
            Marshal.FreeHGlobal(ptr);
            ptr = IntPtr.Zero;
        }
    }
}
