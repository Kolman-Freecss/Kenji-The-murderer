#region

using System;
using System.Collections.Generic;
using Gameplay.Config.Scripts;
using Systems.NarrationSystem.Dialogue.Components;
using Systems.NarrationSystem.Dialogue.Data;
using Systems.NarrationSystem.Flow;
using TMPro;
using UnityEngine;

#endregion

public class InGameInitManager : RoundManager
{
    public static InGameInitManager Instance { get; private set; }

    #region Inspector Variables

    [SerializeField] private TextMeshProUGUI m_roundStartText;

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

    protected new void Start()
    {
        base.Start();
        m_encounters.ForEach(encounter => { encounter.onEncounterFinished.AddListener(OnEncounterEnded); });
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
        m_roundStartText.text = currentEncountersFinished + "/" + m_encounters.Count + " Waves";
        if (currentEncountersFinished == m_encounters.Count)
        {
            EndRound();
        }
    }

    private void InitRoundData()
    {
        currentEncountersFinished = 0;
        m_roundStartText.text = currentEncountersFinished + "/" + m_encounters.Count + " Waves";
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
        //Time.timeScale = 0f;
        if (GameManager.Instance.m_player == null)
        {
            Debug.LogError("RoundManager: No player found");
            return;
        }

        GameManager.Instance.m_player.enabled = false;
    }

    public void DialogueEnded()
    {
        //Time.timeScale = 1f;
        if (GameManager.Instance.m_player == null)
        {
            Debug.LogError("RoundManager: No player found");
            return;
        }

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
        portalsWrapper.SetActive(true);
        portalsWrapper.GetComponent<AudioSource>()?.Play();
        // m_roundPortals.ForEach(portal =>
        //     portal.Value.gameObject.SetActive(true));
    }

    public override void UsePortal(object portalInteractable)
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