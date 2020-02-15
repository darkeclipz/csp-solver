using System;

namespace csp_solver_cs
{
    class Program
    {
        static void Main(string[] args)
        {
            var model = Models.PhoneFeatureModel();
            model.ResolveUnaryConstraints();
            model.Solve(verbose: true);
            PrintSolution(model);
        }

        static void PrintSolution(CspModel model)
        {
            Console.WriteLine($"Feasible: {model.IsFeasible()}");
            foreach (var variable in model.Variables)
            {
                Console.WriteLine($"{variable.Name} = {variable.Value}");
            }
        }
    }

    static class Models
    {
        internal static CspModel PhoneFeatureModel()
        {
            var model = new CspModel();

            var phone = model.AddVariable("phone");
            var calls = model.AddVariable("calls");
            var screen = model.AddVariable("screen");
            var screenBw = model.AddVariable("b/w screen");
            var screenColor = model.AddVariable("color screen");
            var screenHd = model.AddVariable("hd screen");
            var gps = model.AddVariable("gps");
            var media = model.AddVariable("media");
            var camera = model.AddVariable("camera");
            var mp3 = model.AddVariable("mp3");

            // Unary constraints
            model.AddConstraint(phone == 1); 
            model.AddConstraint(calls == 1);
            model.AddConstraint(screen == 1);  

            // Binary constraints
            model.AddConstraint(media >= mp3);
            model.AddConstraint(media >= camera);
            model.AddConstraint(screenColor >= camera);

            model.AddConstraint(screenBw + screenColor + screenHd == screen);
            model.AddConstraint(camera == 1);
            return model;
        }
    }
}
