using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BarrelByParticles : Barrel
{
    ParticleSystem.EmissionModule emission;

    private void Awake()
    {
        emission = GetComponentInChildren<ParticleSystem>().emission;
    }

    private void Start()
    {
        emission.enabled = false;
    }

    public override void StartShooting()
    {
        emission.enabled = true;
    }

    public override void StopShooting()
    {
        emission.enabled = false;
    }
}
