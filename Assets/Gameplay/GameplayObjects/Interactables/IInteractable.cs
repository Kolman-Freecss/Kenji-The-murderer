namespace Gameplay.GameplayObjects.Interactables
{
    /// <summary>
    /// @Author: Kolman-Freecss (Sergio Martínez Román)
    /// </summary>
    public interface IInteractable<TData> where TData : IInteractable<TData>
    {
        void DoInteraction(TData obj);
    }
}