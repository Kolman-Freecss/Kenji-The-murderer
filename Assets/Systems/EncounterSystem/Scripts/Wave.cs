#region

using UnityEngine;

#endregion

public class Wave : MonoBehaviour
{
    [Header("Debug")] [SerializeField] private bool debugStartWave;
    [SerializeField] private bool debugHasFinished;

    private void OnValidate()
    {
        if (debugStartWave)
        {
            debugStartWave = false;
            StartWave();
        }
    }

    private void Awake()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        debugHasFinished = HasFinished();
    }

    public void StartWave()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    public bool HasFinished()
    {
        return transform.childCount == 0;
    }
}