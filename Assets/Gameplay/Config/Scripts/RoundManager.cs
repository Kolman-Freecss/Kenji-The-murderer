#region

using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay.GameplayObjects.Interactables;
using Gameplay.GameplayObjects.Interactables._derivatives;
using Systems.NarrationSystem.Dialogue.Components;
using Systems.NarrationSystem.Dialogue.Data;
using Systems.NarrationSystem.Flow;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

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

        public enum ZoneType
        {
            None,
            Init,
            Second
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

        [SerializeField] protected Volume m_volume;

        [SerializeField] protected float m_maxHeightFogWhenNotEncounter = 3f;
        [SerializeField] protected float m_maxHeightFogWhenEncounter = 12f;
        [SerializeField] protected float m_baseHeightFogWhenEncounter = 5f;

        [SerializeField] protected float m_baseFogWhenNotEncounter = 1f;

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
        private bool m_roundStartedDialogueInit = false;
        protected ZoneType m_currentZone = ZoneType.None;

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
            Debug.Log("RoundManager: Start");
            m_roundStartedDialogueInit = false;
            portalsWrapper.SetActive(false);
            m_CurrentRoundState = RoundState.Starting;
            m_roundPortals.ForEach(portal =>
            {
                //portal.Value.gameObject.SetActive(false);
                portal.Value.OnInteraction.AddListener(UsePortal);
                Debug.Log("RoundManager: Portal " + portal.Value.name + " set Listener");
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
            m_roundStartText.text = currentEncountersFinished + "/" + m_encounters.Count + " Encounters";
        }

        #region Encounter Logic

        public void StartEncounter()
        {
            SoundManager.Instance.StartTemporalBackground(SoundManager.BackgroundMusic.StartEncounter);
            m_encounterInCourse = true;
            Fog fog = m_volume.profile.TryGet(out fog) ? fog : null;
            if (fog != null)
            {
                fog.maximumHeight.value = m_maxHeightFogWhenEncounter;
                fog.baseHeight.value = m_baseHeightFogWhenEncounter;
                StartCoroutine(RemoveFogGradually(fog));
            }

            IEnumerator RemoveFogGradually(Fog fogComponent)
            {
                float elapsedTime = 0f;
                float durationTime = 4f;

                float fogHeight = fog.maximumHeight.value;
                float fogBaseHeight = fog.baseHeight.value;
                while (elapsedTime < durationTime)
                {
                    fogHeight = Mathf.Lerp(m_maxHeightFogWhenEncounter, m_maxHeightFogWhenNotEncounter,
                        elapsedTime / durationTime);
                    fogBaseHeight = Mathf.Lerp(m_baseHeightFogWhenEncounter, m_baseFogWhenNotEncounter,
                        elapsedTime / durationTime);
                    fog.maximumHeight.value = fogHeight;
                    fog.baseHeight.value = fogBaseHeight;
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
            }
        }

        protected void OnEncounterEnded()
        {
            currentEncountersFinished++;
            m_roundStartText.text = currentEncountersFinished + "/" + m_encounters.Count + " Encounters";
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
            OnStartRound();
        }

        protected virtual void InitNarrationRound()
        {
            try
            {
                DialogueInstigator.Instance.DialogueChannel.RaiseRequestDialogue(m_RoundStartDialogue);
            }
            catch (Exception e)
            {
                Debug.LogError("RoundManager: Error while initializing narration round: " + e);
            }
        }

        public void DialogueStarted()
        {
            Debug.Log("RoundManager: Dialogue started1");
            if (GameManager.Instance.m_player == null)
            {
                GameManager.Instance.m_player = FindObjectOfType<PlayerController>();
                if (GameManager.Instance.m_player == null)
                {
                    Debug.LogError("RoundManager: No player found");
                    return;
                }
            }

            if (roundType == GameManager.RoundTypes.InGame_Init)
            {
                m_roundStartedDialogueInit = true;
            }

            GameManager.Instance.PauseGameEvent(true);
        }

        public void DialogueEnded()
        {
            Debug.Log("RoundManager: Dialogue ended1");
            if (GameManager.Instance.m_player == null)
            {
                GameManager.Instance.m_player = FindObjectOfType<PlayerController>();
                if (GameManager.Instance.m_player == null)
                {
                    Debug.LogError("RoundManager: No player found");
                    return;
                }
            }

            if (m_roundStartedDialogueInit && roundType == GameManager.RoundTypes.InGame_Init)
            {
                InGameMenu.Instance.ActiveTutorialText();
                m_roundStartedDialogueInit = false;
                StartRound();
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
            Debug.Log("RoundManager: Portal used " + portalInteractable);
            portalsWrapper.GetComponent<AudioSource>()?.Stop();
            GameManager.Instance.OnPlayerEndRound(roundType, (PortalInteractable)portalInteractable);
        }

        #endregion

        public bool IsEncounterInCourse()
        {
            return m_encounterInCourse;
        }

        #region Destructor

        private void OnDisable()
        {
            m_encounters.ForEach(encounter => encounter.onEncounterFinished.RemoveListener(OnEncounterEnded));
            m_roundPortals.ForEach(portal =>
            {
                if (portal.Value != null)
                {
                    portal.Value.OnInteraction.RemoveAllListeners();
                    Debug.Log("RoundManager: Portal " + portal.Value.name + " Remove Listener");
                }
                else
                {
                    Debug.LogError("RoundManager: Portal " + portal.Key + " is null");
                }
            });
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