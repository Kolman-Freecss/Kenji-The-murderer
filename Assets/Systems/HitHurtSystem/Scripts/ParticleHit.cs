#region

using UnityEngine;

#endregion

public class ParticleHit : MonoBehaviour
{
    [SerializeField] private float damagePerParticle = 0.05f;

    private void OnParticleCollision(GameObject other)
    {
        other.GetComponent<HurtBox>().NotifyHit(this, damagePerParticle);
    }
}