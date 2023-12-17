#region

using System.Collections;
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
    [SerializeField] private Light light;
    [SerializeField] private GameObject hitPrefab;
    [SerializeField] private GameObject noHitPrefab;

    // [SerializeField] private LayerMask layerMask;
    [SerializeField] float damage = 5f;

    private bool isContinuousShooting;

    private float nextShotTime = 0f;

    private void Update()
    {
        light.intensity = Mathf.Lerp(light.intensity, 0f, 10f * Time.deltaTime);
        light.spotAngle = Mathf.Lerp(light.spotAngle, 0f, 10f * Time.deltaTime);
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
        base.Shot();
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
            if (hit.collider.TryGetComponent(out HurtBox hurtBox))
            {
                Instantiate(hitPrefab, hit.point, Quaternion.Euler(hit.normal.x - 90, hit.normal.y, hit.normal.z));
                hurtBox.NotifyHit(this, damage);
            }
            else
            {
                Instantiate(noHitPrefab, hit.point, Quaternion.Euler(hit.normal.x - 90, hit.normal.y, hit.normal.z));
            }
        }

        GameObject tracerGo = Instantiate(tracerPrefab);
        light.gameObject.SetActive(true);
        StartCoroutine(StopMuzzleFlash());
        light.intensity = 10000000f;
        light.spotAngle = 255f;

        Tracer tracer = tracerGo.GetComponent<Tracer>();
        tracer.Init(shootPoint.position, finalShotPosition);

        IEnumerator StopMuzzleFlash()
        {
            yield return new WaitForSeconds(0.2f);
            light.gameObject.SetActive(false);
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