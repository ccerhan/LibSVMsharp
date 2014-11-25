using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibSVMsharp.Helpers
{
    public enum SVMNormType
    {
        L1 = 1,
        L2 = 2,
        L3 = 3,
        L4 = 4,
        L5 = 5
    }

    public static class SVMProblemHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="problem"></param>
        /// <returns></returns>
        public static SVMProblem RemoveDuplicates(SVMProblem problem)
        {
            SVMProblem temp = new SVMProblem();
            for (int i = 0; i < problem.Length; i++)
            {
                bool same = false;
                for (int j = i + 1; j < problem.Length; j++)
                {
                    same |= SVMNodeHelper.IsEqual(problem.X[i], problem.Y[i], problem.X[j], problem.Y[j]);

                    if (same)
                    {
                        break;
                    }
                }

                if (!same)
                {
                    temp.Add(problem.X[i], problem.Y[i]);
                }
            }

            return temp;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="problem"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static SVMProblem Normalize(SVMProblem problem, SVMNormType type)
        {
            SVMProblem temp = new SVMProblem();
            for (int i = 0; i < problem.Length; i++)
            {
                SVMNode[] x = SVMNodeHelper.Normalize(problem.X[i], type);
                temp.Add(x, problem.Y[i]);
            }
            return temp;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="problem"></param>
        /// <returns></returns>
        public static Dictionary<double, int> GetLabelsCount(SVMProblem problem)
        {
            Dictionary<double, int> dic = new Dictionary<double, int>();
            for (int i = 0; i < problem.Length; i++)
            {
                if (!dic.ContainsKey(problem.Y[i]))
                {
                    dic.Add(problem.Y[i], 1);
                }
                else
                {
                    dic[problem.Y[i]]++;
                }
            }
            return dic;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="problem"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool Save(SVMProblem problem, string filename)
        {
            if (String.IsNullOrWhiteSpace(filename) || problem == null || problem.Length == 0)
            {
                return false;
            }

            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";

            using (StreamWriter sw = new StreamWriter(filename))
            {
                for (int i = 0; i < problem.Length; i++)
                {
                    sw.Write(problem.Y[i]);

                    if (problem.X[i].Length > 0)
                    {
                        sw.Write(" ");

                        for (int j = 0; j < problem.X[i].Length; j++)
                        {
                            sw.Write(problem.X[i][j].Index);
                            sw.Write(":");
                            sw.Write(problem.X[i][j].Value.ToString(provider));

                            if (j < problem.X[i].Length - 1)
                            {
                                sw.Write(" ");
                            }
                        }
                    }

                    sw.Write("\n");
                }
            }

            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static SVMProblem Load(string filename)
        {
            if (String.IsNullOrWhiteSpace(filename) || !File.Exists(filename))
            {
                return null;
            }

            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";

            SVMProblem problem = new SVMProblem();
            using (StreamReader sr = new StreamReader(filename))
            {
                while (true)
                {
                    string line = sr.ReadLine();
                    if (line == null)
                        break;

                    string[] list = line.Trim().Split(' ');

                    double y = Convert.ToDouble(list[0].Trim(), provider);

                    List<SVMNode> nodes = new List<SVMNode>();
                    for (int i = 1; i < list.Length; i++)
                    {
                        string[] temp = list[i].Split(':');
                        SVMNode node = new SVMNode();
                        node.Index = Convert.ToInt32(temp[0].Trim());
                        node.Value = Convert.ToDouble(temp[1].Trim(), provider);
                        nodes.Add(node);
                    }

                    problem.Add(nodes.ToArray(), y);
                }
            }

            return problem;
        }
    }
}
