#region

using UnityEngine;
using UnityEngine.UI;

#endregion

public class SettingsUIManager : MonoBehaviour
{
    #region Inspector Variables

    [Header("Buttons")] [SerializeField] private Button backButton;

    [Header("Sliders")] [SerializeField] private Slider m_MasterVolumeSlider;

    [SerializeField] private Slider m_EffectsVolumeSlider;

    [SerializeField] private Slider m_MusicVolumeSlider;

    #endregion

    #region InitData

    private void OnEnable()
    {
        backButton
            .onClick
            .AddListener(() =>
            {
                SoundManager.Instance.PlayButtonClickSound();
                SceneTransitionHandler.Instance.LoadScene(SceneTransitionHandler.SceneStates.Home);
            });

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

    #endregion

    #region Logic

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

    #endregion

    #region Destructor

    private void OnDisable()
    {
        backButton.onClick.RemoveAllListeners();
        m_MasterVolumeSlider.onValueChanged.RemoveListener(OnMasterVolumeSliderChanged);
        m_MusicVolumeSlider.onValueChanged.RemoveListener(OnMusicVolumeSliderChanged);
        m_EffectsVolumeSlider.onValueChanged.RemoveListener(OnEffectsVolumeSliderChanged);
    }

    #endregion
}