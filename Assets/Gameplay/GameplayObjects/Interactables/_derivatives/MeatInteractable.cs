#region

using Gameplay.GameplayObjects.Interactables._common;
using Gameplay.GameplayObjects.Player.Script;

#endregion

namespace Gameplay.GameplayObjects.Interactables._derivatives
{
    public class MeatInteractable : InteractableObject<MeatInteractable>
    {
        public float meatRecovery = 10f;

        private void Start()
        {
            m_OnInteraction.AddListener(GameManager.Instance.m_player.GetComponent<PlayerStats>()
                .OnMeatCollected);
        }
    }
}