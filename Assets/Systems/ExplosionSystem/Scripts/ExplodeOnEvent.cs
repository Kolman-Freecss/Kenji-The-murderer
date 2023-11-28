#region

using UnityEngine;

#endregion

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
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private void OnDisable()
    {
        hitBox.onHit.RemoveListener(Explode);
        hitBox.onCollisionWithoutHit.RemoveListener(Explode);
    }
}