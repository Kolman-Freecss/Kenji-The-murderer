using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class HurtBox : MonoBehaviour
{

    [SerializeField] private UnityEvent onHitNotified;
    [SerializeField] private UnityEvent <Transform> onHitNotifiedWithOffender;
    [SerializeField] private UnityEvent <HitBox> onHitNotifiedWithHitBox;
    [SerializeField] private UnityEvent <Barrel> onHitNotifiedWithBarrel;
    [SerializeField] private UnityEvent <Explosion> onHitNotifiedWithExplosion;
    [SerializeField] private UnityEvent <ParticleHit> onHitNotifiedWithParticleHit;
    
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
}
