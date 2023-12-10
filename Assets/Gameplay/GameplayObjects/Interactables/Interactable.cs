#region

using UnityEngine;
using UnityEngine.Events;

#endregion

namespace Gameplay.GameplayObjects.Interactables
{
    /// <summary>
    /// Base class for all interactable objects.
    /// </summary>
    public abstract class Interactable : MonoBehaviour, IInteractable
    {
        #region Member Variables

        [SerializeField] protected UnityEvent<object> m_OnInteraction;

        [HideInInspector] public bool m_IsInteractable = true;

        #endregion

        public virtual void DoInteraction<TObject>(TObject obj)
            where TObject : IInteractable
        {
            m_OnInteraction.Invoke(obj);
        }

        private void OnDestroy()
        {
            m_OnInteraction.RemoveAllListeners();
        }

        private void OnDisable()
        {
            // RemoveFromAnyList that contains this interactable
        }
    }
}