using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Notifies the HurtBox when a HitBox collides with it
/// </summary>
public class HitBox : MonoBehaviour
{
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
        collider.GetComponent<HurtBox>()?.NotifyHit(this);
    }
}
