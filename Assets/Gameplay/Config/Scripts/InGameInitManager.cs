#region

using System;
using System.Collections.Generic;
using Gameplay.Config.Scripts;
using Gameplay.GameplayObjects.Interactables._derivatives;
using Systems.NarrationSystem.Dialogue.Components;
using Systems.NarrationSystem.Dialogue.Data;
using Systems.NarrationSystem.Flow;
using UnityEngine;

#endregion

public class InGameInitManager : RoundManager
{
    public static InGameInitManager Instance { get; private set; }

    #region Inspector Variables

    [SerializeField] private List<Encounter> m_encounters;

    [SerializeField] private Dialogue m_RoundStartDialogue;

    #endregion

    #region Member Variables

    private int currentEncountersFinished = 0;

    #endregion

    #region InitData

    protected new void Awake()
    {
        base.Awake();
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

    private void Start()
    {
        m_encounters.ForEach(encounter => encounter.onEncounterFinished.AddListener(OnEncounterEnded));
        m_CurrentRoundState = RoundState.Starting;
        if (m_RoundStartDialogue != null)
        {
            InitNarrationRound();
        }
        else
        {
            Debug.LogError("RoundManager: No round start dialogue set");
            StartRound();
        }

        InitRoundData();
    }

    private void OnEncounterEnded()
    {
        currentEncountersFinished++;
        if (currentEncountersFinished == m_encounters.Count)
        {
            EndRound();
        }
    }

    private void InitRoundData()
    {
        currentEncountersFinished = 0;
    }

    #endregion

    #region Dialogue Logic

    private void InitNarrationRound()
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

    private void OnFlowStateChanged(FlowState state)
    {
        DialogueInstigator.Instance.FlowChannel.OnFlowStateChanged -= OnFlowStateChanged;
        OnStartRound();
    }

    public void DialogueStarted()
    {
        Time.timeScale = 0f;
        GameManager.Instance.m_player.enabled = false;
    }

    public void DialogueEnded()
    {
        Time.timeScale = 1f;
        if (GameManager.Instance.m_player == null)
            return;
        GameManager.Instance.m_player.enabled = true;
    }

    #endregion

    #region Round Flow

    /// <summary>
    /// Is called by the RoundManagerUI when the player clicks on the start round button.
    /// </summary>
    public void OnStartRound()
    {
        StartRound();
    }

    public void StartRound()
    {
        m_CurrentRoundState = RoundState.Started;
        OnRoundStarted?.Invoke();
    }

    public void EndRound()
    {
        m_CurrentRoundState = RoundState.Ended;
        m_roundPortals.ForEach(portal =>
            portal.Value.gameObject.SetActive(true));
    }

    public override void UsePortal(PortalInteractable portalInteractable)
    {
        //TODO: Add logic for concrete portal usage
        base.UsePortal(portalInteractable);
    }

    #endregion

    #region Destructor

    private void OnDestroy()
    {
        m_encounters.ForEach(encounter => encounter.onEncounterFinished.RemoveListener(OnEncounterEnded));
    }

    #endregion
}