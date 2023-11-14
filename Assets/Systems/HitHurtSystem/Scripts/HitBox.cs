using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Notifies the HurtBox when a HitBox collides with it
/// </summary>
public class HitBox : MonoBehaviour
{
    [SerializeField] public UnityEvent onHit;
    [SerializeField] public UnityEvent <Collider> onHitWithCollider;
    [SerializeField] public UnityEvent onCollisionWithoutHit;
    
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
            hurtBox.NotifyHit(this);
            onHit.Invoke();
            onHitWithCollider.Invoke(collider);
        }
        else
        {
            onCollisionWithoutHit.Invoke();
        }
    }
}
