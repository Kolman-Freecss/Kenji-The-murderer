#region

using UnityEngine;
using UnityEngine.Events;

#endregion

public class Encounter : MonoBehaviour
{
    [SerializeField] Transform target;

    [SerializeField] private Transform encounterLeftDoor;
    [SerializeField] private Transform encounterRightDoor;
    [SerializeField] private Transform encounterLimitsParent;

    [SerializeField] UnityEvent onEncounterFinished;
    private bool hasFinished;

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
        hasFinished = false;
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
                    hasFinished = true;
                    onEncounterFinished.Invoke();
                }

                debugHasFinished = HasFinished();
            }
        }
    }

    private void SetDoorsActivation(bool activation)
    {
        // Rotate left door
        if (activation)
        {
            encounterLeftDoor.localEulerAngles = Vector3.up * 90f;
        }
        else
        {
            encounterLeftDoor.localEulerAngles = Vector3.zero;
        }

        // Rotate right door
        if (activation)
        {
            encounterRightDoor.localEulerAngles = Vector3.up * -90f;
        }
        else
        {
            encounterRightDoor.localEulerAngles = Vector3.zero;
        }
    }

    public bool HasFinished()
    {
        return currentWave >= waves.Length;
    }

    public void NotifyTriggered()
    {
        if (!hasFinished)
        {
            StartEncounter();
        }
    }
}