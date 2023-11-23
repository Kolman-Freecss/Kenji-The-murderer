#region

using UnityEngine;

#endregion

public class EncounterTrigger : MonoBehaviour
{
    Encounter encounter;

    private void Awake()
    {
        encounter = GetComponentInParent<Encounter>();
    }

    private void OnTriggerEnter(Collider other)
    {
        encounter.NotifyTriggered();
    }
}