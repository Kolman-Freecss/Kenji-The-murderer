#region

#endregion

namespace Gameplay.GameplayObjects.Interactables
{
    /// <summary>
    /// Base class for all interactable objects.
    /// </summary>
    public abstract class Interactable<TData> : BaseInteractable, IBaseInteractable<TData> where TData : IInteractable
    {
        protected override void OnDestroy()
        {
            base.OnDestroy();
            RemoveFromPlayerInteractables();
        }

        private void OnDisable()
        {
            RemoveFromPlayerInteractables();
        }

        private void RemoveFromPlayerInteractables()
        {
            if (GameManager.Instance.m_player)
            {
                GameManager.Instance.m_player.GetComponent<PlayerInteractionInstigator>().OnDestroyInteractable(this);
            }
        }
    }
}