#region

using System;
using System.Collections.Generic;
using Gameplay.GameplayObjects.Interactables._derivatives;
using UnityEngine;

#endregion

namespace Gameplay.Config.Scripts
{
    public abstract class RoundManager : MonoBehaviour
    {
        public enum RoundState
        {
            NotStarted,
            Starting,
            Started,
            Ended
        }

        [Header("Round Settings")] public GameManager.RoundTypes roundType = GameManager.RoundTypes.InGame_Init;

        [SerializeField]
        protected List<SerializableDictionaryEntry<GameManager.RoundTypes, PortalInteractable>> m_roundPortals;

        // Round Settings
        protected RoundState m_CurrentRoundState;
        public Action OnRoundStarted;

        protected void Awake()
        {
            m_CurrentRoundState = RoundState.NotStarted;
        }

        protected void Start()
        {
            m_roundPortals.ForEach(portal =>
            {
                portal.Value.gameObject.SetActive(false);
                portal.Value.OnInteraction.AddListener(UsePortal);
            });
        }

        public virtual void OnPlayerDeath()
        {
            GameManager.Instance.OnPlayerDeath(roundType);
        }

        public virtual void UsePortal(object portalInteractable)
        {
            GameManager.Instance.OnPlayerEndRound(roundType, (PortalInteractable)portalInteractable);
        }
    }
}