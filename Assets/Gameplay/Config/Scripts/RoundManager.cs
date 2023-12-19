#region

using System;
using System.Collections.Generic;
using Gameplay.GameplayObjects.Interactables;
using Gameplay.GameplayObjects.Interactables._derivatives;
using Systems.NarrationSystem.Dialogue.Components;
using Systems.NarrationSystem.Dialogue.Data;
using Systems.NarrationSystem.Flow;
using TMPro;
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

        public enum DynamicAssets
        {
            Meat
        }

        public static RoundManager Instance { get; private set; }

        private bool m_encounterInCourse = false;

        #region Inspector Variables

        [Header("Round Settings")] public GameManager.RoundTypes roundType = GameManager.RoundTypes.InGame_Init;

        [SerializeField] protected Dialogue m_RoundStartDialogue;

        [SerializeField]
        protected List<SerializableDictionaryEntry<GameManager.RoundTypes, PortalInteractable>> m_roundPortals;

        [SerializeField] protected GameObject portalsWrapper;

        [Header("SFX")] [SerializeField] private AudioClip m_portalsOpened;

        [SerializeField] private TextMeshProUGUI m_roundStartText;

        [SerializeField] protected List<Encounter> m_encounters;

        [SerializeField] protected List<SerializableDictionaryEntry<DynamicAssets, BaseInteractable>> m_dynamicAssets;

        #endregion

        #region Member Variables

        private int currentEncountersFinished = 0;

        // Round Settings
        protected RoundState m_CurrentRoundState;
        public Action OnRoundStarted;

        #endregion

        protected void Awake()
        {
            m_CurrentRoundState = RoundState.NotStarted;
            ManageSingleton();
        }

        private void ManageSingleton()
        {
            if (Instance != null)
            {
                gameObject.SetActive(false);
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        protected void Start()
        {
            portalsWrapper.SetActive(false);
            m_CurrentRoundState = RoundState.Starting;
            m_roundPortals.ForEach(portal =>
            {
                //portal.Value.gameObject.SetActive(false);
                portal.Value.OnInteraction.AddListener(UsePortal);
            });
            if (m_RoundStartDialogue != null)
            {
                InitNarrationRound();
            }
            else
            {
                Debug.LogError("RoundManager: No round start dialogue set");
                StartRound();
            }

            m_encounters.ForEach(encounter => { encounter.onEncounterFinished.AddListener(OnEncounterEnded); });
            InitRoundData();
        }

        protected virtual void InitRoundData()
        {
            currentEncountersFinished = 0;
            m_roundStartText.text = currentEncountersFinished + "/" + m_encounters.Count + " Waves";
        }

        #region Encounter Logic

        public void StartEncounter()
        {
            SoundManager.Instance.StartTemporalBackground(SoundManager.BackgroundMusic.StartEncounter);
            m_encounterInCourse = true;
        }

        protected void OnEncounterEnded()
        {
            currentEncountersFinished++;
            m_roundStartText.text = currentEncountersFinished + "/" + m_encounters.Count + " Waves";
            if (currentEncountersFinished == m_encounters.Count)
            {
                EndRound();
            }
        }

        public void EndEncounter()
        {
            SoundManager.Instance.StartPreviousBackground();
            m_encounterInCourse = false;
        }

        #endregion

        #region Dialogue Logic

        protected virtual void OnFlowStateChanged(FlowState state)
        {
            DialogueInstigator.Instance.FlowChannel.OnFlowStateChanged -= OnFlowStateChanged;
            OnStartRound();
        }

        protected virtual void InitNarrationRound()
        {
            try
            {
                DialogueInstigator.Instance.FlowChannel.OnFlowStateChanged += OnFlowStateChanged;
                DialogueInstigator.Instance.DialogueChannel.RaiseRequestDialogue(m_RoundStartDialogue);
            }
            catch (Exception e)
            {
                Debug.LogError("RoundManager: Error while initializing narration round: " + e);
            }
        }

        public void DialogueStarted()
        {
            //Time.timeScale = 0f;
            if (GameManager.Instance.m_player == null)
            {
                GameManager.Instance.m_player = FindObjectOfType<PlayerController>();
                if (GameManager.Instance.m_player == null)
                {
                    Debug.LogError("RoundManager: No player found");
                    return;
                }
            }

            GameManager.Instance.PauseGameEvent(true);
        }

        public void DialogueEnded()
        {
            //Time.timeScale = 1f;
            if (GameManager.Instance.m_player == null)
            {
                GameManager.Instance.m_player = FindObjectOfType<PlayerController>();
                if (GameManager.Instance.m_player == null)
                {
                    Debug.LogError("RoundManager: No player found");
                    return;
                }
            }

            GameManager.Instance.PauseGameEvent(false);
        }

        #endregion


        #region Round Flow

        public virtual void OnPlayerDeath()
        {
            GameManager.Instance.OnPlayerDeath(roundType);
        }

        /// <summary>
        /// Is called by the RoundManagerUI when the player clicks on the start round button.
        /// </summary>
        protected virtual void OnStartRound()
        {
            StartRound();
        }

        protected virtual void StartRound()
        {
            m_CurrentRoundState = RoundState.Started;
            OnRoundStarted?.Invoke();
        }

        protected virtual void EndRound()
        {
            m_CurrentRoundState = RoundState.Ended;
            portalsWrapper.SetActive(true);
            portalsWrapper.GetComponent<AudioSource>()?.Play();
            // m_roundPortals.ForEach(portal =>
            //     portal.Value.gameObject.SetActive(true));
        }

        public virtual void UsePortal(object portalInteractable)
        {
            GameManager.Instance.OnPlayerEndRound(roundType, (PortalInteractable)portalInteractable);
        }

        #endregion

        public bool IsEncounterInCourse()
        {
            return m_encounterInCourse;
        }

        #region Destructor

        private void OnDestroy()
        {
            m_encounters.ForEach(encounter => encounter.onEncounterFinished.RemoveListener(OnEncounterEnded));
        }

        #endregion

        #region Getters & Setters

        public BaseInteractable GetDynamicAsset(DynamicAssets asset)
        {
            return m_dynamicAssets.Find(entry => entry.Key == asset).Value;
        }

        #endregion
    }
}