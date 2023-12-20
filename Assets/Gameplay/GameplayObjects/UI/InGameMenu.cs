#region

using System;
using Gameplay.Config.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

#endregion

public class InGameMenu : MonoBehaviour
{
    public static InGameMenu Instance { get; private set; }

    [Header("Input References")] [SerializeField]
    private InputActionReference pauseAction;

    [SerializeField] private InputActionReference tutorialAction;

    private delegate void TutorialActionDelegate(InputAction.CallbackContext context);

    private TutorialActionDelegate onTutorialAction;

    private delegate void PauseActionDelegate(InputAction.CallbackContext context);

    private PauseActionDelegate onPauseAction;

    [Header("Tutorial Settings")] [SerializeField]
    private GameObject tutorialPanel;

    [SerializeField] private TextMeshProUGUI tutorialText;

    [Header("Buttons")] [SerializeField] private Button backButton;
    [SerializeField] private Button menuButton;

    [Header("Sliders")] [SerializeField] private Slider m_MasterVolumeSlider;
    [SerializeField] private Slider m_EffectsVolumeSlider;

    [SerializeField] private Slider m_MusicVolumeSlider;
    [SerializeField] private Toggle windowedToggle;

    [SerializeField] private CanvasRenderer canvas;

    private Boolean m_isPaused = false;

    #region InitData

    private void Awake()
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
        }
    }

    private void OnEnable()
    {
        try
        {
            pauseAction.action.Enable();
            onPauseAction = new PauseActionDelegate(OnPause);
            pauseAction.action.performed += onPauseAction.Invoke;

            backButton
                .onClick
                .AddListener(() => { OnPause(); });

            menuButton
                .onClick
                .AddListener(() => { OnExitMenu(); });

            tutorialAction.action.Enable();
            onTutorialAction = new TutorialActionDelegate(OnTutorial);
            tutorialAction.action.performed += onTutorialAction.Invoke;

            // Note that we initialize the slider BEFORE we listen for changes (so we don't get notified of our own change!)
            m_MasterVolumeSlider.value = SoundManager.Instance.MasterAudioVolume;
            m_MasterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeSliderChanged);

            // initialize music slider similarly.
            m_MusicVolumeSlider.value = SoundManager.Instance.MusicAudioVolume;
            m_MusicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeSliderChanged);

            // initialize effects slider similarly.
            m_EffectsVolumeSlider.value = SoundManager.Instance.EffectsAudioVolume;
            m_EffectsVolumeSlider.onValueChanged.AddListener(OnEffectsVolumeSliderChanged);
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
        }
    }

    void Start()
    {
        Init();
    }

    void Init()
    {
        tutorialPanel.SetActive(false);
        tutorialText.gameObject.SetActive(false);
        m_isPaused = false;
        canvas.gameObject.SetActive(m_isPaused);
        m_MusicVolumeSlider.value = SoundManager.Instance.MusicAudioVolume;
        m_EffectsVolumeSlider.value = SoundManager.Instance.EffectsAudioVolume;
        windowedToggle.isOn = DisplaySettingsManager.Instance.windowed;
    }

    #endregion

    public void OnExitMenu()
    {
        SoundManager.Instance.PlayButtonClickSound();
        SceneTransitionHandler.Instance.LoadScene(SceneTransitionHandler.SceneStates.Home);
    }

    private void OnMasterVolumeSliderChanged(float newValue)
    {
        SoundManager.Instance.SetMasterVolume(newValue);
    }

    private void OnMusicVolumeSliderChanged(float newValue)
    {
        SoundManager.Instance.SetMusicVolume(newValue);
    }

    private void OnEffectsVolumeSliderChanged(float newValue)
    {
        SoundManager.Instance.SetEffectsVolume(newValue);
    }

    public void OnPause(InputAction.CallbackContext context = default)
    {
        m_isPaused = !m_isPaused;
        canvas.gameObject.SetActive(m_isPaused);
        if (m_isPaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

        GameManager.Instance.PauseGameEvent(m_isPaused);
    }

    public void ActiveTutorialText()
    {
        tutorialText.gameObject.SetActive(true);
    }

    public void OnTutorial(InputAction.CallbackContext context = default)
    {
        tutorialPanel.SetActive(!tutorialPanel.activeSelf);
        if (tutorialText.gameObject.activeSelf)
        {
            tutorialText.gameObject.SetActive(false);
        }
    }

    public void OnToggleWindowed()
    {
        SoundManager.Instance.PlayButtonClickSound();
        DisplaySettingsManager.Instance.ToggleWindowed(windowedToggle.isOn);
    }

    public Boolean IsPaused()
    {
        return m_isPaused;
    }

    #region Destructor

    private void OnDisable()
    {
        tutorialAction.action.performed -= onTutorialAction.Invoke;
        pauseAction.action.performed -= onPauseAction.Invoke;
        tutorialAction.action.Disable();
        pauseAction.action.Disable();
        backButton.onClick.RemoveAllListeners();
        m_MasterVolumeSlider.onValueChanged.RemoveListener(OnMasterVolumeSliderChanged);
        m_MusicVolumeSlider.onValueChanged.RemoveListener(OnMusicVolumeSliderChanged);
        m_EffectsVolumeSlider.onValueChanged.RemoveListener(OnEffectsVolumeSliderChanged);
    }

    #endregion
}