using GraphProcessor;
using Ludole.Core.Editor;
using UnityEngine;

namespace Ludole.Inventory.Editor
{
    public class LudoleGraphWindow : BaseGraphWindow
    {
        private LudoleGraphToolbarView _toolbarView;

        public static void OpenGraph(TooltipGraph graph)
        {
            LudoleGraphWindow wnd = CreateWindow<LudoleGraphWindow>();
            wnd.InitializeWindow(graph);
            wnd.Show();
        }

        protected override void InitializeWindow(BaseGraph g)
        {
            graph = g;
            titleContent = new GUIContent("Ludole Graph");

            if (graphView == null)
            {
                graphView = new TooltipGraphView(this);
                _toolbarView = new LudoleGraphToolbarView(graphView);
                graphView.Add(_toolbarView);
            }

            rootView.Add(graphView);
        }


    }
}