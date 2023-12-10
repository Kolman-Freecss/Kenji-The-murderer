namespace Gameplay.GameplayObjects.Interactables
{
    /// <summary>
    /// @Author: Kolman-Freecss (Sergio Martínez Román)
    /// </summary>
    public interface IBaseInteractable<TData> : IInteractable where TData : IInteractable
    {
        void DoInteraction();
    }
}