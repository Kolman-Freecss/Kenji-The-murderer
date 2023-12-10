#region

using System;
using Gameplay.GameplayObjects.Interactables._derivatives;
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

    #region Member properties

    public static GameManager Instance { get; private set; }

    public event Action OnGameStarted;
    public bool IsGameStarted { get; private set; }

    [HideInInspector] public PlayerController m_player;

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
        SceneTransitionHandler.Instance.LoadScene(SceneTransitionHandler.SceneStates.EndGame);
        SoundManager.Instance.StartBackgroundMusic(isWin
            ? SoundManager.BackgroundMusic.WinGame
            : SoundManager.BackgroundMusic.LostGame);
        IsGameStarted = false;
    }

    #endregion
}