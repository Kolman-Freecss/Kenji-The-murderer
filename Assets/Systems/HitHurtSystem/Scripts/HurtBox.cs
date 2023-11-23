#region

using UnityEngine;
using UnityEngine.Events;

#endregion

public class HurtBox : MonoBehaviour
{
    [SerializeField] public UnityEvent onHitNotified;
    [SerializeField] public UnityEvent<Transform> onHitNotifiedWithOffender;
    [SerializeField] public UnityEvent<HitBox> onHitNotifiedWithHitBox;
    [SerializeField] public UnityEvent<Barrel> onHitNotifiedWithBarrel;
    [SerializeField] public UnityEvent<Explosion> onHitNotifiedWithExplosion;
    [SerializeField] public UnityEvent<ParticleHit> onHitNotifiedWithParticleHit;
    [SerializeField] public UnityEvent<EntityMeleeAttack> onHitNotifiedWithEntityMeleeAttack;

    internal virtual void NotifyHit(HitBox hitBox)
    {
        onHitNotified?.Invoke();
        onHitNotifiedWithOffender?.Invoke(hitBox.transform);
        onHitNotifiedWithHitBox?.Invoke(hitBox);
    }

    internal virtual void NotifyHit(Barrel barrelByRaycast)
    {
        onHitNotified?.Invoke();
        onHitNotifiedWithOffender?.Invoke(barrelByRaycast.transform);
        onHitNotifiedWithBarrel?.Invoke(barrelByRaycast);
    }

    internal void NotifyHit(Explosion barrelByRaycast)
    {
        onHitNotified?.Invoke();
        onHitNotifiedWithOffender?.Invoke(barrelByRaycast.transform);
        onHitNotifiedWithExplosion?.Invoke(barrelByRaycast);
    }

    internal void NotifyHit(ParticleHit particleHit)
    {
        onHitNotified?.Invoke();
        onHitNotifiedWithOffender?.Invoke(particleHit.transform);
        onHitNotifiedWithParticleHit?.Invoke(particleHit);
    }

    internal void NotifyHit(EntityMeleeAttack entityMeleeAttack)
    {
        onHitNotified?.Invoke();
        onHitNotifiedWithOffender?.Invoke(entityMeleeAttack.transform);
        onHitNotifiedWithEntityMeleeAttack?.Invoke(entityMeleeAttack);
    }
}