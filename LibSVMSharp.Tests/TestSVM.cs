using System;
using LibSVMsharp;
using NUnit.Framework;

namespace LibSVMSharp.Tests
{
    [TestFixture]
    public class TestSVM
    {
        [Test]
        public void SVM_Train_ProblemIsNull_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(()=> SVM.Train(null, new SVMParameter()));
        }

        [Test]
        public void SVM_Train_ParameterIsNull_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => SVM.Train(new SVMProblem(), null));
        }
        //[TestMethod]
        public void SVM_Train_Correct()
        {

        }

        [Test]
        public void SVM_CrossValidation_ProblemIsNull_ThrowsException()
        {
            double[] target;
            Assert.Throws<ArgumentNullException>(() => SVM.CrossValidation(null, new SVMParameter(), 5, out target));
        }
        [Test]
        public void SVM_CrossValidation_ParameterIsNull_ThrowsException()
        {
            double[] target;
            Assert.Throws<ArgumentNullException>(() => SVM.CrossValidation(new SVMProblem(), null, 5, out target));
        }
        [Test]
        public void SVM_CrossValidation_FoldNumberIsOutOfRange_ThrowsException()
        {
            double[] target;
            Assert.Throws<ArgumentOutOfRangeException>(() => SVM.CrossValidation(new SVMProblem(), new SVMParameter(), 1, out target));
        }
        //[TestMethod]
        public void SVM_CrossValidation_Correct()
        {

        }

        [Test]
        public void SVM_SaveModel_ModelIsNull_ReturnsFalse()
        {
            bool success = SVM.SaveModel(null, Contants.CORRECT_MODEL_PATH_TO_BE_SAVED);

            Assert.IsFalse(success);
        }
        [Test]
        public void SVM_SaveModel_FilenameIsInvalid_ReturnsFalse()
        {
            bool success = SVM.SaveModel(new SVMModel(), "");

            Assert.IsFalse(success);
        }
        //[TestMethod]
        public void SVM_SaveModel_Correct()
        {

        }

        [Test]
        public void SVM_LoadModel_FilenameIsInvalid_ReturnsNull()
        {
            SVM.LoadModel("");
        }
        [Test]
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

        [Test]
        public void SVM_PredictValues_ModelIsNull_ThrowsException()
        {
            double[] values;
            Assert.Throws<ArgumentNullException>(() => SVM.PredictValues(null, new SVMNode[5], out values));
        }
        [Test]
        public void SVM_PredictValues_ModelIsZero_ThrowsException()
        {
            double[] values;
            Assert.Throws<ArgumentNullException>(() => SVM.PredictValues(IntPtr.Zero, new SVMNode[5], out values));
        }
        [Test]
        public void SVM_PredictValues_InputVectorIsNull_ThrowsException()
        {
            double[] values;
            Assert.Throws<ArgumentNullException>(() => SVM.PredictValues(new SVMModel(), null, out values));
        }
        //[TestMethod]
        public void SVM_PredictValues_Correct()
        {

        }

        [Test]
        public void SVM_Predict_ModelIsNull_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => SVM.Predict(null, new SVMNode[5]));
        }
        [Test]
        public void SVM_Predict_ModelIsZero_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => SVM.Predict(IntPtr.Zero, new SVMNode[5]));
        }
        [Test]
        public void SVM_Predict_InputVectorIsNull_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => SVM.Predict(new SVMModel(), null));
        }
        //[TestMethod]
        public void SVM_Predict_Correct()
        {

        }

        [Test]
        public void SVM_PredictProbability_ModelIsNull_ThrowsException()
        {
            double[] values;
            Assert.Throws<ArgumentNullException>(() => SVM.PredictProbability(null, new SVMNode[5], out values));
        }
        [Test]
        public void SVM_PredictProbability_ModelIsZero_ThrowsException()
        {
            double[] values;
            Assert.Throws<ArgumentNullException>(() => SVM.PredictProbability(IntPtr.Zero, new SVMNode[5], out values));
        }
        [Test]
        public void SVM_PredictProbability_InputVectorIsNull_ThrowsException()
        {
            double[] values;
            Assert.Throws<ArgumentNullException>(() => SVM.PredictProbability(new SVMModel(), null, out values));
        }
        //[TestMethod]
        public void SVM_PredictProbability_NotProbabilityModel_ReturnsNegativeNumber()
        {

        }
        //[TestMethod]
        public void SVM_PredictProbability_Correct()
        {

        }

        [Test]
        public void SVM_CheckParameter_ProblemIsNull_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => SVM.CheckParameter(null, new SVMParameter()));
        }
        [Test]
        public void SVM_CheckParameter_ParameterIsNull_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => SVM.CheckParameter(new SVMProblem(), null));
        }
        //[TestMethod]
        public void SVM_CheckParameter_Correct()
        {

        }

        [Test]
        public void SVM_CheckProbabilityModel_ModelIsNull_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => SVM.CheckProbabilityModel(null));
        }
        //[TestMethod]
        public void SVM_CheckProbabilityModel_Correct()
        {

        }
    }
}
