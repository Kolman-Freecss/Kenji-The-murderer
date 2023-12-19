#region

using System;
using Entity.Scripts;
using UnityEngine;
using UnityEngine.AI;

#endregion

public class Enemy : MonoBehaviour, IEntityAnimable
{
    [Serializable]
    public enum EnemyType
    {
        Ninja,
        Archer,
        Giant
    }

    [SerializeField] public Transform target;
    [SerializeField] private float attackDistance = 1f;
    [SerializeField] EnemyType enemyType;

    private NavMeshAgent agent;
    private Animator animator;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        Vector3 destination = target ? target.position : transform.position;

        agent.SetDestination(destination);

        animator.SetBool(
            "isAttacking",
            target ? Vector3.Distance(target.position, transform.position) < attackDistance : false);
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

    public bool haveWeapon()
    {
        return false;
    }

    #endregion

    #region Getter & Setters

    public EnemyType GetEnemyType()
    {
        return enemyType;
    }

    #endregion
}