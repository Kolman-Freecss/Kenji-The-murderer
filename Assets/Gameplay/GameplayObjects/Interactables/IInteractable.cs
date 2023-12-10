namespace Gameplay.GameplayObjects.Interactables
{
    public interface IInteractable
    {
        void DoInteraction<TObject>(TObject obj)
            where TObject : IInteractable;
    }
}
