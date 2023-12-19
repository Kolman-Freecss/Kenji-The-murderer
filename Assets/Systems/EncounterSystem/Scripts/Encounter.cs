#region

using Gameplay.Config.Scripts;
using UnityEngine;
using UnityEngine.Events;

#endregion

public class Encounter : MonoBehaviour
{
    [SerializeField] Transform target;

    [SerializeField] private Transform encounterLeftDoor;
    [SerializeField] private Transform encounterRightDoor;
    [SerializeField] private Transform encounterLimitsParent;

    public UnityEvent onEncounterFinished;
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
        if (encounterLeftDoor || encounterRightDoor)
        {
            SetDoorsActivation(false);
        }

        waves = GetComponentsInChildren<Wave>();

        Enemy[] enemies = GetComponentsInChildren<Enemy>(true);
        foreach (Enemy enemy in enemies)
        {
            enemy.target = target;
        }
    }

    void StartEncounter()
    {
        if (encounterLeftDoor || encounterRightDoor)
        {
            SetDoorsActivation(true);
        }

        RoundManager.Instance.StartEncounter();

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
                    if (encounterLeftDoor || encounterRightDoor)
                    {
                        SetDoorsActivation(false);
                    }

                    hasFinished = true;
                    RoundManager.Instance.EndEncounter();
                    onEncounterFinished.Invoke();
                }

                debugHasFinished = HasFinished();
            }
        }
    }

    private void SetDoorsActivation(bool activation)
    {
        // Rotate left door
        if (encounterLeftDoor != null)
        {
            if (activation)
            {
                encounterLeftDoor.localEulerAngles = Vector3.up * 90f;
            }
            else
            {
                encounterLeftDoor.localEulerAngles = Vector3.zero;
            }
        }

        if (encounterRightDoor != null)
        {
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
    }

    public bool HasFinished()
    {
        return currentWave >= waves.Length;
    }

    public void NotifyTriggered()
    {
        if (!hasFinished && !RoundManager.Instance.IsEncounterInCourse())
        {
            StartEncounter();
        }
    }
}