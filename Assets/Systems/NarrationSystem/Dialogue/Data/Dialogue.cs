#region

using Systems.NarrationSystem.Dialogue.Data.Nodes;
using UnityEngine;

#endregion

namespace Systems.NarrationSystem.Dialogue.Data
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Narration/Dialogue/Dialogue")]
    public class Dialogue : ScriptableObject
    {
        [SerializeField]
        private DialogueNode m_FirstNode;
        public DialogueNode FirstNode => m_FirstNode;
    }
}
