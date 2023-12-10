#region

using Systems.NarrationSystem.Dialogue.Data.Nodes;

#endregion

namespace Systems.NarrationSystem.Dialogue.Data
{
    public interface IDialogueNodeVisitor
    {
        void Visit(BasicDialogueNode node);
        void Visit(ChoiceDialogueNode node);
    }
}
