using System;
using System.Linq;

namespace csp_solver_cs
{
    class Program
    {
        static void Main(string[] args)
        {
             var model = Models.AustralianMapColoringModel();
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

            var phone = model.AddVariable("phone", CspModel.Domain.Binary);
            var calls = model.AddVariable("calls", CspModel.Domain.Binary);
            var screen = model.AddVariable("screen", CspModel.Domain.Binary);
            var screenBw = model.AddVariable("b/w screen", CspModel.Domain.Binary);
            var screenColor = model.AddVariable("color screen", CspModel.Domain.Binary);
            var screenHd = model.AddVariable("hd screen", CspModel.Domain.Binary);
            var gps = model.AddVariable("gps", CspModel.Domain.Binary);
            var media = model.AddVariable("media", CspModel.Domain.Binary);
            var camera = model.AddVariable("camera", CspModel.Domain.Binary);
            var mp3 = model.AddVariable("mp3", CspModel.Domain.Binary);
            var test = model.AddVariable("test", CspModel.Domain.Range(0, 10));

            // Unary constraints
            model.AddConstraint(phone == 1); 
            model.AddConstraint(calls == 1);
            model.AddConstraint(screen == 1);  

            // Binary constraints
            model.AddConstraint(media >= mp3);
            model.AddConstraint(media >= camera);
            model.AddConstraint(screenColor >= camera);

            model.AddConstraint(test >= 5);

            model.AddConstraint(screenBw + screenColor + screenHd >= 1);
            model.AddConstraint(camera == 1);
            model.AddConstraint(test == 8);
            return model;
        }

        internal static CspModel AustralianMapColoringModel()
        {
            var model = new CspModel();

            var wa = model.AddVariable("West Australia", CspModel.Domain.Range(0, 4));
            var nt = model.AddVariable("Northern Territory", CspModel.Domain.Range(0, 4));
            var sa = model.AddVariable("South Australia", CspModel.Domain.Range(0, 4));
            var qu = model.AddVariable("Queensland", CspModel.Domain.Range(0, 4));
            var nsw = model.AddVariable("New South Wales", CspModel.Domain.Range(0, 4));
            var vi = model.AddVariable("Victoria", CspModel.Domain.Range(0, 4));
            var ta = model.AddVariable("Tasmania", CspModel.Domain.Range(0, 4));

            model.AddConstraint(wa != nt);
            model.AddConstraint(wa != sa);
            model.AddConstraint(nt != sa);
            model.AddConstraint(nt != qu);
            model.AddConstraint(sa != nsw);
            model.AddConstraint(sa != vi);
            model.AddConstraint(sa != qu);
            model.AddConstraint(nsw != vi);
            model.AddConstraint(qu != nsw);
            model.AddConstraint(vi != wa);

            return model;
        }
    }
}
