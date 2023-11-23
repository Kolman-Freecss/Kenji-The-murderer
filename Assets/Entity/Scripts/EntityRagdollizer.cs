#region

using UnityEngine;

#endregion

public class EntityRagdollizer : MonoBehaviour
{
    [SerializeField] bool startAsRagdoll;
    Collider[] colliders;
    Rigidbody[] rigidbodies;
    [Header("Debug")] [SerializeField] bool debugRagdollize;
    [SerializeField] bool debugDeragdollize;
    [SerializeField] Vector3 debugDirection;
    [SerializeField] bool debugPush;
    [SerializeField] float debugMinForce;
    [SerializeField] float debugMaxForce;


    private void OnValidate()
    {
        if (debugRagdollize)
        {
            debugRagdollize = false;
            Ragdollize();
        }

        if (debugDeragdollize)
        {
            debugDeragdollize = false;
            Deragdollize();
        }

        if (debugPush)
        {
            debugPush = false;
            Push(debugDirection.normalized, debugMinForce, debugMaxForce);
        }
    }

    private void Awake()
    {
        colliders = GetComponentsInChildren<Collider>();
        rigidbodies = GetComponentsInChildren<Rigidbody>();
    }

    private void Start()
    {
        if (!startAsRagdoll)
        {
            Deragdollize();
        }
    }

    public void Ragdollize()
    {
        foreach (Collider c in colliders)
        {
            c.enabled = true;
        }

        foreach (Rigidbody r in rigidbodies)
        {
            r.isKinematic = false;
        }
    }

    public void Deragdollize()
    {
        foreach (Collider c in colliders)
        {
            c.enabled = false;
        }

        foreach (Rigidbody r in rigidbodies)
        {
            r.isKinematic = true;
        }
    }

    public void Push(Vector3 direction, float minForce, float maxForce)
    {
        foreach (Rigidbody r in rigidbodies)
        {
            r.AddForce(direction.normalized * Random.Range(minForce, maxForce));
        }
    }
}