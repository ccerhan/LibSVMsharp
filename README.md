LibSVMsharp
===========

LibSVMsharp is a simple and easy-to-use C# wrapper for Support Vector Machines. 
It uses the latest LibSVM version 3.18 which released on 1st of April in 2014.

LibSVM : http://www.csie.ntu.edu.tw/~cjlin/libsvm/

Simplest Example Code
===========

```C#
SVMProblem problem = SVMProblemHelper.Load(@"dataset_path.txt");

SVMParameter parameter = new SVMParameter();
parameter.Type = SVMType.C_SVC;
parameter.Kernel = SVMKernelType.RBF;
parameter.C = 1;
parameter.Gamma = 1;

SVMModel model = SVM.Train(problem, parameter);

double correct = 0;
for (int i = 0; i < problem.Length; i++)
{
  double y = SVM.Predict(model, problem.X[i]);
  if (y == problem.Y[i])
    correct++;
}
```
