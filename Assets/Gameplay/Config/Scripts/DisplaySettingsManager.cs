#region

using UnityEngine;

#endregion

namespace Gameplay.Config.Scripts
{
    public class DisplaySettingsManager : MonoBehaviour
    {
        #region Member Variables

        public static DisplaySettingsManager Instance { get; private set; }

        public bool windowed = false;

        #endregion

        #region InitData

        private void Awake()
        {
            ManageSingleton();
        }

        void ManageSingleton()
        {
            if (Instance != null)
            {
                gameObject.SetActive(false);
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        #endregion

        public void ToggleWindowed(bool value)
        {
            SoundManager.Instance.PlayButtonClickSound();
            windowed = value;
            Screen.fullScreen = windowed;
        }
    }
}