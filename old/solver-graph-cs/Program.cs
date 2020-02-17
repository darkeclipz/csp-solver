using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.GraphViewerGdi;
using System;
using System.Collections.Generic;
using System.Linq;

namespace csp_solver_cs
{
    class Program
    {
        static void Main(string[] args)
        {
            DrawGraphLayout();


            //var model = Models.SudokuSmallModel();
            //model.Summary();
            //model.ResolveUnaryConstraints();
            //model.Summary();
            //model.ResolveBinaryConstraints();
            //model.Summary();
            ////model.Solve(verbose: true);
            //PrintSolution(model);
        }

        static void PrintSolution(CspModel model)
        {
            Console.WriteLine($"Feasible: {model.IsFeasible()}");
            foreach (var variable in model.Variables)
            {
                Console.WriteLine($"{variable.Name} = {variable.Value}");
            }
        }

        static void DrawGraphLayout()
        {
            var graph = new Graph("graph");
            graph.AddEdge("A", "B");
            graph.AddEdge("B", "C");
            graph.AddEdge("A", "C").Attr.Color = Color.Green;
            graph.FindNode("A").Attr.FillColor = Color.Magenta;
            graph.FindNode("B").Attr.FillColor = Color.MistyRose;

            var c = graph.FindNode("C");
            c.Attr.FillColor = Color.PaleGreen;
            c.Attr.Shape = Shape.Diamond;

            
            var renderer = new GraphRenderer(graph);
            int width = 50;
            var bitmap = new System.Drawing.Bitmap(width, (int)(graph.Height * (width / graph.Height)), System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            renderer.Render(bitmap);
            bitmap.Save("graph.bmp");
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

            model.AddConstraint(test <= 5);

            model.AddConstraint(screenBw + screenColor + screenHd >= 1);
            model.AddConstraint(camera == 1);
            model.AddConstraint(test >= screenColor);
            model.AddConstraint(test <= screenColor);
            return model;
        }

        internal static CspModel AustralianMapColoringModel()
        {
            var model = new CspModel();

            int colors = 3;
            var wa = model.AddVariable("West Australia", CspModel.Domain.Range(0, colors));
            var nt = model.AddVariable("Northern Territory", CspModel.Domain.Range(0, colors));
            var sa = model.AddVariable("South Australia", CspModel.Domain.Range(0, colors));
            var qu = model.AddVariable("Queensland", CspModel.Domain.Range(0, colors));
            var nsw = model.AddVariable("New South Wales", CspModel.Domain.Range(0, colors));
            var vi = model.AddVariable("Victoria", CspModel.Domain.Range(0, colors));
            var ta = model.AddVariable("Tasmania", CspModel.Domain.Range(0, colors));

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

        internal static CspModel SudokuSmallModel()
        {
            var model = new CspModel();
            var variables = new List<CspModel.Variable>();
            for(int i=0; i < 16; i++)  
            {
                var variable = model.AddVariable($"x{i}", CspModel.Domain.Range(1, 5));
                variables.Add(variable);
            }

            for(int i=0; i < 16; i++)
            {
                int row = i / 4;
                int col = i % 4;

                for(int j=row; j < row+4; j++)
                {
                    if(i!=j) 
                    {
                        Console.WriteLine($"{i}!={j}");
                        model.AddConstraint(variables[i] != variables[j]);
                    }
                }
/*
                for(int j=col; j < 16; j += 4)
                {
                    if(i != j)
                    {
                        model.AddConstraint(variables[i] != variables[j]);
                    }
                }*/
            }

            model.AddConstraint(variables[0] == 1);         
            model.AddConstraint(variables[1] == 2);         
            model.AddConstraint(variables[2] == 3);         
            model.AddConstraint(variables[3] == 4);         

            return model;
        }

        internal static CspModel SudokuModel()
        {
            var model = new CspModel();
            var variables = new List<CspModel.Variable>();
            for(int i=0; i < 81; i++)  
            {
                var variable = model.AddVariable($"x{i}", CspModel.Domain.Range(1, 10));
                variables.Add(variable);
            }

            for(int i=0; i < 81; i++)
            {
                int row = i / 9;
                int col = i % 9;

                for(int j=row; j < row+9; j++)
                {
                    if(i!=j) 
                    {
                        Console.WriteLine($"{i}!={j}");
                        model.AddConstraint(variables[i] != variables[j]);
                    }
                }

                for(int j=col; j < 81; j += 9)
                {
                    if(i != j)
                    {
                        model.AddConstraint(variables[i] != variables[j]);
                    }
                }

                // for(int j=0; j < 9; j++)
                // {

                // }

                // {
                //     {0,1,2,9,10,11,18,19,20},
                //     {3,4,5,12,13,14,21,22,23},
                //     {6,7,8,15,16,17,24,25,26},
                //     {27,28,29,36,37,38,45,46,47},
                //     {30,31,32,39,40,41,48,49,50},
                //     {33,34,35,42,43,44,51,52,53},
                //     {54,55,56,63,64,65,72,73,74},
                //     {57,58,59,66,67,68,75,76,77},
                //     {60,61,62,69,70,71,78,79 ,80}
                // }
            }

            return model;
        }
    }
}
