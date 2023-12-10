namespace Gameplay.GameplayObjects.Interactables._common
{
    /// <summary>
    /// Base class for all interactable objects.
    /// (Any object that can be interacted with by the player)
    /// </summary>
    public class BaseInteractableObject<TData> : BaseInteractable<TData> where TData : IInteractable
    {
        public override void DoInteraction()
        {
            if (this is BaseInteractableObject<TData> obj)
            {
                // Do something
            }

            base.DoInteraction();
        }
    }
}