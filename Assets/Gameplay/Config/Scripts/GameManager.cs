#region

using System;
using UnityEngine;

#endregion

public class GameManager : MonoBehaviour
{
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
        SceneTransitionHandler.Instance.LoadScene(SceneTransitionHandler.SceneStates.InGame);
        SoundManager.Instance.StartBackgroundMusic(SoundManager.BackgroundMusic.InGame);
        IsGameStarted = true;
        OnGameStarted?.Invoke();
    }

    public void EndGame()
    {
        SceneTransitionHandler.Instance.LoadScene(SceneTransitionHandler.SceneStates.EndGame);
        IsGameStarted = false;
    }

    public void RestartGame()
    {
        StartGame();
    }

    #endregion
}