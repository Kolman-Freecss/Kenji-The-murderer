using UnityEngine;

public class Explosion : MonoBehaviour
{
    
    [SerializeField] private float range = 5f;
    [SerializeField] private float force = 1000f;
    [SerializeField] private float upwardsModifier = 1000f;
    [SerializeField] private LayerMask layerMask = Physics.DefaultRaycastLayers;
    
    void Start()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, range, layerMask);
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<HurtBox>(out HurtBox hb))
            {
                hb.NotifyHit(this);
            }
            if (collider.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.AddExplosionForce(force, transform.position, range, upwardsModifier);
            }
        }        
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

}
