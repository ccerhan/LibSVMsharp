using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace LibSVMsharp.Core
{
    /// <summary>
    /// For more information about libsvm project:
    /// Official: http://www.csie.ntu.edu.tw/~cjlin/libsvm/
    /// Github: https://github.com/cjlin1/libsvm
    /// </summary>
    public static class libsvm
    {
        public static string VERSION = "3.20";

        /// <param name="prob">svm_problem</param>
        /// <param name="param">svm_parameter</param>
        /// <returns>svm_model</returns>
        [DllImport("libsvm.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr svm_train(IntPtr prob, IntPtr param);
        /// <param name="prob">svm_problem</param>
        /// <param name="param">svm_parameter</param>
        /// <param name="nr_fold">int</param>
        /// <param name="target">double[]</param>
        [DllImport("libsvm.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void svm_cross_validation(IntPtr prob, IntPtr param, int nr_fold, IntPtr target);

        /// <param name="model_file_name">string</param>
        /// <param name="model">svm_model</param>
        /// <returns>bool</returns>
        [DllImport("libsvm.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int svm_save_model(IntPtr model_file_name, IntPtr model);
        /// <param name="model_file_name">string</param>
        /// <returns>svm_model</returns>
        [DllImport("libsvm.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr svm_load_model(IntPtr model_file_name);

        /// <param name="model">svm_model</param>
        /// <returns>int</returns>
        [DllImport("libsvm.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int svm_get_svm_type(IntPtr model);
        /// <param name="model">svm_model</param>
        /// <returns>int</returns>
        [DllImport("libsvm.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int svm_get_nr_class(IntPtr model);
        /// <param name="model">svm_model</param>
        /// <param name="label">int[]</param>
        [DllImport("libsvm.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void svm_get_labels(IntPtr model, IntPtr label);
        /// <param name="model">svm_model</param>
        /// <param name="label">int[]</param>
        [DllImport("libsvm.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void svm_get_sv_indices(IntPtr model, IntPtr sv_indices);
        /// <param name="model">svm_model</param>
        /// <returns>int</returns>
        [DllImport("libsvm.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int svm_get_nr_sv(IntPtr model);
        /// <param name="model">svm_model</param>
        /// <returns>double</returns>
        [DllImport("libsvm.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double svm_get_svr_probability(IntPtr model);

        /// <param name="model">svm_model</param>
        /// <param name="x">svm_node[]</param>
        /// <param name="dec_values">double[]</param>
        /// <returns>double</returns>
        [DllImport("libsvm.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double svm_predict_values(IntPtr model, IntPtr x, IntPtr dec_values);
        /// <param name="model">svm_model</param>
        /// <param name="dec_values">double[]</param>
        /// <returns>double</returns>
        [DllImport("libsvm.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double svm_predict(IntPtr model, IntPtr x);
        /// <param name="model">svm_model</param>
        /// <param name="x">svm_node[]</param>
        /// <param name="dec_values">double[]</param>
        /// <returns>double</returns>
        [DllImport("libsvm.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double svm_predict_probability(IntPtr model, IntPtr x, IntPtr prob_estimates);

        /// <param name="model_ptr">svm_model</param>
        [DllImport("libsvm.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void svm_free_model_content(IntPtr model_ptr);
        /// <param name="model_ptr_ptr">svm_model*</param>
        [DllImport("libsvm.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void svm_free_and_destroy_model(ref IntPtr model_ptr_ptr);
        /// <param name="param">svm_parameter</param>
        [DllImport("libsvm.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void svm_destroy_param(IntPtr param);

        /// <param name="prob">svm_problem</param>
        /// <param name="param">svm_parameter</param>
        /// <returns>string</returns>
        [DllImport("libsvm.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr svm_check_parameter(IntPtr prob, IntPtr param);
        /// <param name="model">svm_model</param>
        /// <returns>int</returns>
        [DllImport("libsvm.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool svm_check_probability_model(IntPtr model);

        /// <param name="print_function">void (*)(const char *)</param>
        [DllImport("libsvm.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void svm_set_print_string_function(IntPtr print_function);
    }
}
