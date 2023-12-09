#region

using UnityEngine;
using UnityEngine.Events;

#endregion

/// <summary>
/// Notifies the HurtBox when a HitBox collides with it
/// </summary>
public class HitBox : MonoBehaviour
{
    [SerializeField] public UnityEvent<Collider> onHit;
    [SerializeField] public UnityEvent<Collider> onHitWithCollider;
    [SerializeField] public UnityEvent<Collider> onCollisionWithoutHit;
    [SerializeField] private float damage = 1f;

    private void OnTriggerEnter(Collider other)
    {
        CheckCollider(other);
    }

    private void OnCollisionEnter(Collision other)
    {
        CheckCollider(other.collider);
    }

    private void CheckCollider(Collider collider)
    {
        HurtBox hurtBox = collider.GetComponent<HurtBox>();
        if (hurtBox)
        {
            hurtBox.NotifyHit(this, damage);
            onHit.Invoke(collider);
            onHitWithCollider.Invoke(collider);
        }
        else
        {
            onCollisionWithoutHit.Invoke(collider);
        }
    }
}