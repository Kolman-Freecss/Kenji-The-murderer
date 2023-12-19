#region

using UnityEngine;
using UnityEngine.Events;

#endregion

namespace Gameplay.GameplayObjects.Interactables
{
    public abstract class BaseInteractable : MonoBehaviour
    {
        #region Member Variables

        [SerializeField] protected UnityEvent<object> m_OnInteraction;

        [HideInInspector] public bool m_IsInteractable = true;

        [SerializeField] protected AudioClip m_audioInteraction;

        protected AudioSource m_AudioSource;

        #endregion

        private void Awake()
        {
            m_AudioSource = GetComponent<AudioSource>();
        }

        public virtual void DoInteraction()
        {
            if (m_AudioSource != null && m_audioInteraction != null)
            {
                m_AudioSource.volume = SoundManager.Instance.EffectsAudioVolume / 100f;
                m_AudioSource.PlayOneShot(m_audioInteraction);
            }
            else
            {
                Debug.LogWarning("No audio source attached to interactable");
            }

            m_OnInteraction.Invoke(this);
        }

        protected virtual void OnDestroy()
        {
            m_OnInteraction.RemoveAllListeners();
        }


        public UnityEvent<object> OnInteraction => m_OnInteraction;
        public AudioSource AudioSource => m_AudioSource;
    }
}