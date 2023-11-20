#region

using Entity.Scripts;
using UnityEngine;
using UnityEngine.AI;

#endregion

public class Enemy : MonoBehaviour, IEntityAnimable
{
    [SerializeField] private Transform target;

    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        Vector3 destination = target ? target.position : transform.position;

        agent.SetDestination(destination);
    }

    #region IEntityAnimable Implementation

    public Vector3 GetLastVelocity()
    {
        return agent.velocity;
    }

    public float GetVerticalVelocity()
    {
        return 0f;
    }

    public float GetJumpSpeed()
    {
        return 0f;
    }

    public bool IsGrounded()
    {
        return true;
    }

    #endregion
}