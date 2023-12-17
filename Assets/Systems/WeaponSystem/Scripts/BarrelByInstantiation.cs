#region

using UnityEngine;

#endregion

public class BarrelByInstantiation : Barrel
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] float cadence = 0.5f; //Shots/s

    private float nextShotTime = 0f;
    private bool shooting = false;

    public override void Shot()
    {
        if (Time.time > nextShotTime && !shooting)
        {
            shooting = true;
            nextShotTime = Time.time + 1f / cadence;
            base.Shot();
            Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
            shooting = false;
        }
    }
}