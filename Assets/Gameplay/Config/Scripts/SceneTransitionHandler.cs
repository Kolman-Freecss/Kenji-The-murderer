#region

using UnityEngine;
using UnityEngine.SceneManagement;

#endregion

public class SceneTransitionHandler : MonoBehaviour
{
    #region Inspector Variables

    [SerializeField] public SceneStates DefaultScene = SceneStates.Home;

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
        InGame,
        EndGame
    }

    #region InitData

    void Awake()
    {
        ManageSingleton();
        SetSceneState(SceneStates.InitBootstrap);
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
        if (m_SceneState == SceneStates.InitBootstrap)
        {
            LoadScene(DefaultScene);
        }
    }

    #endregion

    #region Logic

    public void LoadScene(SceneStates sceneState)
    {
        SceneManager.LoadSceneAsync(sceneState.ToString());
        SetSceneState(sceneState);
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