using Csp.Debugger.Views;
using Csp.Model;
using Csp.Model.Examples;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.WpfGraphControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Csp.Debugger.Controls
{
    /// <summary>
    /// Interaction logic for GraphUserControl.xaml
    /// </summary>
    public partial class GraphUserControl : UserControl
    {
        private readonly GraphUserControlView view;
        
        private readonly GraphViewer graphViewer = new GraphViewer();
        private readonly DockPanel graphViewerPanel = new DockPanel();

        public GraphUserControl(GraphUserControlView view)
        {
            InitializeComponent();
            graphViewerPanel.ClipToBounds = true;
            this.view = view;
            graphViewer.BindToPanel(graphViewerPanel);
            mainGrid.Children.Clear();
            mainGrid.Children.Add(graphViewerPanel);

            Loaded += (a, b) => RenderViewGraph();
            view.OnViewUpdate += RenderViewGraph;
        }

        private void RenderViewGraph()
        {
            var graph = CreateGraphLayout(view.Model);
            graphViewer.Graph = graph;
        }

        private static Graph CreateGraphLayout(CspModel model)
        {
            var graph = new Graph();
            graph.Directed = false;
            graph.Attr.LayerDirection = LayerDirection.LR;

            var createdEdges = new List<string>();

            foreach (var variable in model.Variables)
            {
                var n = graph.AddNode($"{variable.Name}={variable.Value}");
                if (variable.Value == -1)
                {
                    n.Attr.FillColor = Microsoft.Msagl.Drawing.Color.Yellow;
                }
                n.Attr.Shape = Microsoft.Msagl.Drawing.Shape.Box;
                foreach (var constraint in model.Constraints.Where(c => c.GetVariables().Any(v => v.Name == variable.Name)))
                {
                    graph.AddNode(constraint.ToString());
                    //graph.AddEdge(variable.ToString(), constraint.ToString());
                    foreach (var target in constraint.GetVariables())
                    {
                        
                        if (!createdEdges.Contains(constraint.ToString() + "->" + target.Name))
                        {
                            graph.AddNode(target.Name + "=" + target.Value);
                            graph.AddEdge(constraint.ToString(), target.Name + "=" + target.Value).Attr.ArrowheadAtTarget = ArrowStyle.None;
                            createdEdges.Add(constraint.ToString() + "->" + target.Name);
                        }
                        
                    }
                }
            }

            return graph;
        }

        private static Graph CreateGraphLayout2(CspModel model)
        {
            var graph = new Graph();
            graph.Attr.LayerDirection = LayerDirection.LR;

            var createdEdges = new List<string>();

            foreach (var variable in model.Variables)
            {
                var n = graph.AddNode($"{variable.Name}={variable.Value}");
                if (variable.Value == -1)
                {
                    n.Attr.FillColor = Microsoft.Msagl.Drawing.Color.Yellow;
                }
                n.Attr.Shape = Microsoft.Msagl.Drawing.Shape.Box;
                foreach (var constraint in model.Constraints.Where(c => c.GetVariables().Any(v => v.GetVariable().Name == variable.Name)))
                {
                    graph.AddNode(constraint.ToString());
                    graph.AddEdge(variable.Name + "=" + variable.Value, constraint.ToString());
                    foreach (var target in constraint.GetVariables())
                    {
                        if (target.GetVariable().Name != variable.Name)
                        {
                            if (!createdEdges.Contains(constraint.ToString() + "->" + target.GetVariable().Name))
                            {
                                graph.AddNode(target.GetVariable().Name + "=" + target.GetVariable().Value);
                                graph.AddEdge(constraint.ToString(), target.GetVariable().Name + "=" + target.GetVariable().Value);
                                createdEdges.Add(constraint.ToString() + "->" + target.GetVariable().Name);
                            }
                        }
                    }
                }
            }

            return graph;
        }
    }
}
