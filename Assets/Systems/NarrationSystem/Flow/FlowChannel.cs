#region

using UnityEngine;

#endregion

namespace Systems.NarrationSystem.Flow
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Flow/Flow Channel")]
    public class FlowChannel : ScriptableObject
    {
        #region Member Variables

        public delegate void FlowStateCallback(FlowState state);

        public FlowStateCallback OnFlowStateRequested;
        public FlowStateCallback OnFlowStateChanged;

        #endregion

        public void RaiseFlowStateRequest(FlowState state)
        {
            OnFlowStateRequested?.Invoke(state);
        }

        public void RaiseFlowStateChanged(FlowState state)
        {
            OnFlowStateChanged?.Invoke(state);
        }
    }
}
