#region

using UnityEngine;
using UnityEngine.Events;

#endregion

namespace Gameplay.GameplayObjects.Interactables
{
    /// <summary>
    /// Base class for all interactable objects.
    /// </summary>
    public abstract class BaseInteractable<TData> : MonoBehaviour, IBaseInteractable<TData> where TData : IInteractable
    {
        #region Member Variables

        [SerializeField] protected UnityEvent<object> m_OnInteraction;

        [HideInInspector] public bool m_IsInteractable = true;

        protected AudioSource m_AudioSource;

        #endregion

        private void Awake()
        {
            m_AudioSource = GetComponent<AudioSource>();
        }

        public virtual void DoInteraction()
        {
            m_OnInteraction.Invoke(this);
        }

        private void OnDestroy()
        {
            m_OnInteraction.RemoveAllListeners();
        }

        private void OnDisable()
        {
            // RemoveFromAnyList that contains this interactable
        }

        public UnityEvent<object> OnInteraction => m_OnInteraction;
        public AudioSource AudioSource => m_AudioSource;
    }
}