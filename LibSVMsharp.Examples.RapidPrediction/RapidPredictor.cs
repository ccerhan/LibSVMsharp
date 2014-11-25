using LibSVMsharp.Core;
using LibSVMsharp.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LibSVMsharp.Examples.RapidPrediction
{
    public class RapidPredictor : IDisposable
    {
        private IntPtr _ptr_model = IntPtr.Zero;

        public RapidPredictor() : this(null) { }
        public RapidPredictor(SVMModel model)
        {
            SetModel(model);
        }

        public SVMModel Model { get; private set; }

        public void SetModel(SVMModel model)
        {
            if (model != null)
            {
                Model = model.Clone();
                _ptr_model = SVMModel.Allocate(Model);
            }
        }
        public double Predict(SVMNode[] x)
        {
            return x.Predict(_ptr_model);
        }
        public double PredictProbability(SVMNode[] x, out double[] estimations)
        {
            return x.PredictProbability(_ptr_model, out estimations);
        }
        public double PredictValues(SVMNode[] x, out double[] values)
        {
            return x.PredictValues(_ptr_model, out values);
        }
        public void Dispose()
        {
            SVMModel.Free(_ptr_model);
            _ptr_model = IntPtr.Zero;
            Model = null;
        }
    }
}
