using UnityEngine;

public class ExplodeOnEvent : MonoBehaviour
{
    [SerializeField] GameObject explosionPrefab;
    HitBox hitBox;
    
    private void Awake()
    {
        hitBox = GetComponent<HitBox>();
    }

    private void OnEnable()
    {
        hitBox.onHit.AddListener(Explode);
        hitBox.onCollisionWithoutHit.AddListener(Explode);
    }
    
    void Explode()
    {
        Destroy(gameObject);
        Instantiate(explosionPrefab, transform.position, transform.rotation);
    }

    private void OnDisable()
    {
        hitBox.onHit.RemoveListener(Explode);
        hitBox.onCollisionWithoutHit.RemoveListener(Explode);
    }
}
