#region

using UnityEngine;

#endregion

namespace Systems.NarrationSystem.Flow
{
    public class FlowStateMachine : MonoBehaviour
    {
        #region Inspector Variables

        [SerializeField] private FlowChannel m_Channel;

        [SerializeField] private FlowState m_StartupState;

        #endregion

        #region Member Variables

        private FlowState m_CurrentState;
        public FlowState CurrentState => m_CurrentState;

        private static FlowStateMachine ms_Instance;
        public static FlowStateMachine Instance => ms_Instance;

        #endregion

        #region Init Data

        private void Awake()
        {
            ms_Instance = this;
            m_CurrentState = m_StartupState;
            m_Channel.OnFlowStateRequested += SetFlowState;
        }

        private void Start()
        {
        }

        #endregion

        private void SetFlowState(FlowState state)
        {
            if (m_CurrentState != state)
            {
                m_CurrentState = state;
                m_Channel.RaiseFlowStateChanged(m_CurrentState);
            }
        }

        #region Destructor

        private void OnDestroy()
        {
            m_Channel.OnFlowStateRequested -= SetFlowState;

            ms_Instance = null;
        }

        private void OnDisable()
        {
            m_Channel.OnFlowStateRequested -= SetFlowState;

            ms_Instance = null;
        }

        #endregion
    }
}