using System;

namespace csp_solver_cs
{
    class Program
    {
        static void Main(string[] args)
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

            model.AddConstraint(phone == 1); // Required constraint
            model.AddConstraint(calls == 1);
            model.AddConstraint(screen == 1); // Required constraint           
            model.AddConstraint(media >= camera);
            model.AddConstraint(screenBw + screenHd + screenColor == screen); // Alternative constraint
            model.AddConstraint(screenHd + screenColor >= gps); // Or constraint
            model.AddConstraint(screenHd >= camera);

            model.AddConstraint(gps == 1); // Required constraint
            model.AddConstraint(camera == 1);

            model.Solve();

            Console.WriteLine($"Feasible: {model.IsFeasible()}");
            foreach (var variable in model.Variables)
            {
                Console.WriteLine($"{variable.Name} = {variable.Value}");
            }
        }
    }
}
