#region

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#endregion

/// <summary>
/// @Author: Kolman-Freecss (Sergio Martínez Román)
/// </summary>
public class SceneTransitionHandler : MonoBehaviour
{
    #region Inspector Variables

    [SerializeField] public SceneStates DefaultScene = SceneStates.Home;

    [Header("Loading Screen")] [SerializeField]
    private GameObject LoadingScreen;

    [SerializeField] private Image loadingBarFill;

    public bool debug;

    #endregion

    #region Member properties

    public static SceneTransitionHandler Instance { get; private set; }

    private SceneStates m_SceneState;

    #endregion

    #region Event Delegates

    public delegate void SceneStateChangedDelegateHandler(SceneStates newState);

    public event SceneStateChangedDelegateHandler OnSceneStateChanged;

    #endregion

    public enum SceneStates
    {
        InitBootstrap,
        Home,
        Settings,
        Credits,
        InGameInit,
        InGameSecond,
        EndGame
    }

    #region InitData

    void Awake()
    {
        ManageSingleton();
        if (debug)
        {
            Debug.Log("SceneTransitionHandler Awake");
        }
        else
        {
            SetSceneState(SceneStates.InitBootstrap);
        }
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

    void Start()
    {
        if (debug) return;
        if (m_SceneState == SceneStates.InitBootstrap)
        {
            LoadScene(DefaultScene);
        }
    }

    #endregion

    #region Logic

    public void LoadScene(SceneStates sceneState)
    {
        StartCoroutine(LoadingSceneAsync(sceneState));

        IEnumerator LoadingSceneAsync(SceneStates sceneState)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneState.ToString());
            operation.completed += (op) =>
            {
                SetSceneState(sceneState);
                LoadingScreen.SetActive(false);
                operation.completed -= (op1) => { };
            };

            LoadingScreen.SetActive(true);

            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / .9f);
                loadingBarFill.fillAmount = progress;
                yield return null;
            }
        }
    }

    private void SetSceneState(SceneStates sceneState)
    {
        m_SceneState = sceneState;
        if (OnSceneStateChanged != null)
        {
            OnSceneStateChanged.Invoke(m_SceneState);
        }
    }

    #endregion

    #region Destructor

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    #endregion

    #region Getter & Setter

    public SceneStates GetCurrentSceneState()
    {
        return m_SceneState;
    }

    #endregion
}