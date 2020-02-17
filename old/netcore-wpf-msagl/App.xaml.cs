using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.WpfGraphControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace netcore_wpf_msagl
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public static readonly RoutedUICommand LoadSampleGraphCommand = new RoutedUICommand("Open File...", "OpenFileCommand",
                                                                                     typeof(Window));
        public static readonly RoutedUICommand HomeViewCommand = new RoutedUICommand("Home view...", "HomeViewCommand",
                                                                                     typeof(Window));

        Window appWindow;
        Grid mainGrid = new Grid();
        DockPanel graphViewerPanel = new DockPanel();
        ToolBar toolBar = new ToolBar();
        GraphViewer graphViewer = new GraphViewer();
        TextBox statusTextBox;

        protected override void OnStartup(StartupEventArgs e)
        {
            appWindow = new Window
            {
                Title = "WpfApplicationSample",
                Content = mainGrid,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                WindowState = WindowState.Normal
            };

            SetupToolbar();
            graphViewerPanel.ClipToBounds = true;
            mainGrid.Children.Add(toolBar);
            toolBar.VerticalAlignment = VerticalAlignment.Top;
            graphViewer.ObjectUnderMouseCursorChanged += graphViewer_ObjectUnderMouseCursorChanged;

            mainGrid.Children.Add(graphViewerPanel);
            graphViewer.BindToPanel(graphViewerPanel);

            SetStatusBar();
            graphViewer.MouseDown += WpfApplicationSample_MouseDown;
            appWindow.Loaded += (a, b) => CreateAndLayoutAndDisplayGraph(null, null);

            //CreateAndLayoutAndDisplayGraph(null,null);
            //graphViewer.MainPanel.MouseLeftButtonUp += TestApi;
            appWindow.Show();
        }

        void WpfApplicationSample_MouseDown(object sender, MsaglMouseEventArgs e)
        {
            statusTextBox.Text = "there was a click...";
        }

        void SetStatusBar()
        {
            var statusBar = new StatusBar();
            statusTextBox = new TextBox { Text = "No object" };
            statusBar.Items.Add(statusTextBox);
            mainGrid.Children.Add(statusBar);
            statusBar.VerticalAlignment = VerticalAlignment.Bottom;
        }

        void graphViewer_ObjectUnderMouseCursorChanged(object sender, ObjectUnderMouseCursorChangedEventArgs e)
        {
            var node = graphViewer.ObjectUnderMouseCursor as IViewerNode;
            if (node != null)
            {
                var drawingNode = (Node)node.DrawingObject;
                statusTextBox.Text = drawingNode.Label.Text;
            }
            else
            {
                var edge = graphViewer.ObjectUnderMouseCursor as IViewerEdge;
                if (edge != null)
                    statusTextBox.Text = ((Edge)edge.DrawingObject).SourceNode.Label.Text + "->" +
                                         ((Edge)edge.DrawingObject).TargetNode.Label.Text;
                else
                    statusTextBox.Text = "No object";
            }
        }

        void SetupToolbar()
        {
            SetupCommands();
            DockPanel.SetDock(toolBar, Dock.Top);
            SetMainMenu();
            //edgeRangeSlider = CreateRangeSlider();
            // toolBar.Items.Add(edgeRangeSlider.Visual);
        }

        void SetupCommands()
        {
            appWindow.CommandBindings.Add(new CommandBinding(LoadSampleGraphCommand, CreateAndLayoutAndDisplayGraph));
            appWindow.CommandBindings.Add(new CommandBinding(HomeViewCommand, (a, b) => graphViewer.SetInitialTransform()));
            appWindow.InputBindings.Add(new InputBinding(LoadSampleGraphCommand, new KeyGesture(Key.L, System.Windows.Input.ModifierKeys.Control)));
            appWindow.InputBindings.Add(new InputBinding(HomeViewCommand, new KeyGesture(Key.H, System.Windows.Input.ModifierKeys.Control)));

        }

        void SetMainMenu()
        {
            var mainMenu = new Menu { IsMainMenu = true };
            toolBar.Items.Add(mainMenu);
            SetFileMenu(mainMenu);
            SetViewMenu(mainMenu);
        }

        void SetViewMenu(Menu mainMenu)
        {
            var viewMenu = new MenuItem { Header = "_View" };
            var viewMenuItem = new MenuItem { Header = "_Home", Command = HomeViewCommand };
            viewMenu.Items.Add(viewMenuItem);
            mainMenu.Items.Add(viewMenu);
        }

        void SetFileMenu(Menu mainMenu)
        {
            var fileMenu = new MenuItem { Header = "_File" };
            var openFileMenuItem = new MenuItem { Header = "_Load Sample Graph", Command = LoadSampleGraphCommand };
            fileMenu.Items.Add(openFileMenuItem);
            mainMenu.Items.Add(fileMenu);
        }

        void CreateAndLayoutAndDisplayGraph(object sender, ExecutedRoutedEventArgs ex)
        {
            try
            {
                Graph graph = new Graph();
                graph.AddEdge("47", "58");
                graph.AddEdge("70", "71");

                var subgraph = new Subgraph("subgraph1");
                graph.RootSubgraph.AddSubgraph(subgraph);
                subgraph.AddNode(graph.FindNode("47"));
                subgraph.AddNode(graph.FindNode("58"));

                var subgraph2 = new Subgraph("subgraph2");
                subgraph2.Attr.Color = Microsoft.Msagl.Drawing.Color.Black;
                subgraph2.Attr.FillColor = Microsoft.Msagl.Drawing.Color.Yellow;
                subgraph2.AddNode(graph.FindNode("70"));
                subgraph2.AddNode(graph.FindNode("71"));
                subgraph.AddSubgraph(subgraph2);
                graph.AddEdge("58", subgraph2.Id);
                graph.Attr.LayerDirection = LayerDirection.LR;
                graphViewer.Graph = graph;

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Load Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
