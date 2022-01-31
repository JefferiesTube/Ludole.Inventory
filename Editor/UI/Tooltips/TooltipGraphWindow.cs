using GraphProcessor;
using UnityEngine;

namespace Ludole.Inventory.Editor
{
    public class TooltipGraphWindow : BaseGraphWindow
    {
        public static void OpenTooltipGraph(TooltipFactoryGraph graph)
        {
            TooltipGraphWindow wnd = CreateWindow<TooltipGraphWindow>();
            wnd.InitializeWindow(graph);
            wnd.Show();
        }

        protected override void InitializeWindow(BaseGraph g)
        {
            graph = g;
            titleContent = new GUIContent("Tooltip Graph");

            if (graphView == null)
            {
                graphView = new TooltipGraphView(this);
            }

            rootView.Add(graphView);
        }
    }
}