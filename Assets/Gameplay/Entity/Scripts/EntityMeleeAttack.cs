#region

using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

#endregion

public class EntityMeleeAttack : MonoBehaviour
{
    [SerializeField] private Vector3 offset = Vector3.forward + Vector3.up;

    [FormerlySerializedAs("radious")] [SerializeField]
    private float radius = 0.35f;

    [SerializeField] private LayerMask layerMask = Physics.DefaultRaycastLayers;
    [SerializeField] private string[] affectedTags = { "Untagged", "Player" };

    [Header("Debug")] [SerializeField] private bool debugAttack;

    [SerializeField] private float damage = 1f;

    private void OnValidate()
    {
        if (debugAttack)
        {
            debugAttack = false;
            PerformAttack();
        }
    }

    public virtual void PerformAttack()
    {
        Collider[] colliders = Physics.OverlapSphere(CalculateAttackPosition(), radius, layerMask);
        foreach (Collider collider in colliders)
        {
            if (affectedTags.Contains(collider.tag))
            {
                HurtBox hurtBox = collider.GetComponent<HurtBox>();
                hurtBox?.NotifyHit(this, damage);
            }
        }
    }

    Vector3 CalculateAttackPosition()
    {
        return transform.position + transform.TransformDirection(offset);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(CalculateAttackPosition(), radius);
    }
}