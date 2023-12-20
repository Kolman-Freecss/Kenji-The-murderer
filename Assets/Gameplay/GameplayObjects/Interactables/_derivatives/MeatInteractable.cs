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
            //TODO: Remove this
            if (!GameManager.Instance.m_player && GameManager.Instance.debug)
            {
                GameManager.Instance.SetPlayerDebug();
            }

            m_OnInteraction.AddListener(GameManager.Instance.m_player.GetComponent<PlayerStats>()
                .OnMeatCollected);
        }
    }
}