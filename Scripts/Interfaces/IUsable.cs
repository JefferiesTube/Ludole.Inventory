using System.Collections.Generic;
using UnityEngine.Events;

namespace Ludole.Inventory
{
    public interface IUsable
    {
        UnityEvent OnUse { get; }
        CooldownGroup CooldownGroup { get; set; }
    }
}