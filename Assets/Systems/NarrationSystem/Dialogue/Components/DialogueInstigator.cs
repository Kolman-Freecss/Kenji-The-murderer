#region

using Systems.NarrationSystem.Dialogue.Logic;
using Systems.NarrationSystem.Flow;
using UnityEngine;

#endregion

namespace Systems.NarrationSystem.Dialogue.Components
{
    /// <summary>
    /// Instigates a dialogue when a dialogue request is received.
    /// Component for player.
    /// </summary>
    [RequireComponent(typeof(FlowListener))]
    public class DialogueInstigator : MonoBehaviour
    {
        [SerializeField] private DialogueChannel m_DialogueChannel;

        [SerializeField] private FlowChannel m_FlowChannel;

        [SerializeField] private FlowState m_DialogueState;

        public static DialogueInstigator Instance { get; private set; }

        private DialogueSequencer m_DialogueSequencer;
        private FlowState m_CachedFlowState;

        private void Awake()
        {
            ManageSingleton();
            Debug.Log("DialogueInstigator Awake");
            m_DialogueSequencer = new DialogueSequencer();

            m_DialogueSequencer.OnDialogueStart += OnDialogueStart;
            m_DialogueSequencer.OnDialogueEnd += OnDialogueEnd;
            m_DialogueSequencer.OnDialogueNodeStart += m_DialogueChannel.RaiseDialogueNodeStart;
            m_DialogueSequencer.OnDialogueNodeEnd += m_DialogueChannel.RaiseDialogueNodeEnd;

            m_DialogueChannel.OnDialogueRequested += m_DialogueSequencer.StartDialogue;
            m_DialogueChannel.OnDialogueNodeRequested += m_DialogueSequencer.StartDialogueNode;
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

        private void OnDestroy()
        {
            Debug.Log("DialogueInstigator OnDestroy");
            m_DialogueChannel.OnDialogueNodeRequested -= m_DialogueSequencer.StartDialogueNode;
            m_DialogueChannel.OnDialogueRequested -= m_DialogueSequencer.StartDialogue;

            m_DialogueSequencer.OnDialogueNodeEnd -= m_DialogueChannel.RaiseDialogueNodeEnd;
            m_DialogueSequencer.OnDialogueNodeStart -= m_DialogueChannel.RaiseDialogueNodeStart;
            m_DialogueSequencer.OnDialogueEnd -= OnDialogueEnd;
            m_DialogueSequencer.OnDialogueStart -= OnDialogueStart;

            m_DialogueSequencer = null;
        }

        private void OnDialogueStart(Data.Dialogue dialogue)
        {
            Debug.Log("DialogueInstigator OnDialogueStart");
            m_DialogueChannel.RaiseDialogueStart(dialogue);

            if (FlowStateMachine.Instance.CurrentState == null)
            {
                Debug.Log("OnDialogueStart FlowStateMachine.Instance.CurrentState == null");
            }
            else
            {
                Debug.Log("OnDialogueStart FlowStateMachine.Instance.CurrentState: " +
                          FlowStateMachine.Instance.CurrentState.m_StateType);
            }

            Debug.Log("OnDialogueStart FlowStateMachine.Instance.CurrentState: " +
                      FlowStateMachine.Instance.CurrentState);
            m_CachedFlowState = FlowStateMachine.Instance.CurrentState;
            Debug.Log("OnDialogueStart m_CachedFlowState: " + m_CachedFlowState);
            m_FlowChannel.RaiseFlowStateRequest(m_DialogueState);
        }

        private void OnDialogueEnd(Data.Dialogue dialogue)
        {
            if (m_CachedFlowState == null)
            {
                Debug.Log("OnDialogueEnd FlowStateMachine.Instance.CurrentState == null");
            }
            else
            {
                Debug.Log("OnDialogueEnd FlowStateMachine.Instance.CurrentState: " +
                          m_CachedFlowState.m_StateType);
            }

            Debug.Log("DialogueInstigator OnDialogueEnd");
            Debug.Log("OnDialogueEnd m_CachedFlowState: " + m_CachedFlowState);
            m_FlowChannel.RaiseFlowStateRequest(m_CachedFlowState);
            m_CachedFlowState = null;

            m_DialogueChannel.RaiseDialogueEnd(dialogue);
        }

        #region Getter & Setters

        public DialogueChannel DialogueChannel
        {
            get => m_DialogueChannel;
        }

        public FlowChannel FlowChannel
        {
            get => m_FlowChannel;
        }

        #endregion
    }
};