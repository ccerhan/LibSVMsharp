##LibSVMsharp

LibSVMsharp is a simple and easy-to-use C# wrapper for Support Vector Machines. 
It uses the latest LibSVM version 3.20 which released on 15th of November in 2014.

For more information visit the official [libsvm](http://www.csie.ntu.edu.tw/~cjlin/libsvm/) webpage.

##How to Install

To install LibSVMsharp, download the [Nuget package](https://www.nuget.org/packages/LibSVMsharp) or run the following command in the Package Manager Console:

`PM> Install-Package LibSVMsharp`

##Simplest Example Code

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

##License
LibSVMsharp is released under the MIT License and libsvm is released under the [modified BSD Lisence](http://www.csie.ntu.edu.tw/~cjlin/libsvm/faq.html#f204) which is compatible with many free software licenses such as GPL.
