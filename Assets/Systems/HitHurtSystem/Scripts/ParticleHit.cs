using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHit : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        other.GetComponent<HurtBox>().NotifyHit(this);
    }
}
