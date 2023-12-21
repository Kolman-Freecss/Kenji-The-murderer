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

    [Serializable]
    public enum FlowStateType
    {
        InGame,
        InDialogue
    }

    public class FlowListener : MonoBehaviour
    {
        public static FlowListener Instance { get; private set; }

        #region Inspector Variables

        [SerializeField] private FlowChannel m_Channel;

        [SerializeField] private FlowListenerEntry[] m_Entries;

        public FlowListenerEntry[] Entries
        {
            get => m_Entries;
            set => m_Entries = value;
        }

        #endregion

        #region Init Data

        private void Awake()
        {
            m_Channel.OnFlowStateChanged += OnFlowStateChanged;
            ManageSingleton();
        }

        private void ManageSingleton()
        {
            if (Instance != null)
            {
                gameObject.SetActive(false);
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        #endregion

        private void OnFlowStateChanged(FlowState state)
        {
            FlowListenerEntry foundEntry = Array.Find(m_Entries, x =>
            {
                if (state != null && x.m_State != null)
                {
                    Debug.Log($"Comparing {x.m_State.m_StateType} with {state.m_StateType}");
                    return x.m_State.m_StateType == state.m_StateType;
                }
                else
                {
                    Debug.Log("One of the states is null => " + (state == null ? "state" : "x.m_State"));
                    return false;
                }
            });
            if (foundEntry != null)
            {
                if (foundEntry.m_Event == null)
                {
                    Debug.LogError($"Event for {foundEntry.m_State.m_StateType} is null");
                    return;
                }

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