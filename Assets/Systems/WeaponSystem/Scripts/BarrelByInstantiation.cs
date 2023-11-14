using UnityEngine;

public class BarrelByInstantiation : Barrel
{

    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform shootPoint;

    public override void Shot()
    {
        Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
    }
}
