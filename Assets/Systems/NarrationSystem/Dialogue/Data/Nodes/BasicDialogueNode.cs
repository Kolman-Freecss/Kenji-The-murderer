#region

using UnityEngine;

#endregion

namespace Systems.NarrationSystem.Dialogue.Data.Nodes
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Narration/Dialogue/Node/Basic")]
    public class BasicDialogueNode : DialogueNode
    {
        [SerializeField]
        private DialogueNode m_NextNode;
        public DialogueNode NextNode => m_NextNode;

        public override bool CanBeFollowedByNode(DialogueNode node)
        {
            return m_NextNode == node;
        }

        public override void Accept(IDialogueNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
