#region

using System;
using System.Linq;
using UnityEngine;

#endregion

namespace Systems.NarrationSystem.Dialogue.Data.Nodes
{
    [Serializable]
    public class DialogueChoice
    {
        [SerializeField]
        private string m_ChoicePreview;

        [SerializeField]
        private DialogueNode m_ChoiceNode;

        public string ChoicePreview => m_ChoicePreview;
        public DialogueNode ChoiceNode => m_ChoiceNode;
    }

    [CreateAssetMenu(menuName = "Scriptable Objects/Narration/Dialogue/Node/Choice")]
    public class ChoiceDialogueNode : DialogueNode
    {
        [SerializeField]
        private DialogueChoice[] m_Choices;
        public DialogueChoice[] Choices => m_Choices;

        public override bool CanBeFollowedByNode(DialogueNode node)
        {
            return m_Choices.Any(x => x.ChoiceNode == node);
        }

        public override void Accept(IDialogueNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
