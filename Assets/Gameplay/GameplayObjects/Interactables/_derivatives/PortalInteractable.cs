#region

using Gameplay.GameplayObjects.Interactables._common;
using UnityEngine;

#endregion

namespace Gameplay.GameplayObjects.Interactables._derivatives
{
    public class PortalInteractable : BaseInteractableObject<PortalInteractable>
    {
        [SerializeField] private AudioClip m_PortalInteraction;
        [SerializeField] private GameManager.RoundTypes m_NextRoundType;

        public override void DoInteraction()
        {
            if (m_AudioSource != null)
                m_AudioSource.PlayOneShot(m_PortalInteraction);
            else
            {
                Debug.LogWarning("No audio source attached to portal interactable");
            }

            base.DoInteraction();
        }

        public GameManager.RoundTypes NextRoundType => m_NextRoundType;
    }
}