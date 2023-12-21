#region

using Systems.NarrationSystem.Dialogue;
using Systems.NarrationSystem.Dialogue.Data;
using Systems.NarrationSystem.Dialogue.Data.Nodes;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

#endregion

namespace Systems.NarrationSystem.UI
{
    public class UIDialogueTextBoxController : MonoBehaviour, IDialogueNodeVisitor
    {
        [SerializeField] private TextMeshProUGUI m_SpeakerText;

        [SerializeField] private TextMeshProUGUI m_DialogueText;

        [SerializeField] private RectTransform m_ChoicesBoxTransform;

        [SerializeField] private UIDialogueChoiceController m_ChoiceControllerPrefab;

        [SerializeField] private DialogueChannel m_DialogueChannel;

        [SerializeField] private TextMeshProUGUI m_ContinueText;

        [Header("Input Actions")] [SerializeField]
        private InputActionReference m_SubmitAction;

        private bool m_ListenToInput = false;
        private DialogueNode m_NextNode = null;

        private void Awake()
        {
            m_DialogueChannel.OnDialogueNodeStart += OnDialogueNodeStart;
            m_DialogueChannel.OnDialogueNodeEnd += OnDialogueNodeEnd;

            gameObject.SetActive(false);
            m_ChoicesBoxTransform.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            m_SubmitAction.action.Enable();
            m_SubmitAction.action.performed += OnSubmitAction;
        }

        private void OnDestroy()
        {
            m_DialogueChannel.OnDialogueNodeEnd -= OnDialogueNodeEnd;
            m_DialogueChannel.OnDialogueNodeStart -= OnDialogueNodeStart;
        }

        private void OnSubmitAction(InputAction.CallbackContext context)
        {
            Debug.Log("Submit action");
            if (m_ListenToInput)
            {
                m_DialogueChannel.RaiseRequestDialogueNode(m_NextNode);
            }
        }

        private void OnDialogueNodeStart(DialogueNode node)
        {
            if (node is ChoiceDialogueNode)
            {
                m_ContinueText.text = "Select a choice";
            }
            else
            {
                m_ContinueText.text = "Press Enter to continue";
            }

            m_ContinueText.gameObject.SetActive(true);
            gameObject.SetActive(true);

            m_DialogueText.text = node.DialogueLine.Text;
            m_SpeakerText.text = node.DialogueLine.Speaker.CharacterName;

            node.Accept(this);
        }

        private void OnDialogueNodeEnd(DialogueNode node)
        {
            m_NextNode = null;
            m_ListenToInput = false;
            m_DialogueText.text = "";
            m_SpeakerText.text = "";

            foreach (Transform child in m_ChoicesBoxTransform)
            {
                Destroy(child.gameObject);
            }

            m_ContinueText.gameObject.SetActive(false);
            gameObject.SetActive(false);
            m_ChoicesBoxTransform.gameObject.SetActive(false);
        }

        public void Visit(BasicDialogueNode node)
        {
            m_ListenToInput = true;
            m_NextNode = node.NextNode;
        }

        public void Visit(ChoiceDialogueNode node)
        {
            m_ChoicesBoxTransform.gameObject.SetActive(true);

            foreach (DialogueChoice choice in node.Choices)
            {
                UIDialogueChoiceController newChoice = Instantiate(m_ChoiceControllerPrefab, m_ChoicesBoxTransform);
                newChoice.Choice = choice;
            }
        }

        private void OnDisable()
        {
            m_SubmitAction.action.performed -= OnSubmitAction;
            m_SubmitAction.action.Disable();
        }
    }
}