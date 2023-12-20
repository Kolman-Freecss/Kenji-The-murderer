#region

using UnityEngine;

#endregion

namespace Systems.NarrationSystem.Flow
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Flow/Flow State")]
    public class FlowState : ScriptableObject
    {
        public FlowStateType m_StateType;
        // Empty class, used as a marker for the FlowChannel
    }
}