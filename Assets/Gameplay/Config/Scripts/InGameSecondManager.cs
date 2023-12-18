#region

using Gameplay.Config.Scripts;

#endregion

public class InGameSecondManager : RoundManager
{
    public static InGameSecondManager Instance { get; private set; }

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

    #endregion
}