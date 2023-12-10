namespace Gameplay.GameplayObjects.Interactables._common
{
    /// <summary>
    /// Base class for all interactable objects.
    /// (Any object that can be interacted with by the player)
    /// </summary>
    public class BaseInteractableObject : Interactable
    {
        public override void DoInteraction<TData>(TData obj)
        {
            if (obj is BaseInteractableObject)
            {
                // Do something
            }
            base.DoInteraction(obj);
        }
    }
}
