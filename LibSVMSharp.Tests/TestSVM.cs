using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LibSVMsharp;

namespace LibSVMSharp.Tests
{
    [TestClass]
    public class TestSVM
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SVM_Train_ProblemIsNull_ThrowsException()
        {
            SVM.Train(null, new SVMParameter());
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SVM_Train_ParameterIsNull_ThrowsException()
        {
            SVM.Train(new SVMProblem(), null);
        }
        //[TestMethod]
        public void SVM_Train_Correct()
        {

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SVM_CrossValidation_ProblemIsNull_ThrowsException()
        {
            double[] target;
            SVM.CrossValidation(null, new SVMParameter(), 5, out target);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SVM_CrossValidation_ParameterIsNull_ThrowsException()
        {
            double[] target;
            SVM.CrossValidation(new SVMProblem(), null, 5, out target);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SVM_CrossValidation_FoldNumberIsOutOfRange_ThrowsException()
        {
            double[] target;
            SVM.CrossValidation(new SVMProblem(), new SVMParameter(), 1, out target);
        }
        //[TestMethod]
        public void SVM_CrossValidation_Correct()
        {

        }

        [TestMethod]
        public void SVM_SaveModel_ModelIsNull_ReturnsFalse()
        {
            bool success = SVM.SaveModel(null, Contants.CORRECT_MODEL_PATH_TO_BE_SAVED);

            Assert.IsFalse(success);
        }
        [TestMethod]
        public void SVM_SaveModel_FilenameIsInvalid_ReturnsFalse()
        {
            bool success = SVM.SaveModel(new SVMModel(), "");

            Assert.IsFalse(success);
        }
        //[TestMethod]
        public void SVM_SaveModel_Correct()
        {

        }

        [TestMethod]
        public void SVM_LoadModel_FilenameIsInvalid_ReturnsNull()
        {
            SVM.LoadModel("");
        }
        [TestMethod]
        public void SVM_LoadModel_FilenameDoesNotExist_ReturnsNull()
        {
            SVM.LoadModel(Contants.WRONG_MODEL_PATH_TO_BE_LOADED);
        }
        //[TestMethod]
        public void SVM_LoadModel_ModelFileIsInvalid_ReturnsNull()
        {

        }
        //[TestMethod]
        public void SVM_LoadModel_Correct()
        {

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SVM_PredictValues_ModelIsNull_ThrowsException()
        {
            double[] values;
            SVM.PredictValues(null, new SVMNode[5], out values);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SVM_PredictValues_ModelIsZero_ThrowsException()
        {
            double[] values;
            SVM.PredictValues(IntPtr.Zero, new SVMNode[5], out values);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SVM_PredictValues_InputVectorIsNull_ThrowsException()
        {
            double[] values;
            SVM.PredictValues(new SVMModel(), null, out values);
        }
        //[TestMethod]
        public void SVM_PredictValues_Correct()
        {

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SVM_Predict_ModelIsNull_ThrowsException()
        {
            SVM.Predict(null, new SVMNode[5]);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SVM_Predict_ModelIsZero_ThrowsException()
        {
            SVM.Predict(IntPtr.Zero, new SVMNode[5]);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SVM_Predict_InputVectorIsNull_ThrowsException()
        {
            SVM.Predict(new SVMModel(), null);
        }
        //[TestMethod]
        public void SVM_Predict_Correct()
        {

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SVM_PredictProbability_ModelIsNull_ThrowsException()
        {
            double[] values;
            SVM.PredictProbability(null, new SVMNode[5], out values);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SVM_PredictProbability_ModelIsZero_ThrowsException()
        {
            double[] values;
            SVM.PredictProbability(IntPtr.Zero, new SVMNode[5], out values);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SVM_PredictProbability_InputVectorIsNull_ThrowsException()
        {
            double[] values;
            SVM.PredictProbability(new SVMModel(), null, out values);
        }
        //[TestMethod]
        public void SVM_PredictProbability_NotProbabilityModel_ReturnsNegativeNumber()
        {

        }
        //[TestMethod]
        public void SVM_PredictProbability_Correct()
        {

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SVM_CheckParameter_ProblemIsNull_ThrowsException()
        {
            SVM.CheckParameter(null, new SVMParameter());
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SVM_CheckParameter_ParameterIsNull_ThrowsException()
        {
            SVM.CheckParameter(new SVMProblem(), null);
        }
        //[TestMethod]
        public void SVM_CheckParameter_Correct()
        {

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SVM_CheckProbabilityModel_ModelIsNull_ThrowsException()
        {
            SVM.CheckProbabilityModel(null);
        }
        //[TestMethod]
        public void SVM_CheckProbabilityModel_Correct()
        {

        }
    }
}
