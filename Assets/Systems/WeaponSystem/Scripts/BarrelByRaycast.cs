#region

using UnityEngine;

#endregion

public class BarrelByRaycast : Barrel
{
    [SerializeField] Transform shootPoint;
    [SerializeField] private float range = 100f;
    [SerializeField] private LayerMask layerMask = Physics.DefaultRaycastLayers;
    [SerializeField] float cadence = 10f; //Shots/s
    [SerializeField] Vector2 dispersionAngles = new Vector2(5f, 5f);

    [SerializeField] private GameObject tracerPrefab;

    // [SerializeField] private LayerMask layerMask;
    [SerializeField] float damage = 5f;

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
        Vector3 dispersedForward = DispersedForward();
        Vector3 finalShotPosition = shootPoint.position + (dispersedForward.normalized * range);

        if (Physics.Raycast(shootPoint.position,
                dispersedForward,
                out RaycastHit hit,
                range,
                layerMask
            ))
        {
            finalShotPosition = hit.point;
            hit.collider.GetComponent<HurtBox>()?.NotifyHit(this, damage);
            // if (hit.collider.TryGetComponent(out HurtBox hurtBox))
            // {
            //     hurtBox.NotifyHit(null);
            // }
        }

        GameObject tracerGo = Instantiate(tracerPrefab);
        Tracer tracer = tracerGo.GetComponent<Tracer>();
        tracer.Init(shootPoint.position, finalShotPosition);
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