#region

using Gameplay.GameplayObjects.Interactables._common;
using UnityEngine;

#endregion

namespace Gameplay.GameplayObjects.Interactables._derivatives
{
    public class PortalInteractable : BaseInteractableObject
    {
        [SerializeField] private AudioClip m_PortalInteraction;
        [SerializeField] private GameManager.RoundTypes m_NextRoundType;

        public override void DoInteraction<TData>(TData obj)
        {
            // Audio.PlaySound("PortalInteraction");
            base.DoInteraction(obj);
        }

        public GameManager.RoundTypes NextRoundType => m_NextRoundType;
    }
}