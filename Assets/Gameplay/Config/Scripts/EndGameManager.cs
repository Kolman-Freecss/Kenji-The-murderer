#region

using TMPro;
using UnityEngine;
using UnityEngine.UI;

#endregion

public class EndGameManager : MonoBehaviour
{
    #region Inspector Variables

    [Header("Buttons")] [SerializeField] private Button restartButton;

    [SerializeField] private Button mainMenuButton;

    [SerializeField] private TextMeshProUGUI TitleText;

    #endregion

    #region Init Data

    void Start()
    {
        SubscribeToEvents();
        TitleText.text = GameManager.Instance.m_GameWon ? "You Win!" : "You Lose!";
    }

    void SubscribeToEvents()
    {
        restartButton
            .onClick
            .AddListener(() => { OnRestartButtonClicked(); });
        mainMenuButton
            .onClick
            .AddListener(() => { OnMainMenuButtonClicked(); });
    }

    #endregion

    #region Logic

    void OnRestartButtonClicked()
    {
        SoundManager.Instance.PlayButtonClickSound();
        GameManager.Instance.RestartGame();
    }

    void OnMainMenuButtonClicked()
    {
        SoundManager.Instance.PlayButtonClickSound();
        GameManager.Instance.FinishGameGoMenu();
    }

    #endregion

    #region Destructor

    private void OnDestroy()
    {
        UnsubscribeToEvents();
    }

    void UnsubscribeToEvents()
    {
        restartButton
            .onClick
            .RemoveAllListeners();
        mainMenuButton
            .onClick
            .RemoveAllListeners();
    }

    #endregion
}