using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BarrelByRaycast : Barrel
{
    [SerializeField] Transform shootPoint;
    [SerializeField] private float range = 100f;
    [SerializeField] private LayerMask layerMask = Physics.DefaultRaycastLayers;
    [SerializeField] float cadence = 10f; //Shots/s
    [SerializeField] Vector2 dispersionAngles = new Vector2(5f, 5f);
    // [SerializeField] private LayerMask layerMask;

    private bool isContinuousShooting;

    private float nextShotTime = 0f;

    private void Update()
    {
        if (isContinuousShooting)
        {
            if (Time.time > nextShotTime)
            {
                nextShotTime += 1f / cadence;
                Shot();
            }
        }
        else
        {
            nextShotTime = Time.time;
        }
    }

    public override void Shot()
    {
        if (Physics.Raycast(shootPoint.position,
                DispersedForward(),
                out RaycastHit hit,
                range,
                layerMask
            ))
        {
            hit.collider.GetComponent<HurtBox>()?.NotifyHit(this);
            // if (hit.collider.TryGetComponent(out HurtBox hurtBox))
            // {
            //     hurtBox.NotifyHit(null);
            // }
        }
    }

    private Vector3 DispersedForward()
    {
        float horizontalDispersionAngle = Random.Range(-dispersionAngles.x, dispersionAngles.x);
        float verticalDispersionAngle = Random.Range(-dispersionAngles.y, dispersionAngles.y);

        Quaternion horizontalRotation = Quaternion.AngleAxis(horizontalDispersionAngle, transform.up);
        Quaternion verticalRotation = Quaternion.AngleAxis(verticalDispersionAngle, transform.right);

        return horizontalRotation * verticalRotation * transform.forward;
    }

    public override void StartShooting()
    {
        isContinuousShooting = true;
    }

    public override void StopShooting()
    {
        isContinuousShooting = false;
    }
}