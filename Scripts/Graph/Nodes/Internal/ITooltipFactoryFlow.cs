using System.Collections.Generic;
using System.Reflection;
using GraphProcessor;

namespace Ludole.Inventory
{
    public interface ITooltipFactoryFlow
    {
        IEnumerable<BaseNode> GetExecutedNodes();

        FieldInfo[] GetNodeFields();
    }
}