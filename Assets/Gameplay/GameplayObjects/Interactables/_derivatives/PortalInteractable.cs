#region

using Gameplay.GameplayObjects.Interactables._common;
using UnityEngine;

#endregion

namespace Gameplay.GameplayObjects.Interactables._derivatives
{
    public class PortalInteractable : InteractableObject<PortalInteractable>
    {
        [SerializeField] private GameManager.RoundTypes m_NextRoundType;

        public override void DoInteraction()
        {
            base.DoInteraction();
        }

        public GameManager.RoundTypes NextRoundType => m_NextRoundType;
    }
}