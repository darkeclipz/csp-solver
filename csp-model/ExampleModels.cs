using System;
using System.Collections.Generic;
using System.Text;

namespace Csp.Model.Examples
{
    public static class Models
    {
        public static CspModel PhoneFeatureModel()
        {
            var model = new CspModel();

            var phone = model.AddVariable("phone", Domain.Binary);
            var calls = model.AddVariable("calls", Domain.Binary);
            var screen = model.AddVariable("screen", Domain.Binary);
            var screenBw = model.AddVariable("b/w", Domain.Binary);
            var screenColor = model.AddVariable("color", Domain.Binary);
            var screenHd = model.AddVariable("hd", Domain.Binary);
            var gps = model.AddVariable("gps", Domain.Binary);
            var media = model.AddVariable("media", Domain.Binary);
            var camera = model.AddVariable("camera", Domain.Binary);
            var mp3 = model.AddVariable("mp3", Domain.Binary);
            var test = model.AddVariable("internet", Domain.Range(0, 10));

            model.AddConstraint(phone == 1);
            model.AddConstraint(calls == 1);
            model.AddConstraint(screen == 1);

            model.AddConstraint(media >= mp3);
            model.AddConstraint(media >= camera);
            model.AddConstraint(screenColor >= camera);
            model.AddConstraint(test <= 5);

            model.AddConstraint(screenBw + screenColor + screenHd == screen);
            model.AddConstraint(camera == 1);
            model.AddConstraint(test >= screenColor);
            model.AddConstraint(test <= screenColor);

            return model;
        }

        public static CspModel AustralianMapColoringModel()
        {
            var model = new CspModel();

            int colors = 3;
            var wa = model.AddVariable("wa", Domain.Range(0, colors));
            var nt = model.AddVariable("nt", Domain.Range(0, colors));
            var sa = model.AddVariable("sa", Domain.Range(0, colors));
            var qu = model.AddVariable("ql", Domain.Range(0, colors));
            var nsw = model.AddVariable("nsw", Domain.Range(0, colors));
            var vi = model.AddVariable("vi", Domain.Range(0, colors));
            var ta = model.AddVariable("ta", Domain.Range(0, colors));

            model.AddConstraint(wa != nt);
            model.AddConstraint(wa != sa);
            model.AddConstraint(nt != sa);
            model.AddConstraint(nt != qu);
            model.AddConstraint(sa != nsw);
            model.AddConstraint(sa != vi);
            model.AddConstraint(sa != qu);
            model.AddConstraint(nsw != vi);
            model.AddConstraint(qu != nsw);

            return model;
        }

        public static CspModel SudokuSmallModel()
        {
            var model = new CspModel();
            var variables = new List<Variable>();
            for (int i = 0; i < 16; i++)
            {
                var variable = model.AddVariable($"x{i}", Domain.Range(1, 5));
                variables.Add(variable);
            }

            for (int i = 0; i < 16; i++)
            {
                int row = i / 4;
                int col = i % 4;

                for (int j = row; j < row + 4; j++)
                {
                    if (i != j)
                    {
                        Console.WriteLine($"{i}!={j}");
                        model.AddConstraint(variables[i] != variables[j]);
                    }
                }

                for (int j = col; j < 16; j += 4)
                {
                    if (i != j)
                    {
                        model.AddConstraint(variables[i] != variables[j]);
                    }
                }

                // TODO: Add diagonal constraints.
            }
            //model.AddConstraint(variables[0] != variables[1] != variables[4] != variables[5]); => AllDiff(x0, x1, x2, x3)
            // also => AllEqual(1,2,3,4)

            model.AddConstraint(variables[0] == 1);
            model.AddConstraint(variables[1] == 2);
            model.AddConstraint(variables[2] == 3);
            model.AddConstraint(variables[3] == 4);

            return model;
        }
    }
}
