#region

using UnityEngine;

#endregion

namespace Systems.NarrationSystem.Dialogue.Data.Nodes
{
    public abstract class DialogueNode : ScriptableObject
    {
        [SerializeField]
        private NarrationLine m_DialogueLine;

        public NarrationLine DialogueLine => m_DialogueLine;

        public abstract bool CanBeFollowedByNode(DialogueNode node);
        public abstract void Accept(IDialogueNodeVisitor visitor);
    }
}
