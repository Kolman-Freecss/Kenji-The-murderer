#region

using System;
using UnityEngine;
using UnityEngine.Events;

#endregion

namespace Systems.NarrationSystem.Flow
{
    [Serializable]
    public class FlowListenerEntry
    {
        public FlowState m_State;
        public UnityEvent m_Event;
    }

    public class FlowListener : MonoBehaviour
    {
        #region Inspector Variables

        [SerializeField]
        private FlowChannel m_Channel;

        [SerializeField]
        private FlowListenerEntry[] m_Entries;

        #endregion

        #region Init Data

        private void Awake()
        {
            m_Channel.OnFlowStateChanged += OnFlowStateChanged;
        }

        #endregion

        private void OnFlowStateChanged(FlowState state)
        {
            FlowListenerEntry foundEntry = Array.Find(m_Entries, x => x.m_State == state);
            if (foundEntry != null)
            {
                foundEntry.m_Event.Invoke();
            }
        }

        #region Destructor

        private void OnDestroy()
        {
            m_Channel.OnFlowStateChanged -= OnFlowStateChanged;
        }

        #endregion
    }
}
