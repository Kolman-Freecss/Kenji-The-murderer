#region

using UnityEngine;

#endregion

public class EntityMeleeAttackEventForwarder : MonoBehaviour
{
    EntityMeleeAttack entityMeleeAttack;

    private void Awake()
    {
        entityMeleeAttack = GetComponentInParent<EntityMeleeAttack>();
    }

    /// <summary>
    /// Event function invoked from Animation Event.
    /// </summary>
    public void Attack()
    {
        entityMeleeAttack.PerformAttack();
    }

    public void KatanaSlash()
    {
        entityMeleeAttack.PerformAttack();
    }
}