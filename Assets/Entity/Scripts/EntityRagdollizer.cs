#region

using UnityEngine;

#endregion

public class EntityRagdollizer : MonoBehaviour
{
    [SerializeField] bool startAsRagdoll = false;

    Collider[] colliders;
    Rigidbody[] rigidbodies;

    [Header("Debug")] [SerializeField] private bool debugRagdollize = false;
    [SerializeField] private bool debugDeragdollize = false;
    [SerializeField] private Vector3 direction;
    [SerializeField] private bool debugPush;
    [SerializeField] private float debugMinForce;
    [SerializeField] private float debugMaxForce;

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
            DeRagdollize();
        }

        if (debugPush)
        {
            debugPush = false;
            Push(direction.normalized, debugMinForce, debugMaxForce);
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
            DeRagdollize();
        }
    }

    public void Ragdollize()
    {
        foreach (Collider collider in colliders)
        {
            collider.enabled = true;
        }

        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = false;
        }
    }

    public void DeRagdollize()
    {
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }

        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = true;
        }
    }

    public void Push(Vector3 direction, float minForce, float maxForce)
    {
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.AddForce(direction.normalized * Random.Range(minForce, maxForce), ForceMode.Impulse);
        }
    }
}