#region

using System;
using System.Collections;
using Gameplay.GameplayObjects.Interactables._derivatives;
using Gameplay.GameplayObjects.Player.Script;
using Systems.NarrationSystem.Dialogue.Components;
using Systems.NarrationSystem.Dialogue.Data;
using Systems.NarrationSystem.Flow;
using UnityEngine;

#endregion

public class GameManager : MonoBehaviour
{
    public enum RoundTypes
    {
        None,
        InGame_Init,
        InGame_Second
    }

    public float timeToFinishGame = 5f;

    [SerializeField] private Dialogue m_GameFinalDialogue;

    #region Member properties

    public static GameManager Instance { get; private set; }

    public event Action OnGameStarted;
    public bool IsGameStarted { get; private set; }

    [HideInInspector] public PlayerController m_player;

    [HideInInspector] public bool m_GameWon;

    public bool gamePaused;

    private Action m_OnLastDialogueFinish;
    private bool lastFinalDialogueInit = false;

    #endregion

    #region InitData

    void Awake()
    {
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
            DontDestroyOnLoad(this);
        }
    }

    #endregion

    #region Logic

    public void StartGame()
    {
        SceneTransitionHandler.Instance.LoadScene(SceneTransitionHandler.SceneStates.InGameInit);
        SoundManager.Instance.StartBackgroundMusic(SoundManager.BackgroundMusic.InGameInit);
        IsGameStarted = true;
        OnGameStarted?.Invoke();
    }

    public void RestartGame()
    {
        StartGame();
    }

    public void PauseGameEvent(bool mIsPaused)
    {
        gamePaused = mIsPaused;
        m_player.enabled = !mIsPaused;
        m_player.GetComponent<PlayerMeleeAttackController>().enabled = !mIsPaused;
        m_player.GetComponent<PlayerInteractionInstigator>().enabled = !mIsPaused;
    }

    public void OnPlayerDeath(RoundTypes roundType)
    {
        EndGame(false);
    }

    public void OnPlayerEndRound(RoundTypes roundType, PortalInteractable portalInteractable)
    {
        switch (roundType)
        {
            case RoundTypes.InGame_Init:
                if (portalInteractable.NextRoundType == RoundTypes.InGame_Second)
                {
                    SceneTransitionHandler.Instance.LoadScene(SceneTransitionHandler.SceneStates.InGameSecond);
                    SoundManager.Instance.StartBackgroundMusic(SoundManager.BackgroundMusic.InGameSecond);
                }
                else
                {
                    EndGame(true);
                }

                break;
            case RoundTypes.InGame_Second:
                EndGame(true);
                break;
        }
    }

    public void EndGame(bool isWin)
    {
        if (m_GameFinalDialogue != null)
        {
            m_OnLastDialogueFinish += () => FinishGame();
            InitFinalNarration();
        }
        else
        {
            FinishGame();
        }

        void FinishGame()
        {
            m_OnLastDialogueFinish -= () => FinishGame();
            m_GameWon = isWin;
            SoundManager.Instance.StartBackgroundMusic(isWin
                ? SoundManager.BackgroundMusic.WinGame
                : SoundManager.BackgroundMusic.LostGame);
            StartCoroutine(OnEndGame());
        }

        IEnumerator OnEndGame()
        {
            yield return new WaitForSeconds(timeToFinishGame);
            SceneTransitionHandler.Instance.LoadScene(SceneTransitionHandler.SceneStates.EndGame);
            IsGameStarted = false;
        }
    }

    private void InitFinalNarration()
    {
        try
        {
            FlowListener.Instance.Entries[0].m_Event.RemoveAllListeners();
            FlowListener.Instance.Entries[1].m_Event.RemoveAllListeners();
            FlowListener.Instance.Entries[0].m_Event.AddListener(DialogueStarted);
            FlowListener.Instance.Entries[1].m_Event.AddListener(DialogueEnded);
            DialogueInstigator.Instance.FlowChannel.OnFlowStateChanged += OnFlowStateChanged;
            DialogueInstigator.Instance.DialogueChannel.RaiseRequestDialogue(m_GameFinalDialogue);
        }
        catch (Exception e)
        {
            Debug.LogError("GameManager: Error while initializing narration round: " + e);
        }
    }

    private void OnFlowStateChanged(FlowState state)
    {
        DialogueInstigator.Instance.FlowChannel.OnFlowStateChanged -= OnFlowStateChanged;
    }

    public void DialogueStarted()
    {
        if (Instance.m_player == null)
        {
            Instance.m_player = FindObjectOfType<PlayerController>();
            if (Instance.m_player == null)
            {
                Debug.LogError("RoundManager: No player found");
                return;
            }
        }

        lastFinalDialogueInit = true;

        PauseGameEvent(true);
    }

    public void DialogueEnded()
    {
        if (Instance.m_player == null)
        {
            Instance.m_player = FindObjectOfType<PlayerController>();
            if (Instance.m_player == null)
            {
                Debug.LogError("RoundManager: No player found");
                return;
            }
        }

        if (lastFinalDialogueInit)
        {
            m_OnLastDialogueFinish?.Invoke();
        }

        PauseGameEvent(false);
    }

    #endregion
}