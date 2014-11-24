using LibSVMsharp.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace LibSVMsharp
{
    public enum SVMType : int
    {
        /// <summary>
        /// Multi-class classification.
        /// </summary>
        C_SVC = 0,
        /// <summary>
        /// Multi-class classification.
        /// </summary>
        NU_SVC = 1,
        /// <summary>
        /// One class SVM.
        /// </summary>
        ONE_CLASS = 2,
        /// <summary>
        /// Regression.
        /// </summary>
        EPSILON_SVR = 3,
        /// <summary>
        /// Regression.
        /// </summary>
        NU_SVR = 4
    }

    public enum SVMKernelType : int
    {
        /// <summary>
        /// Linear: u'*v
        /// </summary>
        LINEAR = 0,
        /// <summary>
        /// Polynomial: (gamma*u'*v + coef0)^degree
        /// </summary>
        POLY = 1,
        /// <summary>
        /// Radial Basis Function: exp(-gamma*|u-v|^2)
        /// </summary>
        RBF = 2,
        /// <summary>
        /// Sigmoid: tanh(gamma*u'*v + coef0)
        /// </summary>
        SIGMOID = 3
    }

    public delegate void SVMPrintFunction(string output);

    public static class SVM
    {
        public static string Version { get { return libsvm.VERSION; } }

        /// <summary>
        /// This function constructs and returns an SVM model according to the given training data and parameters.
        /// </summary>
        /// <param name="problem">Training data.</param>
        /// <param name="parameter">Parameter set.</param>
        /// <returns>SVM model according to the given training data and parameters.</returns>
        public static SVMModel Train(SVMProblem problem, SVMParameter parameter)
        {
            IntPtr ptr_problem = SVMProblem.Allocate(problem);
            IntPtr ptr_parameter = SVMParameter.Allocate(parameter);

            IntPtr ptr_model = libsvm.svm_train(ptr_problem, ptr_parameter);
            SVMModel model = SVMModel.Convert(ptr_model);

            SVMProblem.Free(ptr_problem);
            SVMParameter.Free(ptr_parameter);
            libsvm.svm_free_model_content(ptr_model);

            return model;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="problem"></param>
        /// <param name="parameter"></param>
        /// <param name="nFolds"></param>
        /// <param name="target"></param>
        public static void CrossValidation(SVMProblem problem, SVMParameter parameter, int nFolds, out double[] target)
        {
            IntPtr ptr_target = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(double)) * problem.Length);
            IntPtr ptr_problem = SVMProblem.Allocate(problem);
            IntPtr ptr_parameter = SVMParameter.Allocate(parameter);

            libsvm.svm_cross_validation(ptr_problem, ptr_parameter, nFolds, ptr_target);

            target = new double[problem.Length];
            Marshal.Copy(ptr_target, target, 0, target.Length);

            SVMProblem.Free(ptr_problem);
            SVMParameter.Free(ptr_parameter);
            Marshal.FreeHGlobal(ptr_target);
            ptr_target = IntPtr.Zero;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool SaveModel(SVMModel model, string filename)
        {
            if (String.IsNullOrWhiteSpace(filename) || model == null)
            {
                return false;
            }

            IntPtr ptr_model = SVMModel.Allocate(model);
            IntPtr ptr_filename = Marshal.StringToHGlobalAnsi(filename);

            bool success = libsvm.svm_save_model(ptr_filename, ptr_model) == 0;

            Marshal.FreeHGlobal(ptr_filename);
            ptr_filename = IntPtr.Zero;

            return success;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static SVMModel LoadModel(string filename)
        {
            if (String.IsNullOrWhiteSpace(filename) || !File.Exists(filename))
            {
                return null;
            }

            IntPtr ptr_filename = Marshal.StringToHGlobalAnsi(filename);

            IntPtr ptr_model = libsvm.svm_load_model(ptr_filename);

            Marshal.FreeHGlobal(ptr_filename);
            ptr_filename = IntPtr.Zero;

            if (ptr_model == IntPtr.Zero)
            {
                return null;
            }
            else
            {
                SVMModel model = SVMModel.Convert(ptr_model);

                // There is a little memory leackage here !!!
                libsvm.svm_free_model_content(ptr_model);
                ptr_model = IntPtr.Zero;

                return model;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="x"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static double PredictValues(SVMModel model, SVMNode[] x, out double[] values)
        {
            IntPtr ptr_model = SVMModel.Allocate(model);
            double result = PredictValues(ptr_model, x, out values);
            SVMModel.Free(ptr_model);
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptr_model"></param>
        /// <param name="x"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static double PredictValues(IntPtr ptr_model, SVMNode[] x, out double[] values)
        {
            if (ptr_model == IntPtr.Zero)
                throw new ArgumentNullException("ptr_model");

            int classCount = libsvm.svm_get_nr_class(ptr_model);
            int size = (int)(classCount * (classCount - 1) * 0.5);
            IntPtr ptr_values = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(double)) * size);

            List<SVMNode> nodes = x.Select(a => a.Clone()).ToList();
            nodes.Add(new SVMNode(-1, 0));
            IntPtr ptr_nodes = SVMNode.Allocate(nodes.ToArray());

            double result = libsvm.svm_predict_values(ptr_model, ptr_nodes, ptr_values);

            values = new double[size];
            Marshal.Copy(ptr_values, values, 0, values.Length);

            SVMNode.Free(ptr_nodes);
            Marshal.FreeHGlobal(ptr_values);
            ptr_values = IntPtr.Zero;

            return result;
        }
        /// <summary>
        /// This function does classification or regression on a test vector x given a model.
        /// </summary>
        /// <param name="model">SVM model.</param>
        /// <param name="x">Test vector.</param>
        /// <returns>For a classification model, the predicted class for x is returned.
        /// For a regression model, the function value of x calculated using the model is returned. 
        /// For an one-class model, +1 or -1 is returned.</returns>
        public static double Predict(SVMModel model, SVMNode[] x)
        {
            IntPtr ptr_model = SVMModel.Allocate(model);
            double result = Predict(ptr_model, x);
            SVMModel.Free(ptr_model);
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptr_model"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double Predict(IntPtr ptr_model, SVMNode[] x)
        {
            if (ptr_model == IntPtr.Zero)
                throw new ArgumentNullException("ptr_model");

            List<SVMNode> nodes = x.Select(a => a.Clone()).ToList();
            nodes.Add(new SVMNode(-1, 0));
            IntPtr ptr_nodes = SVMNode.Allocate(nodes.ToArray());

            double result = libsvm.svm_predict(ptr_model, ptr_nodes);

            SVMNode.Free(ptr_nodes);

            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="x"></param>
        /// <param name="estimations"></param>
        /// <returns></returns>
        public static double PredictProbability(SVMModel model, SVMNode[] x, out double[] estimations)
        {
            IntPtr ptr_model = SVMModel.Allocate(model);
            double result = PredictProbability(ptr_model, x, out estimations);
            SVMModel.Free(ptr_model);
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptr_model"></param>
        /// <param name="x"></param>
        /// <param name="estimations"></param>
        /// <returns></returns>
        public static double PredictProbability(IntPtr ptr_model, SVMNode[] x, out double[] estimations)
        {
            if (ptr_model == IntPtr.Zero)
                throw new ArgumentNullException("ptr_model");

            bool isProbabilityModel = libsvm.svm_check_probability_model(ptr_model);
            if (!isProbabilityModel)
            {
                SVMModel.Free(ptr_model);
                estimations = null;
                return -1;
            }

            int classCount = libsvm.svm_get_nr_class(ptr_model);

            IntPtr ptr_estimations = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(double)) * classCount);
            List<SVMNode> nodes = x.Select(a => a.Clone()).ToList();
            nodes.Add(new SVMNode(-1, 0));
            IntPtr ptr_nodes = SVMNode.Allocate(nodes.ToArray());

            double result = libsvm.svm_predict_probability(ptr_model, ptr_nodes, ptr_estimations);

            estimations = new double[classCount];
            Marshal.Copy(ptr_estimations, estimations, 0, estimations.Length);

            SVMNode.Free(ptr_nodes);
            Marshal.FreeHGlobal(ptr_estimations);
            ptr_estimations = IntPtr.Zero;

            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="problem"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static string CheckParameter(SVMProblem problem, SVMParameter parameter)
        {
            IntPtr ptr_problem = SVMProblem.Allocate(problem);
            IntPtr ptr_parameter = SVMParameter.Allocate(parameter);

            IntPtr ptr_output = libsvm.svm_check_parameter(ptr_problem, ptr_parameter);

            SVMProblem.Free(ptr_problem);
            SVMParameter.Free(ptr_parameter);

            string output = Marshal.PtrToStringAnsi(ptr_output);
            Marshal.FreeHGlobal(ptr_output);
            ptr_output = IntPtr.Zero;

            return output;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool CheckProbabilityModel(SVMModel model)
        {
            IntPtr ptr_model = SVMModel.Allocate(model);
            bool success = libsvm.svm_check_probability_model(ptr_model);
            SVMModel.Free(ptr_model);
            return success;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="function"></param>
        public static void SetPrintStringFunction(SVMPrintFunction function)
        {
            throw new NotImplementedException("There is an issue about this method in the github page of this library. Please visit 'https://github.com/ccerhan/LibSVMsharp/issues/1' and solve the issue and send a pull request.");

            IntPtr ptr_function = Marshal.GetFunctionPointerForDelegate(function);
            libsvm.svm_set_print_string_function(ptr_function);

            //Marshal.FreeHGlobal(ptr_function);
            //ptr_function = IntPtr.Zero;
        }
    }
}
