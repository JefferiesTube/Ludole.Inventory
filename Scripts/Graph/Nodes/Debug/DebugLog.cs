using System;
using GraphProcessor;
using UnityEngine;

namespace Ludole.Inventory
{
    [NodeMenuItem("Debug/Log")]
    public class DebugLog : FlowNode
    {
        [Input(name = "Type"), SerializeField] public LogType Type;
        [Input(name = "Message"), SerializeField] public string Message;

        public override string name => "Log";

        protected override void Process()
        {
            switch (Type)
            {
                case LogType.Exception:
                case LogType.Error:
                    Debug.LogError(Message);
                    break;
                case LogType.Assert:
                    Debug.LogAssertion(Message);
                    break;
                case LogType.Warning:
                    Debug.LogWarning(Message);
                    break;
                case LogType.Log:
                    Debug.Log(Message);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            base.Process();
        }
    }
}