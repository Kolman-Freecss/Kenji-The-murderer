#region

using UnityEngine;
using UnityEngine.Events;

#endregion

public class Encounter : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] private Transform encounterDoorsParent;
    [SerializeField] private Transform encounterLimitsParent;

    [SerializeField] UnityEvent onEncounterFinished;

    Wave[] waves;
    private int currentWave = 0;

    [Header("Debug")] [SerializeField] private bool debugStartEncounter;
    [SerializeField] private bool debugHasFinished;

    private void OnValidate()
    {
        if (debugStartEncounter)
        {
            debugStartEncounter = false;
            StartEncounter();
        }
    }

    private void Awake()
    {
        SetDoorsActivation(false);

        waves = GetComponentsInChildren<Wave>();

        Enemy[] enemies = GetComponentsInChildren<Enemy>(true);
        foreach (Enemy enemy in enemies)
        {
            enemy.target = target;
        }
    }

    void StartEncounter()
    {
        SetDoorsActivation(true);

        waves[currentWave].StartWave();
    }

    private void Update()
    {
        if (currentWave < waves.Length)
        {
            if (waves[currentWave].HasFinished())
            {
                currentWave++;
                if (currentWave < waves.Length)
                {
                    waves[currentWave].StartWave();
                }
                else
                {
                    SetDoorsActivation(false);
                    onEncounterFinished.Invoke();
                }

                debugHasFinished = HasFinished();
            }
        }
    }

    private void SetDoorsActivation(bool activation)
    {
        foreach (Transform child in encounterDoorsParent)
        {
            child.gameObject.SetActive(activation);
        }
    }

    public bool HasFinished()
    {
        return currentWave >= waves.Length;
    }

    public void NotifyTriggered()
    {
        StartEncounter();
    }
}