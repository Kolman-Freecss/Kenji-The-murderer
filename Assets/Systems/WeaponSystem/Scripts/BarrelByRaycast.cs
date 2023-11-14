using UnityEngine;

public class BarrelByRaycast : Barrel
{
    
        [SerializeField] Transform shootPoint;
        [SerializeField] private float range = 100f;
        [SerializeField] private LayerMask layerMask = Physics.DefaultRaycastLayers;
        
        // [SerializeField] private LayerMask layerMask;
    
        public override void Shot()
        {
            if (Physics.Raycast(shootPoint.position, 
                    shootPoint.forward, 
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
}
