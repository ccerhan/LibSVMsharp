using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibSVMsharp.Helpers
{
    public static class SVMNodeHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        public static bool IsEqual(SVMNode[] x1, double y1, SVMNode[] x2, double y2)
        {
            bool same = false;
            if (y1 == y2 && x1.Length == x2.Length)
            {
                same = true;
                for (int i = 0; i < x1.Length; i++)
                {
                    same &= x1[i].Equals(x2[i]);
                }
            }
            return same;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static SVMNode[] Normalize(SVMNode[] x, SVMNormType type)
        {
            double norm_l = (double)(int)type;
            double norm = x.Sum(a => Math.Pow(a.Value, norm_l));
            norm = Math.Pow(norm, 1 / norm_l);
            SVMNode[] y = x.Select(a => new SVMNode(a.Index, a.Value / norm)).ToArray();
            return y;
        }
    }
}
