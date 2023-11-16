using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum ShotMode
    {
        ShotByShot,
        Continuous
    };

    public ShotMode shotMode;

    Barrel[] barrels;

    private void Awake()
    {
        barrels = GetComponentsInChildren<Barrel>();
    }

    public void Shot()
    {
        foreach (Barrel barrel in barrels)
        {
            barrel.Shot();
        }
    }

    public void StartShooting()
    {
        foreach (Barrel barrel in barrels)
        {
            barrel.StartShooting();
        }
    }

    public void StopShooting()
    {
        foreach (Barrel barrel in barrels)
        {
            barrel.StopShooting();
        }
    }
}