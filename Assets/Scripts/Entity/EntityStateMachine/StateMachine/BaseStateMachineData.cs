using UnityEditor;
using UnityEngine;

namespace StateMachine
{
    public abstract class BaseStateMachineData : ScriptableObject
    {
        public abstract void initialize();

        public abstract BaseStateMachineData Clone();
    }
}