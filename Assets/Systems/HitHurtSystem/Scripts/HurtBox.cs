#region

using UnityEngine;
using UnityEngine.Events;

#endregion

public class HurtBox : MonoBehaviour
{
    [SerializeField] public UnityEvent<float> onHitNotified;
    [SerializeField] public UnityEvent<float, Transform> onHitNotifiedWithOffender;
    [SerializeField] public UnityEvent<float, HitBox> onHitNotifiedWithHitBox;
    [SerializeField] public UnityEvent<float, Barrel> onHitNotifiedWithBarrel;
    [SerializeField] public UnityEvent<float, Explosion> onHitNotifiedWithExplosion;
    [SerializeField] public UnityEvent<float, ParticleHit> onHitNotifiedWithParticleHit;
    [SerializeField] public UnityEvent<float, EntityMeleeAttack> onHitNotifiedWithEntityMeleeAttack;

    internal virtual void NotifyHit(HitBox hitBox, float damage)
    {
        onHitNotified?.Invoke(damage);
        onHitNotifiedWithOffender?.Invoke(damage, hitBox.transform);
        onHitNotifiedWithHitBox?.Invoke(damage, hitBox);
    }

    internal virtual void NotifyHit(Barrel barrelByRaycast, float damage)
    {
        onHitNotified?.Invoke(damage);
        onHitNotifiedWithOffender?.Invoke(damage, barrelByRaycast.transform);
        onHitNotifiedWithBarrel?.Invoke(damage, barrelByRaycast);
    }

    internal void NotifyHit(Explosion barrelByRaycast, float damage)
    {
        onHitNotified?.Invoke(damage);
        onHitNotifiedWithOffender?.Invoke(damage, barrelByRaycast.transform);
        onHitNotifiedWithExplosion?.Invoke(damage, barrelByRaycast);
    }

    internal void NotifyHit(ParticleHit particleHit, float damage)
    {
        onHitNotified?.Invoke(damage);
        onHitNotifiedWithOffender?.Invoke(damage, particleHit.transform);
        onHitNotifiedWithParticleHit?.Invoke(damage, particleHit);
    }

    internal void NotifyHit(EntityMeleeAttack entityMeleeAttack, float damage)
    {
        onHitNotified?.Invoke(damage);
        onHitNotifiedWithOffender?.Invoke(damage, entityMeleeAttack.transform);
        onHitNotifiedWithEntityMeleeAttack?.Invoke(damage, entityMeleeAttack);
    }
}